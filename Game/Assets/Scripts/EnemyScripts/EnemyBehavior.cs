using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public enum EnemyState
{
    Patrol,
    Aggro
}

public class EnemyBehavior : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;
    private float patrolSpeed = 2f;
    private float agrroSpeed = 1f;
    private readonly Vector3[] directions = {Vector3.down, Vector3.left, Vector3.right, Vector3.up};
    private Vector3 directionVec = Vector3.down;
    public Collider2D Bounds;
    private Transform myTransform;
    private bool isEnterCollision = false;
    private Collider2D myCollider;
    public Transform Character;
    private Vector3 toCharacterVec;
    private float aggroDistance = 5f;
    private EnemyState state = EnemyState.Patrol;
    private const int bitmask = 1 << 8;
    private SpriteRenderer mySpriteRenderer;
    
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        myTransform = GetComponent<Transform>();
        myCollider = GetComponent<Collider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        UpdateState();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void UpdateState()
    {
        toCharacterVec = Character.position - myTransform.position;
        if (toCharacterVec.magnitude <= aggroDistance && state != EnemyState.Aggro)
            state = EnemyState.Aggro;
    }

    private void Move()
    {
        if (state == EnemyState.Aggro)
        {
            var nextPos = myTransform.position + toCharacterVec * agrroSpeed * Time.deltaTime;
            rigidbody2D.rotation = Mathf.Atan2(toCharacterVec.y, toCharacterVec.x) * Mathf.Rad2Deg + 180;
            rigidbody2D.MovePosition(nextPos);
        }
        if (state == EnemyState.Patrol)
        {
            var nextPos = myTransform.position + directionVec * patrolSpeed * Time.deltaTime;
            RaycastHit2D hit = Physics2D.Raycast(myTransform.position, directionVec,
                0.9f + Vector3.Distance(myTransform.position, nextPos), bitmask);
            if (Bounds.bounds.Contains(nextPos) && hit.collider == null)
            {
                rigidbody2D.rotation = Mathf.Atan2(directionVec.y, directionVec.x) * Mathf.Rad2Deg + 180;
                rigidbody2D.MovePosition(nextPos);
            }
            else
                ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        directionVec = directions[Random.Range(0, 4)];
    }
}
