using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyController : MonoBehaviour
{
    private enum State
    {
        Moving,
        Attacking,
        Knockback,
        Dead,
    }

    private State currentState;

    [SerializeField]
    private float
        groundCheckDistance,
        wallCheckDistance,
        movementSpeed,
        maxHealth,
        knockbackDuration,
        lastTouchDamageTime,
        touchDamageCooldown,
        touchDamage,
        touchDamageWidth,
        touchDamageHeight,
        attackRange;

    [SerializeField]
    private Transform
        groundCheck,
        wallCheck,
        touchDamageCheck;

    [SerializeField]
    private LayerMask
        whatIsGround,
        whatIsPlayer;

    [SerializeField]
    private Vector2 knockbackSpeed;

    [SerializeField]
    private GameObject
        hitParticle,
        deathChunkParticle,
        deathBloodParticle;

    private float
        currentHealth,
        knockbackStartTime;

    private float[] attackDetails = new float[2];

    private int
        facingDirection,
        damageDirection;

    private Vector2
        movement,
        touchDamageBotLeft,
        touchDamageTopRight;

    private bool
        groundDetected,
        wallDetected,
        isAttacking;

    private GameObject alive;
    private Rigidbody2D aliveRb;
    private Animator aliveAnim;

    private Vector2 previousPosition;
    private Vector2 currentPosition;
    private bool isMovingRight;

    public GameObject player;
    private float speed = 3f;
    private float distance;

    public GameObject conversa;
    public GameObject pausa;

    public GameObject vida1, vida2, vida3, vida4, vida5;

    private void Start()
    {
        alive = transform.Find("Alive").gameObject;
        aliveRb = alive.GetComponent<Rigidbody2D>();
        aliveAnim = alive.GetComponent<Animator>();

        currentHealth = maxHealth;
        facingDirection = 1;

        previousPosition = transform.position;
        SwitchState(State.Moving);
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Moving:
                UpdateMovingState();
                break;
            case State.Knockback:
                UpdateKnockbackState();
                break;
            case State.Dead:
                UpdateDeadState();
                break;
            case State.Attacking:
                UpdateAttackingState();
                break;
        }

        if (conversa.activeSelf == false && pausa.activeSelf == false)
        {
            distance = player.transform.position.x - transform.position.x;

            if (Mathf.Abs(distance) <= attackRange)
            {
                aliveAnim.SetBool("Attack", true);
                EnterAttackingState();
            }
            else
            {
                Vector2 direction = player.transform.position - transform.position;
                transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
            }



            currentPosition = transform.position;

            if (currentPosition.x > previousPosition.x)
            {
                isMovingRight = true;
                StartCoroutine(moveresquerda());
            }
            else if (currentPosition.x < previousPosition.x)
            {
                isMovingRight = false;
                StartCoroutine(moverdireita());
            }

            previousPosition = currentPosition;

        }
    }

    IEnumerator moverdireita()
    {
        yield return new WaitForSeconds(0.2f);
        alive.transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    IEnumerator moveresquerda()
    {
        yield return new WaitForSeconds(0.2f);
        alive.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void EnterMovingState() { }

    private void UpdateMovingState()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);

        CheckTouchDamage();

        if (!groundDetected || wallDetected)
        {
            Flip();
        }
    }

    private void ExitMovingState() { }

    private void EnterKnockbackState()
    {
        knockbackStartTime = Time.time;
        movement.Set(knockbackSpeed.x * damageDirection, knockbackSpeed.y);
        aliveRb.velocity = movement;
        aliveAnim.SetBool("Knockback", true);
    }

    private void UpdateKnockbackState()
    {
        if (Time.time >= knockbackStartTime + knockbackDuration)
        {
            SwitchState(State.Moving);
        }
    }

    private void ExitKnockbackState()
    {
        aliveAnim.SetBool("Knockback", false);
    }

    private void EnterDeadState()
    {
        Instantiate(deathChunkParticle, alive.transform.position, deathChunkParticle.transform.rotation);
        Instantiate(deathBloodParticle, alive.transform.position, deathBloodParticle.transform.rotation);
        Destroy(gameObject);
    }

    private void UpdateDeadState() { }

    private void ExitDeadState() { }

    private void EnterAttackingState()
    {
        aliveAnim.SetBool("Attack", true);
        isAttacking = true;
    }

    private void UpdateAttackingState()
    {
        if (aliveAnim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            SwitchState(State.Moving);
        }
    }

    private void ExitAttackingState()
    {
        aliveAnim.SetBool("Attack", false);
    }

    private void Damage(float[] attackDetails)
    {
        currentHealth -= attackDetails[0];
        Debug.Log(currentHealth);
        if (currentHealth < 10.0f)
        {
            vida5.SetActive(false);
            vida4.SetActive(false);
            vida3.SetActive(false);
            vida2.SetActive(false);
            vida1.SetActive(false);
        }
        else if (currentHealth <= 10.0f)
        {
            vida5.SetActive(false);
            vida4.SetActive(false);
            vida3.SetActive(false);
            vida2.SetActive(false);
        }
        else if (currentHealth <= 20.0f)
        {
            vida4.SetActive(false);
            vida3.SetActive(false);
            vida2.SetActive(false);
        }
        else if (currentHealth <= 30.0f)
        {
            vida3.SetActive(false);
            vida2.SetActive(false);
        }
        else if (currentHealth <= 40.0f)
        {
            vida2.SetActive(false);
        }

        Instantiate(hitParticle, alive.transform.position, Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)));

        if (attackDetails[1] > alive.transform.position.x)
        {
            damageDirection = -1;
        }
        else
        {
            damageDirection = 1;
        }

        if (currentHealth > 0.0f)
        {
            SwitchState(State.Knockback);
        }
        else if (currentHealth <= 0.0f)
        {
            SwitchState(State.Dead);
        }
    }



    private void CheckTouchDamage()
    {
        if (Time.time >= lastTouchDamageTime + touchDamageCooldown)
        {
            touchDamageBotLeft.Set(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
            touchDamageTopRight.Set(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

            Collider2D hit = Physics2D.OverlapArea(touchDamageBotLeft, touchDamageTopRight, whatIsPlayer);

            if (hit != null)
            {
                lastTouchDamageTime = Time.time;
                attackDetails[0] = touchDamage;
                attackDetails[1] = alive.transform.position.x;
                hit.SendMessage("Damage", attackDetails);
            }
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        alive.transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void SwitchState(State state)
    {
        switch (currentState)
        {
            case State.Moving:
                ExitMovingState();
                break;
            case State.Knockback:
                ExitKnockbackState();
                break;
            case State.Dead:
                ExitDeadState();
                break;
            case State.Attacking:
                ExitAttackingState();
                break;
        }

        switch (state)
        {
            case State.Moving:
                EnterMovingState();
                break;
            case State.Knockback:
                EnterKnockbackState();
                break;
            case State.Dead:
                EnterDeadState();
                break;
            case State.Attacking:
                EnterAttackingState();
                break;
        }

        currentState = state;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance, wallCheck.position.y));

        Vector2 botLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 botRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y - (touchDamageHeight / 2));
        Vector2 topRight = new Vector2(touchDamageCheck.position.x + (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));
        Vector2 topLeft = new Vector2(touchDamageCheck.position.x - (touchDamageWidth / 2), touchDamageCheck.position.y + (touchDamageHeight / 2));

        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
    }
}
