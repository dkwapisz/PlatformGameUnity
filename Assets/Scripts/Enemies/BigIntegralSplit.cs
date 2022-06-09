using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BigIntegralSplit : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    private Vector2 spawnPosition;
    
    void Start()
    {
        Debug.Log("I am able to split"); 
    }
    
    void Update()
    {
        spawnPosition = transform.position;
        // Debug.Log(spawnPosition);
        if (Input.GetKey(KeyCode.P))
        {
            OnSpawnPrefab();
            OnSpawnPrefab();
            Destroy(gameObject);//destroys gameobject which has this script attached 
        }
    }

    public void OnSpawnPrefab()
    {
        float x = Random.Range(0, 15);
        float y = Random.Range(2, 4);
        Instantiate(prefab, spawnPosition + new Vector2(x,y), Quaternion.identity);
    }
}
