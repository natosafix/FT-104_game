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
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

using Vector2 = UnityEngine.Vector2;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    private float Acceleration = 10f;
    public Animator Animator;
    private Vector2 moveVec;
    private const int PPU = 64;
    
    void Start()
    {
        rigidbody2D = rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //UpdateAnimation();
        moveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetMouseButton((int) MouseButton.RightMouse))
        {
            var mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            var playerPos = (Vector2) Camera.main.WorldToScreenPoint(transform.position);
            var playerToMouseVec = (mousePos - playerPos).normalized;

            rigidbody2D.rotation = Mathf.Atan2(playerToMouseVec.y, playerToMouseVec.x) * Mathf.Rad2Deg + 270;
        }
        else
        {
            DirectionMove();
        }
    }

    /*private void LateUpdate()
    {
        rigidbody2D.position.x = (Mathf.Round(parent.position.x * PPU) / PPU) - parent.position.x; 
        rigidbody2D.position.y = (Mathf.Round(parent.position.y * PPU) / PPU) - parent.position.y;
    }*/

    void FixedUpdate()
    {
        Move();
        //DirectionMove();
    }

    private void UpdateAnimation()
    {
        moveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        var velocity = rigidbody2D.velocity;
        
        Animator.SetFloat("Speed", moveVec.magnitude);

        if (Input.GetMouseButton((int) MouseButton.RightMouse))
        {
            var mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            var playerPos = (Vector2) Camera.main.WorldToScreenPoint(transform.position);
            var playerToMouseVec = (mousePos - playerPos).normalized;
            
            Animator.SetFloat("Horizontal", playerToMouseVec.x);
            Animator.SetFloat("Vertical", playerToMouseVec.y);
            Animator.SetFloat("HorizontalState", playerToMouseVec.x);
            Animator.SetFloat("VerticalState", playerToMouseVec.y);
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
        rigidbody2D.AddForce(moveVec.normalized * Acceleration);
        rigidbody2D.velocity *= 0.75f;
    }
    
    private void DirectionMove()
    {
        if (rigidbody2D.velocity.magnitude > 0.2f)
            rigidbody2D.rotation = Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x) * Mathf.Rad2Deg + 270;
    }
}