using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseOverlay;
    public GameObject deathOverlay;
    public GameObject inGameOverlay;
    public GameObject PreGameOverlay;
    public GameObject SettingsOverlay;

    [Header("Upgrade Menu References")]
    public GameObject UpgradeMenu;
    public GameObject UpgradeButtonOne;
    public GameObject UpgradeButtonTwo;
    public GameObject UpgradeButtonThree;
    public Sprite[] UpgradeOneIcons;
    public Sprite[] UpgradeTwoIcons;

    public Sprite[] UpgradeOneIcons_Special;
    public Sprite[] UpgradeTwoIcons_Special;
    public Sprite[] UpgradeThreeIcons_Special;

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
    public void UpdateMaxHealth(float maxHealth)
    {
        if (healthBar == null)
        {
            healthBar = FindObjectOfType<Slider>();
        }
        healthBar.maxValue = maxHealth;
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

    public void OpenSetingsMenu()
    {
        SettingsOverlay.SetActive(true);
    }

    public void CloseSetingsMenu()
    {
        SettingsOverlay.SetActive(false);
    }

    public void OpenUpgradeMenuOne(int UpgradeOneIndex, int UpgradeTwoIndex)
    {
        UpgradeMenu.SetActive(true);
        UpgradeButtonOne.SetActive(true);
        //UpgradeButtonOne.GetComponentInChildren<SpriteRenderer>().sprite = UpgradeOneIcons[UpgradeOneIndex];
        UpgradeButtonTwo.SetActive(true);
        //UpgradeButtonTwo.GetComponentInChildren<SpriteRenderer>().sprite = UpgradeTwoIcons[UpgradeTwoIndex];
        UpgradeButtonThree.SetActive(false);
    }

    public void OpenUpgradeMenuTwo(int UpgradeOneIndex, int UpgradeTwoIndex, int UpgradeThreeIndex)
    {
        UpgradeMenu.SetActive(true);
        UpgradeButtonOne.SetActive(true);
        //UpgradeButtonOne.GetComponentInChildren<SpriteRenderer>().sprite = UpgradeOneIcons_Special[UpgradeOneIndex];
        UpgradeButtonTwo.SetActive(true);
        //UpgradeButtonTwo.GetComponentInChildren<SpriteRenderer>().sprite = UpgradeTwoIcons_Special[UpgradeTwoIndex];
        UpgradeButtonThree.SetActive(true);
        //UpgradeButtonThree.GetComponentInChildren<SpriteRenderer>().sprite = UpgradeThreeIcons_Special[UpgradeThreeIndex];
    }

    public void UpgradeOneChosen()
    {

        FindObjectOfType<UpgradeMenu>().UpgradeChosen(1);
        UpgradeMenu.SetActive(false);
    }

    public void UpgradeTwoChosen()
    {
        FindObjectOfType<UpgradeMenu>().UpgradeChosen(2);
        UpgradeMenu.SetActive(false);
    }

    public void UpgradeThreeChosen()
    {
        FindObjectOfType<UpgradeMenu>().UpgradeChosen(3);
        UpgradeMenu.SetActive(false);
    }



}
