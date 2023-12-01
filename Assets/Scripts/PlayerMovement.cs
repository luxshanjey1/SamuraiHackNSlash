using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public KeyCode lightAttack = KeyCode.Mouse0;
    public KeyCode heavyAttack = KeyCode.Mouse1;
    public KeyCode jump = KeyCode.Space;
    public KeyCode rangedAttack = KeyCode.E;
    public KeyCode roll = KeyCode.Q;
    public string xMoveAxis = "Horizontal";

    private Animator animator = null;
    private Rigidbody2D rb2;
    private bool attemptJump = false;
    private bool attempLAttack = false;
    private bool attemptRangedAttack = false;
    private bool attemptHAttack = false;
    private bool attemptRoll = false;
    private float moveIntentionX = 0;
    private float timeUntilShurkienReadied = 0;
    private float timeUntilMeleeReadied = 0;
    public float healthPool = 10f;


    public float speed;
    public float jumpForce;
    public float groundLeeway;
    private float currentHealth = 1f;

    public Transform attackOrigin = null;
    public Transform rangedAttackOrigin = null;
    public GameObject shurkien = null;
    public float attackRadius = 0.6f;
    public float damage = 2f;
    public float attackDelay = 1f;
    public float rangedAttackdelay = 0.3f;
    public LayerMask enemyLayer = 8;
    private bool isLightAttacking = false;
    private bool isHeavyAttacking = false;
    private bool isRolling = false;

    void Awake()
    {
        currentHealth = healthPool;
        if (GetComponent<Rigidbody2D>())
        {
            rb2 = GetComponent<Rigidbody2D>();
        }
        if (GetComponent<Animator>())
        {
            animator = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        GetInput();

        HandleJump();
        HandleAttack();
        HandleRangedAttack();
        HandleAnimations();
    }

    void FixedUpdate()
    {
        HandleRun();
    }

    void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, -Vector2.up * groundLeeway, Color.green);
        if (attackOrigin != null)
        {
            Gizmos.DrawWireSphere(attackOrigin.position, attackRadius);
        }
    }

    private void GetInput()
    {
        moveIntentionX = Input.GetAxis(xMoveAxis);
        attemptHAttack = Input.GetKeyDown(heavyAttack);
        attempLAttack = Input.GetKeyDown(lightAttack);
        attemptJump = Input.GetKeyDown(jump);
        attemptRangedAttack = Input.GetKeyDown(rangedAttack);
        attemptRoll = Input.GetKeyDown(roll);

    }

    private void HandleJump()
    {
        if (attemptJump && CheckGrounded())
        {
            rb2.velocity = new Vector2(rb2.velocity.x, jumpForce);
        }
    }

    private void HandleAttack()
    {
        if ((attempLAttack || attemptHAttack) && timeUntilMeleeReadied <= 0)
        {
            Debug.Log("attempting attack");
            Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius, enemyLayer);
            for (int i = 0; i < overlappedColliders.Length; i++)
            {
                Health enemyAttributes = overlappedColliders[i].GetComponent<Health>();
                if (enemyAttributes != null)
                {
                    if (attempLAttack)
                    {
                        enemyAttributes.TakeDamage(1);
                    }
                    else if (attemptHAttack)
                    {
                        enemyAttributes.TakeDamage(2);
                    }
                    
                }
            }
            timeUntilMeleeReadied = attackDelay;
        }
        else
        {
            timeUntilMeleeReadied -= Time.deltaTime;
        }
    }

    private void HandleRangedAttack()
    {
        if (attemptRangedAttack && timeUntilShurkienReadied <= 0)
        {
            Debug.Log("attempting ranged attack");

            Instantiate(shurkien, rangedAttackOrigin.position, rangedAttackOrigin.rotation);

            timeUntilShurkienReadied = rangedAttackdelay;
        }
        else
        {
            timeUntilShurkienReadied -= Time.deltaTime;
        }
    }

    private void HandleRun()
    {

        if (moveIntentionX < 0 && transform.rotation.y == 0 && !isHeavyAttacking )
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        else if (moveIntentionX > 0 && transform.rotation.y != 0 && !isHeavyAttacking)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        rb2.velocity = new Vector2(moveIntentionX * speed, rb2.velocity.y);
    }

    private void HandleAnimations()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
        animator.SetFloat("Speed", horizontalMove);
        animator.SetBool("Grounded", CheckGrounded());


        if (attemptHAttack)
        {
            if (!isHeavyAttacking)
            {
                StartCoroutine(MeleeAttackAnimDelay());
            }
        }

        if (attempLAttack)
        {
            if (!isLightAttacking)
            {
                animator.SetTrigger("LightAttack");
            }
        }


        if (attemptJump && CheckGrounded() || rb2.velocity.y > 1f)
        {
            if (!isLightAttacking || !isHeavyAttacking)
            {
                animator.SetTrigger("Jump");
            }
            
        }

        if (attemptRoll && CheckGrounded() )
        {
            if (!isRolling)
            {
                animator.SetTrigger("Roll");
            }
        }

        if(Mathf.Abs(moveIntentionX) > 0.1f && CheckGrounded())
        {
            animator.SetInteger("AnimState", 2);
        }

        else
        {
            animator.SetInteger("AnimState", 0);
        }


    }

    private IEnumerator MeleeAttackAnimDelay()
    {
        animator.SetTrigger("HeavyAttack");
        isHeavyAttacking = true;
        yield return new WaitForSeconds(attackDelay);
        isHeavyAttacking = false;
    }

    private bool CheckGrounded()
    {
        return Physics2D.Raycast(transform.position, -Vector2.up, groundLeeway);
    }



}
