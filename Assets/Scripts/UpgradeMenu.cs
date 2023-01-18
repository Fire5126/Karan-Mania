using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    UIManager uIManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        uIManager = FindObjectOfType<UIManager>();
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
            //print(UpgradeTwoOptions.Length);
            uIManager.OpenUpgradeMenuOne(upgradeOneIndex, upgradeTwoIndex);
            print("Upgrade One Chosen");
            return;
        }


        if(RoundTypeIndex >= 3 && FirstAbilityChosen == false)
        {
            
            UpgradeTypeIndex = 1;
            upgradeOneIndex = Random.Range(0, UpgradeOneOptions.Length);
            upgradeTwoIndex = Random.Range(0, UpgradeTwoOptions.Length);
            upgradeThreeIndex = Random.Range(0, UpgradeThreeOptions.Length);
            uIManager.OpenUpgradeMenuThree(upgradeOneIndex, upgradeTwoIndex, upgradeThreeIndex);
            return;
        }

        if (RoundTypeIndex >= 3 && FirstAbilityChosen == true)
        {
            
            UpgradeTypeIndex = 2;
            upgradeTwoIndex = Random.Range(0, UpgradeTwoOptions.Length);
            upgradeThreeIndex = Random.Range(0, UpgradeThreeOptions.Length);
            uIManager.OpenUpgradeMenuTwo(upgradeTwoIndex, upgradeThreeIndex);
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
                if(FirstAbilityChosen == false)
                {
                    FirstAbilityChosen = true;
                    uIManager.ActivateAbilityButton();
                }
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
                if (FirstAbilityChosen == false)
                {
                    FirstAbilityChosen = true;
                    uIManager.ActivateAbilityButton();
                }
                GiveAbility(upgradeThreeIndex);
            }
        }

        gameManager.SoftUnPause();
    }

    void UpgradeProjectileStatistic(int StatisticIndex)
    {
        if (StatisticIndex == 0)
        {
            FindObjectOfType<PlayerAttack>().toiletPaperDamage += 2;
        }
        if (StatisticIndex == 1)
        {
            if (FindObjectOfType<PlayerAttack>().attackDelay >= 0.3)
            {
                FindObjectOfType<PlayerAttack>().attackDelay -= 0.1f;
            }
            if (FindObjectOfType<PlayerAttack>().attackDelay >= 0.1f)
            {
                FindObjectOfType<PlayerAttack>().attackDelay -= 0.05f;
            }
            if (FindObjectOfType<PlayerAttack>().attackDelay <= 0.1f)
            {
            }

        }
    }

    void HealthIncrease(int IncreaseTypeIndex)
    {
        if(IncreaseTypeIndex == 0)
        {
            uIManager.UpdateMaxHealth(FindObjectOfType<PlayerController>().maxHealth += 5);
        }
        if(IncreaseTypeIndex == 1)
        {
            if (FindObjectOfType <PlayerController>().health < FindObjectOfType<PlayerController>().maxHealth)
            {
                FindObjectOfType<PlayerController>().health += 5;
            }
        }
    }

    void GiveAbility(int AbilityIndex)
    {
        FindObjectOfType<PlayerAttack>().ChangeAbility(AbilityIndex);
    }

    void UpgradeAbility()
    {
        // do something
    }
}
