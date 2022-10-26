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
    // Start is called before the first frame update
    void Start()
    {
        enemySpawnPoint = gameObject.transform;
        player = GameObject.FindGameObjectWithTag("Player");
        SpawnEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnEnemy()
    {
        bottomleft = player.transform.GetChild(0).GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0, 0));
        topright = player.transform.GetChild(0).GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1, 1));
        GeneratePosition();
        while((spawnX <= (topright.x) || spawnX >= bottomleft.x) && (spawnY <= topright.y || spawnY >= bottomleft.y))
        {
            GeneratePosition();
        }
        enemySpawnPoint.position = new Vector2(spawnX, spawnY);
        GameObject enemy = Instantiate<GameObject>(Enemy, enemySpawnPoint);
        enemy.GetComponent<AIDestinationSetter>().target = player.transform;
    }
    void GeneratePosition()
    {
        spawnX = Random.Range(-2.5f, 22.5f);
        spawnY = Random.Range(-3.45f, 7.5f);
    }
}
