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

    // Main Attack Timer
    [Header("Main Attack Timer")]
    public float attackDelay;
    float nextAttackTime;

    [Header("Screaming Stun Ability")]
    public float abilityCooldown_ScreamingStunAbility;
    public float affectedRadius;
    public float stunTime;
    Collider2D[] hitColliders;

    [Header("High Heel Run Ability")]
    public float speedIncrease;
    float movementSpeedBackup;
    public float abilityDuration_HighHeelRun;
    public float abilityCooldown_HighHeelRun;

    [Header("Banana Peel Land Mine Ability")]
    public GameObject BananaPeelLandMinePrefab;
    public float landMineAffectedRadius;
    public float abilityCooldown_BananaPeelLandMine;
    public float damage_BananaPeelLandMine;
    public Vector2 landMineSpawnOffset;
    public float blastForceMultiplier;

    [Header("Piercing Toilet Paper Ability")]
    public int ability_NumberOfEnemiesToPierceThrough;
    public int numberOfEnemiesToPierceThrough;
    public float abilityCooldown_PiercingToiletPaper;
    public float abilityDuration_PiercingToiletPaper;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();

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
                Invoke(attackTypes[attackIndex], 0);
            }

            // Player Ability
           /* if (Input.GetKeyDown(KeyCode.Mouse1) && Time.time > nextAbility && hasAbility)
            {
                nextAbility = Time.time + abilityCooldownDelay;
                Invoke(abilityTypes[abilityIndex], 0f);
            }*/
        }
    }

    public void ActivateAbility()
    {
        if (Time.time > nextAbility && hasAbility)
        {
            nextAbility = Time.time + abilityCooldownDelay;
            Invoke(abilityTypes[abilityIndex], 0f);
        }
    }

    public void ChangeAbility(int abilityIndexInput)
    {
        hasAbility = true;
        abilityIndex = abilityIndexInput;
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
    void ToiletPaper()
    {
        Quaternion rotation = transform.GetChild(1).transform.rotation;
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
        abilityCooldownDelay = abilityCooldown_BananaPeelLandMine;
        Vector2 spawnOffset = transform.position;
        spawnOffset = spawnOffset + landMineSpawnOffset;
        GameObject landMineRef = Instantiate(BananaPeelLandMinePrefab, spawnOffset, Quaternion.identity);
        landMineRef.GetComponent<BananaPeelLandMine>().InitiateVariables(affectedRadius, damage_BananaPeelLandMine, blastForceMultiplier);
    }

    void PiercingToiletPaper()
    {
        abilityCooldownDelay = abilityCooldown_PiercingToiletPaper;
        numberOfEnemiesToPierceThrough = ability_NumberOfEnemiesToPierceThrough;
        Invoke("EndPiercingToiletPaper", abilityDuration_PiercingToiletPaper);
    }

    void EndPiercingToiletPaper()
    {
        numberOfEnemiesToPierceThrough = 0;
    }
}
