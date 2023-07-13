using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Game Variables
    int waveScore = 0;
    int killScore = 0;
    float cameraSize;
    static float t = 0.0f;
    private bool adWatched = false;
    [SerializeField] private Button adButton;


    // Game Properties
    bool gamePaused = false;
    bool gameStarted = false;
    bool softPauseActive = false;

    // Game Objects
    [Header("References")]
    public TestSpawner waveManager;
    public UIManager uiManager;
    PlayerController player;
    SoundManager soundManager;
    AudioSource titleScreenMusic;
    AudioSource maintheme;
    [SerializeField] private GameObject startgamebutton;

    [Header("Sector Barriers")]
    [SerializeField] private BoxCollider2D sector2Barrier;
    [SerializeField] private BoxCollider2D sector3Barrier;
    [SerializeField] private Animator Sector2Anim;
    [SerializeField] private Animator Sector3Anim;
    [SerializeField] private int room2WaveNumber;
    [SerializeField] private int room3WaveNumber;

    // wave timer
    bool timerActivated;
    float timer;

    

    private void Awake()
    {
        cameraSize = FindObjectOfType<Camera>().orthographicSize;
        soundManager = FindObjectOfType<SoundManager>();
        player = FindObjectOfType<PlayerController>();
        Application.targetFrameRate = 60;
    }

    void Start()
    {

        Setup();
    }

    void Update()
    {
        // starts the game
        /*if (Input.GetKeyDown(KeyCode.Space) && !gameStarted)
        {
            StartGame();
        }*/

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
        }
    }

    public void UIStartGame()
    {
        startgamebutton.SetActive(false);
        StartGameAnimation();
        
    }

    void Setup()
    {
        //introManager.GameStartSequence();
        uiManager.UpdateHighScores(PlayerPrefs.GetInt("HighScore"), PlayerPrefs.GetInt("WaveHighScore"));
        
        titleScreenMusic = soundManager.Play("TitleScreenMusic", true);
        
        
        FindObjectOfType<Camera>().orthographicSize = 2.5f;

        adButton.interactable = true;
        adWatched = false;
        adButton.GetComponent<RewardedAdsButton>().LoadAd();
        gamePaused = false;
        gameStarted = false;
        player.gameStarted = false;
        //waveManager.enabled = false;
        waveManager.gamePaused = gamePaused;
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
        uiManager.DisablePreGameOverlay();
        player.StartGameAnimation();
        FindObjectOfType<WorkerIntroAnimScript>().GameStarted();
    }

    public void StartGame()
    {


        gameStarted = true;
        player.gameStarted = true;
        //waveManager.enabled = true;
        uiManager.EnableInGameOverlay();
        
        uiManager.UpdateWaveScoreUI(waveScore);
        uiManager.UpdateKillsScoreUI(killScore);
        AddWaveScore();
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
            Sector2Anim.Play("Explosion");
            uiManager.ActivateSectorTextAlert();
            var graphToScan = AstarPath.active.data.gridGraph;
            AstarPath.active.Scan(graphToScan);
            waveManager.InitialisePlayArea();
        }

        if (waveScore == room3WaveNumber)
        {
            sector3Barrier.enabled = false;
            Sector3Anim.Play("Explosion");
            uiManager.ActivateSectorTextAlert();
            TeleporterBackDoor[] teleporters = FindObjectsOfType<TeleporterBackDoor>();
            foreach (TeleporterBackDoor door in teleporters)
            {
                door.ActivateDoors();
            }
            var graphToScan = AstarPath.active.data.gridGraph;
            AstarPath.active.Scan(graphToScan);
            waveManager.InitialisePlayArea();
        }
        
        waveManager.StartNextWave(waveScore);
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

    public void PauseGame()
    {
        gamePaused = true;

        if(softPauseActive == false)
        {
            waveManager.gamePaused = gamePaused;
            player.isPaused = gamePaused;
            Time.timeScale = 0;
        }
            
        uiManager.TogglePauseOverlay(gamePaused);
    }

    public void UnPauseGame()
    {
        gamePaused = false;

        if(softPauseActive == false)
        {
            waveManager.gamePaused = gamePaused;
            player.isPaused = gamePaused;
            Time.timeScale = 1;
        }
            
        uiManager.TogglePauseOverlay(gamePaused); 
    }
    
    /*public void TogglePauseGame()
    {
        
        
        if (gamePaused == true)
        {
            
        }
        else if (gamePaused == false)
        {
           
        }
        else
        {
            Debug.Log("Pause code broken");
        }
    }*/

    public void UIUnpause()
    {
        UnPauseGame();
    }

    public void SoftPause()
    {
        softPauseActive = true;
        waveManager.gamePaused = true;
        player.isPaused = true;
        Time.timeScale = 0;
    }

    public void SoftUnPause()
    {
        softPauseActive = false;
        waveManager.gamePaused = false;
        player.isPaused = false;
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PlayerDied()
    {
        gamePaused = true;
        waveManager.gamePaused = gamePaused;
        uiManager.ActivateDeathOverlay();
        player.isPaused = gamePaused;
        Time.timeScale = 0;
        adButton.interactable = !adWatched;
    }

    public void AdRevivePlayer()
    {
        adWatched = true;
        adButton.interactable = !adWatched;
        gamePaused = false;
        waveManager.gamePaused = gamePaused;
        uiManager.deathOverlay.SetActive(false);
        player.isPaused = gamePaused;
        Time.timeScale = 1;
        player.health = player.maxHealth;
        uiManager.UpdateHealthStat(player.health);
        player.TemporaryInvincibility(4);
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
        Invoke("AddWaveScore", time);
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
