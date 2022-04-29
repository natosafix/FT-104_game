using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaAttack : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 7)
        {
            var enemy = other.GetComponent<Enemy>();
            enemy.SetDamage(1);
        }
    }
}
