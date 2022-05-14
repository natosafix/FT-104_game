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
    private HashSet<int> IgnoreLayers = new HashSet<int> { 9, 10, 11, 12};
    private const int EnemyLayer = 7;
    private Transform thisTransform;
    private Rigidbody2D rigidbody2D;
    
    void Start()
    {
        thisTransform = transform;
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.AddForce(new Vector2(0, 1).Rotate(rigidbody2D.rotation).normalized * BulletSpeed, ForceMode2D.Impulse);
        Invoke(nameof(DestroyShot), DestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo is null || IgnoreLayers.Contains(hitInfo.gameObject.layer))
            return;
        if (hitInfo.gameObject.layer == EnemyLayer)
        {
            var enemy = hitInfo.gameObject.GetComponent<Entity>();
            enemy.SetDamage(1f);
        }
        DestroyShot();
        Instantiate(ImpactAnim, thisTransform.position, Quaternion.identity);
    }

    void Update()
    {
    }
    
    void DestroyShot()
    {
        Destroy(gameObject);
    }
}