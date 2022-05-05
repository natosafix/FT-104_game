using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator Animator;
    public GameObject DeadAnim;
    
    protected float HP { get; private set; }
    public Transform thisTransform;
    protected Rigidbody2D rigidbody2D;

    protected virtual void SetUp()
    {
        HP = 1;
        rigidbody2D = GetComponent<Rigidbody2D>();
        thisTransform = GetComponent<Transform>();
    }

    public virtual void SetDamage(float damage)
    {
        HP = 0;
        if (Animator != null)
            Animator.SetTrigger("Dead");
        if (DeadAnim != null)
            Instantiate(DeadAnim, thisTransform.position, Quaternion.identity);
        DestroyObject();
    }

    protected virtual void DestroyObject()
    {
        Destroy(gameObject, 2);
    }
    
    public virtual bool IsAlive()
    {
        return HP > 0;
    }
}
