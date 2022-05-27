using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public WayPoint[] Neighbours;
    public Vector2 Position;
    
    void Awake()
    {
        Position = gameObject.transform.position;
        var colliders = Physics2D.OverlapBoxAll(Position, 
            new Vector2(0.1f, 0.1f), 0f, 1 << 12);
        Neighbours = colliders.Select(x => x.gameObject.GetComponent<WayPoint>())
            .Where(x =>
            {
                var xPos = (Vector2) x.gameObject.transform.position;
                var hitWalls = Physics2D.BoxCast(Position, 
                    new Vector2(0.9f, 0.2f), 0,
                    (xPos - Position).normalized,
                    (xPos - Position).magnitude, 
                    1 << 8);
                return hitWalls.collider == null && x.gameObject != gameObject;
            })
            .ToArray();
    }

    void Update()
    {
        
    }
}
