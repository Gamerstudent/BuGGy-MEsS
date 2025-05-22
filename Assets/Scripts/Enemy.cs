using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagables
{
    public float healthPool = 10f;
    public float currentHealth = 10f;
    public Animator onii;
    public float deathDelay = 100f;
    private bool attackIntermyass;
    public float speed = 3.0f;
    private Transform player;
    public float x;

    [Header("Combat")]
    public float attackRadius = 0.5f;
    public float attackDamage = 1f;
    public float attackCooldown = 2f;
    public LayerMask playerLayer;
    private float cooldownTimer = 0f;
    public Transform attackOrigin;



    void Start()
    {
        currentHealth = healthPool;
        attackIntermyass = true;
        player = GameObject.FindWithTag("Player").transform;


    }

   

    private void TryAttack() 
    {
        onii.SetTrigger("Attack");
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius, playerLayer);
        foreach (Collider2D hit in hits) 
        {
            IDamagables player = hit.GetComponent<IDamagables>();
            if (player == null) 
            {
                Debug.Log("yor really fked");
            }
            if (player != null) 
            {
                player.ApplyDamage(attackDamage);
                Debug.Log("yor fkd");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
    }

    public Action OnDeath;

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0)
        {
            TryAttack();
            cooldownTimer = attackCooldown;

        }

        if (player != null) 
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.position += (Vector3)direction * speed * Time.deltaTime;

            if (direction.x >= 0.01f)
            {
                transform.localScale = new Vector3(-x, x, x);
            }
            else if (direction.x <= -0.01f) 
            {
                transform.localScale = new Vector3(x, x, x);
            }
        }
    }

    public virtual void ApplyDamage(float amount) 
    {
        if (attackIntermyass == true) 
        {
            currentHealth -= amount;
            Debug.Log("ITS WORKING");
            onii.SetTrigger("IsDTaken");
        }
        
        if (currentHealth <= 0) 
        {

            attackIntermyass = false;
            Die();
        }
    }

    private void Die() 
    {
        onii.SetBool("IsDead", true);

        //GetComponent<Collider2D>().enabled = false;

        StartCoroutine(DieAfterDelay());
    }

    private IEnumerator DieAfterDelay() 
    {
        yield return new WaitForSeconds(deathDelay);

        OnDeath?.Invoke();
        
        Destroy(gameObject);
        attackIntermyass = true;
    }


}
