using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalAmpMovement : MonoBehaviour
{

    [SerializeField] float range = 5f;
    [SerializeField] float speed = 0.1f;
    [SerializeField] float smoothTime = 0.5f;
    float offset = 0.01f;
    private SpriteRenderer spriteRenderer;
    private GameObject goombaSprite;
    Vector2 leftLimit, righLimit, velocity;
    Vector2 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        goombaSprite = GameObject.FindGameObjectWithTag("GoombaSprite");
        spriteRenderer = goombaSprite.GetComponent<SpriteRenderer>();
        leftLimit = new Vector2(transform.position.x - range, transform.position.y);
        righLimit = new Vector2(transform.position.x + range, transform.position.y);
        targetPosition = righLimit;
        spriteRenderer.flipX = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Floor")
        {
            turnBack();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        transform.position = Vector2.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime,
            speed
        );

        if (transform.position.x >= righLimit.x - offset ||
            transform.position.x <= leftLimit.x + offset) {

            turnBack();
        }
    }

    void turnBack() {
        
        if (targetPosition == righLimit) {
            spriteRenderer.flipX = false;
            targetPosition = leftLimit;

        } else {
            spriteRenderer.flipX = true;
            targetPosition = righLimit;
            
        }
        Debug.Log("GOOMBA: Turned back");
    }
}
