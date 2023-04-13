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
    static float t = 0.0f;


    // Game Properties
    bool gamePaused = false;
    bool gameStarted = false;
    bool softPauseActive = false;

    // Game Objects
    [Header("References")]
    [SerializeField] private IntroManager introManager;
    public WaveManager waveManager;
    public UIManager uiManager;
    PlayerController player;
    SoundManager soundManager;
    AudioSource titleScreenMusic;
    AudioSource maintheme;

    [Header("Sector Barriers")]
    [SerializeField] private BoxCollider2D sector2Barrier;
    [SerializeField] private BoxCollider2D sector3Barrier;
    [SerializeField] private int room2WaveNumber;
    [SerializeField] private int room3WaveNumber;

    // wave timer
    bool timerActivated;
    float timer;

    

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {

        Setup();
    }

    void Update()
    {
        // starts the game
        if (Input.GetKeyDown(KeyCode.Space) && !gameStarted)
        {
            StartGame();
        }

        // in between wave timer
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

        if (gameStarted)
        {
            FindObjectOfType<Camera>().orthographicSize = Mathf.Lerp(2.5f, 4.5f, t);
            t += 2f * Time.deltaTime;
            Debug.Log("changing size");
        }
    }

    public void UIStartGame()
    {
        StartGameAnimation();
    }

    void Setup()
    {
        //introManager.GameStartSequence();
        uiManager.UpdateHighScores(PlayerPrefs.GetInt("HighScore"), PlayerPrefs.GetInt("WaveHighScore"));
        soundManager = FindObjectOfType<SoundManager>();
        titleScreenMusic = soundManager.Play("TitleScreenMusic", true);
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

    void StartGameAnimation()
    {

        soundManager.StopPlaying(titleScreenMusic);
        maintheme = soundManager.Play("MainMusic", true);
        
        player.StartGameAnimation();
        FindObjectOfType<WorkerIntroAnimScript>().GameStarted();
    }

    public void StartGame()
    {


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
        if (waveScore > PlayerPrefs.GetInt("WaveHighScore"))
        {
            PlayerPrefs.SetInt("WaveHighScore", waveScore);
            uiManager.UpdateHighScores(PlayerPrefs.GetInt("HighScore"), PlayerPrefs.GetInt("WaveHighScore"));
        }

        if(waveScore == room2WaveNumber)
        {
            sector2Barrier.enabled = false;
            uiManager.ActivateSectorTextAlert();
        }

        if (waveScore == room3WaveNumber)
        {
            sector3Barrier.enabled = false;
            uiManager.ActivateSectorTextAlert();
            TeleporterBackDoor[] teleporters = FindObjectsOfType<TeleporterBackDoor>();
            foreach (TeleporterBackDoor door in teleporters)
            {
                door.ActivateDoors();
            }
        }
    }

    public void AddKillScore()
    {
        soundManager.Play("EnemyDeath");
        killScore++;
        uiManager.UpdateKillsScoreUI(killScore);
        if (killScore > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", killScore);
            uiManager.UpdateHighScores(PlayerPrefs.GetInt("HighScore"), PlayerPrefs.GetInt("WaveHighScore"));
        }
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

    public void PlayButtonSound()
    {
        soundManager.Play("ButtonClick");
    }

}
