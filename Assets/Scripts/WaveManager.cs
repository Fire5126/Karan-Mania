using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WaveManager : MonoBehaviour
{
    // Misc Variables
    [Header("Misc Variables")]
    public bool isPaused;

    // Game Objects
    PlayerController player;
    Camera gameCamera;
    GameManager gameManager;
    UpgradeMenu upgradeMenu;

    // Enemy GameObjects
    [Header("Enemy GameObject List")]
    public GameObject[] enemyTypes;

    //public List<GameObject> enemyTypes2;

    // Graph Values
    float maxGraphX;
    float maxGraphY;
    float minGraphX;
    float minGraphY;
    Vector2 spawnPoint;
    GridNode[] nodes;


    // Wave Management Variables
    [Header("Wave Management Variables")]
    public float gameDifficulty;
    public float spawnDelay;
    public float waveDelay;
    bool waveActive = false;
    int enemiesToSpawn;
    int enemiesSpawned;
    float nextSpawnTime;
    Enemy[] enemies;
    bool gameActive = false;
    int waveIndex = 0;

    // Amount Of Enemies
    int retailWorker = 0;
    int angryWorker = 0;
    int supervisorWorker = 0;
    int managerWorker = 0;
    int CEOWorker = 0;
    int CEOsSpawned = 1;


    // Functions
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameCamera = FindObjectOfType<Camera>();
        upgradeMenu = FindObjectOfType<UpgradeMenu>();
        player = FindObjectOfType<PlayerController>();
        InitialisePlayArea();
        InitialiseGame();
    }

    void Update()
    {
        

        if (gameActive && !isPaused)
        {
            
            ProgressGame();
        }
    }

    void InitialisePlayArea()
    {
        nodes = AstarPath.active.data.gridGraph.nodes;

        maxGraphX = ((AstarPath.active.data.gridGraph.width * AstarPath.active.data.gridGraph.nodeSize) / 2) + AstarPath.active.data.gridGraph.center.x;
        maxGraphY = ((AstarPath.active.data.gridGraph.Depth * AstarPath.active.data.gridGraph.nodeSize) / 2) + AstarPath.active.data.gridGraph.center.y;
        minGraphX = maxGraphX - (AstarPath.active.data.gridGraph.width * AstarPath.active.data.gridGraph.nodeSize);
        minGraphY = maxGraphY - (AstarPath.active.data.gridGraph.Depth * AstarPath.active.data.gridGraph.nodeSize);
    }

    void InitialiseEnemySpawnPoint()
    {
        
        Vector2 bottomleft = gameCamera.ViewportToWorldPoint(new Vector3(0, 0));
        Vector2 topright = gameCamera.ViewportToWorldPoint(new Vector3(1, 1));
        GridNode[] walkableNodes;
        int x = 0;
        int y = 0;
        
        

        foreach (GridNode node in nodes)
        {
            Vector2 vectorNode = (Vector3)node.position;
            if (node.Walkable == true)
            {
                if (vectorNode.x > topright.x || vectorNode.x < bottomleft.x || vectorNode.y > topright.y || vectorNode.y < bottomleft.y)
                {
                    x++;
                }
            }
        }
        walkableNodes = new GridNode[x];
        foreach (GridNode node in nodes)
        {
            Vector2 vectorNode = (Vector3)node.position;
            if (node.Walkable == true)
            {
                if (vectorNode.x > topright.x || vectorNode.x < bottomleft.x || vectorNode.y > topright.y || vectorNode.y < bottomleft.y)
                {
                    
                    walkableNodes[y] = node;
                    y++;
                }
            }
        }

        int randomnum = Random.Range(0, walkableNodes.Length);
        spawnPoint = (Vector3)walkableNodes[randomnum].position;
    }

    void SpawnEnemy(GameObject enemyToSpawn)
    {
        InitialiseEnemySpawnPoint();
        Instantiate<GameObject>(enemyToSpawn, spawnPoint, Quaternion.identity).GetComponent<AIDestinationSetter>().target = player.transform;
        enemiesSpawned++;
    }

    int[] CalculateEnemyToSpawn()
    {
        int waveScore = gameManager.GetWaveScore();
        int[] enemyList = new int[enemiesToSpawn];

        if (waveScore <= 2)
        {
            // ratio of 1
            int ratioTotal = 1;

            retailWorker = Mathf.RoundToInt(1 * (enemiesToSpawn / ratioTotal));
            int x = 0;

            for (int i = 0; i < retailWorker; i++)
            {
                enemyList[i] = 0;
                x = i;
            }
            return enemyList;
        }
        if (waveScore <= 3)
        {

            // ratio of 3:2
            int ratioTotal = 3 + 2;

            retailWorker = Mathf.RoundToInt(3 * (enemiesToSpawn / ratioTotal));
            angryWorker = Mathf.RoundToInt(2 * (enemiesToSpawn / ratioTotal));
            int x = 0;

            for (int i = 0; i < retailWorker; i++)
            {
                enemyList[i] = 0;
                x = i;
            }
            for (int i = 0; i < angryWorker; i++)
            {
                enemyList[i + x] = 1;
                x = i;
            }
            return enemyList;
        }
        if (waveScore <= 5)
        {
            // ratio of 4:3:1
            int ratioTotal = 4 + 3 + 1;

            retailWorker = Mathf.RoundToInt(4 * (enemiesToSpawn / ratioTotal));
            angryWorker = Mathf.RoundToInt(3 * (enemiesToSpawn / ratioTotal));
            supervisorWorker = Mathf.RoundToInt(1 * (enemiesToSpawn / ratioTotal));

            int x = 0;

            for (int i = 0; i < retailWorker; i++)
            {
                enemyList[i] = 0;
                x = i;
            }
            for (int i = 0; i < angryWorker; i++)
            {
                enemyList[i + x] = 1;
                x = i;
            }
            for (int i = 0; i < supervisorWorker; i++)
            {
                enemyList[i + x] = 2;
                x = i;
            }

            return enemyList;
        }
        if (waveScore <= 8)
        {
            // ratio of 4:3:2:1
            int ratioTotal = 4 + 3 + 2 + 1;

            retailWorker = Mathf.RoundToInt(4 * (enemiesToSpawn / ratioTotal));
            angryWorker = Mathf.RoundToInt(3 * (enemiesToSpawn / ratioTotal));
            supervisorWorker = Mathf.RoundToInt(2 * (enemiesToSpawn / ratioTotal));
            managerWorker = Mathf.RoundToInt(1 * (enemiesToSpawn / ratioTotal));

            int x = 0;

            for (int i = 0; i < retailWorker; i++)
            {
                enemyList[i] = 0;
                x = i;
            }
            for (int i = 0; i < angryWorker; i++)
            {
                enemyList[i + x] = 1;
                x = i;
            }
            for (int i = 0; i < supervisorWorker; i++)
            {
                enemyList[i + x] = 2;
                x = i;
            }
            for (int i = 0; i < managerWorker; i++)
            {
                enemyList[i + x] = 3;
                x = i;
            }

            return enemyList;
        }
        if (waveScore > 10)
        {
            // ratio of 4:3:2:1
            int ratioTotal = 5 + 4 + 2 + 2;

            retailWorker = Mathf.RoundToInt(5 * (enemiesToSpawn - 1 / ratioTotal));
            angryWorker = Mathf.RoundToInt(4 * (enemiesToSpawn - 1 / ratioTotal));
            supervisorWorker = Mathf.RoundToInt(2 * (enemiesToSpawn - 1 / ratioTotal));
            managerWorker = Mathf.RoundToInt(2 * (enemiesToSpawn - 1 / ratioTotal));
            CEOWorker = CEOsSpawned;

            int x = 0;

            for (int i = 0; i < retailWorker; i++)
            {
                enemyList[i] = 0;
                x = i;
            }
            for (int i = 0; i < angryWorker; i++)
            {
                enemyList[i + x] = 1;
                x = i;
            }
            for (int i = 0; i < supervisorWorker; i++)
            {
                enemyList[i + x] = 2;
                x = i;
            }
            for (int i = 0; i < managerWorker; i++)
            {
                enemyList[i + x] = 3;
                x = i;
            }
            for (int i = 0; i < CEOWorker; i++)
            {
                enemyList[enemiesToSpawn - i] = 4;
            }

            CEOsSpawned++;

            return enemyList;
        }

        return null;
    }

    void InitialiseWave()
    {
        AstarPath.active.data.gridGraph.Scan();
        enemiesToSpawn = Mathf.RoundToInt(enemiesToSpawn * gameDifficulty + 5);
        enemiesSpawned = 0;
        waveActive = true;
        gameManager.AddWaveScore();
        waveIndex++;
    }

    public void ResetValues()
    {
        gameActive = false;
        if (enemies != null)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                Destroy(enemies[i].gameObject);
            }
        }
        enemiesSpawned = 0;
        enemiesToSpawn = 0;
        waveActive = false;
    }

    public void InitialiseGame()
    {
        if (!isPaused)
        {
            ResetValues();
            gameActive = true;
            InitialiseWave();
        }
    }

    void ProgressGame()
    {
        enemies = FindObjectsOfType<Enemy>();
        int[] enemyIndexes = CalculateEnemyToSpawn();
        if (Time.time > nextSpawnTime && waveActive && enemiesToSpawn > enemiesSpawned)
        {
            if (enemyIndexes == null)
            {
                return;
            }
            int enemyIndex = Random.Range(0, enemyIndexes.Length);
            while (enemyIndexes[enemyIndex] == -1)
            {
                enemyIndex = Random.Range(0, enemyIndexes.Length);
            }
            int enemyIndexToSpawn;
            enemyIndexToSpawn = enemyIndexes[enemyIndex];
            enemyIndexes[enemyIndex] = -1;

            SpawnEnemy(enemyTypes[enemyIndexToSpawn]);
            nextSpawnTime = Time.time + spawnDelay;
        }
        else if (enemiesToSpawn <= enemiesSpawned && waveActive && enemies.Length == 0)
        {
            upgradeMenu.OfferUpgrades(waveIndex);
            if (waveIndex >= 3)
            {
                waveIndex = 0;
            }
            waveActive = false;
            gameManager.StartWaveTimer(waveDelay);
            Invoke("InitialiseWave", waveDelay);
        }
    }
}
