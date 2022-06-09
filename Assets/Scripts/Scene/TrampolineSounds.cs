using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineSounds : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioSource jumpTrampolineSoundEffect;
        

    
    private void OnCollisionEnter2D(Collision2D collider) {
        if (collider.gameObject.tag.Equals("Trampoline")) {
            Debug.Log("Player touched trampoline");
            jumpTrampolineSoundEffect.Play();
        }
    }

    
}
    