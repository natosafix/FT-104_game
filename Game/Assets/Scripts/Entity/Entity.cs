using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected float HP { get; private set; }
    protected Transform thisTransform;

    protected virtual void SetUp()
    {
        thisTransform = transform;
    }
    
    protected virtual void SetDamage(float damage)
    { }

    protected virtual bool IsAlive()
    {
        return HP > 0;
    }
    
}
