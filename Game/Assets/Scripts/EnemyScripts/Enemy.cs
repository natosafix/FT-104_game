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
    Aggro,
    LostTarget
}

public class Enemy : Entity
{
    protected int bitmask = 1 << 8 | 1 << 7;
    
    protected static Entity Target;
    
    public Vector2 directionVec;
    
    public float aggroDistance;
    protected float patrolSpeed;
    protected float aggroSpeed;
    protected float aggroTime;
    protected float aggroTimeCount;
    
    protected Vector3 toTargetVec;
    protected float distanceToPlayer;
    protected EnemyState state = EnemyState.Patrol;
    public WayPoint startWayPoint;
    protected WayPoint currentWayPoint;
    protected WayPoint currentTarget;
    protected WayPoint pathToTarget;
    protected WayPoint pathToStart;
    protected bool isNoCollision = false;
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
        currentTarget = startWayPoint;
        if (startWayPoint != null)
            directionVec = (startWayPoint.Position - (Vector2)thisTransform.position).normalized;
    }

    private WayPoint FindNearestWayPoint()
    {
        var thisPosition = (Vector2) gameObject.transform.position;
        var wayPoints = Physics2D.OverlapBoxAll(thisPosition, 
            new Vector2(0.1f, 0.1f), 0f, 1 << 12);
        return wayPoints
            .Select(x => x.GetComponent<WayPoint>())
            .OrderBy(x => Vector2.Distance(x.Position, thisPosition))
            .Where(x =>
            {
                var xPos = x.Position;
                var hitWalls = Physics2D.BoxCast(thisPosition, 
                    new Vector2(thisCollider2D.bounds.size.x, 0.2f), 0,
                    (xPos - thisPosition).normalized,
                    (xPos - thisPosition).magnitude, 
                    1 << 8);
                return hitWalls.collider == null && x.gameObject != gameObject;
            })
            .FirstOrDefault();
    }
    
    protected virtual void UpdateState()
    {
        if (!Target.IsAlive())
        {
            state = EnemyState.Patrol;
            return;
        }
        toTargetVec = Target.thisTransform.position - thisTransform.position;
        distanceToPlayer = toTargetVec.magnitude;
        pathToTarget = BFS.FindPath(currentWayPoint, Target.thisTransform, this, 3.0f);
        hitTarget = Physics2D.BoxCast(thisTransform.position, 
            new Vector2(thisCollider2D.bounds.size.x - 0.1f, thisCollider2D.bounds.size.y - 0.1f), 0,
            Target.thisTransform.position - thisTransform.position, aggroDistance, 
            1 << 6 | 1 << 8);
        if (hitTarget.collider != null && hitTarget.collider.gameObject.layer == 6 && state != EnemyState.Aggro)
        {
            aggroTimeCount = aggroTime;
            wasAggred = true;
            state = EnemyState.Aggro;
        }
        else if (pathToTarget != null && state == EnemyState.Aggro)
        {
            currentTarget = pathToTarget;
        }
        else if (aggroTimeCount > 0)
            state = EnemyState.LostTarget;
        else if (state != EnemyState.Patrol)
        {
            if (wasAggred)
                currentTarget = currentWayPoint;

            state = EnemyState.Patrol;
        }
        toTargetVec.Normalize();
    }

    protected virtual void Move()
    {
        if (state == EnemyState.Aggro)
            AggroBehaviour();
        if (state == EnemyState.Patrol)
            PatrolBehaviour();
        if (state == EnemyState.LostTarget)
            LostTargetBehaviour();
            
    }

    protected virtual void AggroBehaviour()
    {

        if (pathToTarget == currentWayPoint)
            AggroBehaviorNoCollision();
        else
            AggroBehaviourCollision();
    }

    protected virtual void AggroBehaviorNoCollision()
    {
        var nextPos = thisTransform.position + toTargetVec.normalized * (aggroSpeed * Time.deltaTime);
        rigidbody2D.rotation = Mathf.Atan2(toTargetVec.y, toTargetVec.x) * Mathf.Rad2Deg + 270;
        var nearestWayPoint = FindNearestWayPoint();
        currentWayPoint = nearestWayPoint ? nearestWayPoint : currentWayPoint;
        rigidbody2D.MovePosition(nextPos);
    }

    protected virtual void AggroBehaviourCollision()
    {
        var currentPosition = (Vector2)thisTransform.position;
        if (Vector2.Distance(currentTarget.Position, currentPosition) < 0.3f)
        {
            currentWayPoint = currentTarget;
            currentTarget = pathToTarget;
        }
        var nextPosVec = (currentTarget.Position - currentPosition).normalized;
        var nextPos = currentPosition + nextPosVec * (aggroSpeed * Time.deltaTime);
        var hitEnemies = Physics2D.BoxCast(currentPosition + nextPosVec.normalized, 
            new Vector2(thisCollider2D.bounds.size.x, 0.01f),
            0, nextPosVec, 0.01f, 1 << 7);
        if (hitEnemies.collider != null)
        {
            ChangeDirection(currentWayPoint);
            return;
        }
        rigidbody2D.rotation = Mathf.Atan2(nextPosVec.y, nextPosVec.x) * Mathf.Rad2Deg + 270;
        rigidbody2D.MovePosition(nextPos);
    }

    protected virtual void PatrolBehaviour()
    {
        var nextPos = (Vector2)thisTransform.position + directionVec * (patrolSpeed * Time.deltaTime);
        if (wasAggred)
        {
            var currentPosition = (Vector2)thisTransform.position;
            if (Vector2.Distance(currentTarget.Position, currentPosition) < 0.1f)
            {
                pathToStart = BFS.FindPath(currentWayPoint, startWayPoint.transform, this, Mathf.Infinity);
                currentWayPoint = currentTarget;
                currentTarget = pathToStart;
                if (currentWayPoint == startWayPoint)
                {
                    wasAggred = false;
                    return;
                }
            }
            var nextPosVec = (currentTarget.Position - currentPosition).normalized * (patrolSpeed * Time.deltaTime);
            nextPos = currentPosition + nextPosVec;
            /*var hitEnemies = Physics2D.BoxCast((Vector2)thisTransform.position + nextPosVec.normalized * 
                thisCollider2D.bounds.size.y, new Vector2(thisCollider2D.bounds.size.x, 0.1f),
                0, nextPosVec, 0.1f, 1 << 7);
            if (hitEnemies.collider != null)
            {
                ChangeDirection(currentWayPoint);
                return;
            }*/
            rigidbody2D.rotation = Mathf.Atan2(nextPosVec.y, nextPosVec.x) * Mathf.Rad2Deg + 270;
            rigidbody2D.MovePosition(nextPos);
            return;
        }
        var hitWalls = Physics2D.BoxCast((Vector2)thisTransform.position + directionVec.normalized * 
            thisCollider2D.bounds.size.y, new Vector2(thisCollider2D.bounds.size.x, 0.1f),
            0, directionVec, 0.1f, bitmask);
        if (Vector2.Distance(currentTarget.Position, thisTransform.position) > 0.1f && hitWalls.collider == null)
        {
            rigidbody2D.rotation = Mathf.Atan2(directionVec.y, directionVec.x) * Mathf.Rad2Deg + 270;
            rigidbody2D.MovePosition(nextPos);
        }
        else
            ChangeDirection(startWayPoint);
    }

    protected virtual void LostTargetBehaviour()
    {
        aggroTimeCount -= Time.fixedDeltaTime;
        rigidbody2D.velocity = Vector2.zero;
    }
    
    protected virtual void ChangeDirection(WayPoint fromWayPoint)
    {
        var random = Random.Range(0, fromWayPoint.Neighbours.Length + 1);
        currentTarget = random == fromWayPoint.Neighbours.Length ? fromWayPoint : fromWayPoint.Neighbours[random];
        directionVec = (currentTarget.Position - (Vector2)thisTransform.position).normalized;
    }
}

public static class EnemyExtensions
{
    public static RaycastHit2D Hit(this Enemy start, MonoBehaviour target, int layerMask, float maxDist = Mathf.Infinity)
    {
        var origin = (Vector2) start.transform.position;
        var vecDir = ((Vector2) target.transform.position - origin).normalized;
        return Physics2D.BoxCast(origin, new Vector2(start.thisCollider2D.bounds.size.x, 0.1f), 
            0, vecDir, maxDist, layerMask);
    }
}
