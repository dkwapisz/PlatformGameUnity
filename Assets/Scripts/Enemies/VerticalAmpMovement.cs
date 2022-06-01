using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalAmpMovement : MonoBehaviour
{

    [SerializeField] float amplitude = 5f;
    private Vector2 startPosition;
    private float shift;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "Floor")
        {
            Debug.Log("GOOMBA: Move direction changed");
            amplitude = -amplitude;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        shift = amplitude * Mathf.Sin(Time.time / 2);
        transform.position = startPosition + new Vector2(shift, 0.0f);
    }
}
