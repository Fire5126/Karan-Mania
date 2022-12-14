using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Game Variables
    int waveScore;
    int killScore;

    // Game Properties
    bool gamePaused = false;

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

    public bool TogglePauseGame()
    {
        if (gamePaused == true)
        {
            gamePaused = false;
            return gamePaused;
        }
        else if (gamePaused == false)
        {
            gamePaused = true;
            return gamePaused;
        }
        Debug.Log("Pause code broken");
        return gamePaused;
    }
}
