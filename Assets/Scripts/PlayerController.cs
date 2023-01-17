using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // Misc Variables
    [Header("Misc Variables")]
    public bool isPaused = false;
    public bool gameStarted = false;

    // Player
    [Header("Player Stats")]
    public float movementSpeed;
    public float maxHealth = 20;
    bool isDead = false;
    float health = 20;
    Rigidbody2D rb;
    Vector2 moveDirection;

    // Game Objects
    [Header("References")]
    public Joystick joystick;
    GameManager gameManager;

    void Start()
    {
        health = maxHealth;
        gameManager = FindObjectOfType<GameManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isPaused && gameStarted == true)
        {
            // Player movement
            //float moveX = Input.GetAxisRaw("Horizontal");
            //float moveY = Input.GetAxisRaw("Vertical");
            //moveDirection = new Vector2(moveX, moveY).normalized;
            float moveX = joystick.Horizontal;
            float moveY = joystick.Vertical;
            moveDirection = new Vector2(moveX, moveY);
        }

        // Pause Game
        if (Input.GetKeyDown(KeyCode.Escape) && !isDead)
        {
            gameManager.TogglePauseGame();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveDirection.x * movementSpeed, moveDirection.y * movementSpeed);
        
    }

    



    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;
        gameManager.UpdatePlayerHealthStat(health);
        if (health <= 0)
        {
            isDead = true;
            gameManager.PlayerDied();
        }
    }
}
