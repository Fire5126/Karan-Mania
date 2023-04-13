using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _PlayerAttack : MonoBehaviour
{
    [Header("References")] 
    public Joystick joystick;
    private PlayerController player;
    private SoundManager soundManager;

    [Header("Toilet Paper Settings")] 
    [SerializeField] private Transform projectileSpawnLocation;
    [SerializeField] private GameObject toiletPaper;
    [SerializeField] private float toiletPaperDamage = 5f;
    [SerializeField] private float toiletPaperDuration = 5f;
    [SerializeField] private float projectileSpeed = 8f;
    [Space(10)] 
    [SerializeField] private float attackDelay = 0.5f;
    private float nextAttackTime;
    
    private bool attackDisabled = false;
    
    [Header("Piercing Toilet Paper Ability")]
    private int numberOfEnemiesToPierceThrough;
    
    //[Header("Banana Peel Land Mine Ability")]
    
    //[Header("High Heel Run Ability")]
    
    //[Header("Screaming Stun Ability")]
    
    //attack upgrade variables
    [HideInInspector] public enum ProjectileTypes
    {
        Single,
        Double,
        Triple
    }
    private ProjectileTypes currentAttackType;
    private bool attackUpgradeEnabled = false;
    private float upgradeTimeLeft;

    void Update()
    {
        // Makes sure player is not paused or that the game hasn't started
        if (player.isPaused || !player.gameStarted) return;
        
        //Checks if the player is trying to shoot
        if (joystick.Direction.magnitude > 0)
        {
            RotateFirePosition();

            //Checks if the player is able to shoot
            if (nextAttackTime <= Time.time)
            {
                // resets shoot delay
                nextAttackTime = Time.time + attackDelay;
                ShootProjectile();
                player.attackMagnitude = joystick.Direction.magnitude;
            }
            else { player.attackMagnitude = 0; }
        }

        // Timer and resetter for attack type upgrade
        if (!attackUpgradeEnabled) return;
        upgradeTimeLeft -= Time.deltaTime;
        if (upgradeTimeLeft <= 0)
        {
            currentAttackType = ProjectileTypes.Single;
        }
    }

    public void ShootProjectile()
    {
        InitiateProjectileShot(currentAttackType);
    }
    
    private void InitiateProjectileShot(ProjectileTypes attackType)
    {
        // Initiates variables
        Quaternion rotation;
        Quaternion rotation2;
        Quaternion rotation3;
        float zrot;
        
        switch (attackType)
        {
            
            
            // Shoots single toilet paper
            case ProjectileTypes.Single:
                
                rotation = projectileSpawnLocation.rotation;
                Instantiate<GameObject>(toiletPaper, 
                    projectileSpawnLocation.position, 
                    rotation).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough);
                break;
            
            // Shoots two toilet papers
            case ProjectileTypes.Double:
                
                // Sets Variables
                zrot = projectileSpawnLocation.rotation.eulerAngles.z + 10;
                rotation = Quaternion.Euler(0, 0, zrot);
                zrot = projectileSpawnLocation.rotation.eulerAngles.z - 10;
                rotation2 = Quaternion.Euler(0, 0, zrot);
                
                //Spawns first TP
                Instantiate<GameObject>(toiletPaper, 
                    projectileSpawnLocation.position, 
                    rotation).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough);
                
                //Spawns second TP
                Instantiate<GameObject>(toiletPaper, 
                    projectileSpawnLocation.position, 
                    rotation2).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough); ;
                
                break;
            
            // Shoots three toilet papers
            case ProjectileTypes.Triple:
                
                // Sets Variables
                zrot = projectileSpawnLocation.rotation.eulerAngles.z + 20;
                rotation = Quaternion.Euler(0, 0, zrot);
                zrot = projectileSpawnLocation.rotation.eulerAngles.z - 20;
                rotation2 = Quaternion.Euler(0, 0, zrot);
                
                // Spawns first TP
                Instantiate<GameObject>(toiletPaper, 
                    projectileSpawnLocation.position, 
                    rotation).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough);
                
                // Spawns Second TP
                Instantiate<GameObject>(toiletPaper, 
                    projectileSpawnLocation.position,
                    rotation2).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough);
                
                // Spawns Third TP
                rotation3 = projectileSpawnLocation.rotation;
                Instantiate<GameObject>(toiletPaper, 
                    projectileSpawnLocation.position, 
                    rotation3).transform.GetComponent<ToiletPaperScript>().InitiateVariables(numberOfEnemiesToPierceThrough);
                
                break;
        }
    }
    
    // sets the projectile type for a specified time (for obtainable upgrade)
    public void SetProjectileType(ProjectileTypes attackType, float time)
    {
        currentAttackType = attackType;
        upgradeTimeLeft = Time.time + time;
        attackUpgradeEnabled = true;
    }
    
    // Animations
    public void StartGameThrowAnimation()
    {
        float vel = projectileSpeed;
        projectileSpeed = 3;
        ShootProjectile();
        player.animator.Play("Throwing");
        projectileSpeed = vel;
    }
    
    void RotateFirePosition()
    {
        //Vector3 mouseScreenPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Vector3 lookAt = mouseScreenPosition;
        //float AngleRad = Mathf.Atan2(lookAt.y - this.transform.position.y, lookAt.x - this.transform.position.x);
        //float AngleDeg = (180 / Mathf.PI) * AngleRad;
        //gameObject.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, AngleDeg);

        float AngleRad = Mathf.Atan2(joystick.Vertical, joystick.Horizontal);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        gameObject.transform.GetChild(1).transform.rotation = Quaternion.Euler(0, 0, AngleDeg);
    }
    
    public void EnemyHit(Collider2D enemy)
    {
        enemy.gameObject.GetComponent<Enemy>().UpdateHealth(toiletPaperDamage);
    }
}
