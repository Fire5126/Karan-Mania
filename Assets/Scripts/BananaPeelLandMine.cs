using System;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BananaPeelLandMine : MonoBehaviour
{
    float affectedRadius;
    float damage;
    float blastForce;
    Collider2D[] hitColliders;

    bool mineEnabled = false;

    public void InitiateVariables(float radius, float dmg, float blastForceMultiplier)
    {
        affectedRadius = radius;
        damage = dmg;
        blastForce = blastForceMultiplier;
        mineEnabled = true;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.tag == "Enemy" && mineEnabled)
        {
            mineEnabled = false;
            hitColliders = Physics2D.OverlapCircleAll(this.transform.position, affectedRadius);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.tag == "Enemy")
                {

                    Vector2 force = (hitCollider.transform.position - transform.position);
                    force.x = 1 / force.x;
                    force.y = 1 / force.y;
                    force = force * blastForce;
                    hitCollider.GetComponent<AIPath>().enabled = false;
                    hitCollider.GetComponent<Rigidbody2D>().AddForce(force);
                    hitCollider.GetComponent<Enemy>().UpdateHealth(damage);
                }
            }
            Invoke("ReEnableAIPath", 0.5f);
        }
    }

    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if(collision.transform.tag == "Enemy" && mineEnabled)
    //     {
    //         mineEnabled = false;
    //         hitColliders = Physics2D.OverlapCircleAll(this.transform.position, affectedRadius);
    //         foreach (var hitCollider in hitColliders)
    //         {
    //             if (hitCollider.tag == "Enemy")
    //             {
    //
    //                 Vector2 force = (hitCollider.transform.position - transform.position);
    //                 force.x = 1 / force.x;
    //                 force.y = 1 / force.y;
    //                 force = force * blastForce;
    //                 hitCollider.GetComponent<AIPath>().enabled = false;
    //                 hitCollider.GetComponent<Rigidbody2D>().AddForce(force);
    //                 hitCollider.GetComponent<Enemy>().UpdateHealth(damage);
    //             }
    //         }
    //         Invoke("ReEnableAIPath", 0.5f);
    //     }
    // }

    void ReEnableAIPath()
    {
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Enemy")
            {
                hitCollider.GetComponent<AIPath>().enabled = true;

            }
        }
        Destroy(gameObject);
    }
}
