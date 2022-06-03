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
    public Collider2D thisCollider2D;
    protected float HP { get; private set; }
    protected Rigidbody2D rigidbody2D;
    public bool invincible = false;

    protected virtual void SetUp()
    {
        HP = 1;
        thisCollider2D = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        thisTransform = GetComponent<Transform>();
    }

    protected virtual void SetUp(int health)
    {
        HP = health;
        thisCollider2D = GetComponent<Collider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        thisTransform = GetComponent<Transform>();
    }

    public virtual void SetDamage(float damage)
    {
        if (!invincible)
        {
            HP -= 1;
            if (HP <= 0)
            {
                if (Animator != null)
                    Animator.SetTrigger("Dead");
                if (DeadAnim != null)
                    Instantiate(DeadAnim, thisTransform.position, Quaternion.identity);
                DestroyObject();
            }
        }
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
