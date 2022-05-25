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

    public static WayPoint FindPath2(this Entity start, Entity target, float aggroDistance)
    {
        var track = new Dictionary<WayPoint, WayPoint>();
        var queue = new Queue<WayPoint>();
        foreach (var wayPoint in start.GetClosestWayPoints())
        {
            track[wayPoint] = null;
            queue.Enqueue(wayPoint);
        }

        while (queue.Count > 0)
        {
            var currentPoint = queue.Dequeue();

            if (currentPoint.TryHit(target, 1 << 6 | 1 << 8, out var hitInfo, 0.55f, aggroDistance))
                return StartPointInTrack(currentPoint, track);
            foreach (var point in currentPoint.Neighbours.Where(x => !track.ContainsKey(x)))
            {
                track[point] = currentPoint;
                queue.Enqueue(point);
            }
        }

        return null;
    }

    private static IEnumerable<WayPoint> GetClosestWayPoints(this Entity entity)
    {
        var wayPoints = Physics2D.OverlapBoxAll(entity.thisTransform.position, 
            new Vector2(1f, 1f), 0f, 1 << 12);
        var sortedPoints = wayPoints
            .OrderBy(x => (x.transform.position - entity.thisTransform.position).magnitude)
            .Select(x => x.GetComponent<WayPoint>());
        return sortedPoints;
    }

    private static WayPoint StartPointInTrack(this WayPoint point, Dictionary<WayPoint, WayPoint> track)
    {
        WayPoint prev = null;
        while (point != null)
        {
            prev = point;
            point = track[point];
        }
        return prev;
    }
}
