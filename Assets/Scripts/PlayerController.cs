using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Player
    public float movementSpeed;
    public float maxHealth = 20;
    float horizontal;
    float vertical;
    Rigidbody2D rb;
    Vector2 moveDirection;

    // Player Score Stats
    public int kills = 0;

    // Toilet Paper
    public GameObject toiletPaper;
    public float toiletPaperDamage = 10f;
    public float toiletPaperDuration = 5f;
    public float projectileSpeed = 10f;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(moveX, moveY).normalized;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Quaternion rotation = transform.GetChild(1).transform.rotation;
            GameObject instance = Instantiate<GameObject>(toiletPaper, transform.GetChild(1).transform.position, rotation);
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
    public void EnemyHit(Collision2D enemy)
    {
        enemy.gameObject.GetComponent<Enemy>().UpdateHealth(toiletPaperDamage);
    }
}
