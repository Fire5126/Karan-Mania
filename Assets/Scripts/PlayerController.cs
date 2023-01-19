using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    // Misc Variables
    [Header("Misc Variables")]
    public bool isPaused = false;
    public bool gameStarted = false;
    public bool damageDisabled = false;
    bool audioPlaying = false;
    AudioSource runAudio;

    // Player
    [Header("Player Stats")]
    public float movementSpeed;
    public float maxHealth = 20;
    bool isDead = false;
    public float health = 20;
    Rigidbody2D rb;
    Vector2 moveDirection;

    // Game Objects
    [Header("References")]
    public Joystick joystick;
    GameManager gameManager;
    SoundManager soundManager;

    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
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
            moveDirection = new Vector2(moveX, moveY).normalized;
        }

        if(moveDirection.x > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
        }
        else if(moveDirection.x <0)
        {
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        

        if (audioPlaying == false && moveDirection.magnitude > 0 && !isPaused)
        {
            audioPlaying = true;
            runAudio = soundManager.Play("KarenRun", true);
        }
        else if (audioPlaying == true && moveDirection.magnitude <= 0)
        {
            audioPlaying=false;
            soundManager.StopPlaying(runAudio);
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
        if (damageDisabled == false)
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

    public void PlayToiletPaperHit()
    {
        if (Random.Range(0, 2) == 1)
        {
            soundManager.Play("ToiletPaperHit1");
            return;
        }
        else
        {
            soundManager.Play("ToiletPaperHit2");
            return;
        }
    }
}
