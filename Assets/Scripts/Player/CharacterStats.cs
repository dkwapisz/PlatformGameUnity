using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

    public int health = 5;
    public int marks = 0;

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
            health -= 1;
            // TODO - cooldown 1-2s after getting hit from enemy
            Debug.Log("Health: " + health);
        }
        
    }
}
