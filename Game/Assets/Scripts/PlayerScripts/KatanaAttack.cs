using UnityEngine;

public class KatanaAttack : MonoBehaviour
{
    public AudioClip[] WallHitSounds;
    public AudioClip[] EnemyHitSounds;

    private AudioSource audioSource;
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
            {
                var i = Random.Range(0, EnemyHitSounds.Length);
                audioSource.PlayOneShot(EnemyHitSounds[i]);
                
                enemy.SetDamage(1);
            }
        }
        else if (other.gameObject.layer == 8)
        {
            var i = Random.Range(0, WallHitSounds.Length);
            audioSource.PlayOneShot(WallHitSounds[i]);
        }
    }
}
