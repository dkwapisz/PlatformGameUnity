using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBehaviour : MonoBehaviour {

    [SerializeField] float damageCooldownSeconds = 2.0f;
    [SerializeField] float throwCooldownSeconds = 0.5f;
    [SerializeField] Rigidbody2D bulletPrefab;
    public int health = 5;
    public int maxHealth = 5;
    public int marks = 0;
    public int allMarksOnScene = 0;
    public int ammo = 0;
    public float throwCooldown = 1.0f;
    public float throwForce = 12.0f;
    public float bulletTorque = 2.0f;
    Vector2 throwDirection = new Vector2(0.5f, 0.5f);
    private bool damageCooldownActive = false;
    private bool throwCooldownActive = false;
    private Rigidbody2D rb2D;
    private GameObject healthBar;
    private Slider healthSlider;
    private GameObject marksUI;
    private Text marksAmount;
    private GameObject bottlesUI;
    private Text bottlesAmount;
    
    [SerializeField] private AudioSource beerCollectSoundEffect;
    [SerializeField] private AudioSource ectsCollectSoundEffect;
    [SerializeField] private AudioSource bottleCollectSoundEffect;
    [SerializeField] private AudioSource bottleThrowSoundEffect;
    [SerializeField] private AudioSource hurtSoundEffect;
    //[SerializeField] private AudioSource burnSoundEffect;
    
    

    private void Start() {
        rb2D = GetComponent<Rigidbody2D>();

        var allMarks = GameObject.FindGameObjectsWithTag("Mark");
        allMarksOnScene = allMarks.Length;
        
        healthBar = GameObject.FindGameObjectWithTag("HealthBar");
        healthSlider = healthBar.GetComponent<Slider>();
        healthSlider.value = 1f;
        
        marksUI = GameObject.FindGameObjectWithTag("MarksUI");
        marksAmount = marksUI.GetComponentInChildren<Text>();
        marksAmount.text = "0/" + allMarksOnScene;
        
        bottlesUI = GameObject.FindGameObjectWithTag("BottlesUI");
        bottlesAmount = bottlesUI.GetComponentInChildren<Text>();
        bottlesAmount.text = "0";
    }


    void FixedUpdate() {
        updateUI();
    }

    private void updateUI() {
        healthSlider.value = health / 5f;
        marksAmount.text = marks + "/" + allMarksOnScene;
        bottlesAmount.text = ammo.ToString();
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
                if (health != maxHealth) {
                    health += 1;
                    Debug.Log("Beer collected. Health: " + health);
                    beerCollectSoundEffect.Play();
                    Destroy(collider.gameObject);
                }
                break;

            case "Mark":
                marks += 1;
                Debug.Log("Mark collected. Marks: " + marks);
                Destroy(collider.gameObject);
                ectsCollectSoundEffect.Play();
                break;

            case "Weapon":
                ammo += 10;
                Debug.Log("Empty bottles collected. Bottles: " + ammo);
                bottleCollectSoundEffect.Play();
                Destroy(collider.gameObject);
                break;
        }

        
    }

    private void OnCollisionEnter2D(Collision2D collider) {
        if (collider.gameObject.tag.Equals("Spikes")) {
            hurt();
        }
    }

    public void hurt(int damage = 1) {
        if (!damageCooldownActive)
        {
            health = health - damage;
            hurtSoundEffect.Play();
            Debug.Log("Player hurt: Current HP: " + health);
            StartCoroutine(activateDamageCooldown());
        }

    }

    public void throwBullet() {
        if (ammo > 0 && !throwCooldownActive) {
            Rigidbody2D bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            bullet.velocity = transform.TransformDirection(throwDirection * throwForce + GetComponent<Rigidbody2D>().velocity);
            bullet.AddTorque(bulletTorque, ForceMode2D.Impulse);
            bottleThrowSoundEffect.Play();
            ammo--;
            StartCoroutine(activateThrowCooldown());
            Debug.Log("Bullet thrown in direction: " + throwDirection.x);
        }
    }

    public void playerTurnedBack() {
        throwDirection.x = -throwDirection.x;
    }

    IEnumerator activateDamageCooldown()
    {
        damageCooldownActive = true;
        Debug.Log("Damage cooldown activated");
        yield return new WaitForSeconds(damageCooldownSeconds);
        damageCooldownActive = false;
        Debug.Log("Damage cooldown deactivated");
    }

    IEnumerator activateThrowCooldown()
    {
        throwCooldownActive = true;
        Debug.Log("Throw cooldown activated");
        yield return new WaitForSeconds(throwCooldownSeconds);
        throwCooldownActive = false;
        Debug.Log("Trow cooldown deactivated");
    }
}
