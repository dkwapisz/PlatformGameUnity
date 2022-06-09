using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossMovement : MonoBehaviour
{
    [SerializeField] float viewingDistance = 14.0f;
    [SerializeField] float speed = 1.0f;
    [SerializeField] float offsetAbovePlayer = 4.0f;
    [SerializeField] float raidOffsetAbovePlayer = 3.0f;
    [SerializeField] float bombDropRange = 5.0f;
    Rigidbody2D rigidbody;
    GameObject player;
    Vector2 playerPosition;
    Vector2 playerPositionWhenAttackStarted;
    Vector2 flightDirection;
    float distanceToPlayer;
    bool baseMovement = true;
    bool attackUnderway = false;
    bool flightToStartAttackPosition = false;
    bool flightToEndAttackPosition = false;
    float destinationOffset = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidbody = GetComponent<Rigidbody2D>();
        playerPosition = player.transform.position;
        flightDirection.x = 0;
        flightDirection.y = playerPosition.y - transform.position.y + offsetAbovePlayer;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPosition = player.transform.position;
        distanceToPlayer = Vector2.Distance(playerPosition, transform.position);
        if (distanceToPlayer <= viewingDistance && baseMovement)  {
            followPlayer();

        } else if (attackUnderway) {
            flyOverPlayer();

        }
    }

    void followPlayer() {
        // Debug.Log("Following player");
        flightDirection.y = playerPosition.y - transform.position.y + offsetAbovePlayer;
        flightDirection.x = playerPosition.x - transform.position.x;
        rigidbody.velocity = transform.TransformDirection(flightDirection * speed);

    }

    void flyOverPlayer() {

        flightDirection.y = playerPositionWhenAttackStarted.y - transform.position.y + raidOffsetAbovePlayer;

        if (flightToStartAttackPosition) {
            // Debug.Log("Lot na start");
            if (transform.position.x >= playerPositionWhenAttackStarted.x + bombDropRange - destinationOffset) {
                flightToStartAttackPosition = false;
                flightToEndAttackPosition = true;
            }
            flightDirection.x = playerPositionWhenAttackStarted.x - transform.position.x + bombDropRange;
        
        } else if (flightToEndAttackPosition) {
            // Debug.Log("Lot na koniec");
            if (transform.position.x <= playerPositionWhenAttackStarted.x - bombDropRange + destinationOffset) {
                endAttak();
            }
            flightDirection.x = playerPositionWhenAttackStarted.x - transform.position.x - bombDropRange;
        }

        rigidbody.velocity = transform.TransformDirection(flightDirection * speed);

    }

    public void beginAttack() {
        playerPositionWhenAttackStarted = player.transform.position;
        baseMovement = false;
        attackUnderway = true;
        flightToStartAttackPosition = true;
        flightToEndAttackPosition = false;
    }

    void endAttak() {
        Debug.Log("Koniec ataku");
        flightToEndAttackPosition = false;
        baseMovement = true;
        attackUnderway = false;
        GetComponent<ThirdBossController>().attackEnded();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (attackUnderway) {
            if (flightToStartAttackPosition) {
                flightToStartAttackPosition = false;
                flightToEndAttackPosition = true;
            } else if (flightToEndAttackPosition) {
                endAttak();
            }
        }
    }
}
