using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float MaxHealth = 10;
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
    
    
    
    

    // Start is called before the first frame update
    void Start()
    {
        speed = GetComponent<AIPath>().maxSpeed;
        damageStat = damage;
        gameManager = FindObjectOfType<GameManager>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void Update()
    {

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
