using UnityEngine;
using UnityEngine.SceneManagement;

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
    private bool spriteIsOffsetted;

    private Vector2 standColliderSize;
    private Vector2 standColliderOffset;
    private Vector2 crouchColliderSize;
    private Vector2 crouchColliderOffset;

    // private bool isJumping;
    // private bool alreadyJumping;
    private bool isCrouching;
    private bool isRunning;
    private bool isGrounded = true;
  
    private bool throwBullet;
    public int forwardDirection;
    
    ReverseGravity reverseGravity;

    [SerializeField] private AudioSource jumpSoundEffect;
    [SerializeField] private AudioSource walkSoundEffect;

    void Awake()
    {
        reverseGravity = GameObject.Find("Player").GetComponent<ReverseGravity>();
    }

    void Start() {
        forwardDirection =  1;

        rb2D = GetComponent<Rigidbody2D>();
        playerSprite = GameObject.FindGameObjectWithTag("PlayerSprite");
        animator = playerSprite.GetComponent<Animator>();
        spriteRenderer = playerSprite.GetComponent<SpriteRenderer>();
        playerCollider2D = GetComponent<BoxCollider2D>();
        playerBehaviour = GetComponent<CharacterBehaviour>();
        playerMaterial = new PhysicsMaterial2D();
        playerMaterial.friction = 0.4f;
        playerCollider2D.sharedMaterial = playerMaterial;
        spriteIsOffsetted = false;

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
        //HandleDeath();
        HandleThrow();
        HandleExit();
    }

    private void FixedUpdate() {
        MoveHorizontal();
        // Jump();
        Crouch();
        StandUp();
    }

    private void HandleRunning() {
        if (Input.GetKey(KeyCode.LeftShift)) {
            isRunning = true;
        }
        else {
            isRunning = false;
        }
    }

    private void HandleFlipSprite() {
        if (rb2D.velocity.x < 0)
        {
            spriteRenderer.flipX = true;
            if (!spriteIsOffsetted)  {
                spriteRenderer.transform.position -= new Vector3( 1 , 0 , 0);
                spriteIsOffsetted = true;
            }

        }
        else if (rb2D.velocity.x > 0){
            spriteRenderer.flipX = false;
            if (spriteIsOffsetted)  {
                spriteRenderer.transform.position += new Vector3( 1 , 0 , 0);
                spriteIsOffsetted = false;
            }
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
            // if (!isJumping) {
            //     isJumping = true;
            //     alreadyJumping = false;
            // }
            if (isGrounded) {
                Jump();
            }
        }
    }

    private void HandleExit() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            SceneManager.LoadScene("Menu");
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
            if (moveHorizontal > 0) {
                if (forwardDirection != 1) {
                    playerTurnedBack(1);

                }

            } else if (moveHorizontal < 0) {
                if (forwardDirection != -1) {
                    playerTurnedBack(-1);
            
                }

            }
        }
        else {
            animator.SetBool("IsRunning", false);
        }
    }

    private void Jump() {
        // if (isJumping && !alreadyJumping) {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
            isGrounded = false;
            // alreadyJumping = true;
            jumpSoundEffect.Play();
            animator.SetTrigger("Jump");
        // }
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

    public void playerTurnedBack(int factor) {
        forwardDirection = factor;
        GetComponent<CharacterBehaviour>().playerTurnedBack();
        // Debug.Log("Player turned back.");
        
        // Debug.Log("Player turned back.");
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
        
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position,
            Vector2.up,
            standColliderSize.magnitude);
        
        RaycastHit2D hitUpLeftLeft = Physics2D.Raycast(transform.position - transform.right / 2,
            Vector2.up,
            standColliderSize.magnitude);
        
        RaycastHit2D hitUpLeft = Physics2D.Raycast(transform.position - transform.right / 4,
            Vector2.up,
            standColliderSize.magnitude);
        
        RaycastHit2D hitUpRightRight = Physics2D.Raycast(transform.position + transform.right / 2,
            Vector2.up,
            standColliderSize.magnitude);
        
        RaycastHit2D hitUpRight = Physics2D.Raycast(transform.position + transform.right / 4,
            Vector2.up,
            standColliderSize.magnitude);
        
       
        
        if (reverseGravity.isGravityNormal) {
             return hitDown.collider || 
                    hitDownLeft.collider || 
                    hitDownRight.collider || 
                    hitDownLeftLeft.collider || 
                    hitDownRightRight.collider;
        } else {
            return hitUp.collider || 
                   hitUpLeft.collider || 
                   hitUpRight.collider || 
                   hitUpLeftLeft.collider || 
                   hitUpRightRight.collider;
        }
    }
    
    
    private void OnCollisionEnter2D(Collision2D collision) {
        // if ((collision.gameObject.tag.Equals("Floor") || collision.gameObject.tag.Equals("Spikes")) 
        //     && rb2D.velocity.y == 0 && IsPlayerTouchingGround()) {
        //     isJumping = false;
        // }
        if ((collision.gameObject.tag.Equals("Floor") || collision.gameObject.tag.Equals("Spikes")) &&
                rb2D.velocity.y == 0 && IsPlayerTouchingGround()) {
            isGrounded = true;
        }
    }

    public void HandleDeath() {
        animator.SetTrigger("Dies");
    }
}
