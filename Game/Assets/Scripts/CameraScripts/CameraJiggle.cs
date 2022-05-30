using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraJiggle : MonoBehaviour
{
    private Transform thisTransform;
    private float shakeRemaining;
    private float currentOffset;
    
    void Start()
    {
        thisTransform = transform;
    }

    void FixedUpdate()
    {
        if (shakeRemaining > 0)
        {
            shakeRemaining -= Time.fixedDeltaTime;
            StartCoroutine(Jiggle());
        }
    }
    
    public void JiggleCamera(float offset)
    {
        shakeRemaining = Mathf.Clamp(shakeRemaining + offset, 0, 0.4f);
    }
 
    private IEnumerator Jiggle()
    {
        while (shakeRemaining > 0)
        {
            var x = Random.Range(-shakeRemaining, shakeRemaining);
            var y = Random.Range(-shakeRemaining, shakeRemaining);
            thisTransform.position = Vector3.Lerp(
                thisTransform.position, 
                thisTransform.position + new Vector3(x, y, 0), 
                Time.deltaTime * 10);
            yield return new WaitForSeconds(0.025f);
        }
    }
    
    /*private void Shake()
    {
        var rotate = Quaternion.Euler(
            Random.Range(-shakeRemaining, shakeRemaining), 
            Random.Range(-shakeRemaining, shakeRemaining), 
            0f);
        
        thisTransform.position
        
        thisTransform.localRotation = Quaternion.Slerp(
            thisTransform.localRotation, 
            thisTransform.localRotation * rotate, 
            0.75f);
    }*/
}