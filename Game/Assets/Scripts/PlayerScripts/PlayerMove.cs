using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;

public enum PlayerStates
{
    Katana = 0,
    WithWeapon = 1,
    CastSpell = 2
};


public class PlayerMove : Entity
{
    [Header("Player Settings")]
    public float ShotDelay = 0.4f;
    public float KatanaDelay = 0.8f;
    public float FireballDelay = 1.0f;
    public float Acceleration = 40f;
    
    [Header("Objects")]
    public GameObject Bullet;
    public GameObject Fireball;
    public GameObject StartBulletPos;
    public GameObject FireballStartPos;
    public GameObject Weapon;
    public GameObject WeaponPanel;

    [Header("Audio Effects")]
    public AudioClip[] KatanaAttackClip;
    public AudioClip ShotGun;
    public AudioClip DeadSound;
    public AudioClip TakeKatana;
    public AudioClip TakeShootGun;
    public AudioClip SpellCast;

    private PlayerStates state = PlayerStates.Katana;
    
    private AudioSource audioSource;
    private Animator bodyAnim;
    private CameraJiggle cameraJiggle;
    
    private float shotCoolDown;
    private float spellCoolDown;
    private Vector2 moveVec;
    private Vector2 mouseVec;
    private bool isGunInInventory = false;
    private Transform bulletStartPosTransform;
    private Transform fireballStartPosTransform;
    public bool isSpellCasted = false;
    private int weaponNum;

    private static bool isFirstStart = true;
    
    void Start()
    {
        //WeaponPanel.GetComponent<WeaponChange>().weaponNum = 1;

        cameraJiggle = Camera.main.GetComponent<CameraJiggle>();
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
    }

    void Update()
    {
        if (!IsAlive())
            return;
        UpdateAnim();
    }

    void FixedUpdate()
    {
        if (!IsAlive())
            return;
        Attack();
        if (isSpellCasted)
        {
            rigidbody2D.velocity = Vector2.zero;
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
            if (state is PlayerStates.Katana)
                return;
            state = PlayerStates.Katana;
            audioSource.PlayOneShot(TakeKatana);
            bodyAnim.SetInteger("PlayerState", (int) state);
            Weapon.GetComponent<Renderer>().enabled = false;
            //WeaponPanel.GetComponent<WeaponChange>().weaponNum = 1;
        }
        if (Input.GetKey(KeyCode.Alpha2) && isGunInInventory)
        {
            if (state is PlayerStates.WithWeapon)
                return;
            state = PlayerStates.WithWeapon;
            audioSource.PlayOneShot(TakeShootGun);
            Weapon.GetComponent<Renderer>().enabled = true;
            bodyAnim.SetInteger("PlayerState", (int) state);
            //WeaponPanel.GetComponent<WeaponChange>().weaponNum = 2;
        }
        if (Input.GetKey(KeyCode.G) && !isSpellCasted && spellCoolDown <= 0)
        {
            state = PlayerStates.CastSpell;
            Weapon.GetComponent<Renderer>().enabled = false;
            bodyAnim.SetInteger("PlayerState", (int) state);
            //WeaponPanel.GetComponent<WeaponChange>().weaponNum = 1;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(dash());
        }
    }

    IEnumerator dash()
    {
        invincible = true;
        mouseVec = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        var playerPos = (Vector2)Camera.main.WorldToScreenPoint(thisTransform.position);
        var playerToMouseVec = (mouseVec - playerPos).normalized;
        rigidbody2D.AddForce(playerToMouseVec * 1000);
        yield return new WaitForSeconds(0.3f);
        invincible = false;
    }

    void Attack()
    {
        if (shotCoolDown > 0)
            shotCoolDown -= Time.fixedDeltaTime;
        if (spellCoolDown > 0)
            spellCoolDown -= Time.fixedDeltaTime;

        switch (state)
        {
            case PlayerStates.Katana:
                if (!Input.GetMouseButton((int) MouseButton.LeftMouse) || shotCoolDown > 0)
                    break;

                var i = Random.Range(0, KatanaAttackClip.Length);
                audioSource.PlayOneShot(KatanaAttackClip[i]);
                
                bodyAnim.SetTrigger("KatanaAttack");
                shotCoolDown = KatanaDelay;
                rigidbody2D.AddForce(Vector2.up.Rotate(rigidbody2D.rotation) * 10, ForceMode2D.Impulse);
                break;
            case PlayerStates.WithWeapon:
                if (!Input.GetMouseButton((int) MouseButton.LeftMouse) || shotCoolDown > 0)
                    break;
                cameraJiggle.JiggleCamera(0.3f);
                audioSource.PlayOneShot(ShotGun);
                Instantiate(Bullet, bulletStartPosTransform.position, Quaternion.Euler(0, 0, rigidbody2D.rotation));
                shotCoolDown = ShotDelay;
                break;
            case PlayerStates.CastSpell:
                if (spellCoolDown > 0 || isSpellCasted)
                    break;
                
                cameraJiggle.JiggleCamera(0.6f);
                bodyAnim.SetTrigger("SpellAttack");
                isSpellCasted = true;
                Instantiate(Fireball, fireballStartPosTransform.position, Quaternion.Euler(0, 0, rigidbody2D.rotation));
                spellCoolDown = FireballDelay;
                state = PlayerStates.Katana;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
        {
            isGunInInventory = true;
        }
        
    }

    void Move()
    {
        moveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rigidbody2D.AddForce(moveVec.normalized * Acceleration);
        rigidbody2D.velocity *= 0.75f;
    }
    
    void PlayerRotate()
    {
        mouseVec = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        var playerPos = (Vector2) Camera.main.WorldToScreenPoint(thisTransform.position);
        var playerToMouseVec = (mouseVec - playerPos).normalized;

        rigidbody2D.rotation = Mathf.Atan2(playerToMouseVec.y, playerToMouseVec.x) * Mathf.Rad2Deg + 270;
    }

    protected override void DestroyObject()
    {
        //SceneManager.LoadScene("TestScene");
        SceneManager.LoadScene("FirstLevel");
    }

}