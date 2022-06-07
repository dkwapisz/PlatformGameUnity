using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasketballSounds : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private AudioSource bounceSoundEffect;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        bounceSoundEffect.Play();
    }
}
