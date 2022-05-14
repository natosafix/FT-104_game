using System.Linq;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class PlayerLegs : MonoBehaviour
{
    public GameObject Player;
    public Animator Animator;

    private Vector2 moveVec;
    private Transform thisTransform;
    private Transform playerTransform;

    void Start()
    {
        Player = GameObject.Find("Player");
        playerTransform = Player.transform;
        thisTransform = transform;
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
}