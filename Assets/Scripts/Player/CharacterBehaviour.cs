using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour {

    public int health = 5;
    public int marks = 0;
    public float enemyHitCooldown = 1.5f;
    private float nextHitTimer;
    private Rigidbody2D rb2D;

    private void Start() {
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider) {

        if (collider.gameObject.tag.Equals("Beer")) {
            health += 1;
            Destroy(collider.gameObject);
            
            Debug.Log("Beer collected. Health: " + health);
        } else if (collider.gameObject.tag.Equals("Mark")) {
            marks += 1;
            Destroy(collider.gameObject);
            
            Debug.Log("Mark collected. Marks: " + marks);
        }
        if (collider.CompareTag("IntegralCage"))
        {
            Physics2D.IgnoreLayerCollision(3,8, true);//3 Player Layer //8 IntegralCageLayer
        }
    }

    private void OnCollisionEnter2D(Collision2D collider) {
        if (collider.gameObject.tag.Equals("Spikes")) {
            if (nextHitTimer <= 0) {
                health -= 1;
                nextHitTimer = enemyHitCooldown;
            }
            else {
                nextHitTimer -= Time.deltaTime;
            }
            
            Debug.Log("Health: " + health);
        }
    }
}
