using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeGun : MonoBehaviour
{
    public AudioClip TakeSound;
    private AudioSource audioSource;
    public int ammoCount = 10;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        {
            audioSource.PlayOneShot(TakeSound);
            GetComponent<Renderer>().enabled = false;
            GetComponent<Light>().enabled = false;
            var player = other.gameObject.GetComponent<PlayerMove>();
            player.ammoLeft += ammoCount;
            player.WeaponPanel.GetComponent<WeaponChange>().ChangeAmmoLeft(player.ammoLeft);
            Destroy(this.gameObject, 1);
        }
    }
}
