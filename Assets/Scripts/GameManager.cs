using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    int waveScore;
    int killScore;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddWaveScore()
    {
        waveScore++;
    }

    public void AddKillScore()
    {
        killScore++;
    }

    public int GetWaveScore()
    {
        return waveScore;
    }

    public int GetKillScore()
    {
        return killScore;
    }
}
