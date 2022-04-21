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
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    public float Acceleration = 40f;
    private Vector2 moveVec;
    private Vector2 mouseVec;
    private const int PPU = 64;
    
    void Start()
    {
        rigidbody2D = rigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        mouseVec = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    void FixedUpdate()
    {
        Move();
        PlayerRotate();
    }
    
    void Move()
    {
        rigidbody2D.AddForce(moveVec.normalized * Acceleration);
        rigidbody2D.velocity *= 0.75f;
    }
    
    void PlayerRotate()
    {
        if (Input.GetMouseButton((int) MouseButton.RightMouse))
        {
            var playerPos = (Vector2) Camera.main.WorldToScreenPoint(transform.position);
            var playerToMouseVec = (mouseVec - playerPos).normalized;

            rigidbody2D.rotation = Mathf.Atan2(playerToMouseVec.y, playerToMouseVec.x) * Mathf.Rad2Deg + 270;
        }
        else
        {
            if (moveVec.magnitude > 0.2f)
                rigidbody2D.rotation = Mathf.Atan2(moveVec.y, moveVec.x) * Mathf.Rad2Deg + 270;
        }
    }
}