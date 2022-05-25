using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Entity : MonoBehaviour
{
    public Animator Animator;
    public GameObject DeadAnim;
    
    public Transform thisTransform; 
    public Rigidbody2D thisRigidbody2D;
    public Collider2D ThisCollider2D;
    protected float HP { get; private set; }

    protected virtual void SetUp()
    {
        HP = 1;
        ThisCollider2D = GetComponent<Collider2D>();
        thisRigidbody2D = GetComponent<Rigidbody2D>();
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

public static class MonoBehaviourExtension
{
    public static RaycastHit2D TryHit(this Entity start, MonoBehaviour target, int layerMask, float maxDist = Mathf.Infinity)
    {
        var origin = (Vector2) start.transform.position;
        var vecDir = ((Vector2) target.transform.position - origin).normalized;
        return Physics2D.BoxCast(origin, new Vector2(start.ThisCollider2D.bounds.size.x, 0.1f), 
            0, vecDir, Mathf.Infinity, layerMask);
    }
}
    
