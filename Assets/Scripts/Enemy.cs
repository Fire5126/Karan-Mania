using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float MaxHealth = 10;
    GameObject player;
    GameManager gameManager;
    public int damage;
    bool ableToAttack = false;
    float attackDelay = 1;
    float nextAttack;

    // Start is called before the first frame update
    void Start()
    {
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
}
