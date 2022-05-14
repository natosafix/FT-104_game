using System.Linq;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerLegs : MonoBehaviour
{
    public GameObject Player;
    public Animator Animator;

    public AudioClip[] footsteps;
    private AudioSource audioSource;

    private Vector2 moveVec;
    private Rigidbody2D playerRigidbody2D;
    private Transform thisTransform;
    private Transform playerTransform;
    
    void Start()
    {
        Player = GameObject.Find("Player");
        playerTransform = Player.transform;
        thisTransform = transform;
        audioSource = GetComponent<AudioSource>();
        playerRigidbody2D = Player.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        thisTransform.position = playerTransform.position;
    }

    private void UpdateAnimation()
    {
        Animator.SetFloat("Speed", moveVec.magnitude);
        Animator.SetFloat("Horizontal", moveVec.x);
        Animator.SetFloat("Vertical", moveVec.y);
    }
    
    void FootStep()
    {
        if (playerRigidbody2D.velocity.magnitude < 0.2f)
            return;
        var i = Random.Range(0, footsteps.Length);
        audioSource.PlayOneShot(footsteps[i]);
    }
}