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
    private const int IgnoreLayers = 9;
    private const int EnemyLayer = 7;
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
        Debug.Log(hitInfo.gameObject.layer);
        if (hitInfo is null || hitInfo.gameObject.layer == IgnoreLayers)
            return;
        Instantiate(ImpactAnim, thisTransform.position, Quaternion.identity);
        if (hitInfo.gameObject.layer == EnemyLayer)
        { 
            Entity enemy = hitInfo.gameObject.GetComponent<Entity>();
            enemy.SetDamage(1f);
        }

        DestroyShot();
    }
    
    void Update()
    {
        
    }
    
    void DestroyShot()
    {
        Destroy(gameObject);
    }
}