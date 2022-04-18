using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerLegs : MonoBehaviour
{
    public GameObject Player;
    private Vector2 moveVec;
    public Animator Animator;

    void Start()
    {
        Player = GameObject.Find("Player");
    }

    void Update()
    {
        moveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        transform.position = Player.transform.position;
    }

    private void UpdateAnimation()
    {
        Animator.SetFloat("Speed", moveVec.magnitude);
        Animator.SetFloat("Horizontal", moveVec.x);
        Animator.SetFloat("Vertical", moveVec.y);
    }
}