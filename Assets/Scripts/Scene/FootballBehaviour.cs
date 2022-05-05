using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballBehaviour : MonoBehaviour
{
   
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("FootballGate"))
        {
            Debug.Log("GOAL!");
        }
    }
}
