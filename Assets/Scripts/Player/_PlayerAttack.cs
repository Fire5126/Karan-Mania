using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Pathfinding;
using UnityEngine;

public class _PlayerAttack : MonoBehaviour
{
    //////////////////////////////////////////////////////////////
    ///                                                        ///
    ///                       Variables                        ///
    ///                                                        ///
    //////////////////////////////////////////////////////////////
    
    [Header("References")] 
    public Joystick joystick;
    private PlayerController player;
    private SoundManager soundManager;

    [Header("Toilet Paper Settings")]
    [SerializeField] private Transform projectileSpawnLocation;
    [SerializeField] private GameObject toiletPaper;
    public float toiletPaperDamage = 5f;
    public float toiletPaperDuration = 5f;
    public float projectileSpeed = 8f;
    [Space(10)] 
    [SerializeField] private float attackDelay = 0.5f;
    private float nextAttackTime;
    private bool attackDisabled = false;

    ////////////////////////////////////
    ///       Ability Variables      ///
    ////////////////////////////////////

    [Header("Ability Variables")]
    public bool hasAbility = false;
    public AbilityUpgrades abilityLevel = AbilityUpgrades.level1;
    public AbilityTypes CurrentAbility = AbilityTypes.None;
    public enum AbilityUpgrades
    {
        level1 = 0,
        level2 = 1,
        level3 = 2
    }
    public enum AbilityTypes
    {
        None = -1,
        ScreamingStun = 0,
        HighHeelRun = 1,
        BananaPeelLandmine = 2,
        PiercingToiletPaperAbility = 3
    }
    private float abilityCooldownDelay = 10f;
    private float nextAbilityTime;
    
    [Header("Screaming Stun Ability")]
    [SerializeField] private float stunTime_Level1;
    [SerializeField] private float stunTime_Level2;
    [SerializeField] private float stunTime_Level3;
    private float stunTime; // variable for actual stun time
    [SerializeField] private GameObject StunGameObject;
    [SerializeField] private float affectedRadius = 3f;
    private Collider2D[] hitColliders; // used to check which enemies are hit
    [SerializeField] private float abilityCooldown_ScreamingStunAbility;

    [Header("High Heel Run Ability")] 
    [SerializeField] private float speedIncrease;
    private float movementSpeedBackup;
    private float abilityDuration_HighHeelRun;
    [SerializeField] private float abilityDuration_HighHeelRun_Lvl1;
    [SerializeField] private float abilityDuration_HighHeelRun_Lvl2;
    [SerializeField] private float abilityDuration_HighHeelRun_Lvl3;
    [SerializeField] private float abilityCooldown_HighHeelRun;
    
    [Header("Banana Peel Land Mine Ability")] 
    [SerializeField] private GameObject BananaPeelLandMinePrefab;
    [SerializeField] private float landMineAffectedRadius = 4f;
    private float abilityCooldown_BananaPeelLandMine;
    [SerializeField] private float abilityCooldown_BananaPeelLandMine_Lvl1;
    [SerializeField] private float abilityCooldown_BananaPeelLandMine_Lvl2;
    [SerializeField] private float abilityCooldown_BananaPeelLandMine_lvl3;
    [SerializeField] private float damage_BananaPeelLandMine;
    [SerializeField] private Vector2 landMineSpawnOffset;
    [SerializeField] private float blastForceMultiplier;
    
    [Header("Piercing Toilet Paper Ability")]
    [SerializeField] private int numberOfEnemiesToPierceThrough_Lvl1;
    [SerializeField] private int numberOfEnemiesToPierceThrough_Lvl2;
    [SerializeField] private int numberOfEnemiesToPierceThrough_Lvl3;
    [SerializeField] private float abilityCooldown_PiercingToiletPaper;
    [SerializeField] private float abilityDuration_PiercingToiletPaper;
    private int ability_NumberOfEnemiesToPierceThrough; // the ability variable
    private int numberOfEnemiesToPierceThrough; // used in actually spawning the Toilet Paper
    
    
    ////////////////////////////////////
    ///   Attack Upgrade Variables   ///
    ////////////////////////////////////
    
