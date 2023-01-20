using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletPaperScript : MonoBehaviour
{

    PlayerController player;
    PlayerAttack attackScript;
    Rigidbody2D rb;

    bool collideCheck = true;
    bool collideDisabled = false;
    public Collider2D toiletPaperCollider;
    int enemiesToPierce = 0;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("StopCollideCheck", 0.1f);
        Invoke("EnableCollision", 0.2f);
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

    public void InitiateVariables(int piercingCount)
    {
        enemiesToPierce = piercingCount;
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
            enemiesToPierce--;
        }
        if (enemiesToPierce < 0)
        {
            DestroyToiletPaper();
        }
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collideCheck)
        {
            collideDisabled = true;
            Physics2D.IgnoreLayerCollision(6, 7, true);
            Invoke("EnableCollision", 0.2f);
            return;
        }
        if (collision.gameObject.tag != "Projectile")
        {
            if(collision.gameObject.tag != "Obstacles")
            {
                print(collision.gameObject.tag + " " + collision.gameObject.ToString());
                
            }
            DestroyToiletPaper();
        }

    }

    void EnableCollision()
    {
        Physics2D.IgnoreLayerCollision(6, 7, false);
        collideDisabled = false;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collideDisabled == true)
        {
            collideDisabled = false;
            Physics2D.IgnoreLayerCollision(6, 7, false);
        }
    }

    void DestroyToiletPaper()
    {
        player.PlayToiletPaperHit();
        Destroy(gameObject);
    }
}
