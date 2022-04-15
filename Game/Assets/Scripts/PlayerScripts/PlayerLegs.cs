using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerLegs : MonoBehaviour
{
    private Rigidbody2D Body;
    private Rigidbody2D rigidbody2D;
    private float Acceleration = 10f;
    private Vector2 moveVec;
    public Animator Animator;

    void Start()
    {
        rigidbody2D = rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void UpdateAnimation()
    {
        //var velocity = rigidbody2D.velocity;

        Animator.SetFloat("Speed", moveVec.magnitude);
        Animator.SetFloat("Horizontal", moveVec.x);
        Animator.SetFloat("Vertical", moveVec.y);
        
        /*if (velocity.magnitude > 0.2f)
        {
            Animator.SetFloat("HorizontalState", velocity.x);
            Animator.SetFloat("VerticalState", velocity.y);
        }*/
        


    }

    void Move()
    {
        rigidbody2D.AddForce(moveVec.normalized * Acceleration);
        rigidbody2D.velocity *= 0.75f;
    }

    private void DirectionMove()
    {
        if (rigidbody2D.velocity.magnitude > 0.2f)
            rigidbody2D.rotation = Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x) * Mathf.Rad2Deg + 270;
    }
}