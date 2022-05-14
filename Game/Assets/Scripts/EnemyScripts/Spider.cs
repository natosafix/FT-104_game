using UnityEngine;

public class Spider : Enemy
{
    public GameObject Player;
    public Collider2D bounds;
    public GameObject[] BloodsEffects;
    public float AttackDistance = 2f;
    
    void Start()
    {
        SetUp();
        aggroDistance = 10;
        aggroSpeed = 6;
        patrolSpeed = 3;
        Bounds = bounds;
    }

    void Update()
    {
        //if (IsAlive())
           
    }

    void FixedUpdate()
    {
        if (IsAlive())
            UpdateState();
            Move();
    }

    protected override void DestroyObject()
    {
        rigidbody2D.velocity = Vector2.zero;
        var rand = Random.Range(0, BloodsEffects.Length);
        Instantiate(BloodsEffects[rand], thisTransform.position, Quaternion.identity);
        Destroy(bounds.gameObject);
        base.DestroyObject();
    }

    public override void Move()
    {
        if (IsAlive())
        {
            base.Move();
            Attack();
        }
    }

    private void Attack()
    {
        if (state == EnemyState.Patrol)
            return;

        var distanceToPlayer = (Target.thisTransform.position - thisTransform.position).magnitude;
        if (distanceToPlayer <= AttackDistance)
            Animator.SetTrigger("Attack");
    }
}