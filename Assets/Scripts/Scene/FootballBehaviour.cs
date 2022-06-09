using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballBehaviour : MonoBehaviour
{
    
    [SerializeField] GameObject footBallGate;
    [SerializeField] private AudioSource goalSoundEffect;
    public bool wasGoalSoundPlayed = false;
    
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("FootballGate"))
        {
            if (col is BoxCollider2D)
            {
                 Debug.Log("GOAL");
                 if (wasGoalSoundPlayed == false)
                 {
                     goalSoundEffect.Play();
                     wasGoalSoundPlayed = true;
                     var door = GameObject.FindGameObjectWithTag("BossDoor");
                     Destroy(door);
                 }

                 footBallGate.GetComponent<CapsuleCollider2D>().enabled = true; //prevents ball from escaping footballgate
            }
        }
    }
}
