using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMove : MonoBehaviour
{
    public int Damage = 25;
    public float BulletSpeed = 10.0f;
    public float DestroyTime = 2.0f;
    public GameObject ImpactAnim;
    public GameObject DemonExplosionAnim;
    protected HashSet<int> ignoreLayers = new HashSet<int>{0, 9, 10, 11, 12};
    protected HashSet<int> hitLayers = new HashSet<int> {6, 7};
    protected Transform thisTransform;
    protected Rigidbody2D rigidbody2D;
    
    protected void SetUp()
    {
        thisTransform = transform;
        rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.AddForce(new Vector2(0, 1).Rotate(rigidbody2D.rotation).normalized * BulletSpeed, ForceMode2D.Impulse);
        Invoke(nameof(DestroyShot), DestroyTime);
    }

    protected void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo is null || ignoreLayers.Contains(hitInfo.gameObject.layer))
            return;
        if (hitLayers.Contains(hitInfo.gameObject.layer))
        {
            var enemy = hitInfo.gameObject.GetComponent<Entity>();
            if (enemy is null || !enemy.IsAlive())
                return;
            enemy.SetDamage(1f);
            Instantiate(DemonExplosionAnim, thisTransform.position, Quaternion.identity);
        }
        else
        {
            if (thisTransform != null)
                Instantiate(ImpactAnim, thisTransform.position, Quaternion.identity);
        }
        DestroyShot();
    }

    void DestroyShot()
    {
        Destroy(gameObject);
    }
}
