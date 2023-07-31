using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSystem : MonoBehaviour
{
    private bool isAttacking = false;
    public Animator animator;
    public Transform attackPoint;
    [SerializeField] public Vector2 attackRange;
    public LayerMask enemyLayers;
    public HealthSystem healthSystem;
    [SerializeField] public int attackDamage;
    [SerializeField] private float cooldown;
    private float lastAttackTime = 0f;
    private StaminaSystem staminaSystem;
    [SerializeField] private float staminaRequirement;

    private void Start()
    {
        animator = GetComponent<Animator>();
        staminaSystem = GetComponent<StaminaSystem>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            if (staminaSystem.currentStamina >= staminaRequirement)
            {
                Attack();
            }
        }
    }

    private void Attack()
    {
        if (Time.time - lastAttackTime < cooldown) return;
        if (!isAttacking)
        {
            isAttacking = true;
            staminaSystem.currentStamina -= staminaRequirement;
            animator.SetTrigger("Attack");
            Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(attackPoint.position, attackRange, 0, enemyLayers);
            foreach(Collider2D Enemy in hitEnemies)
            {
                Debug.Log("Enemy hit: " + Enemy.name);
                healthSystem = Enemy.gameObject.GetComponent<HealthSystem>();
                healthSystem.TakeDamage(attackDamage);
            }
            lastAttackTime = Time.time;
            isAttacking = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireCube(attackPoint.position, attackRange);
    }
}
