using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowableBulletController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float destroyDelay = 0.2f;
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag.Equals("Player")) {
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.tag.Equals("Player"))
        {
            StartCoroutine(destroyObject());
        }
    }

    IEnumerator destroyObject() {
        yield return new WaitForSeconds(destroyDelay);
        Destroy(gameObject);
    }
}
