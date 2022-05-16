using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastExplosion : MonoBehaviour
{
    void Start()
    {
        var audioSource = GetComponent<AudioSource>();
        Destroy(gameObject, audioSource.clip.length);
    }

    void Update()
    {
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7)
        {
            var enemy = other.GetComponent<Enemy>();
            if (enemy != null && enemy.IsAlive())
                enemy.SetDamage(1);
        }
    }
}
