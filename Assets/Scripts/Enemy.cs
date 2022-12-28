using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float MaxHealth = 10;
    GameObject player;
    public int damage;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void UpdateHealth(float damage)
    {
        MaxHealth -= damage;
        if (MaxHealth <= 0)
        {
            player.GetComponent<PlayerController>().kills += 1;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            ApplyPlayerDamage();
        }
    }

    void ApplyPlayerDamage()
    {
        player.GetComponent<PlayerController>().TakeDamage(damage);
    }
}
