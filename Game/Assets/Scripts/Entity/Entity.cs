using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected float HP { get; private set; }
    protected Transform thisTransform;
    protected Rigidbody2D rigidbody2D;

    protected virtual void SetUp()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        thisTransform = GetComponent<Transform>();
    }
    
    protected virtual void SetDamage(float damage)
    { }

    protected virtual bool IsAlive()
    {
        return HP > 0;
    }
}
