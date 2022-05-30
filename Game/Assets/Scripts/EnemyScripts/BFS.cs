using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BFS
{
    public static WayPoint FindPath(WayPoint start, Transform target, Enemy enemy, float aggroDistance)
    {
        var targetWayPoint = target.gameObject.GetComponent<WayPoint>();
        var track = new Dictionary<WayPoint, WayPoint>();
        var queue = new Queue<WayPoint>();
        track[start] = null;
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            var currentPoint = queue.Dequeue();
            var toTargetVec = (Vector2) target.position - currentPoint.Position;
            var hit = Physics2D.BoxCast(currentPoint.Position, 
                new Vector2(enemy.thisCollider2D.bounds.size.x- 0.1f, enemy.thisCollider2D.bounds.size.y - 0.1f),
                0, toTargetVec, aggroDistance, 1 << 6 | 1 << 8);
            if (hit.collider != null && hit.collider.gameObject.layer == 6 || currentPoint == targetWayPoint)
                return StartPointInTrack(currentPoint, track);
            foreach (var point in currentPoint.Neighbours.Where(x => !track.ContainsKey(x)))
            {
                track[point] = currentPoint;
                queue.Enqueue(point);
            }
        }
        return null;
    }
    
    private static WayPoint StartPointInTrack(this WayPoint point, Dictionary<WayPoint, WayPoint> track)
    {
        var path = new List<WayPoint>();
        while (point != null)
        {
            path.Add(point);
            point = track[point];
        }
        return path.Count == 1 ? path[0] : path[path.Count - 2];
    }
}
