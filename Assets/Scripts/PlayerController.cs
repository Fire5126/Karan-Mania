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
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector2(moveX, moveY).normalized;
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
        RotateFirePosition();
    }

    void RotateFirePosition()
    {
        Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 lookAt = mouseScreenPosition;
        float AngleRad = Mathf.Atan2(lookAt.y - this.transform.position.y, lookAt.x - this.transform.position.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        gameObject.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
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
