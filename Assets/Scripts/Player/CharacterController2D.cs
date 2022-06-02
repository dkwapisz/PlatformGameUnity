using UnityEngine;

public class CharacterController2D : MonoBehaviour {
    
    public float movementSpeed = 3f;
    public float runningSpeedFactor = 2f;
    public float jumpForce = 300f;
    public float crouchPercentOfHeight = 0.5f;
    private float moveHorizontal;

    private Rigidbody2D rb2D;
    private BoxCollider2D playerCollider2D;
    private CharacterBehaviour playerBehaviour;
    private PhysicsMaterial2D playerMaterial;
    private Vector2 currentVelocity;
    private GameObject playerSprite;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private Vector2 standColliderSize;
    private Vector2 standColliderOffset;
    private Vector2 crouchColliderSize;
    private Vector2 crouchColliderOffset;

    private bool isJumping;
    private bool alreadyJumping;
    private bool isCrouching;
    private bool isRunning;
  
    private bool throwBullet;
    public int forwardDirection = 1;


    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        playerSprite = GameObject.FindGameObjectWithTag("PlayerSprite");
        animator = playerSprite.GetComponent<Animator>();
        spriteRenderer = playerSprite.GetComponent<SpriteRenderer>();
        playerCollider2D = GetComponent<BoxCollider2D>();
        playerBehaviour = GetComponent<CharacterBehaviour>();
        playerMaterial = new PhysicsMaterial2D();
        playerMaterial.friction = 0.4f;
        playerCollider2D.sharedMaterial = playerMaterial;

        standColliderSize = playerCollider2D.size;
        standColliderOffset = playerCollider2D.offset;
        
        crouchColliderSize = new Vector2(standColliderSize.x, standColliderSize.y * crouchPercentOfHeight);
        crouchColliderOffset = new Vector2(standColliderOffset.x, -(standColliderSize.y * crouchPercentOfHeight / 2));
    }
    
    void Update() {
        HandleMovement();
        HandleJumping();
        HandleCrouching();
        HandleRunning();
        ChangeFrictionByVelocity();
        HandleFlipSprite();
        HandleDeath();
    }

    private void FixedUpdate() {
        MoveHorizontal();
        Jump();
        Crouch();
        StandUp();
        HandleThrow();
    }

    private void HandleRunning() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            isRunning = true;
        }
        else {
            isRunning = false;
        }
    }

    private void HandleDeath() { //to expand
        if (false) {
            animator.SetTrigger("Dies");
        }
    }

    private void HandleFlipSprite() {
        if (rb2D.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else {
            spriteRenderer.flipX = false;
            //spriteRenderer.transform.position -= Vector3(-1,0,0);
        }
    }
    
    private void ChangeFrictionByVelocity() {
        if (rb2D.velocity.y != 0 || rb2D.velocity.x == 0) {
            playerCollider2D.sharedMaterial.friction = 0f;
        } else {
            playerCollider2D.sharedMaterial.friction = 0.4f;
        }
        
        // Its needed to change friction dynamically 
        playerCollider2D.enabled = false;
        playerCollider2D.enabled = true;
    }
    
    private void HandleCrouching() {
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            isCrouching = true;
        } else if (!IsPlayerUnderWall()) {
            isCrouching = false;
        }
    }

    private void HandleJumping() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (!isJumping) {
                isJumping = true;
                alreadyJumping = false;
            }
        }
    }

    private void HandleMovement() {
        moveHorizontal = Input.GetAxis("Horizontal");
        currentVelocity = rb2D.velocity;
    }

    void HandleThrow()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            ThrowBullet();
        }
    }

    private void MoveHorizontal() {
        if (moveHorizontal != 0) {
            animator.SetBool("IsRunning", true);
            if (!isRunning) {
                rb2D.velocity = new Vector2(moveHorizontal * movementSpeed, currentVelocity.y);
            } else {
                rb2D.velocity = new Vector2(moveHorizontal * movementSpeed * runningSpeedFactor, currentVelocity.y);
            }
            if (moveHorizontal < 0) {
                forwardDirection = -1;
            } else if (moveHorizontal < 0)
            {
                forwardDirection = 1;
            }
        }
        else {
            animator.SetBool("IsRunning", false);
        }
    }

    private void Jump() {
        if (isJumping && !alreadyJumping) {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
            alreadyJumping = true;
            animator.SetTrigger("Jump");
        }
    }

    private void Crouch() {
        if (isCrouching) {
            playerCollider2D.size = crouchColliderSize;
            playerCollider2D.offset = crouchColliderOffset;
            animator.SetBool("Crouch", true);
        }
        else
        {
            animator.SetBool("Crouch", false);
        }
    }

    private void StandUp() {
        if (!isCrouching) {
            playerCollider2D.size = standColliderSize;
            playerCollider2D.offset = standColliderOffset;
        }
    }

    void ThrowBullet() {
        playerBehaviour.throwBullet();
    }

    private bool IsPlayerUnderWall() {
        RaycastHit2D hitLeft1 = Physics2D.Raycast(
            transform.position - transform.right / 2, 
            Vector2.up,
            crouchColliderSize.magnitude);
        
        RaycastHit2D hitRight1 = Physics2D.Raycast(
            transform.position + transform.right / 2, 
            Vector2.up,
            crouchColliderSize.magnitude);
        
        RaycastHit2D hitLeft2 = Physics2D.Raycast(
            transform.position - transform.right / 4, 
            Vector2.up,
            crouchColliderSize.magnitude);
        
        RaycastHit2D hitRight2 = Physics2D.Raycast(
            transform.position + transform.right / 4, 
            Vector2.up,
            crouchColliderSize.magnitude);
        
        return hitLeft1.collider || hitRight1.collider || hitLeft2.collider || hitRight2.collider ;
    }

    private bool IsPlayerTouchingGround() {
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position,
            Vector2.down,
            standColliderSize.magnitude);
        
        RaycastHit2D hitDownLeftLeft = Physics2D.Raycast(transform.position - transform.right / 2,
            Vector2.down,
            standColliderSize.magnitude);
        
        RaycastHit2D hitDownLeft = Physics2D.Raycast(transform.position - transform.right / 4,
            Vector2.down,
            standColliderSize.magnitude);
        
        RaycastHit2D hitDownRightRight = Physics2D.Raycast(transform.position + transform.right / 2,
            Vector2.down,
            standColliderSize.magnitude);
        
        RaycastHit2D hitDownRight = Physics2D.Raycast(transform.position + transform.right / 4,
            Vector2.down,
            standColliderSize.magnitude);

        return hitDown.collider || 
               hitDownLeft || 
               hitDownRight || 
               hitDownLeftLeft || 
               hitDownRightRight;
    }
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag.Equals("Floor") && rb2D.velocity.y == 0 && IsPlayerTouchingGround()) {
            isJumping = false;
        }
    }
}
