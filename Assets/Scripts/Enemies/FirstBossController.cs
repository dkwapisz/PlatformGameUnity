using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossController : Enemy
{
    [SerializeField] GameObject ownPrefab;
    public bool isChild = false;
    int childrenHealthPoints;
    private GameObject boss1Sprite;
    private Animator animator;
    private Vector2 topDirection = Vector2.down;
    private float contactThreshold = 30;

    protected override void Start()
    {
        base.Start();
        childrenHealthPoints = healthPoints/2;
        
        boss1Sprite = gameObject.transform.GetChild(0).gameObject;
        animator = boss1Sprite.GetComponent<Animator>();
    }
    
    protected override void collisionWithPlayer(Collision2D collision) {
        checkCollisionDirection(collision.contacts);
    }

    void checkCollisionDirection(ContactPoint2D[] allCollisionPoints)
    {

        for (int i = 0; i < allCollisionPoints.Length; i++)
        {
            if (Vector2.Angle(allCollisionPoints[i].normal, topDirection) <= contactThreshold)
            {
                topCollision();
            }
            else {
                bouncePlayer();
                hurtPlayer();
            }
        }
    }
    
    void topCollision()
    {
        bouncePlayer();
        hurt();
    }

    protected override void destroyObject()
    {
        // gameObject.SetActive(false);
        Vector2 parentPosition = transform.position;
        
        if (!isChild) {
            Debug.Log("Creating children");
            createChildren(parentPosition);
        }
        base.destroyObject();
    }

    void createChildren(Vector2 parentPosition) {
        Vector2 parentHopForce = GetComponent<FirstBossMovement>().hopForce;
        GameObject child1 = Instantiate(ownPrefab, parentPosition, Quaternion.identity);
        GameObject child2;
        child1.transform.localScale = child1.transform.localScale/2;
        child1.GetComponent<FirstBossController>().healthPoints = childrenHealthPoints;
        child1.GetComponent<FirstBossController>().isChild = true;
        child1.GetComponent<FirstBossMovement>().hopForce = new Vector2(parentHopForce.x, parentHopForce.y * 2);
        child1.SetActive(true);
        child2 = Instantiate(child1, parentPosition, Quaternion.identity);
        child1.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector2(3, 5));
        child2.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(new Vector2(-3, 5));
    }
}
