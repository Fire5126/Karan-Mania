using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyerBeltScript : MonoBehaviour
{
    private bool inTrigger = false;

    private PlayerController playerRef;

    [SerializeField] private Vector2 forceDirection;
    [SerializeField] private int forceMagnitude;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!inTrigger || playerRef == null) return;
        Vector2 force = forceDirection.normalized * forceMagnitude;
        playerRef.GetComponent<Rigidbody2D>().AddForce(forceDirection.normalized * forceMagnitude);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            inTrigger = true;
            playerRef = other.transform.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.transform.tag == "Player")
        {
            inTrigger = false;
            playerRef = null;
        }
    }
}
