using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PlayerAttack : MonoBehaviour
{
    // References
    PlayerController player;

    // Toilet Paper
    [Header("Toilet Paper")]
    public GameObject toiletPaper;
    public float toiletPaperDamage = 10f;
    public float toiletPaperDuration = 5f;
    public float projectileSpeed = 10f;
    public int attackIndex = 0;
    public string[] attackTypes;

    // Ability Cooldown
    [Header("Ability Variables")]
    public float abilityCooldownDelay;
    float nextAbility;
    public int abilityIndex = 0;
    public string[] abilityTypes;
    bool hasAbility = false;

    // Main Attack Timer
    [Header("Main Attack Timer")]
    public float attackDelay;
    float nextAttackTime;

    [Header("Scream For Manager Ability")]
    public float affectedRadius;
    public float stunTime;
    Collider2D[] hitColliders;




    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        if (!player.isPaused && player.gameStarted == true)
        {
            // Player Attack
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.E)) && Time.time > nextAttackTime)
            {
                nextAttackTime = Time.time + attackDelay;
                Invoke(attackTypes[attackIndex], 0);
            }

            // Player Ability
            if (Input.GetKeyDown(KeyCode.Mouse1) && Time.time > nextAbility && hasAbility)
            {
                nextAbility = Time.time + abilityCooldownDelay;
                Invoke(abilityTypes[abilityIndex], 0f);
            }
        }
    }




    // Main ToiletPaper Attack
    void ToiletPaper()
    {
        Quaternion rotation = transform.GetChild(1).transform.rotation;
        Instantiate<GameObject>(toiletPaper, transform.GetChild(1).transform.position, rotation);
    }

    void DoubleToiletPaper()
    {
        float zrot = transform.GetChild(1).transform.rotation.eulerAngles.z + 10;
        Quaternion rotation = Quaternion.Euler(0, 0, zrot);
        zrot = transform.GetChild(1).transform.rotation.eulerAngles.z - 10;
        Quaternion rotation2 = Quaternion.Euler(0, 0, zrot);
        Instantiate<GameObject>(toiletPaper, transform.GetChild(1).transform.position, rotation);
        Instantiate<GameObject>(toiletPaper, transform.GetChild(1).transform.position, rotation2);
    }

    void TripleToiletPaper()
    {
        float zrot = transform.GetChild(1).transform.rotation.eulerAngles.z + 20;
        Quaternion rotation = Quaternion.Euler(0, 0, zrot);
        zrot = transform.GetChild(1).transform.rotation.eulerAngles.z - 20;
        Quaternion rotation2 = Quaternion.Euler(0, 0, zrot);
        Instantiate<GameObject>(toiletPaper, transform.GetChild(1).transform.position, rotation);
        Instantiate<GameObject>(toiletPaper, transform.GetChild(1).transform.position, rotation2);
        Quaternion rotation3 = transform.GetChild(1).transform.rotation;
        Instantiate<GameObject>(toiletPaper, transform.GetChild(1).transform.position, rotation3);
    }

    public void EnemyHit(Collider2D enemy)
    {
        enemy.gameObject.GetComponent<Enemy>().UpdateHealth(toiletPaperDamage);
    }




    // Abilities
    void ScreamAttack()
    {
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

    void PlaceTrap()
    {

    }

    void HighHeelsMovementSpeed()
    {

    }
}
