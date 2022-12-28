using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Game Variables
    int waveScore;
    int killScore;

    // Game Properties
    bool gamePaused = false;

    // Game Objects
    public WaveManager waveManager;
    public UIManager uiManager;
    PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        gamePaused = false;

        waveManager.isPaused = gamePaused;
        uiManager.TogglePauseOverlay(gamePaused);
        player.isPaused = gamePaused;

        Time.timeScale = 1;
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

            waveManager.isPaused = gamePaused;
            uiManager.TogglePauseOverlay(gamePaused);
            player.isPaused = gamePaused;

            Time.timeScale = 1;
            return gamePaused;
        }
        else if (gamePaused == false)
        {
            gamePaused = true;

            waveManager.isPaused = gamePaused;
            uiManager.TogglePauseOverlay(gamePaused);
            player.isPaused = gamePaused;

            Time.timeScale = 0;
            return gamePaused;
        }
        Debug.Log("Pause code broken");
        return gamePaused;
    }

    public void UIUnpause()
    {
        TogglePauseGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayerDied()
    {
        gamePaused = true;
        waveManager.isPaused = gamePaused;
        uiManager.ActivateDeathOverlay();
        player.isPaused = gamePaused;
        Time.timeScale = 0;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void UpdatePlayerHealthStat(float health)
    {
        uiManager.UpdateHealthStat(health);
    }
}
