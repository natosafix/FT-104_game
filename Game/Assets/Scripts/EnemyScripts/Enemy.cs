using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public enum EnemyState
{
    Patrol,
    Aggro
}

public class Enemy : Entity
{
    protected int bitmask = 1 << 8 | 1 << 7;
    
    protected static Entity Target;
    protected Collider2D Bounds;
    
    private readonly Vector3[] directions = {Vector3.down, Vector3.left, Vector3.right, Vector3.up};
    public Vector2 directionVec;
    
    protected float patrolSpeed;
    protected float aggroSpeed;
    protected float aggroDistance;
    
    protected Vector3 toTargetVec;
    protected EnemyState state = EnemyState.Patrol;
    public WayPoint startWayPoint;
    protected WayPoint currentWayPoint;
    protected WayPoint lastWayPoint;
    protected WayPoint pathToTarget;
    protected bool wasAggred;
    protected RaycastHit2D hitTarget;

    public static void EnemiesSetupTarget(Entity player)
    {
        Target = player;
    }

    protected override void SetUp()
    {
        base.SetUp();
        startWayPoint = FindNearestWayPoint();
        currentWayPoint = startWayPoint;
        if (startWayPoint != null)
            directionVec = (startWayPoint.Position - (Vector2)thisTransform.position).normalized;
    }

    private WayPoint FindNearestWayPoint()
    {
        var wayPoints = Physics2D.OverlapBoxAll(gameObject.transform.position, 
            new Vector2(0.1f, 0.1f), 0f, 1 << 12);
        return wayPoints
            .OrderBy(x => (x.gameObject.transform.position - thisTransform.position).magnitude)
            .FirstOrDefault()
            ?.GetComponent<WayPoint>();
    }
    
    protected virtual void UpdateState()
    {
        if (!Target.IsAlive())
        {
            state = EnemyState.Patrol;
            return;
        }
        toTargetVec = Target.thisTransform.position - thisTransform.position;
        pathToTarget = BFS.FindPath(currentWayPoint, Target.thisTransform, 3.0f);
        hitTarget = Physics2D.BoxCast(thisTransform.position, new Vector2(0.55f, 0.55f), 0,
            Target.thisTransform.position - thisTransform.position, aggroDistance, 
            1 << 6 | 1 << 8);
        if (hitTarget.collider != null && hitTarget.collider.gameObject.layer == 6 || 
            toTargetVec.magnitude < aggroDistance && pathToTarget != currentWayPoint)
        {
            wasAggred = true;
            state = EnemyState.Aggro;
        }
        else
            state = EnemyState.Patrol;
        toTargetVec.Normalize();
    }

    protected virtual void Move()
    {
        if (state == EnemyState.Aggro)
            AggroBehaviour();
        if (state == EnemyState.Patrol)
            PatrolBehaviour();
    }

    protected virtual void AggroBehaviour()
    {
        Vector2 nextPos;
        if (hitTarget.collider.gameObject.layer == 6)
        {
            nextPos = thisTransform.position + toTargetVec.normalized * (aggroSpeed * Time.deltaTime);
            rigidbody2D.rotation = Mathf.Atan2(toTargetVec.y, toTargetVec.x) * Mathf.Rad2Deg + 270;
        }
        else
        {
            if (Vector2.Distance(currentWayPoint.Position, thisTransform.position) < 0.1f)
            {
                lastWayPoint = currentWayPoint;
                currentWayPoint = pathToTarget;
            }
            var currentPosition = (Vector2)thisTransform.position;
            var nextPosVec = (currentWayPoint.Position - currentPosition).normalized;
            nextPos = currentPosition + nextPosVec * (aggroSpeed * Time.deltaTime);
            var hitEnemies = Physics2D.BoxCast(currentPosition + nextPosVec * 0.5f,
                new Vector2(0.3f, 0.3f), 0, nextPosVec, 0.1f, 1 << 7);
            if (hitEnemies.collider != null)
            {
                ChangeDirection(currentWayPoint);
                return;
            }
            rigidbody2D.rotation = Mathf.Atan2(nextPosVec.y, nextPosVec.x) * Mathf.Rad2Deg + 270;
        }
        rigidbody2D.MovePosition(nextPos);
    }
    
    protected virtual void PatrolBehaviour()
    {
        var nextPos = (Vector2)thisTransform.position + directionVec * (patrolSpeed * Time.deltaTime);
        if (wasAggred)
        {
            if (currentWayPoint == null)
                currentWayPoint = lastWayPoint;
            if (Vector2.Distance(currentWayPoint.Position, thisTransform.position) < 0.1f)
            {
                if (currentWayPoint == startWayPoint)
                {
                    wasAggred = false;
                    return;
                }
                currentWayPoint = BFS.FindPath(currentWayPoint, startWayPoint.transform, aggroDistance);
            }
            var currentPosition = (Vector2)thisTransform.position;
            var nextPosVec = (currentWayPoint.Position - currentPosition).normalized * (patrolSpeed * Time.deltaTime);
            nextPos = currentPosition + nextPosVec;
            rigidbody2D.rotation = Mathf.Atan2(nextPosVec.y, nextPosVec.x) * Mathf.Rad2Deg + 270;
            rigidbody2D.MovePosition(nextPos);
            return;
        }
        var hitWalls = Physics2D.BoxCast((Vector2)thisTransform.position + directionVec.normalized * 0.5f,
            new Vector2(0.3f, 0.3f), 0, directionVec, 0.2f, bitmask);
        if (Vector2.Distance(currentWayPoint.Position, thisTransform.position) > 0.1f && hitWalls.collider == null)
        {
            rigidbody2D.rotation = Mathf.Atan2(directionVec.y, directionVec.x) * Mathf.Rad2Deg + 270;
            rigidbody2D.MovePosition(nextPos);
        }
        else
            ChangeDirection(startWayPoint);
    }
    
    protected virtual void ChangeDirection(WayPoint fromWayPoint)
    {
        currentWayPoint = fromWayPoint.Neighbours[Random.Range(0, fromWayPoint.Neighbours.Length)];
        if (Random.Range(0, fromWayPoint.Neighbours.Length + 1) == 0)
            currentWayPoint = fromWayPoint;
        directionVec = (currentWayPoint.Position - (Vector2)thisTransform.position).normalized;
    }
}
