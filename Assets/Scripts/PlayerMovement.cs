using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public KeyCode lightAttack = KeyCode.Mouse0;
    public KeyCode heavyAttack = KeyCode.Mouse1;
    public KeyCode jump = KeyCode.Space;
    public KeyCode rangedAttack = KeyCode.E;
    public string xMoveAxis = "Horizontal";

    Animator m_animator;
    private Rigidbody2D rb2;
    private bool attemptJump = false;
    private bool attempLAttack = false;
    private bool attemptRangedAttack = false;
    private bool attemptHAttack = false;
    private float moveIntentionX = 0;
    private float timeUntilShurkienReadied = 0;
    private float timeUntilMeleeReadied = 0;



    public float speed;
    public float jumpForce;
    public float groundLeeway;


    public Transform attackOrigin = null;
    public Transform rangedAttackOrigin = null;
    public GameObject shurkien = null;
    public float attackRadius = 0.6f;
    public float damage = 2f;
    public float attackDelay = 1f;
    public float rangedAttackdelay = 0.3f;
    public LayerMask enemyLayer = 8;
   
    void Start()
    {
        if (GetComponent<Rigidbody2D>())
        {
            rb2 = GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        HandleJump();
        HandleAttack();
        HandleRangedAttack();
    }

    void FixedUpdate()
    {
        HandleRun();
    }

    void OnDrawGizmosSelected()
    {
        Debug.DrawRay(transform.position, -Vector2.up * groundLeeway, Color.green);
        if(attackOrigin != null)
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
        if((attempLAttack || attemptHAttack) && timeUntilMeleeReadied <= 0)
        {
            Debug.Log("attempting attack");
            Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(attackOrigin.position, attackRadius, enemyLayer);
            for(int i = 0; i < overlappedColliders.Length; i ++)
            {
                IDamagable enemyAttributes = overlappedColliders[i].GetComponent<IDamagable>();
                if(enemyAttributes != null)
                {
                    enemyAttributes.applyDamage(damage);
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

            Instantiate(shurkien , rangedAttackOrigin.position, rangedAttackOrigin.rotation);

            timeUntilShurkienReadied = rangedAttackdelay;
        }
        else
        {
            timeUntilShurkienReadied -= Time.deltaTime;
        }
    }

    private void HandleRun()
    {

        if(moveIntentionX < 0 && transform.rotation.y == 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
        else if (moveIntentionX > 0 && transform.rotation.y != 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        rb2.velocity = new Vector2(moveIntentionX * speed, rb2.velocity.y);
    }

    private bool CheckGrounded()
    {
        return Physics2D.Raycast(transform.position, -Vector2.up, groundLeeway);
    }
}
