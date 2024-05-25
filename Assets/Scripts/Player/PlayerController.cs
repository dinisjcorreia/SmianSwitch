using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float health;
    public float maxHealth = 3f;
    private float movementInputDirection;
    private float jumpTimer;
    private float turnTimer;
    private float knockbackStartTime;
    [SerializeField]
    private float knockbackDuration;
    private bool knockback;

    [SerializeField]
    private Vector2 knockbackSpeed;
    private float wallJumpTimer;
    private float dashTimeLeft;
    private float lastImageXpos;
    private float lastDash = -100f;
    private HingeJoint2D hingeJoint;
    public float grabRange = 0.5f;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;
    private int lastWallJumpDirection;

    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool canNormalJump;
    private bool canWallJump;
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;
    private bool hasWallJumped;
    private bool ledgeDetected;
    private bool isDashing;
    private bool isOnPlatform;

    private Rigidbody2D rb;
    private Animator anim;
    private Rigidbody2D platform;

    public int amountOfJumps = 1;

    public float movementSpeed = 10.0f;
    public float jumpForce = 16.0f;
    public float groundCheckRadius;
    public float wallCheckDistance;
    public float wallSlideSpeed;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float wallHopForce;
    public float wallJumpForce;
    public float jumpTimerSet = 0.15f;
    public float turnTimerSet = 0.1f;
    public float wallJumpTimerSet = 0.5f;
    public float dashTime;
    public float dashSpeed;
    public float distanceBetweenImages;
    public float dashCoolDown;

    private bool setParentNextFrame;
    private Transform parentTransform;

    public Vector2 wallHopDirection;
    public Vector2 wallJumpDirection;

    public Transform groundCheck;
    public Transform wallCheck;
    public Transform ledgeCheck;

    public LayerMask whatIsGround;

    public GameObject plataforma;
    public GameObject menuPausa;
  

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
        wallHopDirection.Normalize();
        wallJumpDirection.Normalize();
        hingeJoint = gameObject.AddComponent<HingeJoint2D>();
        hingeJoint.enabled = false;

        health= 3f;
    }
    public GameObject dialogueBox;
    // Update is called once per frame
    void Update()
    {
        if (health <= 0){
            RestartScene();
        }

        if (menuPausa.gameObject.activeInHierarchy==false && dialogueBox.gameObject.activeInHierarchy==false)
        {
            CheckInput();
            CheckMovementDirection();
            UpdateAnimations();
            CheckIfCanJump();
            CheckIfWallSliding();
            CheckJump();
            CheckDash();
            CheckForRopeInteraction();
            CheckForRestart();
            CheckKnockback();

            if (SceneManager.GetActiveScene().name == "Primeiro" || SceneManager.GetActiveScene().name == "BOSS"){
                 if (gameObject.transform.position.y < -25){
                RestartScene();
                }

             if (gameObject.transform.position.y > 2.5){
                RestartScene();
                }
            }

            if (SceneManager.GetActiveScene().name == "Main" ){
                 if (gameObject.transform.position.y < 1){
                RestartScene();
            }

            

            

            
            }
            
        }
           
        
        
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();

        if (platform != null)
        {
            rb.velocity += platform.velocity;
        }
    }

    private void CheckForRopeInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            AttachToRope();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            DetachFromRope();
        }
    }

    void AttachToRope()
    {
        if (hingeJoint.enabled) return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, grabRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("RopeSegment"))
            {
                hingeJoint.connectedBody = collider.GetComponent<Rigidbody2D>();
                hingeJoint.enabled = true;
                rb.velocity = Vector2.zero;  // Stop player movement when attaching
                rb.gravityScale = 0;  // Optional: Disable gravity while on rope
                break;
            }
        }
    }

        void DetachFromRope()
        {
            hingeJoint.enabled = false;
            hingeJoint.connectedBody = null;
            rb.gravityScale = 1;  // Restore default gravity
            rb.drag = 0;  // Restore default drag
            rb.angularDrag = 0.05f;  // Restore default angular drag (adjust as needed)
        }

    private void CheckIfWallSliding()
    {
        if (isTouchingWall && movementInputDirection == facingDirection && rb.velocity.y < 0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void CheckForRestart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartScene();
        }
    }

    private void RestartScene()
    {
        anim.SetBool("Restart", true);
        float restartDelay = GetRestartAnimationDuration();
        StartCoroutine(RestartWithDelay(restartDelay));
    }
    private IEnumerator RestartWithDelay(float delay)
    {
        // Wait for the animation to finish
        yield return new WaitForSeconds(delay);

        // Restart the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private float GetRestartAnimationDuration()
    {
        // Retrieve the duration of the restart animation
        AnimatorClipInfo[] clipInfo = anim.GetCurrentAnimatorClipInfo(0);
        return clipInfo[0].clip.length;
    }


    public bool GetDashStatus()
    {
        return isDashing;
    }

     public void Knockback(int direction)
    {
        knockback = true;
        knockbackStartTime = Time.time;
        rb.velocity = new Vector2(knockbackSpeed.x * direction, knockbackSpeed.y);
    }

      private void CheckKnockback()
    {
        if(Time.time >= knockbackStartTime + knockbackDuration && knockback)
        {
            knockback = false;
            rb.velocity = new Vector2(0.0f, rb.velocity.y);
        }
    }
    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            isOnPlatform = true;
            parentTransform = collision.transform;
            setParentNextFrame = true;
            platform = collision.gameObject.GetComponent<Rigidbody2D>();
        }

         if (collision.gameObject.CompareTag("Botao"))
        {
            plataforma.SetActive(true);
        }

        if (collision.gameObject.CompareTag("Final"))
        {
            if (SceneManager.GetActiveScene().name == "Primeiro"){
                 SceneManager.LoadScene("Main");
            } else if (SceneManager.GetActiveScene().name == "Main"){
                 SceneManager.LoadScene("Cidade");
            }
           
        }
    }

    private void LateUpdate()
    {
        if (setParentNextFrame)
        {
            transform.parent = parentTransform;
            setParentNextFrame = false;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovingPlatform"))
        {
            isOnPlatform = false;
            transform.parent = null;
            platform = null;
        }

          if (collision.gameObject.CompareTag("Botao"))
        {
            plataforma.SetActive(false);
        }
    }

    private void CheckIfCanJump()
    {
        if(isGrounded && rb.velocity.y <= 0.01f)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        if (isTouchingWall)
        {
            checkJumpMultiplier = false;
            canWallJump = true;
        }

        if(amountOfJumpsLeft <= 0)
        {
            canNormalJump = false;
        }
        else
        {
            canNormalJump = true;
        }
      
    }

    private void CheckMovementDirection()
    {
        if(isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if(!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        if(Mathf.Abs(rb.velocity.x) >= 0.01f)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isWallSliding", isWallSliding);

       /*  // Check if the player is on the platform and not moving
        if (isOnPlatform && !isWalking && isGrounded)
        {
            // Play the idle animation
            anim.SetBool("isIdle", true);
        }
        else
        {
            // Otherwise, don't play the idle animation
            anim.SetBool("isIdle", false);
        } */
    }
    public GameObject rodar;
    public GameObject camara;
    public GameObject player;
    private bool rodado=false;
    private void Rodar(){
        if (rodado == true){
            camara.transform.rotation = Quaternion.Euler(0, 0, 0);
            player.transform.rotation = Quaternion.Euler(0, 0, 0);
             player.GetComponent<Rigidbody2D>().gravityScale = 8;
             rodado =false;
        }else {
             camara.transform.rotation = Quaternion.Euler(180, 0, 0);
                player.transform.rotation = Quaternion.Euler(180, 0, 0);
                player.GetComponent<Rigidbody2D>().gravityScale = -8;
                rodado=true;
        }

       /*  if (rodar.transform.position.y == -20){
            rodar.transform.position = new Vector3(rodar.transform.position.x, 0, rodar.transform.position.z);
        rodar.transform.rotation = Quaternion.Euler(0, rodar.transform.rotation.y, rodar.transform.rotation.z);
        }
        else{
            rodar.transform.position = new Vector3(rodar.transform.position.x, -20, rodar.transform.position.z);
        rodar.transform.rotation = Quaternion.Euler(180, rodar.transform.rotation.y, rodar.transform.rotation.z);
        } */
        
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");

       if (Input.GetKeyDown(KeyCode.T))
        {
            Rodar();
        }
        if (Input.GetButtonDown("Jump"))
        {
            if(isGrounded || (amountOfJumpsLeft > 0 && !isTouchingWall))
            {
                NormalJump();
            }
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        if(Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if(!isGrounded && movementInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
        }

        if (turnTimer >= 0)
        {
            turnTimer -= Time.deltaTime;

            if(turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }

        if (Input.GetButtonDown("Dash"))
        {
            if(Time.time >= (lastDash + dashCoolDown))
            AttemptToDash();
        }

    }

    private void AttemptToDash()
    {
        isDashing = true;
        dashTimeLeft = dashTime;
        lastDash = Time.time;

        PlayerAfterImagePool.Instance.GetFromPool();
        lastImageXpos = transform.position.x;
    }

    public int GetFacingDirection()
    {
        return facingDirection;
    }

    private void CheckDash()
    {
        if (isDashing)
        {
            if(dashTimeLeft > 0)
            {
                canMove = false;
                canFlip = false;
                rb.velocity = new Vector2(dashSpeed * facingDirection, 0.0f);
                dashTimeLeft -= Time.deltaTime;

                if (Mathf.Abs(transform.position.x - lastImageXpos) > distanceBetweenImages)
                {
                    PlayerAfterImagePool.Instance.GetFromPool();
                    lastImageXpos = transform.position.x;
                }
            }

            if(dashTimeLeft <= 0 || isTouchingWall)
            {
                isDashing = false;
                canMove = true;
                canFlip = true;
            }
            
        }
    }

    private void CheckJump()
    {
        if(jumpTimer > 0)
        {
            //WallJump
            if(!isGrounded && isTouchingWall && movementInputDirection != 0 && movementInputDirection != facingDirection)
            {
                WallJump();
            }
            else if (isGrounded)
            {
                NormalJump();
            }
        }
       
        if(isAttemptingToJump)
        {
            jumpTimer -= Time.deltaTime;
        }

        if(wallJumpTimer > 0)
        {
            if(hasWallJumped && movementInputDirection == -lastWallJumpDirection)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0.0f);
                hasWallJumped = false;
            }else if(wallJumpTimer <= 0)
            {
                hasWallJumped = false;
            }
            else
            {
                wallJumpTimer -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player collided with a teleportation point
        if (collision.CompareTag("Teleport"))
        {
            // Get the Teleport component from the collided object
            Teleport teleport = collision.GetComponent<Teleport>();

            // If the Teleport component exists, move the player to the teleport destination
            if (teleport != null)
            {
                transform.position = teleport.GetDestination().position;
            }
        }

    }

        private void NormalJump()
    {
        if (canNormalJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
        }
    }

    private void WallJump()
    {
        if (canWallJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
            isWallSliding = false;
            amountOfJumpsLeft = amountOfJumps;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * movementInputDirection, wallJumpForce * wallJumpDirection.y);
            rb.AddForce(forceToAdd, ForceMode2D.Impulse);
            jumpTimer = 0;
            isAttemptingToJump = false;
            checkJumpMultiplier = true;
            turnTimer = 0;
            canMove = true;
            canFlip = true;
            hasWallJumped = true;
            wallJumpTimer = wallJumpTimerSet;
            lastWallJumpDirection = -facingDirection;

        }
    }
        private void ApplyMovement()
    {

        if (!isGrounded && !isWallSliding && movementInputDirection == 0 && !knockback)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if(canMove && !knockback)
        {
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
        

        if (isWallSliding)
        {
            if(rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }
    public void DisableFlip()
    {
        canFlip = false;
    }

    public void EnableFlip()
    {
        canFlip = true;
    }

     private void Flip()
    {
        if (!isWallSliding && canFlip && !knockback)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
    }
}
