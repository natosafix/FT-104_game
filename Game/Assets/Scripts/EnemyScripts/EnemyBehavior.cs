
/*
public class EnemyBehavior : Entity
{
    private float patrolSpeed = 2f;
    private float agrroSpeed = 1f;
    private float aggroDistance = 5f;
    private readonly Vector3[] directions = {Vector3.down, Vector3.left, Vector3.right, Vector3.up};
    private Vector3 directionVec = Vector3.down;
    public Collider2D Bounds;
    private bool isEnterCollision = false;
    private Collider2D myCollider;
    public Transform Character;
    private Vector3 toCharacterVec;
    private EnemyState state = EnemyState.Patrol;
    private const int bitmask = 1 << 8;
    private SpriteRenderer mySpriteRenderer;
    
    void Start()
    {
        SetUp();
        myCollider = GetComponent<Collider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        UpdateState();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void UpdateState()
    {
        toCharacterVec = Character.position - thisTransform.position;
        if (toCharacterVec.magnitude <= aggroDistance && state != EnemyState.Aggro)
            state = EnemyState.Aggro;
    }

    private void Move()
    {
        if (state == EnemyState.Aggro)
        {
            var nextPos = thisTransform.position + toCharacterVec * agrroSpeed * Time.deltaTime;
            rigidbody2D.rotation = Mathf.Atan2(toCharacterVec.y, toCharacterVec.x) * Mathf.Rad2Deg + 180;
            rigidbody2D.MovePosition(nextPos);
        }
        if (state == EnemyState.Patrol)
        {
            var nextPos = thisTransform.position + directionVec * patrolSpeed * Time.deltaTime;
            RaycastHit2D hit = Physics2D.Raycast(thisTransform.position, directionVec,
                0.9f + Vector3.Distance(thisTransform.position, nextPos), bitmask);
            if (Bounds.bounds.Contains(nextPos) && hit.collider == null)
            {
                rigidbody2D.rotation = Mathf.Atan2(directionVec.y, directionVec.x) * Mathf.Rad2Deg + 180;
                rigidbody2D.MovePosition(nextPos);
            }
            else
                ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        directionVec = directions[Random.Range(0, 4)];
    }
}
*/