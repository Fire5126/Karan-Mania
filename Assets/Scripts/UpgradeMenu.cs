using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor.Animations;

public class UpgradeMenu : MonoBehaviour
{
    public Button UpgradeOne;
    public Button UpgradeTwo;
    public Button UpgradeThree;

    int UpgradeTypeIndex = 0;
    bool FirstAbilityChosen = false;
    int upgradeOneIndex = 0;
    int upgradeTwoIndex = 0;
    int upgradeThreeIndex = 0;
    public string[] UpgradeOneOptions; // Projectile
    public string[] UpgradeTwoOptions; // Health
    public string[] UpgradeThreeOptions; // Ability


    GameManager gameManager;
    UIManager UIManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        UIManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OfferUpgrades(int RoundTypeIndex)
    {
        print(RoundTypeIndex);
        gameManager.SoftPause();
        if (RoundTypeIndex <= 2)
        {
            UpgradeTypeIndex = 0;
            
            upgradeOneIndex = Random.Range(0, UpgradeOneOptions.Length);
            upgradeTwoIndex = Random.Range(0, UpgradeTwoOptions.Length);
            UIManager.OpenUpgradeMenuOne(upgradeOneIndex, upgradeTwoIndex);
            print("Upgrade One Chosen");
            return;
        }


        if(RoundTypeIndex >= 3 && FirstAbilityChosen == false)
        {
            UpgradeTypeIndex = 1;
            upgradeOneIndex = Random.Range(0, UpgradeOneOptions.Length);
            upgradeTwoIndex = Random.Range(0, UpgradeTwoOptions.Length);
            upgradeThreeIndex = Random.Range(0, UpgradeThreeOptions.Length);
            return;
        }

        if (RoundTypeIndex >= 3 && FirstAbilityChosen == true)
        {
            UpgradeTypeIndex = 2;
            upgradeTwoIndex = Random.Range(0, UpgradeTwoOptions.Length);
            upgradeThreeIndex = Random.Range(0, UpgradeThreeOptions.Length);
            return;
        }
    }

    public void UpgradeChosen(int UpgradeIndex)
    {
        if (UpgradeTypeIndex == 0)
        {
            if (UpgradeIndex == 1)
            {
                UpgradeProjectileStatistic(upgradeOneIndex);
            }
            if (UpgradeIndex == 2)
            {
                HealthIncrease(upgradeTwoIndex);
            }
        }
        else if (UpgradeTypeIndex == 1)
        {
            if (UpgradeIndex == 1)
            {
                UpgradeProjectileStatistic(upgradeOneIndex);
            }
            if (UpgradeIndex == 2)
            {
                HealthIncrease(upgradeTwoIndex);
            }
            if (UpgradeIndex == 3)
            {
                GiveAbility(upgradeThreeIndex);
            }
        }
        else if (UpgradeTypeIndex == 2)
        {
            if (UpgradeIndex == 1)
            {
                UpgradeAbility();
            }
            if (UpgradeIndex == 2)
            {
                HealthIncrease(upgradeTwoIndex);
            }
            if (UpgradeIndex == 3)
            {
                GiveAbility(upgradeThreeIndex);
            }
        }

        gameManager.SoftUnPause();
    }

    void UpgradeProjectileStatistic(int StatisticIndex)
    {

    }

    void HealthIncrease(int IncreaseTypeIndex)
    {

    }

    void GiveAbility(int AbilityIndex)
    {

    }

    void UpgradeAbility()
    {

    }
}
