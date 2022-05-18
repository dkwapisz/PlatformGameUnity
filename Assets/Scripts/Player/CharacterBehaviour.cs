using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour {

    public int health = 5;
    public int marks = 0;
    public float enemyHitCooldown = 2f;
    private float nextHitTimer;

    private void OnCollisionEnter2D(Collision2D collision) {

        if (collision.gameObject.tag.Equals("Beer")) {
            health += 1;
            Destroy(collision.gameObject);
            
            Debug.Log("Beer collected. Health: " + health);
        } else if (collision.gameObject.tag.Equals("Mark")) {
            marks += 1;
            Destroy(collision.gameObject);
            
            Debug.Log("Mark collected. Marks: " + marks);
        } else if (collision.gameObject.tag.Equals("Enemy")) {
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
