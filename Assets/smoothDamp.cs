using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class smoothDamp : MonoBehaviour
{
    private Vector3 initialPosition;
    private Vector3 currentPosition;
    
    private float smoothTime = 0.05f;
    private Vector3 velocity = Vector3.zero;

    void Awake()
    {
        initialPosition = transform.position;
    }
    
    void Update()
    {
        if (transform.parent != null)
        {
            Vector3 targetPosition = transform.parent.TransformPoint(transform.localPosition);
            
            transform.position = Vector3.SmoothDamp(initialPosition, targetPosition, ref velocity, smoothTime);

            currentPosition = transform.position;
            initialPosition = currentPosition;
        }
    }
}