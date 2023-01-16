using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletPaperScript : MonoBehaviour
{
    PlayerController player;
    PlayerAttack attackScript;
    Rigidbody2D rb;

    bool collideCheck = true;
    public Collider2D toiletPaperCollider;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("StopCollideCheck", 0.1f);
        rb = GetComponent<Rigidbody2D>();

        attackScript = FindObjectOfType<PlayerAttack>();
        player = FindObjectOfType<PlayerController>();
        rb.AddForce(transform.right * attackScript.projectileSpeed, ForceMode2D.Impulse);
        if(rb.velocity.magnitude < attackScript.projectileSpeed)
        {
            DestroyToiletPaper();
        }
        Invoke("DestroyToiletPaper", attackScript.toiletPaperDuration);
    }

    void StopCollideCheck()
    {
        collideCheck = false;
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
            DestroyToiletPaper();
        }
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collideCheck)
        {
            Physics2D.IgnoreLayerCollision(6, 7, true);
            return;
        }
        if (collision.gameObject.tag != "Projectile")
        {
            DestroyToiletPaper();
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Physics2D.IgnoreLayerCollision(6, 7, false);
    }

    void DestroyToiletPaper()
    {
        Destroy(gameObject);
    }
}
