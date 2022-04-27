using UnityEngine;

public class Spider : Enemy
{
    public GameObject Player;
    public Collider2D bounds;
    
    void Start()
    {
        SetUp();
        aggroDistance = 5;
        aggroSpeed = 1;
        patrolSpeed = 2;
        target = Player.transform;
        Bounds = bounds;
    }

    void Update()
    {
        UpdateState();
    }

    void FixedUpdate()
    {
        Move();
    }
}