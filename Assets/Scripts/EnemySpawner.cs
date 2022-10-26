using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemySpawner : MonoBehaviour
{
    GameObject player;
    Vector2 bottomleft;
    Vector2 topright;
    float spawnX;
    float spawnY;
    public GameObject Enemy;
    Transform enemySpawnPoint;
    bool bEnemySpawned;
    public float enemySpawnDelay = 3f;
    public float playAreaLeftX;
    public float playAreaRightX;
    public float playAreaTopY;
    public float playAreaBotomY;
    // Start is called before the first frame update
    void Start()
    {
        enemySpawnPoint = Enemy.transform;
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        // Spawning the Enemy on a loop
        if (bEnemySpawned == true)
        {
            bEnemySpawned = false;
            Invoke("SpawnEnemy", enemySpawnDelay);
        }
    }

    void SpawnEnemy()
    {
        bEnemySpawned = true;
        
        GeneratePosition();

        // Setting the position for the enemy to spawn
        enemySpawnPoint.position = new Vector2(spawnX, spawnY);

        // Spawning the enemy at the given spawn point
        Instantiate<GameObject>(Enemy, enemySpawnPoint).GetComponent<AIDestinationSetter>().target = player.transform;
    }

    void GeneratePosition()
    {
        // Finding Screen Boundaries
        bottomleft = player.transform.GetChild(0).GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0, 0));
        topright = player.transform.GetChild(0).GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1, 1));

        // Generate X Value
        float spawnX1 = Random.Range(playAreaLeftX, bottomleft.x);
        float spawnX2 = Random.Range(topright.x, playAreaRightX);
        if (spawnX1 < playAreaLeftX)
        {
            spawnX = spawnX2;
        }
        else if (spawnX2 > playAreaRightX)
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
        float spawnY1 = Random.Range(topright.y, playAreaTopY);
        float spawnY2 = Random.Range(playAreaBotomY, bottomleft.y);
        if (spawnY1 > playAreaTopY)
        {
            spawnY = spawnY2;
        }
        else if (spawnY2 < playAreaBotomY)
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
    }
}
