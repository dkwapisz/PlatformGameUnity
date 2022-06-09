using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossController : Enemy
{
    [SerializeField] float attackRange = 4.0f;
    Vector2 playerPosition;
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

        if (isPlayerInAttackRange()) {
            // GetComponent<ThirdBossMovement>().stopMove = true;
            attack();
        }
    }

    bool isPlayerInAttackRange() {
        float distanceToPlayer = Vector2.Distance(playerPosition, transform.position);
        if (distanceToPlayer <= attackRange) {
            return true;
        } else {
            return false;
        }
    }

    void attack() {
        Debug.Log("Attack");
        // TODO: Uzupełnić funkcję która będzie atakować gracza
    }
}
