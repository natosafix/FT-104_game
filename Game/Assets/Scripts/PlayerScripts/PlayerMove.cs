using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    public float Acceleration = 20f;
    public Animator Animator;
    
    void Start()
    {
        rigidbody2D = rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        var moveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        var velocity = rigidbody2D.velocity;
        
        if (velocity.magnitude > 0.5f)
        {
            Animator.SetFloat("HorizontalState", velocity.x);
            Animator.SetFloat("VerticalState", velocity.y);
            Animator.SetFloat("Horizontal", velocity.x);
            Animator.SetFloat("Vertical", velocity.y);
            Animator.SetFloat("Speed", moveVec.magnitude);
        }
    }
    
    void FixedUpdate()
    {
        Move();
        //DirectionMove();
    }

    void Move()
    {
        var w = Input.GetKey(KeyCode.W) ? 1 : 0;
        var a = Input.GetKey(KeyCode.A) ? -1 : 0;
        var s = Input.GetKey(KeyCode.S) ? -1 : 0;
        var d = Input.GetKey(KeyCode.D) ? 1 : 0;
        
        var moveVec = new Vector2(a + d, w + s).normalized;

        rigidbody2D.AddForce(moveVec * Acceleration);
        rigidbody2D.velocity *= 0.75f;
    }
    
    private void DirectionMove()
    {
        rigidbody2D.rotation = Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x) * Mathf.Rad2Deg;
    }
}
