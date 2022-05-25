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
            .Where(x =>
            {
                var vecDir = (x.Position - Position).normalized;
                var hitInfo = Physics2D.BoxCast(x.Position, new Vector2(0.9f, 0.1f), 
                    0, vecDir, 1f, 1 << 8);
                return x.gameObject != gameObject && hitInfo.collider == null;
            })
            .ToArray();
    }

    void Update()
    {
        
    }
}
