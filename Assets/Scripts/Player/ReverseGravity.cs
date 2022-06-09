using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReverseGravity : MonoBehaviour
{
    private Rigidbody2D rb2D;
   
    public bool isGravityNormal;
    
    [SerializeField] private AudioSource reverseGravityCollectSoundEffect;
    

    void Start()
    {
        isGravityNormal = true;
        rb2D = GetComponent<Rigidbody2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag.Equals("ReverseGravityTool"))
        {
            //variable = (condition) ? expressionTrue :  expressionFalse;
            isGravityNormal = (isGravityNormal) ?  false : true;
            Reverse();
            reverseGravityCollectSoundEffect.Play();
            GameObject player = GameObject.Find("Player");
            CharacterController2D characterController2D = player.GetComponent<CharacterController2D>();
            characterController2D.jumpForce *= -1;
            if (isGravityNormal)
            {
                rb2D.transform.localScale = new Vector3(1, 2, 1); 
            }
            else
            {
                 rb2D.transform.localScale = new Vector3(-1, 2, 1);
            }

           
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