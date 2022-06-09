using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossController : Enemy
{
    [SerializeField] float attackRange = 4.0f;
    [SerializeField] float raidCooldownSeconds = 10.0f;
    [SerializeField] float dropBombCooldownSeconds = 3.0f;
    [SerializeField] GameObject bombPrefab;
    Vector2 playerPosition;
    Vector2 bottomDirection = Vector2.up;
    float contactThreshold = 30;
    bool raidCooldownActive = false;
    bool dropBombCooldownActive = false;
    public bool raidUnderway = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        playerPosition = player.transform.position;

        if (isPlayerInAttackRange())
        {
            // GetComponent<ThirdBossMovement>().stopMove = true;
            attack();
        }
    }

    bool isPlayerInAttackRange()
    {
        float distanceToPlayer = Vector2.Distance(playerPosition, transform.position);
        if (distanceToPlayer <= attackRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void attack()
    {
        if (!raidUnderway)
        {
            if (!raidCooldownActive)
            {
                raidUnderway = true;
                Debug.Log("RaidStarted");
                GetComponent<ThirdBossMovement>().beginAttack();
            }
            else if (!dropBombCooldownActive)
            {

                dropBomb();

            }
        }

    }

    public void dropBomb()
    {
        float colliderHeight = GetComponent<CapsuleCollider2D>().size.y;
        Vector2 bombPosition = new Vector2(transform.position.x, transform.position.y - colliderHeight);
        Instantiate(bombPrefab, bombPosition, Quaternion.identity);
        StartCoroutine(activateDropBombCooldown());
    }

    public void attackEnded()
    {
        raidUnderway = false;
        StartCoroutine(activateAttackCooldown());
    }

    protected override void collisionWithPlayer(Collision2D collision)
    {
        checkCollisionDirection(collision.contacts);
    }

    void checkCollisionDirection(ContactPoint2D[] allCollisionPoints)
    {

        for (int i = 0; i < allCollisionPoints.Length; i++)
        {
            if (Vector2.Angle(allCollisionPoints[i].normal, bottomDirection) <= contactThreshold)
            {
                bottomCollision();
            }
            else
            {
                bouncePlayer();
                hurtPlayer();
            }
        }
    }

    void bottomCollision()
    {
        // bouncePlayer();
        hurt();
    }

    public IEnumerator activateAttackCooldown()
    {
        raidCooldownActive = true;
        Debug.Log("attackCooldownActive");
        yield return new WaitForSeconds(raidCooldownSeconds);
        raidCooldownActive = false;
        Debug.Log("attackCooldownDeactive");
    }

    public IEnumerator activateDropBombCooldown()
    {
        dropBombCooldownActive = true;
        Debug.Log("dropBombCooldownActive");
        yield return new WaitForSeconds(dropBombCooldownSeconds);
        dropBombCooldownActive = false;
        Debug.Log("dropBombCooldownDeactive");
    }
}
