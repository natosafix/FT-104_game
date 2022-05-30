using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mafia : Enemy
{
    public GameObject[] BloodsEffects;
    public AudioClip[] DeadSounds;
    private const float AttackDistance = 30f;
    public GameObject Bullet;
    public GameObject StartBulletPos;
    public AudioClip ShotGun;
    public float ShotDelay = 0.4f;

    private float shotCoolDown;
    private Transform bulletStartPosTransform;
    private AudioSource audioSource;
    
    void Start()
    {
        
        SetUp();
        aggroDistance = 10;
        aggroSpeed = 4;
        patrolSpeed = 3;
        aggroTime = 2;
        bulletStartPosTransform = StartBulletPos.transform;
            
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //if (IsAlive())
           
    }

    void FixedUpdate()
    {
        if (IsAlive())
        {
            UpdateState();
            Move();
        }
        else
        {
            rigidbody2D.velocity = Vector2.zero;
        }
    }

    protected override void DestroyObject()
    {
        var randBloodIdx = Random.Range(0, BloodsEffects.Length);
        var tmp = thisTransform.position;
        Instantiate(BloodsEffects[randBloodIdx], new Vector3(tmp.x, tmp.y, 0), Quaternion.identity);
        
        var randDeadSoundIdx = Random.Range(0, DeadSounds.Length);
        audioSource.PlayOneShot(DeadSounds[randDeadSoundIdx]);
        base.DestroyObject();
        
    }

    protected override void AggroBehaviour()
    {
        if (hitTarget.collider.gameObject.layer == 6)
            AggroBehaviorNoCollision();
        else
            AggroBehaviourCollision();
    }

    protected override void AggroBehaviorNoCollision()
    {
        if (shotCoolDown > 0)
            shotCoolDown -= Time.fixedDeltaTime;
        distanceToPlayer = (Target.thisTransform.position - thisTransform.position).magnitude;
        if (distanceToPlayer > AttackDistance)
            base.AggroBehaviorNoCollision();
        else
            Attack();
    }

    private void Attack()
    {
        if (shotCoolDown > 0)
            return;
        rigidbody2D.rotation = Mathf.Atan2(toTargetVec.y, toTargetVec.x) * Mathf.Rad2Deg + 270;
        audioSource.PlayOneShot(ShotGun);
        Instantiate(Bullet, bulletStartPosTransform.position, Quaternion.Euler(0, 0, rigidbody2D.rotation));
        shotCoolDown = ShotDelay;
    }
}
