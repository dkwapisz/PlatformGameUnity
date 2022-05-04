using UnityEngine;

public class CharacterController2D : MonoBehaviour {
    
    public float movementSpeed = 3f;
    public float jumpForce = 300f;
    public float crouchPercentOfHeight = 0.5f;
    private float moveHorizontal;

    private Rigidbody2D rb2D;
    private BoxCollider2D playerCollider2D;
    private PhysicsMaterial2D playerMaterial;
    private Vector2 currentVelocity;

    private Vector2 standColliderSize;
    private Vector2 standColliderOffset;
    private Vector2 crouchColliderSize;
    private Vector2 crouchColliderOffset;

    private bool isJumping;
    private bool alreadyJumping;
    private bool isCrouching;

    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
        playerCollider2D = GetComponent<BoxCollider2D>();
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
        ChangeFrictionByVelocity();
    }

    private void FixedUpdate() {
        MoveHorizontal();
        Jump();
        Crouch();
        StandUp();
    }

    private void ChangeFrictionByVelocity() {
        if (rb2D.velocity.y != 0 || rb2D.velocity.x == 0) {
            playerCollider2D.sharedMaterial.friction = 0f;
        } else {
            playerCollider2D.sharedMaterial.friction = 0.4f;
        }
        
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
    
    private void MoveHorizontal() {
        if (moveHorizontal != 0) {
            rb2D.velocity = new Vector2(moveHorizontal * movementSpeed, currentVelocity.y);
        }
    }

    private void Jump() {
        if (isJumping && !alreadyJumping) {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
            alreadyJumping = true;
        }
    }

    private void Crouch() {
        if (isCrouching) {
            playerCollider2D.size = crouchColliderSize;
            playerCollider2D.offset = crouchColliderOffset;
        }
    }

    private void StandUp() {
        if (!isCrouching) {
            playerCollider2D.size = standColliderSize;
            playerCollider2D.offset = standColliderOffset;
        }
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

        return hitDown.collider;
    }
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag.Equals("Floor") && rb2D.velocity.y == 0 && IsPlayerTouchingGround()) {
            isJumping = false;
        }
    }
}
