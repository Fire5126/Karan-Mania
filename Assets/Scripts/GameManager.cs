using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Game Variables
    int waveScore = 0;
    int killScore = 0;

    // Game Properties
    bool gamePaused = false;
    bool gameStarted = false;

    // Game Objects
    [Header("References")]
    public WaveManager waveManager;
    public UIManager uiManager;
    PlayerController player;

    // wave timer
    bool timerActivated;
    float timer;

    void Start()
    {
        Setup();
    }

    void Update()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.touchCount >= 1) && !gameStarted)
        {
            StartGame();
        }

        if (timerActivated)
        {
            timer -= 1 * Time.deltaTime;
            uiManager.UpdateTimerUI(timer);
            if (timer <= 0)
            {
                timerActivated = false;
                uiManager.DisableTimerUI();
            }
        }
    }

    void Setup()
    {
        FindObjectOfType<Camera>().orthographicSize = 2.5f;
        player = FindObjectOfType<PlayerController>();
        gamePaused = false;
        gameStarted = false;
        player.gameStarted = false;
        waveManager.enabled = false;
        waveManager.isPaused = gamePaused;
        uiManager.TogglePauseOverlay(gamePaused);
        player.isPaused = gamePaused;
        uiManager.DisableInGameOverlay();
        uiManager.EnablePreGameOverlay();
        Time.timeScale = 1;
    }

    void StartGame()
    {
        gameStarted = true;
        player.gameStarted = true;
        waveManager.enabled = true;
        FindObjectOfType<Camera>().orthographicSize = 6f;
        uiManager.EnableInGameOverlay();
        uiManager.DisablePreGameOverlay();
        uiManager.UpdateWaveScoreUI(waveScore);
        uiManager.UpdateKillsScoreUI(killScore);
    }

    public void AddWaveScore()
    {
        waveScore++;
        uiManager.UpdateWaveScoreUI(waveScore);
    }

    public void AddKillScore()
    {
        killScore++;
        uiManager.UpdateKillsScoreUI(killScore);
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

    public void StartWaveTimer(float time)
    {
        uiManager.EnableTimerUI();
        timer = time;
        timerActivated = true;
    }

}
