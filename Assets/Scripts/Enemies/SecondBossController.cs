using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SecondBossController : Enemy
{
    // Start is called before the first frame update
    [SerializeField] float attackRange = 4.0f;
    [SerializeField] float attackDelaySeconds = 1.0f;
    Vector2 playerPosition;
    float distanceToPlayer;
    bool attackCooldown = false;
    
    private GameObject boss2Sprite;
    private Animator animator;

    void Awake()
    {
        boss2Sprite = GameObject.FindGameObjectWithTag("Boss2Sprite");
        animator = boss2Sprite.GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (isPlayerInAttackRange() && !attackCooldown) {
            attack();
            StartCoroutine(playAttackAnimation());
        }
    }
    
    public override void hurt(int damage = 1) {
        base.hurt();
        animator.SetTrigger("Hit");
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

    IEnumerator playAttackAnimation() {
        int randomNumber = Random.Range(1, 6);
        if (randomNumber is 1 or 2) {
            yield return new WaitForSeconds(attackDelaySeconds/2);
            animator.SetTrigger("Attack");
        } else if (randomNumber is 3 or 4) {
            yield return new WaitForSeconds(attackDelaySeconds/2);
            animator.SetTrigger("Attack 2");
        } else if (randomNumber is 5 or 6) {
            yield return new WaitForSeconds(attackDelaySeconds/2);
            animator.SetTrigger("Attack 3");
        }
            
        //Debug.Log(randomNumber);
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
