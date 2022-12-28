using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pauseOverlay;
    public GameObject deathOverlay;
    public GameObject inGameOverlay;

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
        healthBar.value = healthBar.maxValue;
    }

    public void UpdateHealthStat(float health)
    {
        healthBar.value = health;
    }
}
