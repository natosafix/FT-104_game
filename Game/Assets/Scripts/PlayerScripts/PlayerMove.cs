using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.UIElements;

using Vector2 = UnityEngine.Vector2;

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
        UpdateAnimation();
    }
    
    void FixedUpdate()
    {
        Move();
        //DirectionMove();
    }

    private void UpdateAnimation()
    {
        var moveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        var velocity = rigidbody2D.velocity;
        
        Animator.SetFloat("Speed", moveVec.magnitude);

        if (Input.GetMouseButton((int) MouseButton.RightMouse))
        {
            var mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            var playerPos = (Vector2) Camera.main.WorldToScreenPoint(transform.position);
            moveVec = (mousePos - playerPos).normalized;
            
            Animator.SetFloat("Horizontal", moveVec.x);
            Animator.SetFloat("Vertical", moveVec.y);
            Animator.SetFloat("HorizontalState", moveVec.x);
            Animator.SetFloat("VerticalState", moveVec.y);
        }
        else
        {
            Animator.SetFloat("Horizontal", moveVec.x);
            Animator.SetFloat("Vertical", moveVec.y);
            if (velocity.magnitude > 0.2f)
            {
                Animator.SetFloat("HorizontalState", velocity.x);
                Animator.SetFloat("VerticalState", velocity.y);
            }
        }
        
        
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
