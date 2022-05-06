using UnityEngine;

public class Spider : Enemy
{
    public GameObject Player;
    public Collider2D bounds;
    public GameObject[] BloodsEffects;
    public float AttackDistance = 5f;
    
    void Start()
    {
        SetUp();
        aggroDistance = 10;
        aggroSpeed = 12;
        patrolSpeed = 4;
        Bounds = bounds;
    }

    void Update()
    {
        if (IsAlive())
            UpdateState();
    }

    void FixedUpdate()
    {
        if (IsAlive())
            Move();
    }

    protected override void DestroyObject()
    {
        var rand = Random.Range(0, BloodsEffects.Length);
        Instantiate(BloodsEffects[rand], thisTransform.position, Quaternion.identity);
        Destroy(bounds.gameObject);
        base.DestroyObject();
    }

    public override void Move()
    {
        base.Move();
        Attack();
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