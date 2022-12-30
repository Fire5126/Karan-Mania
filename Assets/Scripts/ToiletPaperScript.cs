using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletPaperScript : MonoBehaviour
{
    PlayerController player;
    PlayerAttack attackScript;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        attackScript = FindObjectOfType<PlayerAttack>();
        player = FindObjectOfType<PlayerController>();
        rb.AddForce(transform.right * attackScript.projectileSpeed, ForceMode2D.Impulse);
        Invoke("DestroyToiletPaper", attackScript.toiletPaperDuration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            attackScript.EnemyHit(collision);
        }
        DestroyToiletPaper();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Projectile")
        {
            DestroyToiletPaper();
        }
    }

    void DestroyToiletPaper()
    {
        Destroy(gameObject);
    }
}
