using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    // Start is called before the first frame update
    public int Damage = 25;
    public float FireballSpeed = 3.0f;
    public float DestroyTime = 4.0f;
    public GameObject ImpactAnim;
    private HashSet<int> IgnoreLayers = new HashSet<int> {6, 9, 10};
    private const int EnemyLayer = 7;
    private Transform thisTransform;
    private Rigidbody2D rigidbody2D;
    private bool isSpawned;
    private float spawnDelay = 0.55f;
    
    void Start()
    {
        thisTransform = transform;
        rigidbody2D = GetComponent<Rigidbody2D>();
        Invoke(nameof(DestroyShot), DestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (IgnoreLayers.Contains(hitInfo.gameObject.layer))
            return;
        if (hitInfo.gameObject.layer == EnemyLayer)
        {
            var enemy = hitInfo.gameObject.GetComponent<Entity>();
            enemy.SetDamage(1f);
        }
        DestroyShot();
        Instantiate(ImpactAnim, thisTransform.position, 
            Quaternion.Euler(0, 0, Random.Range(0, 180)));
    }

    void Update()
    {
        if (isSpawned)
            return;
        while (spawnDelay > 0)
        {
            spawnDelay -= Time.deltaTime;
            return;
        }
        isSpawned = true;
        rigidbody2D.AddForce(new Vector2(0, 1).Rotate(rigidbody2D.rotation).normalized * FireballSpeed, ForceMode2D.Impulse);
    }
    
    void DestroyShot()
    {
        Destroy(gameObject);
    }
}
