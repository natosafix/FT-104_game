using UnityEngine;

public class Spider : Enemy
{
    public GameObject Player;
    public Collider2D bounds;
    public GameObject[] BloodsEffects;
    public AudioClip[] DeadSounds;
    public float AttackDistance = 2f;

    private AudioSource audioSource;
    
    void Start()
    {
        SetUp();
        aggroDistance = 10;
        aggroSpeed = 6;
        patrolSpeed = 3;
        Bounds = bounds;

        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //if (IsAlive())
           
    }

    void FixedUpdate()
    {
        if (!IsAlive()) return;
        UpdateState();
        Move();
    }

    protected override void DestroyObject()
    {
        rigidbody2D.velocity = Vector2.zero;
        var randBloodIdx = Random.Range(0, BloodsEffects.Length);
        Instantiate(BloodsEffects[randBloodIdx], thisTransform.position, Quaternion.identity);
        
        var randDeadSoundIdx = Random.Range(0, DeadSounds.Length);
        audioSource.PlayOneShot(DeadSounds[randDeadSoundIdx]);
        Destroy(bounds.gameObject);
        
        base.DestroyObject();
    }

    protected override void Move()
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