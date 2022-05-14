using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    public WayPoint[] Neighbours;
    public Vector2 Position;

    // Start is called before the first frame update
    void Awake()
    {
        Position = gameObject.transform.position;
        Collider2D[] colliders = Physics2D.OverlapBoxAll(Position, 
            new Vector2(0.1f, 0.1f), 0f, 1 << 12);
        Neighbours = colliders.Select(x => x.gameObject.GetComponent<WayPoint>()).Where(x => x.gameObject != gameObject).ToArray();
        /*foreach (var e in Neighbours)
        {
            Destroy(e);
        }*/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