    [HideInInspector] public enum ProjectileTypes
    {
        Single,
        Double,
        Triple
    }
    private ProjectileTypes currentAttackType;
    private bool attackUpgradeEnabled = false;
    private float upgradeTimeLeft;

    //////////////////////////////////////////////////////////////
    ///                                                        ///
    ///                        Funtions                        ///
    ///                                                        ///
    //////////////////////////////////////////////////////////////
    
    
    private void Awake()
    {
        player = GetComponent<PlayerController>();
        soundManager = FindObjectOfType<SoundManager>();
    }
    private void Update()
    {
        // Makes sure player is not paused or that the game hasn't started
        if (player.isPaused || !player.gameStarted || attackDisabled) return;
        
        RotateFirePosition();
        
        //Checks if the player is trying to shoot and is able to shoot
        if (joystick.Direction.magnitude > 0 && nextAttackTime <= Time.time)
        {
            // resets shoot delay
            nextAttackTime = Time.time + attackDelay;
            ShootProjectile();
        }
        
        // Sets attack magnitude for animations
        player.attackMagnitude = joystick.Direction.magnitude;

        // Timer and resetter for attack type upgrade
        if (!attackUpgradeEnabled) return;
        upgradeTimeLeft -= Time.deltaTime;
        if (upgradeTimeLeft <= 0)
        {
            currentAttackType = ProjectileTypes.Single;
        }
    }
    private void RotateFirePosition()
    {
        //Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector3 lookAt = mouseScreenPosition;
        //float AngleRad = Mathf.Atan2(lookAt.y - this.transform.position.y, lookAt.x - this.transform.position.x);
        //float AngleDeg = (180 / Mathf.PI) * AngleRad;
        //gameObject.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, AngleDeg);

        float AngleRad = Mathf.Atan2(joystick.Vertical, joystick.Horizontal);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        gameObject.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }

    ////////////////////////////////////
    ///     Projectile Functions     ///
    ////////////////////////////////////
    public void ShootProjectile()
    {
        InitiateProjectileShot(currentAttackType);
    }
    private void InitiateProjectileShot(ProjectileTypes attackType)
    {
        // Initiates variables
        Quaternion rotation;
        Quaternion rotation2;
        Quaternion rotation3;
        float zrot;
        
        switch (attackType)
        {
            
            
            // Shoots single toilet paper
            case ProjectileTypes.Single:
                
                rotation = projectileSpawnLocation.rotation;
                Instantiate<GameObject>(toiletPaper, 
                    projectileSpawnLocation.position, 
                    rotation).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough);
                break;
            
            // Shoots two toilet papers
            case ProjectileTypes.Double:
                
                // Sets Variables
                zrot = projectileSpawnLocation.rotation.eulerAngles.z + 10;
                rotation = Quaternion.Euler(0, 0, zrot);
                zrot = projectileSpawnLocation.rotation.eulerAngles.z - 10;
                rotation2 = Quaternion.Euler(0, 0, zrot);
                
                //Spawns first TP
                Instantiate<GameObject>(toiletPaper, 
                    projectileSpawnLocation.position, 
                    rotation).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough);
                
