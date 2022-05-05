using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basketball_Interference : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Basketball"))
        {
            Debug.Log("Player touched Basketball");
        }
    }
}
