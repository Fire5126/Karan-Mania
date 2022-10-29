using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    // Multiplier to increase wave difficulty exponentially
    public int waveDiffMultiplier = 1;

    // The time that it takes to spawn all enemies at the beggining of a wave
    public float enemySpawnTime = 10;

    // Internal variables for progressing the waves and sending information to other scripts
    int waveNumber;
    int enemiesToSpawn;
    float enemySpawnRate;
    

    void Start()
    {
        InitiateNextWave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitiateNextWave()
    {
        waveNumber = waveNumber + 1;
        enemiesToSpawn = waveDiffMultiplier * waveNumber + 5;
        enemySpawnRate = enemiesToSpawn / 10;

        if (waveNumber >= 2 && waveNumber <4)
        {
            int retailWorker = Mathf.RoundToInt(enemiesToSpawn * 0.8f);
            int angryRetailWorker = Mathf.RoundToInt(enemiesToSpawn * 0.2f);
            gameObject.GetComponent<EnemySpawner>().InitilizeSpawnEnemy(enemySpawnRate, enemiesToSpawn);
        }

        
    }

    public bool WaveFinished()
    {
        Invoke("InitiateNextWave", 15f);
        return false;
    }
}
