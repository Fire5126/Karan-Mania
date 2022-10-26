using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float movementSpeed;
    public float maxHealth = 20;
    float horizontal;
    float vertical;
    Rigidbody2D rb;
    public int kills = 0;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Vector2 playerVelocity = new Vector2 (horizontal * movementSpeed, vertical * movementSpeed);
        rb.velocity = Vector2.ClampMagnitude(playerVelocity, movementSpeed);
    }
}
