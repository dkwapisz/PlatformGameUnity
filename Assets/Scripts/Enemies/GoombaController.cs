using UnityEngine;
using System.Collections;

public class GoombaController : MonoBehaviour
{

    [SerializeField] int bounceForce = 8;
    private GameObject player;
    private Vector2 topDirection = Vector2.down;
    private Vector2 bottomDirection = Vector2.up;
    private Vector2 leftDirection = Vector2.right;
    private Vector2 rightDirection = Vector2.left;
    private float contactThreshold = 30;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            checkCollisionDirection(collision.contacts);
        }
    }

    void checkCollisionDirection(ContactPoint2D[] allCollisionPoints)
    {

        for (int i = 0; i < allCollisionPoints.Length; i++)
        {
            if (Vector2.Angle(allCollisionPoints[i].normal, topDirection) <= contactThreshold)
            {
                topCollision();
                // Debug.Log("GOOMBA: Collision on the top");
            }
            else if (Vector2.Angle(allCollisionPoints[i].normal, bottomDirection) <= contactThreshold)
            {
                bottomCollision();
                // Debug.Log("GOOMBA: Collision on the bottom");
            }
            else if (Vector2.Angle(allCollisionPoints[i].normal, leftDirection) <= contactThreshold)
            {
                leftCollision();
                // Debug.Log("GOOMBA: Collision on the left");
            }
            else if (Vector2.Angle(allCollisionPoints[i].normal, rightDirection) <= contactThreshold)
            {
                rightCollision();
                // Debug.Log("GOOMBA: Collision on the right");
            }
        }
    }


    void leftCollision()
    {
        hurtPlayer();
        bouncePlayer(new Vector2(-bounceForce / 2, 0));
    }

    void rightCollision()
    {
        hurtPlayer();
        bouncePlayer(new Vector2(bounceForce / 2, 0));
    }

    void topCollision()
    {
        bouncePlayer(new Vector2(0, bounceForce));
        Destroy(gameObject);
    }

    void bottomCollision()
    {
        bouncePlayer(new Vector2(0, -bounceForce));
        Destroy(gameObject);
    }

    void hurtPlayer() {

        player.GetComponent<CharacterBehaviour>().hurt();
    }

    void bouncePlayer(Vector2 force)
    {
        player.GetComponent<Rigidbody2D>().AddForce(force, ForceMode2D.Impulse);
    }

}
