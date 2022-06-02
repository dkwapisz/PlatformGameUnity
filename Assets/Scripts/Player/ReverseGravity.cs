using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseGravity : MonoBehaviour
{
    private Rigidbody2D rb2D;
   // private Player player;
    public bool isGravityNormal;
    
    // Start is called before the first frame update
    void Start()
    {
     //   player = GetComponent<Player>();
        isGravityNormal = true;
        rb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag.Equals("ReverseGravityTool"))
        {
            //variable = (condition) ? expressionTrue :  expressionFalse;
            isGravityNormal = (isGravityNormal) ?  false : true;
            Reverse();
            GameObject player = GameObject.Find("Player");
            CharacterController2D characterController2D = player.GetComponent<CharacterController2D>();
            characterController2D.jumpForce *= -1;
            Debug.Log("Gravity Reversed");
            Destroy(collider.gameObject);
        }
    }
    
    private void Reverse()
    {
        rb2D.gravityScale *= -1;
        if (isGravityNormal == false){
            transform.eulerAngles = new Vector3(0, 0, 180f);
        }else
        {
            transform.eulerAngles = Vector3.zero;
        }
    }
}
