using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public GameObject pauseOverlay;
    public GameObject deathOverlay;
    public GameObject inGameOverlay;
    public GameObject PreGameOverlay;

    // Component References
    Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        healthBar = FindObjectOfType<Slider>();
        ResetUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TogglePauseOverlay(bool gamePaused)
    {
        pauseOverlay.SetActive(gamePaused);
    }

    public void ActivateDeathOverlay()
    {
        deathOverlay.SetActive(true);
    }

    public void ResetUI()
    {
        // In game overlay
        if (healthBar == null)
        {
            healthBar = FindObjectOfType<Slider>();
        }
        if (healthBar != null)
        {
            healthBar.value = healthBar.maxValue;
        }
    }

    public void UpdateHealthStat(float health)
    {
        if (healthBar == null)
        {
            healthBar = FindObjectOfType<Slider>();
        }
        healthBar.value = health;
    }

    public void EnableInGameOverlay()
    {
        inGameOverlay.SetActive(true);
    }

    public void DisableInGameOverlay()
    {
        inGameOverlay.SetActive(false);
    }

    public void EnablePreGameOverlay()
    {
        PreGameOverlay.SetActive(true);
    }

    public void DisablePreGameOverlay()
    {
        PreGameOverlay.SetActive(false);
    }

    public void UpdateKillsScoreUI(int kills)
    {
        inGameOverlay.transform.GetChild(1).GetComponent<TMP_Text>().text = "Score: " + kills;
    }

    public void UpdateWaveScoreUI(int wave)
    {
        inGameOverlay.transform.GetChild(2).GetComponent<TMP_Text>().text = "Wave: " + wave;
    }

    public void EnableTimerUI()
    {
        inGameOverlay.transform.GetChild(3).gameObject.SetActive(true);
    }

    public void DisableTimerUI()
    {
        inGameOverlay.transform.GetChild(3).gameObject.SetActive(false);
    }

    public void UpdateTimerUI(float time)
    {
        inGameOverlay.transform.GetChild(3).GetComponent<TMP_Text>().text = "Next Wave In " + time.ToString("0");

    }
}
