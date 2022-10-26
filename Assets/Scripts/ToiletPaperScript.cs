using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToiletPaperScript : MonoBehaviour
{
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Invoke("DestroyToiletPaper", player.GetComponent<PlayerToiletPaper>().toiletPaperDuration);
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
                player.GetComponent<PlayerToiletPaper>().EnemyHit(collision);
            }
            DestroyToiletPaper();
        }
    }

    void DestroyToiletPaper()
    {
        Destroy(gameObject);
    }
}
