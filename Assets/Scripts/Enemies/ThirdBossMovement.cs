using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossMovement : MonoBehaviour
{
    [SerializeField] float viewingDistance = 14.0f;
    [SerializeField] float speed = 1.0f;
    [SerializeField] float offsetAbovePlayer = 4.0f;
    Rigidbody2D rigidbody;
    GameObject player;
    Vector2 playerPosition;
    Vector2 flightDirection;
    float distanceToPlayer;
    public bool stopMove = false;
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
        if (distanceToPlayer <= viewingDistance && !stopMove)  {

            flyToPlayer();
        }
    }

    void flyToPlayer() {
        flightDirection.y = playerPosition.y - transform.position.y + offsetAbovePlayer;
        flightDirection.x = playerPosition.x - transform.position.x;
        rigidbody.velocity = transform.TransformDirection(flightDirection);

    }
}
