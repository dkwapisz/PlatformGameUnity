using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondBossController : Enemy
{
    // Start is called before the first frame update
    [SerializeField] float attackRange = 4.0f;
    [SerializeField] float attackDelaySeconds = 1.0f;
    Vector2 playerPosition;
    float distanceToPlayer;
    bool attackCooldown = false;
    
    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (isPlayerInAttackRange() && !attackCooldown) {
            attack();
        }
    }

    bool isPlayerInAttackRange() {
        playerPosition = player.transform.position;
        distanceToPlayer = Vector2.Distance(transform.position, playerPosition);

        if (distanceToPlayer <= attackRange) {
            return true;
    
        } else {
            return false;
        }
    }

    void attack() {
        StartCoroutine(generateShockwave());
    }

    IEnumerator generateShockwave() {
        attackCooldown = true;
        Debug.Log("Loading shockwave");
        GetComponent<FirstBossMovement>().stopMoving = true;
        yield return new WaitForSeconds(attackDelaySeconds);

        distanceToPlayer = Vector2.Distance(transform.position, playerPosition);
        if (distanceToPlayer <= attackRange) {
            player.GetComponent<CharacterBehaviour>().hurt();
            bouncePlayer();
        }
        Debug.Log("SHOCKWAVE");

        yield return new WaitForSeconds(attackDelaySeconds/2);
        GetComponent<FirstBossMovement>().stopMoving = false;
        attackCooldown = false;
    }

}
