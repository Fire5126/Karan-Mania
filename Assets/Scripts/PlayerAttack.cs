using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    PlayerController player;

    // Toilet Paper
    public GameObject toiletPaper;
    public float toiletPaperDamage = 10f;
    public float toiletPaperDuration = 5f;
    public float projectileSpeed = 10f;

    // Player Attack Timer
    public float attackDelay;
    float nextAttackTime;

    // AttackTypeVariable
    public string[] attackTypes;
    public int attackIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
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
        }
    }

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
}
