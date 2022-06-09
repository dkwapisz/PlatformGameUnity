using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [SerializeField] float explosionDelay = 1.8f;
    [SerializeField] float explosionRange = 2.0f;
    [SerializeField] int damage = 1;
    GameObject player;
    
    [SerializeField] private AudioSource explodeSoundEffect;

    void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(explodeSound());
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;

        if (!collisionObject.tag.Equals("Enemy"))
        {
            StartCoroutine(startDetonation());
        }
    } 

    IEnumerator startDetonation() {
        yield return new WaitForSeconds(explosionDelay);

        explode();

        // yield return new WaitForSeconds(explosionAnimationDuration);
        Destroy(gameObject);
    }
    
    IEnumerator explodeSound() {
        yield return new WaitForSeconds(0.4f);
        explodeSoundEffect.Play();
    }

    void explode() {
        if (Vector2.Distance(transform.position, player.transform.position) <= explosionRange) {
            player.GetComponent<CharacterBehaviour>().hurt(damage);
        }
        Debug.Log("BOOM");
        // Space to place explosion animation
    }
}
