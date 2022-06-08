using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossMovement : MonoBehaviour
{
    [SerializeField] Vector2 hopForce = new Vector2(1.0f, 1.0f);
    [SerializeField] float hopCooldownSeconds = 1.0f;
    [SerializeField] float viewingDistance = 20.0f;
    Vector2 playerPosition;
    Vector2 hopLeftDirection;
    Vector2 hopRightDirection;
    GameObject player;
    Rigidbody2D rigidbody;
    public bool stopMoving = false;
    bool isGrounded = true;
    bool hopToRight = false;
    bool hopToLeft = false;
    bool hopCooldownActive = false;
    float distanceToPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidbody = GetComponent<Rigidbody2D>();
        hopRightDirection = hopForce;
        hopLeftDirection = new Vector2(-hopForce.x, hopForce.y);
        // distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPosition = player.transform.position;
        distanceToPlayer = Vector2.Distance(playerPosition, transform.position);


        if (isGrounded) {
            if (transform.position.x > playerPosition.x) {
                hopToLeft = true;
            } else if (transform.position.x < playerPosition.x) {
                hopToRight = true;
            }
        }

        if ((hopToLeft || hopToRight) && 
            !hopCooldownActive && 
            !stopMoving &&
            distanceToPlayer <= viewingDistance) {
            move();
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag.Equals("Floor")) {
            isGrounded = true;
            // Debug.Log("Grounded");
        }
    }

    void move() {
        if (hopToLeft) {
            rigidbody.velocity = transform.TransformDirection(hopLeftDirection);
            // Debug.Log("Hop on the left");
        } else if (hopToRight) {
            rigidbody.velocity = transform.TransformDirection(hopRightDirection);
            // Debug.Log("Hop on the right");
        }

        hopToRight = false;
        hopToLeft = false;
        isGrounded = false;
        StartCoroutine(activateHopCooldown());
    }

    IEnumerator activateHopCooldown()
    {
        hopCooldownActive = true;
        // Debug.Log("Hop cooldown activated");
        yield return new WaitForSeconds(hopCooldownSeconds);
        hopCooldownActive = false;
        // Debug.Log("Hop cooldown deactivated");
    }
}
