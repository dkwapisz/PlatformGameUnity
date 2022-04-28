using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CameraFollow : MonoBehaviour {

    private Transform camTransform;
    
    [SerializeField]
    public Transform target;
    public float followSpeed = 2f;

    private Vector3 originalPos;

    private void Awake() {
        Cursor.visible = false;

        if (camTransform == null) {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    private void OnEnable() {
        originalPos = camTransform.localPosition;
    }

    void Update() {
        Vector3 newPosition = target.position;
        newPosition.z = -10;
        transform.position = Vector3.Slerp(transform.position, newPosition, followSpeed);
    }
    
}
