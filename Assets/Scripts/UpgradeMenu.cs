using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
    private _PlayerAttack player;
    
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        uIManager = FindObjectOfType<UIManager>();
        player = FindObjectOfType<_PlayerAttack>();
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
            player.toiletPaperDamage += 2;
        }
        if (StatisticIndex == 1)
        {
            float ProjectileSpeed = player.projectileSpeed;
            if (ProjectileSpeed <= 13)
            {
                player.projectileSpeed += 1;
            }
            if (ProjectileSpeed <= 17)
            {
                player.projectileSpeed += 0.5f;
            }
            if (ProjectileSpeed <= 20)
            {
                player.projectileSpeed += 0.25f;
            }
                
                
                
            /*if (FindObjectOfType<PlayerAttack>().attackDelay >= 0.3)
            {
                FindObjectOfType<PlayerAttack>().attackDelay -= 0.05f;
            }
            if (FindObjectOfType<PlayerAttack>().attackDelay >= 0.1f)
            {
                FindObjectOfType<PlayerAttack>().attackDelay -= 0.025f;
            }
            if (FindObjectOfType<PlayerAttack>().attackDelay <= 0.1f)
            {
            }*/

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
                uIManager.UpdateHealthStat(FindObjectOfType<PlayerController>().health += 5);
            }
        }
    }

    void GiveAbility(int AbilityIndex)
    {
        _PlayerAttack.AbilityTypes ability = _PlayerAttack.AbilityTypes.None;
        switch (AbilityIndex)
        {
            case(0):
                ability = _PlayerAttack.AbilityTypes.ScreamingStun;
                break;
            case(1):
                ability = _PlayerAttack.AbilityTypes.HighHeelRun;
                break;
            case(2):
                ability = _PlayerAttack.AbilityTypes.BananaPeelLandmine;
                break;
            case(3):
                ability = _PlayerAttack.AbilityTypes.PiercingToiletPaperAbility;
                break;
        }
        player.ChangeAbility(ability);
        Debug.LogWarning("Chosen ability = " + ability.ToString());
    }

    void UpgradeAbility()
    {
        if (player.abilityLevel == _PlayerAttack.AbilityUpgrades.level3)
        {
            return;
        }
        else
        {
            player.abilityLevel++;
        }
    }
}
