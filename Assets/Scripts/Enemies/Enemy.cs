using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int healthPoints = 1;
    [SerializeField] protected int bounceForce = 8;
    private float damageCooldownSeconds = 1.0f;
    protected bool died;
    protected GameObject player;
    private bool damageCooldownActive = false;
    private Animator _animator;
    private GameObject boss1Sprite;
    
    //[SerializeField] private AudioSource enemyDeathSoundEffect;
    //[SerializeField] private AudioSource enemyCasualSoundEffect;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        died = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void FixedUpdate()
    {
        //enemyCasualSoundEffect.Play();
        ///do testowania dzwieku POCZATEK
        if (Input.GetKey(KeyCode.P))
        {
            died = true;
        }
        ///do testowania KONIEC
       if (died) { 
           //DeathSoundEffect.Play();
           StartCoroutine(destroyEnemyObject());
       }
    }
    
    IEnumerator destroyEnemyObject() {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag.Equals("Player"))
        {
            collisionWithPlayer(collision);
        
        }
    }

    public virtual void hurt(int damage = 1) {
        if (!damageCooldownActive) {
            healthPoints = healthPoints - damage;
            StartCoroutine(activateDamageCooldown());
        }
        if (healthPoints <= 0) {
            died = true;
        }
        Debug.Log("Enemy hurt. HP: " + healthPoints);
    }

    IEnumerator activateDamageCooldown()
    {
        damageCooldownActive = true;
        Debug.Log("Damage cooldown activated");
        yield return new WaitForSeconds(damageCooldownSeconds);
        damageCooldownActive = false;
        Debug.Log("Damage cooldown deactivated");
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
