using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdBossMovement : MonoBehaviour
{
    [SerializeField] float viewingDistance = 14.0f;
    [SerializeField] float speed = 1.0f;
    [SerializeField] float offsetAbovePlayer = 2.8f;
    [SerializeField] float raidOffsetAbovePlayer = 4.2f;
    [SerializeField] float raidRange = 5.0f;
    [SerializeField] int raidDensity = 10;
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
    Vector2 lastBombDroppedPostion;
    float gapBetweenBombs;
    private SpriteRenderer spriteRenderer;
    private GameObject boss3Sprite;
    private Animator animator;
    
    [SerializeField] private AudioSource fireballSoundEffect;
   // [SerializeField] private AudioSource explodeSoundEffect;
    
    // private SpriteRenderer bulletSpriteRenderer;
    // private GameObject bulletSprite;
    // private Animator bulletAnimator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidbody = GetComponent<Rigidbody2D>();
        gapBetweenBombs = raidRange * 2 / raidDensity - destinationOffset / raidDensity;
        boss3Sprite = GameObject.FindGameObjectWithTag("Boss3Sprite");
        animator = boss3Sprite.GetComponent<Animator>();
        spriteRenderer = boss3Sprite.GetComponent<SpriteRenderer>();
        spriteRenderer.flipX = true;

        // bulletSprite = GameObject.FindGameObjectWithTag("BulletSprite");
        // bulletAnimator = bulletSprite.GetComponent<Animator>();
        // bulletSpriteRenderer = bulletSprite.GetComponent<SpriteRenderer>();
        
        // playerPosition = player.transform.position;
        // flightDirection.x = 0;
        // flightDirection.y = playerPosition.y - transform.position.y + offsetAbovePlayer;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPosition = player.transform.position;
        distanceToPlayer = Vector2.Distance(playerPosition, transform.position);
        if (distanceToPlayer <= viewingDistance && baseMovement)
        {
            followPlayer();

        }
        else if (attackUnderway)
        {
            flyOverPlayer();

        }
    }

    void followPlayer()
    {
        // Debug.Log("Following player");
        flightDirection.y = playerPosition.y - transform.position.y + offsetAbovePlayer;
        flightDirection.x = playerPosition.x - transform.position.x;
        rigidbody.velocity = transform.TransformDirection(flightDirection * speed);

    }

    void flyOverPlayer()
    {

        flightDirection.y = playerPositionWhenAttackStarted.y - transform.position.y + raidOffsetAbovePlayer;

        if (flightToStartAttackPosition)
        {
            // Debug.Log("Lot na start");
            if (transform.position.x >= playerPositionWhenAttackStarted.x + raidRange - destinationOffset)
            {
                flightToStartAttackPosition = false;
                flightToEndAttackPosition = true;
                spriteRenderer.flipX = true;
            }
            flightDirection.x = playerPositionWhenAttackStarted.x - transform.position.x + raidRange;

        }
        else if (flightToEndAttackPosition)
        {
            dropBomb();
            // Debug.Log("Lot na koniec");
            if (transform.position.x <= playerPositionWhenAttackStarted.x - raidRange + destinationOffset)
            {
                endAttak();
                spriteRenderer.flipX = false;
                
            }
            flightDirection.x = playerPositionWhenAttackStarted.x - transform.position.x - raidRange;
        }

        rigidbody.velocity = transform.TransformDirection(flightDirection * speed);

    }

    void dropBomb()
    {
        if (Vector2.Distance(lastBombDroppedPostion, transform.position) >= gapBetweenBombs)
        {
            GetComponent<ThirdBossController>().dropBomb();
            lastBombDroppedPostion = transform.position;
            animator.SetTrigger("Attack");
        }
    }

    public void beginAttack()
    {
        StartCoroutine(fireBallSound());
        lastBombDroppedPostion = new Vector2(1000.0f, 1000.0f); // big values to be sure that first bomb will be dropped
        playerPositionWhenAttackStarted = player.transform.position;
        baseMovement = false;
        attackUnderway = true;
        flightToStartAttackPosition = true;
        flightToEndAttackPosition = false;
    }

    void endAttak()
    {
        Debug.Log("Koniec ataku");
        flightToEndAttackPosition = false;
        baseMovement = true;
        attackUnderway = false;
        GetComponent<ThirdBossController>().attackEnded();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (attackUnderway)
        {
            if (flightToStartAttackPosition)
            {
                flightToStartAttackPosition = false;
                flightToEndAttackPosition = true;
            }
            else if (flightToEndAttackPosition)
            {
                endAttak();
            }
        }
    }
    
    IEnumerator fireBallSound()
    {
        yield return new WaitForSeconds(3f);
        fireballSoundEffect.Play();
        
    }
}
