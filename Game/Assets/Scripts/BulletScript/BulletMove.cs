using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class BulletMove : MonoBehaviour
{
    public int Damage = 25;
    public float BulletSpeed = 10.0f;
    public float DestroyTime = 2.0f;
    public GameObject ImpactAnim;

    private Transform thisTransform;
    private Rigidbody2D rigidbody2D;
    
    void Start()
    {
        thisTransform = transform;
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.AddForce(new Vector2(0, 1).Rotate(rigidbody2D.rotation).normalized * BulletSpeed, ForceMode2D.Impulse);
        Invoke("DestroyShot", DestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo is null)
            return;

        if (hitInfo.tag == "AttackIgnore")
            return;

        Instantiate(ImpactAnim, thisTransform.position, Quaternion.identity);
        DestroyShot();
    }
    
    void Update()
    {
        
    }
    
    void DestroyShot()
    {
        Destroy(gameObject);
        Destroy(this);
    }
}