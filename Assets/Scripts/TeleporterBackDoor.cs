using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterBackDoor : MonoBehaviour
{

    [SerializeField] private float openDelay = 30;
    private float timeUntillDoorOpen;
    
    [SerializeField] private TeleporterBackDoor otherDoor;
    
    public Transform doorTeleportLocation;

    private bool doorsActive;

    private bool doorOpen;

    private Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!doorsActive || doorOpen) return;
        timeUntillDoorOpen -= Time.deltaTime;

        if (timeUntillDoorOpen <= 0)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        doorOpen = true;
        animator.SetBool("IsOpen", true);
        transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
        //open door
    }

    private void CloseDoor()
    {
        doorOpen = false;
        animator.SetBool("IsOpen", false);
        timeUntillDoorOpen = openDelay;
        transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;
        // close door
    }

    void TeleportPlayer(GameObject player)
    {
        player.transform.position = otherDoor.doorTeleportLocation.position;
        otherDoor.CloseDoor();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.tag == "Player")
        {
            TeleportPlayer(col.gameObject);
            CloseDoor();
        }
    }

    

    public void ActivateDoors()
    {
        doorsActive = true;
        OpenDoor();
    }
    
    
}
