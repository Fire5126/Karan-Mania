using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Random = UnityEngine.Random;

public class TestSpawner : MonoBehaviour
{
    private int currentWave = 0;

    [SerializeField] private GameObject[] enemies;

    public bool gamePaused = false;
    private bool waveStarted = false;
    
    [Header("ratios for enemies for their wave group"), Tooltip("order: 0 = worker, 1 = angryWorker, 2 = supervisor, 3 = manager, 4 = CEO")]
    [SerializeField] private int[] wave1ratio;
    [SerializeField] private int[] wave3ratio;
    [SerializeField] private int[] wave6ratio;
    [SerializeField] private int[] wave8ratio;
    [SerializeField, InspectorName("Wave10Ratio (includes boss)")] private int[] wave10ratio;
    private List<enemiesEnum> enemyListVar = new List<enemiesEnum>();
    private Enemy[] enemiesSpawned;
    private bool enemySpawned;
    int waveIndex = 0;
    private float waveDelay = 5;

    // Graph Values
    float maxGraphX;
    float maxGraphY;
    float minGraphX;
    float minGraphY;
    Vector2 spawnPoint;
    GridNode[] nodes;

    private GameManager gameManager;
    private Camera gameCamera;
    private UpgradeMenu upgradeMenu;
    private PlayerController player;

    [SerializeField] private float enemySpawnDelay = 1.0f;
    private int bossesSpawned = 0;
    

    public enum enemiesEnum
    {
        worker,
        angryWorker,
        Supervisor,
        Manager,
        CEO
    }
    

    private int amountOfEnemiesToSpawn = 0;
    [SerializeField, InspectorName("Wave Difficulty Multiplier")] private float waveDiff = 1.5f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        InitialisePlayArea();
    }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameCamera = FindObjectOfType<Camera>();
        upgradeMenu = FindObjectOfType<UpgradeMenu>();
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        // checks if the wave has started or if the game has paused.
        if (!waveStarted || gamePaused) return;

        if (!enemySpawned) return;
        
        enemiesSpawned = FindObjectsOfType<Enemy>();
        if (enemiesSpawned.Length > 0)
        {
            return;
        }
        
        FinishWave();
    }

    private List<enemiesEnum> CalculateEnemyToSpawn(int enemyTotal, int[] ratio, bool boss)
    {
        List<int> amountOfEnemyTypes = new List<int>();
        int ratioTotal = 0;
        List<enemiesEnum> enemiesList = new List<enemiesEnum>();

        for (int i = 0; i < ratio.Length; i++)
        {
            ratioTotal += ratio[i];
        }

        for (int i = 0; i < ratio.Length; i++)
        {
            amountOfEnemyTypes.Add(ratio[i] * (enemyTotal / ratioTotal));
        }

        if (boss)
        {
            amountOfEnemyTypes.Add(++bossesSpawned);
        }

        for (int a = 0; a < amountOfEnemyTypes.Count; a++)
        {
            for (int j = 0; j < amountOfEnemyTypes[a]; j++)
            {
                switch (a)
                {
                    case 0:
                        enemiesList.Add(enemiesEnum.worker);
                        break;
                    case 1:
                        enemiesList.Add(enemiesEnum.angryWorker);
                        break;
                    case 2:
                        enemiesList.Add(enemiesEnum.Supervisor);
                        break;
                    case 3:
                        enemiesList.Add(enemiesEnum.Manager);
                        break;
                    case 4:
                        enemiesList.Add(enemiesEnum.CEO);
                        break;
                }
            }
        }

        return enemiesList;
    }

    private IEnumerator SpawnEnemies(List<enemiesEnum> enemyType)
    {
        foreach (var x in enemyType)
        {
            yield return new WaitForSeconds(enemySpawnDelay);
            Instantiate<GameObject>(enemies[(int)x], InitialiseEnemySpawnPoint(), Quaternion.identity).GetComponent<AIDestinationSetter>().target = player.transform;
            enemySpawned = true;
        }
    }
    
    public void InitialisePlayArea()
    {
        nodes = AstarPath.active.data.gridGraph.nodes;

        maxGraphX = ((AstarPath.active.data.gridGraph.width * AstarPath.active.data.gridGraph.nodeSize) / 2) + AstarPath.active.data.gridGraph.center.x;
        maxGraphY = ((AstarPath.active.data.gridGraph.Depth * AstarPath.active.data.gridGraph.nodeSize) / 2) + AstarPath.active.data.gridGraph.center.y;
        minGraphX = maxGraphX - (AstarPath.active.data.gridGraph.width * AstarPath.active.data.gridGraph.nodeSize);
        minGraphY = maxGraphY - (AstarPath.active.data.gridGraph.Depth * AstarPath.active.data.gridGraph.nodeSize);
    }
    
    private Vector2 InitialiseEnemySpawnPoint()
    {
        
        Vector2 bottomleft = gameCamera.ViewportToWorldPoint(new Vector3(0, 0));
        Vector2 topright = gameCamera.ViewportToWorldPoint(new Vector3(1, 1));
        List<GridNode> walkableNodes = new List<GridNode>();

        foreach (GridNode node in nodes)
        {
            Vector2 vectorNode = (Vector3)node.position;
            if (node.Walkable == true)
            {
                if (vectorNode.x > topright.x || vectorNode.x < bottomleft.x || vectorNode.y > topright.y || vectorNode.y < bottomleft.y)
                {
                    walkableNodes.Add(node);
                }
            }
        }

        int randomnum = Random.Range(0, walkableNodes.Count);
        return (Vector3)walkableNodes[randomnum].position;
    }

    public void StartNextWave(int waveNumber)
    {

        if (waveStarted) { Debug.LogError("Wave already in progress!"); return; }

        waveStarted = true;
        waveIndex++;
        
        // calculates the amount of enemies to spawned based on:
        // enemies represented by the equation ((2^bx) + 5) where x is the wave number and b is the difficulity multiplier
        amountOfEnemiesToSpawn = Mathf.RoundToInt(Mathf.Pow(2, waveDiff * waveNumber) + 5);

        enemyListVar = new List<enemiesEnum>();
        switch (currentWave)
        {
            case <=3:
                enemyListVar = CalculateEnemyToSpawn(amountOfEnemiesToSpawn, wave1ratio, false);
                break;
            case <=6:
                enemyListVar = CalculateEnemyToSpawn(amountOfEnemiesToSpawn, wave3ratio, false);
                break;
            case <=8:
                enemyListVar = CalculateEnemyToSpawn(amountOfEnemiesToSpawn, wave6ratio, false);
                break;
            case <=10:
                enemyListVar = CalculateEnemyToSpawn(amountOfEnemiesToSpawn, wave8ratio, false);
                break;
            case >10:
                enemyListVar = CalculateEnemyToSpawn(amountOfEnemiesToSpawn, wave10ratio, true);
                break;
        }
        
        // Spawns the enemies
        StartCoroutine(SpawnEnemies(enemyListVar));
    }

    private void FinishWave()
    {
        enemySpawned = false;
        waveStarted = false;
        upgradeMenu.OfferUpgrades(waveIndex);
        if (waveIndex >= 3)
        {
            waveIndex = 0;
        }
        gameManager.StartWaveTimer(waveDelay);

        // start next wave in gameManager
    }
}