using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerIntroAnimScript : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    [SerializeField] private Animator animController;

    public void GameStarted()
    {
        animController.SetBool("StartInitiated", true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Projectile")
        {
            Debug.LogWarning("Intro Worker Hit!");
            animController.SetBool("IsDead", true);
            gameManager.StartGame();
        }
    }
}
