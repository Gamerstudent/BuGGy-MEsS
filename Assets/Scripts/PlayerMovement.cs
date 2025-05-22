using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D body;
    public float speed;
    public Animator onii;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;
    public float timeCheck = 0.0f;
    private bool iscrouching;
    [SerializeField] private float groundCheckDistance = 0.2f;

    [Header("Combat")]
    public Transform attackOrigin = null;
    public float attackRadius = 0.6f;
    public float attackDamage = 2f;
    public float attackDelay = 1.0f;
    public LayerMask enemyLayer;
    private bool attemptAttack;
    private float timeuntilAttackReadied = 0;

    [Header("Health")]
    public float maxHealth = 10f;
    private float currentHealth;
    private bool isDied = false;

    
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        onii = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    private void GetInput() 
    {
        attemptAttack = Input.GetButtonDown("Fire1");
    }
    
    void Update()
    {

        if (isDied) 
        {
            return;
        }


        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalVelocity = body.velocity.y;
        float stopTimer = 0.0f;
        stopTimer += Time.deltaTime;

        GetInput();
        HandleAttack();

        float groundCheckDistance = 0.2f;
        isGrounded = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundLayer);
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckDistance, Color.red);

        body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);
        
        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(8, 8, 8);
        }
        else if (horizontalInput < -0.01f) 
        {
            transform.localScale = new Vector3(-8, 8, 8);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            body.velocity = new Vector2(body.velocity.x, speed);
        }

        onii.SetBool("IsRunning", horizontalInput != 0);
        onii.SetBool("IsJumping", !isGrounded && verticalVelocity > 0.01f);
        onii.SetBool("IsFalling", !isGrounded && verticalVelocity < -0.01f);
        onii.SetBool("IsGrounded", isGrounded);
        if (stopTimer > timeCheck) 
        {
            onii.SetBool("FinishStop", true);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            onii.SetBool("IsCrouching", true);
            iscrouching = true;
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            onii.SetBool("IsCrouching", false);
            iscrouching = false;
        }

        if (iscrouching)
        {
            onii.Play("Crouch");
        }
        
        

    }

    private void HandleAttack() 
    {
        if (attemptAttack && timeuntilAttackReadied <= 0)
        {
            onii.SetTrigger("Attack");
            Debug.Log("Enjoy pandago!");
            Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius, enemyLayer);   /*array*/
            for (int i = 0; i < overlappedColliders.Length; i++) 
            {
                IDamagables enemyAtributes = overlappedColliders[i].GetComponent<IDamagables>();
                if (enemyAtributes != null)
                {
                    enemyAtributes.ApplyDamage(attackDamage);
                }
               
            }


            timeuntilAttackReadied = attackDelay;
        }
        else 
        {
            timeuntilAttackReadied -= Time.deltaTime;
        }
    }

    public void ApplyDamage(float damage) 
    {
        Debug.Log(isDied);


        if (isDied) 
        {
            return;
        }

        currentHealth -= damage;
        Debug.Log("Player took damage" + damage + ", Current Health :" + currentHealth);
        //onii.SetTrigger("Hit"); add that trigger and uncomment this code.

        if (currentHealth <= 0) 
        {
            Die();
        }
    }

    private void Die() 
    {
        isDied = true;
        onii.SetBool("IsDead", true);
        body.velocity = Vector2.zero;
        body.bodyType = RigidbodyType2D.Static;

        //StartCoroutine(RestartAfterDelay(2f));

    }

    //private IEnumerator RestartAfterDelay(float delay)
    //{
    //    yield return new WaitForSeconds(delay);
    //    SceneManager.LoadSceneAsync("3");
    //}


    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        if (attackOrigin != null) 
        {
            Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
        }
    }
}
