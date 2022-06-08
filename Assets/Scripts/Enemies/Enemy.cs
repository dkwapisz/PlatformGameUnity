using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int healthPoints = 1;
    [SerializeField] protected int bounceForce = 8;
    protected bool died;
    protected GameObject player;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        died = false;
        player = GameObject.FindGameObjectWithTag("Player");
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
