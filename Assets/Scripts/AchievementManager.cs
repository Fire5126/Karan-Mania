using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    private Animator KarenAnimator;
    private WorkerIntroAnimScript StartEmployee;
    private GameManager gameManager;
    public Achievement[] AchievementList;
    public Skin[] skinsList;

    void GiveAchievement(string name)
    {
        Achievement a = Array.Find(AchievementList, achievement => achievement.name == name);
        if (a == null)
        {
            Debug.Log("achievement not found");
            return;
        }
        a.Achieved = true;
    }

    private void Awake()
    {
        KarenAnimator = FindObjectOfType<PlayerController>().GetComponent<Animator>();
        StartEmployee = FindObjectOfType<WorkerIntroAnimScript>();
        gameManager = FindObjectOfType<GameManager>();
    }
    
    void Start()
    {
        int kills = PlayerPrefs.GetInt("HighScore");
        Debug.Log("Kills high score = " + kills);

        foreach (Achievement A in AchievementList)
        {
            if (!A.Achieved && kills >= A.kills)
            {
                A.Achieved = true;
                A.lockedOverlay.SetActive(false);
            }
        }

        int selectedSkin = PlayerPrefs.GetInt("SkinIndex");
        //int selectedSkin = 1;
        switch (selectedSkin)
        {
            default:
                SelectSkin1();
                break;
            case 0:
                SelectSkin1();
                break;
            case 1:
                SelectSkin2();
                break;
            case 2:
                SelectSkin3();
                break;
            case 3:
                SelectSkin4();
                break;
            case 4:
                SelectSkin5();
                break;
            case 5:
                SelectSkin6();
                break;
        }
    }

    public void SelectSkin1()
    {
        KarenAnimator.runtimeAnimatorController = null;
        KarenAnimator.GetComponent<SpriteRenderer>().sprite = skinsList[0].idleSprite;
        KarenAnimator.runtimeAnimatorController = skinsList[0].animController;
        KarenAnimator.Play("AngryKaren");
        //StartAnimEmployee.Play("ArgueState");
        StartEmployee.ResetAnimations();
        PlayerPrefs.SetInt("SkinIndex", 0);
    }

    public void SelectSkin2()
    {
        Debug.LogError("loaded skin");
        if (!AchievementList[0].Achieved) return;
        KarenAnimator.runtimeAnimatorController = null;
        KarenAnimator.GetComponent<SpriteRenderer>().sprite = skinsList[1].idleSprite;
        KarenAnimator.runtimeAnimatorController = skinsList[1].animController;
        Debug.LogWarning(KarenAnimator.runtimeAnimatorController.name);
        KarenAnimator.Play("AngryKaren");
        //StartAnimEmployee.Play("ArgueState");
        StartEmployee.ResetAnimations();
        PlayerPrefs.SetInt("SkinIndex", 1);
    }
    
    public void SelectSkin3()
    {
        if (!AchievementList[1].Achieved) return;
        KarenAnimator.runtimeAnimatorController = null;
        KarenAnimator.GetComponent<SpriteRenderer>().sprite = skinsList[2].idleSprite;
        KarenAnimator.runtimeAnimatorController = skinsList[2].animController;
        KarenAnimator.Play("AngryKaren");
        //StartAnimEmployee.Play("ArgueState");
        StartEmployee.ResetAnimations();
        PlayerPrefs.SetInt("SkinIndex", 2);
    }
    
    public void SelectSkin4()
    {
        if (!AchievementList[2].Achieved) return;
        KarenAnimator.runtimeAnimatorController = null;
        KarenAnimator.GetComponent<SpriteRenderer>().sprite = skinsList[3].idleSprite;
        KarenAnimator.runtimeAnimatorController = skinsList[3].animController;
        KarenAnimator.Play("AngryKaren");
        //StartAnimEmployee.Play("ArgueState");
        StartEmployee.ResetAnimations();
        PlayerPrefs.SetInt("SkinIndex", 3);
    }
    
    public void SelectSkin5()
    {
        if (!AchievementList[3].Achieved) return;
        KarenAnimator.runtimeAnimatorController = null;
        KarenAnimator.GetComponent<SpriteRenderer>().sprite = skinsList[4].idleSprite;
        KarenAnimator.runtimeAnimatorController = skinsList[4].animController;
        KarenAnimator.Play("AngryKaren");
        //StartAnimEmployee.Play("ArgueState");
        StartEmployee.ResetAnimations();
        PlayerPrefs.SetInt("SkinIndex", 4);
    }
    
    public void SelectSkin6()
    {
        if (!AchievementList[4].Achieved) return;
        KarenAnimator.runtimeAnimatorController = null;
        KarenAnimator.GetComponent<SpriteRenderer>().sprite = skinsList[5].idleSprite;
        KarenAnimator.runtimeAnimatorController = skinsList[5].animController;
        KarenAnimator.Play("AngryKaren");
        //StartAnimEmployee.Play("ArgueState");
        StartEmployee.ResetAnimations();
        PlayerPrefs.SetInt("SkinIndex", 5);
    }
    
    
}

[System.Serializable]
public class Skin
{
    public Sprite idleSprite;
    public RuntimeAnimatorController animController;
}

[System.Serializable]
public class Achievement
{
    public bool Achieved;
    public string name;
    public int kills;
    public GameObject lockedOverlay;
}
