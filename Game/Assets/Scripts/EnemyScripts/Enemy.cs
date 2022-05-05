using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyState
{
    Patrol,
    Aggro
}

public class Enemy : Entity
{
    protected int bitmask = 1 << 8;
    
    protected static Entity Target;
    protected Collider2D Bounds;
    
    private readonly Vector3[] directions = {Vector3.down, Vector3.left, Vector3.right, Vector3.up};
    private Vector3 directionVec = Vector3.down;
    
    protected float patrolSpeed;
    protected float aggroSpeed;
    protected float aggroDistance;
    
    protected Vector3 toTargetVec;
    protected EnemyState state = EnemyState.Patrol;

    public static void EnemiesSetupTarget(Entity player)
    {
        Target = player;
    }
    
    protected virtual void UpdateState()
    {
        if (!Target.IsAlive())
        {
            state = EnemyState.Patrol;
            return;
        }
        toTargetVec = Target.thisTransform.position - thisTransform.position;
        if (toTargetVec.magnitude <= aggroDistance && state != EnemyState.Aggro)
            state = EnemyState.Aggro;
        toTargetVec.Normalize();
    }

    public virtual void Move()
    {
        if (state == EnemyState.Aggro)
            AggroBehaviour();
        if (state == EnemyState.Patrol)
            PatrolBehaviour();
    }

    protected virtual void AggroBehaviour()
    {
        var nextPos = thisTransform.position + toTargetVec * aggroSpeed * Time.deltaTime;
        rigidbody2D.rotation = Mathf.Atan2(toTargetVec.y, toTargetVec.x) * Mathf.Rad2Deg + 270;
        rigidbody2D.MovePosition(nextPos);
    }
    
    protected virtual void PatrolBehaviour()
    {
        var nextPos = thisTransform.position + directionVec * patrolSpeed * Time.deltaTime;
        var hit = Physics2D.Raycast(thisTransform.position, directionVec,
            0.9f + Vector3.Distance(thisTransform.position, nextPos), bitmask);
        
        if (Bounds.bounds.Contains(nextPos) && hit.collider == null)
        {
            rigidbody2D.rotation = Mathf.Atan2(directionVec.y, directionVec.x) * Mathf.Rad2Deg + 270;
            rigidbody2D.MovePosition(nextPos);
        }
        else
            ChangeDirection();
    }
    
    protected virtual void ChangeDirection()
    {
        directionVec = directions[Random.Range(0, 4)];
    }
}
