using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public WayPoint[] Neighbours { get; private set; }
    public Vector2 Position;

    void Awake()
    {
        Position = gameObject.transform.position;
        var colliders = Physics2D.OverlapBoxAll(Position, 
            new Vector2(0.1f, 0.1f), 0f, 1 << 12);
        Neighbours = colliders
            .Select(x => x.gameObject.GetComponent<WayPoint>())
            .Where(x => x.gameObject != gameObject && x.TryHit(this, 1 << 8, out _, 
                maxDist: Vector2.Distance(x.Position, Position)) == false)
            .ToArray();
    }

    void Update()
    {
        
    }
}