                //Spawns second TP
                Instantiate<GameObject>(toiletPaper, 
                    projectileSpawnLocation.position, 
                    rotation2).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough); ;
                
                break;
            
            // Shoots three toilet papers
            case ProjectileTypes.Triple:
                
                // Sets Variables
                zrot = projectileSpawnLocation.rotation.eulerAngles.z + 20;
                rotation = Quaternion.Euler(0, 0, zrot);
                zrot = projectileSpawnLocation.rotation.eulerAngles.z - 20;
                rotation2 = Quaternion.Euler(0, 0, zrot);
                
                // Spawns first TP
                Instantiate<GameObject>(toiletPaper, 
                    projectileSpawnLocation.position, 
                    rotation).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough);
                
                // Spawns Second TP
                Instantiate<GameObject>(toiletPaper, 
                    projectileSpawnLocation.position,
                    rotation2).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough);
                
                // Spawns Third TP
                rotation3 = projectileSpawnLocation.rotation;
                Instantiate<GameObject>(toiletPaper, 
                    projectileSpawnLocation.position, 
                    rotation3).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough);
                
                break;
        }
    }
    // sets the projectile type for a specified time (for obtainable upgrade)
    public void SetProjectileType(ProjectileTypes attackType, float time)
    {
        currentAttackType = attackType;
        upgradeTimeLeft = Time.time + time;
        attackUpgradeEnabled = true;
    }
    public void EnemyHit(Collider2D enemy)
    {
        enemy.gameObject.GetComponent<Enemy>().UpdateHealth(toiletPaperDamage);
    }
    
    ////////////////////////////////////
    ///      Animation Functions     ///
    ////////////////////////////////////
    public void StartGameThrowAnimation()
    {
        float vel = projectileSpeed;
        projectileSpeed = 3;
        ShootProjectile();
        player.animator.Play("Throwing");
        projectileSpeed = vel;
    }

    //////////////////////////////////////////////////////////////
    ///                     Ability Functions                  ///
    //////////////////////////////////////////////////////////////
    
    ////////////////////////////////////
    ///      Ability Activation      ///
    ////////////////////////////////////
    public void ActivateAbility()
    {
        if (Time.time > nextAbilityTime && hasAbility)
        {
            nextAbilityTime = Time.time + abilityCooldownDelay;
            switch (CurrentAbility)
            {
                case(AbilityTypes.None):
                    Debug.LogError("No ability selected!") ;
                    break;
                case(AbilityTypes.ScreamingStun):
                    ScreamingStun(abilityLevel);
                    break;
                case(AbilityTypes.BananaPeelLandmine):
                    BananaPeelLandMine(abilityLevel);
                    break;
                case(AbilityTypes.HighHeelRun):
                    HighHeelRun(abilityLevel);
                    break;
                case(AbilityTypes.PiercingToiletPaperAbility):
                    PiercingToiletPaper(abilityLevel);
                    break;
            }
        }
    }
    public void ChangeAbility(AbilityTypes ability)
    {
        hasAbility = true;
        CurrentAbility = ability;
        abilityLevel = AbilityUpgrades.level1;
    }
    
    
    ////////////////////////////////////
    ///         Screaming Stun       ///
    ////////////////////////////////////
    private void ScreamingStun(AbilityUpgrades upgradeLevel)
    {
        // setting upgrade variables
        switch (upgradeLevel)
        {
            case(AbilityUpgrades.level1):
                stunTime = stunTime_Level1;
                break;
            case(AbilityUpgrades.level2):
                stunTime = stunTime_Level2;
                break;
            case(AbilityUpgrades.level3):
                stunTime = stunTime_Level3;
                break;
        }
        
        // stun code
        StunGameObject.SetActive(true);
        StunGameObject.GetComponent<Animator>().enabled = true;
        Invoke("DisableScreamObject", 0.8f);
        abilityCooldownDelay = abilityCooldown_ScreamingStunAbility;
        hitColliders = Physics2D.OverlapCircleAll(this.transform.position, affectedRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy")
            {
                hitCollider.GetComponent<Enemy>().damage = 0;
                hitCollider.GetComponent<AIPath>().maxSpeed = 0;
            }
        }
        Invoke("ScreamAttackUnstunEnemy", stunTime);
    }
    // Unstuns the enemy after scream stun ability
    private void ScreamAttackUnstunEnemy()
    {
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                hitCollider.GetComponent<Enemy>().ResetStunStats();
            }
        }
    }
    private void DisableScreamObject()
    {
        StunGameObject.GetComponent<Animator>().enabled = false;
        StunGameObject.SetActive(false);

    }
    
    
    ////////////////////////////////////
    ///         High Heel Run        ///
    ////////////////////////////////////
    private void HighHeelRun(AbilityUpgrades upgradeLevel)
    {
        // setting upgrade variables
        switch (upgradeLevel)
        {
            case(AbilityUpgrades.level1):
                abilityDuration_HighHeelRun = abilityDuration_HighHeelRun_Lvl1;
                break;
            case(AbilityUpgrades.level2):
                abilityDuration_HighHeelRun = abilityDuration_HighHeelRun_Lvl2;
                break;
            case(AbilityUpgrades.level3):
                abilityDuration_HighHeelRun = abilityDuration_HighHeelRun_Lvl3;
                break;
        }
        
        // enabling ability through setting variables
        abilityCooldownDelay = abilityCooldown_HighHeelRun;
        movementSpeedBackup = player.movementSpeed;
        player.movementSpeed += speedIncrease;
        attackDisabled = true;
        player.damageDisabled = true;
        Invoke("DisableHighHeelRun", abilityDuration_HighHeelRun); // invokes the end of the ability
    }
    private void DisableHighHeelRun()
    {
        // disables high heel run
        attackDisabled = false;
        player.damageDisabled = false;
        player.movementSpeed = movementSpeedBackup;
    }
    
    
    ////////////////////////////////////
    ///     Banana Peel Landmine     ///
    ////////////////////////////////////
    private void BananaPeelLandMine(AbilityUpgrades upgradeLevel)
    {
        // setting upgrade variables
        switch (upgradeLevel)
        {
            case(AbilityUpgrades.level1):
                abilityCooldown_BananaPeelLandMine = abilityCooldown_BananaPeelLandMine_Lvl1;
                break;
            case(AbilityUpgrades.level2):
                abilityCooldown_BananaPeelLandMine = abilityCooldown_BananaPeelLandMine_Lvl2;
                break;
            case(AbilityUpgrades.level3):
                abilityCooldown_BananaPeelLandMine = abilityCooldown_BananaPeelLandMine_lvl3;
                break;
        }
        
        // Spawning the landmine and setting variables
        abilityCooldownDelay = abilityCooldown_BananaPeelLandMine;
        Vector2 spawnOffset = transform.position;
        spawnOffset = spawnOffset + landMineSpawnOffset;
        GameObject landMineRef = Instantiate(BananaPeelLandMinePrefab, 
            spawnOffset, 
            Quaternion.identity);
        landMineRef.GetComponent<BananaPeelLandMine>().InitiateVariables(landMineAffectedRadius, 
            damage_BananaPeelLandMine, 
            blastForceMultiplier);
    }
    
    ////////////////////////////////////
    ///     Piercing Toilet Paper    ///
    ////////////////////////////////////
    private void PiercingToiletPaper(AbilityUpgrades abilityLevel)
    {
        switch (abilityLevel)
        {
            case(AbilityUpgrades.level1):
                ability_NumberOfEnemiesToPierceThrough = numberOfEnemiesToPierceThrough_Lvl1;
                break;
            case(AbilityUpgrades.level2):
                ability_NumberOfEnemiesToPierceThrough = numberOfEnemiesToPierceThrough_Lvl2;
                break;
            case(AbilityUpgrades.level3):
                ability_NumberOfEnemiesToPierceThrough = numberOfEnemiesToPierceThrough_Lvl3;
                break;
        }
        
        abilityCooldownDelay = abilityCooldown_PiercingToiletPaper;
        numberOfEnemiesToPierceThrough = ability_NumberOfEnemiesToPierceThrough;
        Invoke("EndPiercingToiletPaper", abilityDuration_PiercingToiletPaper);
    }
    private void EndPiercingToiletPaper()
    {
        numberOfEnemiesToPierceThrough = 0;
    }
}
