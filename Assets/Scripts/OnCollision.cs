using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnCollision : MonoBehaviour
{
    [SerializeField] private Text healthText;
    private float cooldown = 1f;
    private float lastAttackTime = 0f;
    private int health = 100;
    private PlayerMovement playerMovement;
    private void Start() 
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void OnCollisionEnter2D(Collision2D collision) 
    {
        if (Time.time - lastAttackTime < cooldown) return;
        if (collision.gameObject.CompareTag("Damagable")) {
            if (playerMovement.isCrouched) playerMovement.anim.SetTrigger("cHurt");
            else playerMovement.anim.SetTrigger("hurt");
            health -= 25;
            healthText.text = "Health: " + health;
            if (health == 0) {
                playerMovement.rb.bodyType = RigidbodyType2D.Static;
                if (playerMovement.isCrouched) playerMovement.anim.SetTrigger("cDie");
                else playerMovement.anim.SetTrigger("die");
            }
            lastAttackTime = Time.time;
        }
    }

    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
