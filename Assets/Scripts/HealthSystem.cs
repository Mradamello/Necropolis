using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] public int healthMax;
    [SerializeField] private Text healthText;
    public int health;
    private float cooldown = 1f;
    private float lastAttackTime = 0f;
    public DamageSystem damageSystem;
    private PlayerMovement playerMovement;
    public Animator animator;
    private void Start()
    {
        health = healthMax;
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Time.time - lastAttackTime < cooldown) return;
        if (collision.gameObject.CompareTag("Damagable"))
        {
            damageSystem = collision.gameObject.GetComponent<DamageSystem>();
            TakeDamage(damageSystem.dmg);
            lastAttackTime = Time.time;
        }
    }
    public void TakeDamage(int dmg)
    {
        animator.SetTrigger("TakingDamage");
        health -= dmg;
        if (playerMovement != null)
        {
            if (playerMovement.isCrouched) playerMovement.anim.SetTrigger("cHurt");
            else playerMovement.anim.SetTrigger("hurt");
            if (health <= 0)
            {
                playerMovement.rb.bodyType = RigidbodyType2D.Static;
                if (playerMovement.isCrouched) playerMovement.anim.SetTrigger("cDie");
                else playerMovement.anim.SetTrigger("die");
            }
        }
        if(dmg > 0) Debug.Log(this.name + " health: " + health);
        if (health <= 0) animator.SetTrigger("Die");
    }
    public void Update()
    {
        if (healthText == null) return;
        healthText.text = "Health: " + health;
    }
    private void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
