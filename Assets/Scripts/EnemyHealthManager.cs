using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthManager : MonoBehaviour
{
    public float MaxHealth = 10;
    GameObject player;
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
}
