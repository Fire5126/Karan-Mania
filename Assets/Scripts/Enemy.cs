using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float MaxHealth = 5;
    public int damage;
    int damageStat;
    float speed;

    // Enemy Variable Properties
    bool ableToAttack = false;
    float attackDelay = 1;
    float nextAttack;

    // References
    GameObject player;
    GameManager gameManager;
    AIPath aIPath;
    Animator animator;

    //bool animPlaying = false;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        aIPath = GetComponent<AIPath>();
        speed = GetComponent<AIPath>().maxSpeed;
        damageStat = damage;
        gameManager = FindObjectOfType<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        animator.Play("Running", 0, 0.0f);
    }
    private void Update()
    {
        // Sprite Flipping
        if (aIPath.velocity.x < 0f)
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (aIPath.velocity.x > 0f)
        {
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = false;
        }
        
        // Animation
        //if (aIPath.velocity.x > 0f && animPlaying == false)
        //{
            //animPlaying = true;
            //animator.Play("Running", 0, 0.0f);
            //print("animation playing");
        //}
        //else
        //{
            //animPlaying = false;
            //animator.Play("idle", 0, 0.0f);
            //print("animation not playing");
        //}

        // Attack Player
        if (ableToAttack == true && Time.time > nextAttack)
        {
            ApplyPlayerDamage();
            nextAttack = Time.time + attackDelay;
        }
    }

    public void UpdateHealth(float damage)
    {
        MaxHealth -= damage;
        if (MaxHealth <= 0)
        {
            gameManager.AddKillScore();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            ableToAttack = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            ableToAttack = false;
        }
    }

    void ApplyPlayerDamage()
    {
        player.GetComponent<PlayerController>().TakeDamage(damage);
    }

    public void ResetStunStats()
    {
        damage = damageStat;
        GetComponent<AIPath>().maxSpeed = speed;
    }
}
