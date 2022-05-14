using UnityEngine;

public class DestroyExplosion : MonoBehaviour
{
    public AudioClip[] Sounds;
    
    private AudioSource audioSource;

    void Start()
    {
        var i = Random.Range(0, Sounds.Length);
        
        audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(Sounds[i]);
        Destroy(gameObject, Mathf.Max(2, Sounds[i].length));
    }

    void Update()
    {
    }
}
