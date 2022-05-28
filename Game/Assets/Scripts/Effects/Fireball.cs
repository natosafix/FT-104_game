using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Fireball : MonoBehaviour
{
    [Header("Settings")]
    public float FireballSpeed = 4.0f;
    public float DestroyTime = 4.0f;
    
    [Header("Anim")]
    public GameObject ImpactAnim;

    [Header("Sounds")] 
    public AudioClip SpawnBall;
    public AudioClip BallFlight;
    
    
    private HashSet<int> ignoreLayers = new HashSet<int>{0, 6, 9, 10, 11, 12};
    private const int EnemyLayer = 7;
    private Transform thisTransform;
    private Rigidbody2D rigidbody2D;
    private AudioSource audioSource;
    private CameraJiggle cameraJiggle;
    private Rigidbody2D player;
    private bool isSpawned;
    private float spawnDelay = 0.55f;
    
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        thisTransform = transform;
        rigidbody2D = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        cameraJiggle = Camera.main.GetComponent<CameraJiggle>();
        audioSource.PlayOneShot(SpawnBall);
        audioSource.PlayOneShot(BallFlight);
        
        Destroy(gameObject, DestroyTime);
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (ignoreLayers.Contains(hitInfo.gameObject.layer))
            return;
        if (hitInfo.gameObject.layer == EnemyLayer)
        {
            var enemy = hitInfo.gameObject.GetComponent<Entity>();
            enemy.SetDamage(1f);
        }
        Destroy(gameObject);
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

    void FixedUpdate()
    {
        cameraJiggle.JiggleCamera(
            0.1f / Mathf.Abs(Vector2.Distance(player.position, rigidbody2D.position) + 4f));
    }
    
    void OnDestroy()
    {
        Instantiate(ImpactAnim, thisTransform.position, 
            Quaternion.Euler(0, 0, Random.Range(0, 180)));
    }
}