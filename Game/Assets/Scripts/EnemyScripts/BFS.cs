using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class BFS
{
    public static WayPoint FindPath(WayPoint start, Transform target, float aggroDistance)
    {
        var targetWayPoint = target.gameObject.GetComponent<WayPoint>();
        var track = new Dictionary<WayPoint, WayPoint>();
        var visited = new HashSet<WayPoint>();
        var queue = new Queue<WayPoint>();
        track[start] = null;
        queue.Enqueue(start);
        visited.Add(start);
        while (queue.Count > 0)
        {
            var currentPoint = queue.Dequeue();
            var toTargetVec = (Vector2) target.position - currentPoint.Position;
            var hit = Physics2D.BoxCast(currentPoint.Position, new Vector2(0.55f, 0.55f), 0,
                toTargetVec, aggroDistance, 1 << 6 | 1 << 8);
            if (hit.collider != null && hit.collider.gameObject.layer == 6 || currentPoint == targetWayPoint)
            {
                WayPoint prev = null;
                while (track[currentPoint] != null)
                {
                    prev = currentPoint;
                    currentPoint = track[currentPoint];
                }
                return prev;
            }
            foreach (var point in currentPoint.Neighbours.Where(x => !visited.Contains(x)))
            {
                track[point] = currentPoint;
                queue.Enqueue(point);
                visited.Add(point);
            }
        }
        return start;
    }
}
