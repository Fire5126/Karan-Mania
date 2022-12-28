using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Misc Variables
    public bool isPaused = false;

    // Player
    public float movementSpeed;
    public float maxHealth = 20;
    float health = 20;
    Rigidbody2D rb;
    Vector2 moveDirection;

    // Player Score Stats
    public int kills = 0;

    // Toilet Paper
    public GameObject toiletPaper;
    public float toiletPaperDamage = 10f;
    public float toiletPaperDuration = 5f;
    public float projectileSpeed = 10f;

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
        if (!isPaused)
        {
            // Player movement
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            moveDirection = new Vector2(moveX, moveY).normalized;

            // Player Attack
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Quaternion rotation = transform.GetChild(1).transform.rotation;
                GameObject instance = Instantiate<GameObject>(toiletPaper, transform.GetChild(1).transform.position, rotation);
            }
        }

        // Pause Game
        if (Input.GetKeyDown(KeyCode.Escape))
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

    // Toilet Paper Functions
    public void EnemyHit(Collider2D enemy)
    {
        enemy.gameObject.GetComponent<Enemy>().UpdateHealth(toiletPaperDamage);
    }

    public void TakeDamage(int damageTaken)
    {
        health -= damageTaken;
        gameManager.UpdatePlayerHealthStat(health);
        if (health <= 0)
        {
            gameManager.PlayerDied();
        }
    }
}
