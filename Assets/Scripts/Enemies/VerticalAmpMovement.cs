using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalAmpMovement : MonoBehaviour
{

    [SerializeField] float amplitude = 5f;
    private Vector2 startPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startPosition + new Vector2(amplitude*Mathf.Sin(Time.time / 2), 0.0f);
    }
}
