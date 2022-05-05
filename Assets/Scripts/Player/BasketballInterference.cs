using UnityEngine;

public class BasketballInterference : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Basketball"))
        {
            Debug.Log("Player touched Basketball");
        }
    }
}
