using UnityEngine;

public class CharacterController2D : MonoBehaviour {
    
    public float movementSpeed = 3f;
    public float jumpForce = 300f;
    private float moveHorizontal;
    private float moveVertical;

    private Rigidbody2D rb2D;
    private Vector2 currentVelocity;

    private bool isJumping;
    private bool alreadyJumping;
    
    void Start() {
        rb2D = GetComponent<Rigidbody2D>();
    }

    
    void Update() {
        HandleMovement();
        HandleJumping();
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
        moveVertical = Input.GetAxis("Vertical");
        currentVelocity = rb2D.velocity;
    }

    private void FixedUpdate() {
        if (moveHorizontal != 0) {
            rb2D.velocity = new Vector2(moveHorizontal * movementSpeed, currentVelocity.y);
        }

        if (isJumping && !alreadyJumping) {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
            alreadyJumping = true;
        }
    }
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag.Equals("Floor")) {
            isJumping = false;
        }
    }
}
