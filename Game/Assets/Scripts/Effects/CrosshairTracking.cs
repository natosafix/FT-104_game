using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairTracking : MonoBehaviour
{
    private Transform thisTransform;
    
    void Start()
    {
        thisTransform = transform;
    }

    void Update()
    {
        Vector2 tmp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        thisTransform.position = tmp;
    }
}
