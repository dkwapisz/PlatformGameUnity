using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableBulletController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float destroyDelay = 0.5f;
    [SerializeField] int damage = 1;
    [SerializeField] private AudioSource bottleCrackSoundEffect;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("Player")) {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject collisionObject = collision.gameObject;
        if (!collisionObject.tag.Equals("Player"))
        {
            StartCoroutine(destroyObject());
            if (collisionObject.tag.Equals("Enemy")) {
                collisionObject.GetComponent<Enemy>().hurt(damage);
            }
        }
    }

    IEnumerator destroyObject() {
        bottleCrackSoundEffect.Play();
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
