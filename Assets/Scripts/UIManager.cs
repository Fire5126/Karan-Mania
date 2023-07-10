using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.PlayerLoop;

public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseOverlay;
    public GameObject deathOverlay;
    public GameObject inGameOverlay;
    public GameObject PreGameOverlay;
    public GameObject SettingsOverlay;
    public Slider MusicVolSlider;
    public Slider SFXVolSlider;
    public Slider MusicVolSlider2;
    public Slider SFXVolSlider2;
    public TMP_Text HighScoreText;
    public TMP_Text HighestWaveText;
    public GameObject SectorOpenAlertText;
    public GameObject SkinSelectionMenu;

    [Header("Upgrade Menu References")]
    public GameObject UpgradeMenu;
    public GameObject UpgradeButtonOne;
    public GameObject UpgradeButtonTwo;
    public GameObject UpgradeButtonThree;
    public GameObject UpgradeButtonThree_GFX;
    public Sprite[] UpgradeOneIcons;
    public Sprite[] UpgradeTwoIcons;
    public Sprite[] UpgradeThreeIcons_Special;
    public Sprite UpgradeAbilityIcon;
    public GameObject AbilityButton;

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

    public void UpdateHighScores(int HighScore, int WaveHighScore)
    {
        HighestWaveText.text = "Highest Wave: " + WaveHighScore;
        HighScoreText.text = "High Score: " + HighScore;
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
        UpgradeButtonOne.GetComponent<Image>().sprite = UpgradeOneIcons[UpgradeOneIndex];
        UpgradeButtonTwo.SetActive(true);
        UpgradeButtonTwo.GetComponent<Image>().sprite = UpgradeTwoIcons[UpgradeTwoIndex];
        UpgradeButtonThree.SetActive(false);
        UpgradeButtonThree_GFX.SetActive(false);
    }

    public void OpenUpgradeMenuTwo(int UpgradeTwoIndex, int UpgradeThreeIndex)
    {
        UpgradeMenu.SetActive(true);
        UpgradeButtonOne.SetActive(true);
        UpgradeButtonOne.GetComponent<Image>().sprite = UpgradeAbilityIcon;
        UpgradeButtonTwo.SetActive(true);
        UpgradeButtonTwo.GetComponent<Image>().sprite = UpgradeTwoIcons[UpgradeTwoIndex];
        UpgradeButtonThree.SetActive(true);
        UpgradeButtonThree_GFX.SetActive(true);
        UpgradeButtonThree.GetComponent<Image>().sprite = UpgradeThreeIcons_Special[UpgradeThreeIndex];
    }

    public void OpenUpgradeMenuThree(int UpgradeOneIndex, int UpgradeTwoIndex, int UpgradeThreeIndex)
    {
        UpgradeMenu.SetActive(true);
        UpgradeButtonOne.SetActive(true);
        UpgradeButtonOne.GetComponent<Image>().sprite = UpgradeOneIcons[UpgradeOneIndex];
        UpgradeButtonTwo.SetActive(true);
        UpgradeButtonTwo.GetComponent<Image>().sprite = UpgradeTwoIcons[UpgradeTwoIndex];
        UpgradeButtonThree.SetActive(true);
        UpgradeButtonThree_GFX.SetActive(true);
        UpgradeButtonThree.GetComponent<Image>().sprite = UpgradeThreeIcons_Special[UpgradeThreeIndex];
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

    public void ActivateAbilityButton()
    {
        AbilityButton.SetActive(true);
    }

    public void SFXVolUpdate(float sliderValue)
    {
        FindObjectOfType<SoundManager>().UpdateSFXVolume(sliderValue);
        
    }

    public void MusicVolUpdate(float sliderValue)
    {
        Debug.Log(sliderValue);
        FindObjectOfType<SoundManager>().UpdateMusicVolume(sliderValue);
    }

    public void ActivateSectorTextAlert()
    {
        SectorOpenAlertText.SetActive(true);
        SectorOpenAlertText.GetComponent<Animator>().Play("AlertAnim");
    }

    public void DisableSectorTextAlert()
    {
        SectorOpenAlertText.SetActive(false);
    }
    
    public void ActivateSkinSelectionMenu()
    {
        SkinSelectionMenu.SetActive(true);
    }
    
    public void DisableSkinSelectionMenu()
    {
        SkinSelectionMenu.SetActive(false);
    }

}
