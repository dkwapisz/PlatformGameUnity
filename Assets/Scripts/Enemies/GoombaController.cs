using UnityEngine;
using System.Collections;

public class GoombaController : Enemy
{
    private Vector2 topDirection = Vector2.down;
    private Vector2 bottomDirection = Vector2.up;
    private Vector2 leftDirection = Vector2.right;
    private Vector2 rightDirection = Vector2.left;
    private float contactThreshold = 30;
    private GameObject goombaSprite;
    private Animator animator;

    protected override void Start()
    {
        base.Start();
        goombaSprite = GameObject.FindGameObjectWithTag("GoombaSprite");
        animator = goombaSprite.GetComponent<Animator>();
    }

    protected override void collisionWithPlayer(Collision2D collision)
    {
        checkCollisionDirection(collision.contacts);
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
        bouncePlayer();
    }

    void rightCollision()
    {
        hurtPlayer();
        bouncePlayer();
    }

    void topCollision()
    {
        bouncePlayer();
        animator.SetTrigger("Death");
        hurt();
    }

    void bottomCollision()
    {
        bouncePlayer();
        hurt();
    }

    protected override void hurtPlayer(int damage = 1) {
        base.hurtPlayer();
        animator.SetTrigger("Attack");
    }

    protected override void bouncePlayer()
    {
        base.bouncePlayer();
        animator.SetTrigger("Death");
    }

}
