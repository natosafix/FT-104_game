using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rigidbody2D;

    public float Acceleration = 20f;
    public Animator Animator;
    
    void Start()
    {
<<<<<<< Updated upstream
        rigidbody2D = rigidbody2D = GetComponent<Rigidbody2D>();
=======
        //WeaponPanel.GetComponent<WeaponChange>().weaponNum = 1;
        Weapon.GetComponent<Renderer>().enabled = false;
        SetUp();
        bulletStartPosTransform = StartBulletPos.transform;
        fireballStartPosTransform = FireballStartPos.transform;
        Enemy.EnemiesSetupTarget(this);
        EnemyAttack.InitialisePlayer(this);

        audioSource = GetComponent<AudioSource>();
        bodyAnim = GetComponent<Animator>();
        
        if (!isFirstStart)
            audioSource.PlayOneShot(DeadSound);

        isFirstStart = false;
>>>>>>> Stashed changes
    }

    void Update()
    {
<<<<<<< Updated upstream
        var moveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        var velocity = rigidbody2D.velocity;
        
        if (velocity.magnitude > 0.5f)
=======
        if (!IsAlive())
            return;
        UpdateAnim();
    }

    void FixedUpdate()
    {
        Debug.Log(this.HP);
        if (!IsAlive())
            return;
        Debug.Log(123);
        Attack();
        if (isSpellCasted)
        {
            thisRigidbody2D.velocity = Vector2.zero;
            return;
        }
        Move();
        PlayerRotate();
    }

    void UpdateAnim()
    {
        if (Input.GetKey(KeyCode.Alpha1) || isSpellCasted && spellCoolDown <= 0)
        {
            isSpellCasted = false;
            if (State is PlayerStates.Katana)
                return;
            State = PlayerStates.Katana;
            audioSource.PlayOneShot(TakeKatana);
            bodyAnim.SetInteger("PlayerState", (int) State);
            Weapon.GetComponent<Renderer>().enabled = false;
            WeaponPanel.GetComponent<WeaponChange>().weaponNum = 1;
        }
        if (Input.GetKey(KeyCode.Alpha2) && isGunInInventory)
        {
            if (State is PlayerStates.WithWeapon)
                return;
            State = PlayerStates.WithWeapon;
            audioSource.PlayOneShot(TakeShootGun);
            Weapon.GetComponent<Renderer>().enabled = true;
            bodyAnim.SetInteger("PlayerState", (int) State);
            WeaponPanel.GetComponent<WeaponChange>().weaponNum = 2;
        }
        if (Input.GetKey(KeyCode.G) && !isSpellCasted && spellCoolDown <= 0)
>>>>>>> Stashed changes
        {
            Animator.SetFloat("HorizontalState", velocity.x);
            Animator.SetFloat("VerticalState", velocity.y);
            Animator.SetFloat("Horizontal", velocity.x);
            Animator.SetFloat("Vertical", velocity.y);
            Animator.SetFloat("Speed", moveVec.magnitude);
        }
    }
    
    void FixedUpdate()
    {
        Move();
        //DirectionMove();
    }

    void Move()
    {
        var w = Input.GetKey(KeyCode.W) ? 1 : 0;
        var a = Input.GetKey(KeyCode.A) ? -1 : 0;
        var s = Input.GetKey(KeyCode.S) ? -1 : 0;
        var d = Input.GetKey(KeyCode.D) ? 1 : 0;
        
        var moveVec = new Vector2(a + d, w + s).normalized;

        rigidbody2D.AddForce(moveVec * Acceleration);
        rigidbody2D.velocity *= 0.75f;
    }
    
    private void DirectionMove()
    {
        rigidbody2D.rotation = Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x) * Mathf.Rad2Deg;
    }
}
