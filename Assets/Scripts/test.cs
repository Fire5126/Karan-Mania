using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{

    bool collideCheck = true;
    public Collider2D collider;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("StopCollideCheck", 0.1f);
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
        if (collideCheck)
        {
            collider.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        collider.enabled = true;
    }

}
