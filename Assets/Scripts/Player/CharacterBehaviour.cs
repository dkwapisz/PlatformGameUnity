using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class CharacterBehaviour : MonoBehaviour {

    [SerializeField] int cooldownSeconds = 2;
    [SerializeField] Rigidbody2D bulletPrefab;
    public int health = 5;
    public int marks = 0;
    public int ammo = 0;
    public float throwCooldown = 1.0f;
    public float throwForce = 12.0f;
    public float bulletTorque = 2.0f;
    Vector2 throwDirection = new Vector2(0.5f, 0.5f);
    private bool damageCooldownActive = false;
    private Rigidbody2D rb2D;

    private void Start() {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        refreshThrowDirection();
    }

    private void OnTriggerEnter2D(Collider2D collider) {

        if (collider.gameObject.tag.Equals("Beer") ||
            collider.gameObject.tag.Equals("Mark") ||
            collider.gameObject.tag.Equals("Weapon")) {

            collect(collider);
        }
        if (collider.CompareTag("IntegralCage"))
        {
            Physics2D.IgnoreLayerCollision(3,8, true);//3 Player Layer //8 IntegralCageLayer
        }
    }
    
    void collect(Collider2D collider) {

        switch(collider.gameObject.tag) {
            case "Beer":
                health += 1;
                Debug.Log("Beer collected. Health: " + health);
                break;

            case "Mark":
                marks += 1;
                Debug.Log("Mark collected. Marks: " + marks);
                break;

            case "Weapon":
                ammo += 10;
                Debug.Log("Empty bottles collected. Bottles: " + ammo);
                break;
        }

        Destroy(collider.gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collider) {
        if (collider.gameObject.tag.Equals("Spikes")) {
            hurt();
        }
    }

    public void hurt() {
        if (!damageCooldownActive)
        {
            health -= 1;
            Debug.Log("Player hurt: Current HP: " + health);
            StartCoroutine(activateCooldown());
        }

    }

    public void throwBullet() {
        if (ammo > 0) {
            Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.velocity = transform.TransformDirection(throwDirection * throwForce);
            bullet.AddTorque(bulletTorque, ForceMode2D.Impulse);
            ammo--;
            Debug.Log("Bullet thrown in direction: " + throwDirection.x);
        }
    }

    public void refreshThrowDirection() {
        throwDirection.x = throwDirection.x * GetComponent<CharacterController2D>().forwardDirection;
    }

    IEnumerator activateCooldown()
    {
        damageCooldownActive = true;
        Debug.Log("Damage cooldown activated");
        yield return new WaitForSeconds(cooldownSeconds);
        damageCooldownActive = false;
        Debug.Log("Damage cooldown deactivated");
    }
}
