using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Game Variables
    int waveScore = 0;
    int killScore = 0;
    float cameraSize;


    // Game Properties
    bool gamePaused = false;
    bool gameStarted = false;
    bool softPauseActive = false;

    // Game Objects
    [Header("References")]
    public WaveManager waveManager;
    public UIManager uiManager;
    PlayerController player;

    // wave timer
    bool timerActivated;
    float timer;

    AudioSource maintheme;

    void Start()
    {
        Application.targetFrameRate = 60;
        Setup();
        maintheme = FindObjectOfType<SoundManager>().Play("MainMusic");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !gameStarted)
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

    public void UIStartGame()
    {
        StartGame();
    }

    void Setup()
    {
        cameraSize = FindObjectOfType<Camera>().orthographicSize;
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
        FindObjectOfType<SoundManager>().StopPlaying(maintheme);
        FindObjectOfType<Camera>().orthographicSize = cameraSize;
        gameStarted = true;
        player.gameStarted = true;
        waveManager.enabled = true;
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

    public void TogglePauseGame()
    {
        if (gamePaused == true)
        {
            gamePaused = false;

            if(softPauseActive == false)
            {
                waveManager.isPaused = gamePaused;
                player.isPaused = gamePaused;
                Time.timeScale = 1;
            }
            
            uiManager.TogglePauseOverlay(gamePaused); 
        }
        else if (gamePaused == false)
        {
            gamePaused = true;

            if(softPauseActive == false)
            {
                waveManager.isPaused = gamePaused;
                player.isPaused = gamePaused;
                Time.timeScale = 0;
            }
            
            uiManager.TogglePauseOverlay(gamePaused);
        }
        else
        {
            Debug.Log("Pause code broken");
        }
    }

    public void UIUnpause()
    {
        TogglePauseGame();
    }

    public void SoftPause()
    {
        softPauseActive = true;
        waveManager.isPaused = gamePaused;
        player.isPaused = gamePaused;
        Time.timeScale = 0;
    }

    public void SoftUnPause()
    {
        softPauseActive = false;
        waveManager.isPaused = gamePaused;
        player.isPaused = gamePaused;
        Time.timeScale = 1;
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
        //uiManager.EnableTimerUI();
        Invoke("EnableTimerUIDelayed", 0.01f);
        timer = time;
        timerActivated = true;
    }

    void EnableTimerUIDelayed()
    {
        uiManager.EnableTimerUI();
    }

}
