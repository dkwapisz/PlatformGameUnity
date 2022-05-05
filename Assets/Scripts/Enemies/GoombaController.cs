using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaController : MonoBehaviour
{

    [SerializeField] int bounceForce = 8;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            ContactPoint2D[] allCollisionPoints = new ContactPoint2D[collision.contactCount];
            collision.GetContacts(allCollisionPoints);

            checkIfCollisionFromUp(allCollisionPoints);
        }
    }

    void checkIfCollisionFromUp(ContactPoint2D[] allCollisionPoints){
        int counter = 0;
        float y_position;

        y_position = allCollisionPoints[0].point.y;

        foreach (var i in allCollisionPoints){

            if (i.point.y == y_position) counter++;
        }

        if (counter >= allCollisionPoints.Length){
            bouncePlayer();
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Kolizja od boku");
        }
    }

    void bouncePlayer(){
        player.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, bounceForce), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
