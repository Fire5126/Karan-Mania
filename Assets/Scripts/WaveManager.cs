using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class WaveManager : MonoBehaviour
{
    // Game Objects
    PlayerController player;
    public GameObject enemy;
    Camera gameCamera;

    // Graph Values
    float maxGraphX;
    float maxGraphY;
    float minGraphX;
    float minGraphY;
    Vector2 spawnPoint;

    // Wave Management Variables
    bool waveActive = false;
    public float gameDifficulty;
    int enemiesToSpawn;
    int enemiesSpawned;
    public float spawnDelay;
    float nextSpawnTime;
    public float waveDelay;
    Enemy[] enemies;
    bool gameActive = false;

    // Functions
    void Start()
    {
        gameCamera = FindObjectOfType<Camera>();
        player = FindObjectOfType<PlayerController>();
        InitialisePlayArea();
        InitialiseGame();
    }

    void Update()
    {
        if (gameActive)
        {
            ProgressGame();
        }
    }

    void InitialisePlayArea()
    {
        maxGraphX = ((AstarPath.active.data.gridGraph.width * AstarPath.active.data.gridGraph.nodeSize) / 2) + AstarPath.active.data.gridGraph.center.x;
        maxGraphY = ((AstarPath.active.data.gridGraph.Depth * AstarPath.active.data.gridGraph.nodeSize) / 2) + AstarPath.active.data.gridGraph.center.y;
        minGraphX = maxGraphX - (AstarPath.active.data.gridGraph.width * AstarPath.active.data.gridGraph.nodeSize);
        minGraphY = maxGraphY - (AstarPath.active.data.gridGraph.Depth * AstarPath.active.data.gridGraph.nodeSize);
    }

    void InitialiseEnemySpawnPoint()
    {
        float spawnY;
        float spawnX;

        Vector2 bottomleft = gameCamera.ViewportToWorldPoint(new Vector3(0, 0));
        Vector2 topright = gameCamera.ViewportToWorldPoint(new Vector3(1, 1));

        // Generate X Value
        float spawnX1 = Random.Range(minGraphX, bottomleft.x);
        float spawnX2 = Random.Range(topright.x, maxGraphX);
        if (spawnX1 < minGraphX)
        {
            spawnX = spawnX2;
        }
        else if (spawnX2 > maxGraphX)
        {
            spawnX = spawnX1;
        }
        else
        {
            if (Random.value > 0.5f)
            {
                spawnX = spawnX1;
            }
            else
            {
                spawnX = spawnX2;
            }
        }

        // Generate Y Value
        float spawnY1 = Random.Range(topright.y, maxGraphY);
        float spawnY2 = Random.Range(minGraphY, bottomleft.y);
        if (spawnY1 > maxGraphY)
        {
            spawnY = spawnY2;
        }
        else if (spawnY2 < minGraphY)
        {
            spawnY = spawnY1;
        }
        else
        {
            if (Random.value > 0.5f)
            {
                spawnY = spawnY1;
            }
            else
            {
                spawnY = spawnY2;
            }
        }

        // Combining Spawn Points
        spawnPoint = new Vector2(spawnX, spawnY);
    }

    void SpawnEnemy()
    {
        InitialiseEnemySpawnPoint();
        while (!AstarPath.active.GetNearest(spawnPoint).node.Walkable)
        {
            InitialiseEnemySpawnPoint();
        }
        Instantiate<GameObject>(enemy, spawnPoint, Quaternion.identity).GetComponent<AIDestinationSetter>().target = player.transform;
        enemiesSpawned++;
    }

    void InitialiseWave()
    {
        enemiesToSpawn = Mathf.RoundToInt(enemiesToSpawn * gameDifficulty + 5);
        enemiesSpawned = 0;
        waveActive = true;
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
        ResetValues();
        gameActive = true;
        InitialiseWave();
    }

    void ProgressGame()
    {
        enemies = FindObjectsOfType<Enemy>();
        if (Time.time > nextSpawnTime && waveActive && enemiesToSpawn > enemiesSpawned)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnDelay;
        }
        else if (enemiesToSpawn <= enemiesSpawned && waveActive && enemies.Length == 0)
        {
            waveActive = false;
            Invoke("InitialiseWave", waveDelay);
        }
    }
}
