using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int healthPoints = 1;
    [SerializeField] protected int bounceForce = 8;
    protected bool died;
    protected GameObject player;

    private Animator _animator;
    private GameObject boss1Sprite;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        died = false;
        player = GameObject.FindGameObjectWithTag("Player");
        if (gameObject == GameObject.FindGameObjectWithTag("Boss1Sprite"))
        {
            boss1Sprite = GameObject.FindGameObjectWithTag("Boss1Sprite");
            _animator = boss1Sprite.GetComponent<Animator>();
        }
    }

    protected virtual void FixedUpdate()
    {
       if (died) {
           Destroy(gameObject.GetComponent<Animator>());
           Destroy(gameObject.GetComponent<SpriteRenderer>());
           Destroy(gameObject);
       }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag.Equals("Player"))
        {
            collisionWithPlayer(collision);
            //Debug.Log("im in");
            // if (gameObject == GameObject.FindGameObjectWithTag("Boss1Sprite")) {
            //     int randomNumber = Random.Range(1, 6);
            //     if (randomNumber is 1 or 2) {
            //         _animator.SetTrigger("Attack");
            //     } else if (randomNumber is 3 or 4) {
            //         _animator.SetTrigger("Attack 2");
            //     } else if (randomNumber is 5 or 6) {
            //         _animator.SetTrigger("Attack 3");
            //     }
            //     Debug.Log(randomNumber);
            // }
        
        }
    }

    public void hurt(int damage = 1) {
        healthPoints = healthPoints - damage;
        if (healthPoints <= 0) {
            died = true;
        }
        Debug.Log("Enemy hurt. HP: " + healthPoints);
    }

    protected virtual void collisionWithPlayer(Collision2D collision) {
        hurtPlayer();
        bouncePlayer();
    }

    protected virtual void hurtPlayer(int damage = 1) {
        player.GetComponent<CharacterBehaviour>().hurt(damage);
    }

    protected virtual void bouncePlayer()
    {
        Vector3 playerDirection = player.transform.position;
        Vector3 bounceDirection = (playerDirection - transform.position).normalized;
        player.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(bounceDirection * bounceForce);
    }
    
    protected virtual void destroyObject() {
        Destroy(gameObject);
    }
}
