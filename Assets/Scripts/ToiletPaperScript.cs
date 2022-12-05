using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletPaperScript : MonoBehaviour
{
    PlayerController player;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        player = FindObjectOfType<PlayerController>();
        rb.AddForce(transform.right * player.projectileSpeed, ForceMode2D.Impulse);
        Invoke("DestroyToiletPaper", player.toiletPaperDuration);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Projectile" && collision.gameObject != this.gameObject)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                player.EnemyHit(collision);
            }
            DestroyToiletPaper();
        }
    }

    void DestroyToiletPaper()
    {
        Destroy(gameObject);
    }
}
