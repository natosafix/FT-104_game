using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastExplosion : MonoBehaviour
{
    private CameraJiggle cameraShake;
    private Rigidbody2D player;
    private Rigidbody2D rigidbody2D;
    
    void Start()
    {
        var audioSource = GetComponent<AudioSource>();
        player = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        cameraShake = Camera.main.GetComponent<CameraJiggle>();
        cameraShake.JiggleCamera(
            2 / Mathf.Abs(Vector2.Distance(player.position, rigidbody2D.position) - 4));
        Destroy(gameObject, audioSource.clip.length);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7)
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null && enemy.IsAlive())
            {
                enemy.SetDamage(1);
            }
        }
    }
}
