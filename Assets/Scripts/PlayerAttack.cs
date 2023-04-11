using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlayerAttack : MonoBehaviour
{
    // References
    [Header("References")]
    public Joystick joystick;
    PlayerController player;
    SoundManager soundManager;

    // Toilet Paper
    [Header("Toilet Paper")]
    public float joystickDeadZone;
    public GameObject toiletPaper;
    public float toiletPaperDamage = 5f;
    public float toiletPaperDuration = 5f;
    public float projectileSpeed = 10f;
    public int attackIndex = 0;
    public string[] attackTypes;
    bool attackDisabled = false;
    // Ability Cooldown
    [Header("Ability Variables")]
    public float abilityCooldownDelay;
    float nextAbility;
    public int abilityIndex = 0;
    public string[] abilityTypes;
    public bool hasAbility = false;
    [Range(1, 3)]
    public int AbilityLevel = 1;

    // Main Attack Timer
    [Header("Main Attack Timer")]
    public float attackDelay;
    float nextAttackTime;

    [Header("Screaming Stun Ability")]
    [SerializeField] private float abilityCooldown_ScreamingStunAbility;
    public float affectedRadius;
    private float stunTime;
    [SerializeField] private GameObject StunGameObject;
    [SerializeField] private float stunTime_Level1;
    [SerializeField] private float stunTime_Level2;
    [SerializeField] private float stunTime_Level3;
    Collider2D[] hitColliders;

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
    [SerializeField] private float landMineAffectedRadius;
    private float abilityCooldown_BananaPeelLandMine;
    [SerializeField] private float abilityCooldown_BananaPeelLandMine_Lvl1;
    [SerializeField] private float abilityCooldown_BananaPeelLandMine_Lvl2;
    [SerializeField] private float abilityCooldown_BananaPeelLandMine_lvl3;
    [SerializeField] private float damage_BananaPeelLandMine;
    public Vector2 landMineSpawnOffset;
    public float blastForceMultiplier;

    [Header("Piercing Toilet Paper Ability")]
    private int ability_NumberOfEnemiesToPierceThrough;
    private int numberOfEnemiesToPierceThrough;
    [SerializeField] private int numberOfEnemiesToPierceThrough_Lvl1;
    [SerializeField] private int numberOfEnemiesToPierceThrough_Lvl2;
    [SerializeField] private int numberOfEnemiesToPierceThrough_Lvl3;
    [SerializeField] private float abilityCooldown_PiercingToiletPaper;
    [SerializeField] private float abilityDuration_PiercingToiletPaper;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        soundManager = FindObjectOfType<SoundManager>();
    }

    void Update()
    {
        if (!player.isPaused && player.gameStarted == true)
        {
            // Player Attack
            //if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E)) && Time.time > nextAttackTime)
            //{
            //    nextAttackTime = Time.time + attackDelay;
            //    Invoke(attackTypes[attackIndex], 0);
            //}
            if (attackDisabled)
            {
                return;
            }

            if ((joystick.Vertical >= joystickDeadZone || joystick.Vertical <= -joystickDeadZone || joystick.Horizontal >= joystickDeadZone || joystick.Horizontal <= -joystickDeadZone) && Time.time > nextAttackTime)
            {
                
                nextAttackTime = Time.time + attackDelay;
                InvokeToiletPaperAttack();
            }
            player.attackMagnitude = new Vector2(joystick.Horizontal, joystick.Vertical).magnitude;

            // Player Ability
            /*if (Input.GetKeyDown(KeyCode.Mouse1))
            {

                Invoke(abilityTypes[abilityIndex], 0f);
            }*/
        }
    }

    public void StartGameThrowAnimation()
    {
        InvokeToiletPaperAttack();
        player.animator.Play("Throwing");
    }

    public void ActivateAbility()
    {
        if (Time.time > nextAbility && hasAbility)
        {
            nextAbility = Time.time + abilityCooldownDelay;
            Invoke(abilityTypes[abilityIndex], 0f);
        }
    }

    public void InvokeToiletPaperAttack()
    {
        
        Invoke(attackTypes[attackIndex], 0);
    }

    public void ChangeAbility(int abilityIndexInput)
    {
        hasAbility = true;
        abilityIndex = abilityIndexInput;
        AbilityLevel = 1;
    }

    private void FixedUpdate()
    {
        RotateFirePosition();
    }

    void RotateFirePosition()
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




    // Main ToiletPaper Attack
    public void ToiletPaper()
    {
        Quaternion rotation = transform.GetChild(1).transform.rotation;
        Instantiate<GameObject>(toiletPaper, transform.GetChild(1).transform.position, rotation).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough);
    }

    public void ToiletPaper(Quaternion rotation)
    {
        Instantiate<GameObject>(toiletPaper, transform.GetChild(1).transform.position, rotation).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough);
    }

    void DoubleToiletPaper()
    {
        float zrot = transform.GetChild(1).transform.rotation.eulerAngles.z + 10;
        Quaternion rotation = Quaternion.Euler(0, 0, zrot);
        zrot = transform.GetChild(1).transform.rotation.eulerAngles.z - 10;
        Quaternion rotation2 = Quaternion.Euler(0, 0, zrot);
        Instantiate<GameObject>(toiletPaper, transform.GetChild(1).transform.position, rotation).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough); ;
        Instantiate<GameObject>(toiletPaper, transform.GetChild(1).transform.position, rotation2).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough); ;
    }

    void TripleToiletPaper()
    {
        float zrot = transform.GetChild(1).transform.rotation.eulerAngles.z + 20;
        Quaternion rotation = Quaternion.Euler(0, 0, zrot);
        zrot = transform.GetChild(1).transform.rotation.eulerAngles.z - 20;
        Quaternion rotation2 = Quaternion.Euler(0, 0, zrot);
        Instantiate<GameObject>(toiletPaper, transform.GetChild(1).transform.position, rotation).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough); ;
        Instantiate<GameObject>(toiletPaper, transform.GetChild(1).transform.position, rotation2).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough); ;
        Quaternion rotation3 = transform.GetChild(1).transform.rotation;
        Instantiate<GameObject>(toiletPaper, transform.GetChild(1).transform.position, rotation3).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough); ;
    }

    public void EnemyHit(Collider2D enemy)
    {
        enemy.gameObject.GetComponent<Enemy>().UpdateHealth(toiletPaperDamage);
    }




    // Abilities
    void ScreamingStun()
    {
        if (AbilityLevel == 1)
        {
            stunTime = stunTime_Level1;
        }
        else if (AbilityLevel == 2)
        {
            stunTime = stunTime_Level2;
        }
        else if (AbilityLevel == 3)
        {
            stunTime = stunTime_Level3;
        }

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

    void DisableScreamObject()
    {
        StunGameObject.GetComponent<Animator>().enabled = false;
        StunGameObject.SetActive(false);

    }

    void ScreamAttackUnstunEnemy()
    {
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy")
            {
                hitCollider.GetComponent<Enemy>().ResetStunStats();
            }
        }
    }

    void HighHeelRun()
    {
        if (AbilityLevel == 1)
        {
            abilityDuration_HighHeelRun = abilityDuration_HighHeelRun_Lvl1;
        }
        else if (AbilityLevel == 2)
        {
            abilityDuration_HighHeelRun = abilityDuration_HighHeelRun_Lvl2;
        }
        else if (AbilityLevel == 3)
        {
            abilityDuration_HighHeelRun = abilityDuration_HighHeelRun_Lvl3;
        }
        abilityCooldownDelay = abilityCooldown_HighHeelRun;
        movementSpeedBackup = player.movementSpeed;
        player.movementSpeed += speedIncrease;
        attackDisabled = true;
        player.damageDisabled = true;
        Invoke("DisableHighHeelRun", abilityDuration_HighHeelRun);
    }

    void DisableHighHeelRun()
    {
        attackDisabled = false;
        player.damageDisabled = false;
        player.movementSpeed = movementSpeedBackup;
    }

    void BananaPeelLandMine()
    {
        if (AbilityLevel == 1)
        {
            abilityCooldown_BananaPeelLandMine = abilityCooldown_BananaPeelLandMine_Lvl1;
        }
        else if (AbilityLevel == 2)
        {
            abilityCooldown_BananaPeelLandMine = abilityCooldown_BananaPeelLandMine_Lvl2;
        }
        else if (AbilityLevel == 3)
        {
            abilityCooldown_BananaPeelLandMine = abilityCooldown_BananaPeelLandMine_lvl3;
        }
        abilityCooldownDelay = abilityCooldown_BananaPeelLandMine;
        Vector2 spawnOffset = transform.position;
        spawnOffset = spawnOffset + landMineSpawnOffset;
        GameObject landMineRef = Instantiate(BananaPeelLandMinePrefab, spawnOffset, Quaternion.identity);
        landMineRef.GetComponent<BananaPeelLandMine>().InitiateVariables(affectedRadius, damage_BananaPeelLandMine, blastForceMultiplier);
    }

    void PiercingToiletPaper()
    {
        if (AbilityLevel == 1)
        {
            ability_NumberOfEnemiesToPierceThrough = numberOfEnemiesToPierceThrough_Lvl1;
        }
        else if (AbilityLevel == 2)
        {
            ability_NumberOfEnemiesToPierceThrough = numberOfEnemiesToPierceThrough_Lvl2;
        }
        else if (AbilityLevel == 3)
        {
            ability_NumberOfEnemiesToPierceThrough = numberOfEnemiesToPierceThrough_Lvl3;
        }
        abilityCooldownDelay = abilityCooldown_PiercingToiletPaper;
        numberOfEnemiesToPierceThrough = ability_NumberOfEnemiesToPierceThrough;
        Invoke("EndPiercingToiletPaper", abilityDuration_PiercingToiletPaper);
    }

    void EndPiercingToiletPaper()
    {
        numberOfEnemiesToPierceThrough = 0;
    }
}
