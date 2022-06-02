using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Mafia : Enemy
{
    public GameObject[] BloodsEffects;
    public AudioClip[] DeadSounds;
    private const float AttackDistance = 30f;
    public GameObject Bullet;
    public GameObject StartBulletPos;
    public AudioClip ShotGun;
    private float ShotDelay = 1f;

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
        shotCoolDown = ShotDelay;
            
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
    }

    protected override void DestroyObject()
    {
        var randBloodIdx = Random.Range(0, BloodsEffects.Length);
        var tmp = thisTransform.position;
        Instantiate(BloodsEffects[randBloodIdx], new Vector3(tmp.x, tmp.y, 0), Quaternion.identity);
        
        var randDeadSoundIdx = Random.Range(0, DeadSounds.Length);
        audioSource.PlayOneShot(DeadSounds[randDeadSoundIdx]);
        
        StartCoroutine(Ricochet());
    }

    private IEnumerator Ricochet()
    {
        rigidbody2D.velocity = Vector2.zero;
        var dir = Vector2.up.Rotate(rigidbody2D.rotation + 180).normalized * 10;
        rigidbody2D.AddForce(dir, ForceMode2D.Impulse);
        
        for (int i = 0; i < 50; ++i)
        {
            rigidbody2D.velocity *= 0.9f;
            yield return new WaitForSeconds(0.01f);
        }

        rigidbody2D.velocity = Vector2.zero;
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
        rigidbody2D.rotation = Mathf.Atan2(toTargetVec.y, toTargetVec.x) * Mathf.Rad2Deg + 270;
        if (shotCoolDown > 0)
            return;
        
        audioSource.PlayOneShot(ShotGun);
        Instantiate(Bullet, bulletStartPosTransform.position, Quaternion.Euler(0, 0, rigidbody2D.rotation));
        shotCoolDown = ShotDelay;
    }
}
