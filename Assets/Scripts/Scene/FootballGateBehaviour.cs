using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballGateBehaviour : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            Physics2D.IgnoreLayerCollision(3,6, true);//3 Player Layer //6 Football Gate Layer
        }
    }
}
