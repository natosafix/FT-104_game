using System;
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
    public float ShotDelay = 0.2f;
    public float KatanaDelay = 0.8f;
    public float FireballDelay = 1.0f;
    public float Acceleration = 40f;
    public GameObject Bullet;
    public GameObject Fireball;
    public GameObject StartBulletPos;
    public GameObject FireballStartPos;
    public GameObject Weapon;
    public Animator BodyAnim;
    public PlayerStates State = PlayerStates.Katana;
    private float shotCoolDown;
    private float spellCoolDown;
    private Vector2 moveVec;
    private Vector2 mouseVec;
    private bool isGunInInventory = false;
    private Transform bulletStartPosTransform;
    private Transform fireballStartPosTransform;
    public bool isSpellCasted = false;
    
    void Start()
    {
        Weapon.GetComponent<Renderer>().enabled = false;
        SetUp();
        bulletStartPosTransform = StartBulletPos.transform;
        fireballStartPosTransform = FireballStartPos.transform;
        Enemy.EnemiesSetupTarget(this);
        EnemyAttack.InitialisePlayer(this);
    }

    void Update()
    {
        if (!IsAlive())
            return;
        UpdateAnim();
        moveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        mouseVec = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        //Attack();
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
            State = PlayerStates.Katana;
            BodyAnim.SetInteger("PlayerState", (int) State);
            Weapon.GetComponent<Renderer>().enabled = false;
            isSpellCasted = false;
        }
        if (Input.GetKey(KeyCode.Alpha2) && isGunInInventory)
        {
            State = PlayerStates.WithWeapon;
            Weapon.GetComponent<Renderer>().enabled = true;
            BodyAnim.SetInteger("PlayerState", (int) State);
        }
        if (Input.GetKey(KeyCode.G) && !isSpellCasted && spellCoolDown <= 0)
        {
            State = PlayerStates.CastSpell;
            Weapon.GetComponent<Renderer>().enabled = false;
            BodyAnim.SetInteger("PlayerState", (int) State);
        }
    }

    void Attack()
    {
        if (shotCoolDown > 0)
            shotCoolDown -= Time.fixedDeltaTime;
        if (spellCoolDown > 0)
            spellCoolDown -= Time.fixedDeltaTime;

        switch (State)
        {
            case PlayerStates.Katana:
                if (!Input.GetMouseButton((int) MouseButton.LeftMouse) || shotCoolDown > 0)
                    break;
                BodyAnim.SetTrigger("KatanaAttack");
                shotCoolDown = KatanaDelay;
                rigidbody2D.AddForce(Vector2.up.Rotate(rigidbody2D.rotation) * 10, ForceMode2D.Impulse);
                break;
            case PlayerStates.WithWeapon:
                if (!Input.GetMouseButton((int) MouseButton.LeftMouse) || shotCoolDown > 0)
                    break;
                Instantiate(Bullet, bulletStartPosTransform.position, Quaternion.Euler(0, 0, rigidbody2D.rotation));
                shotCoolDown = ShotDelay;
                break;
            case PlayerStates.CastSpell:
                if (spellCoolDown > 0 || isSpellCasted)
                    break;
                BodyAnim.SetTrigger("SpellAttack");
                isSpellCasted = true;
                Instantiate(Fireball, fireballStartPosTransform.position, Quaternion.Euler(0, 0, rigidbody2D.rotation));
                spellCoolDown = FireballDelay;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 10)
            isGunInInventory = true;
    }

    void Move()
    {
        rigidbody2D.AddForce(moveVec.normalized * Acceleration);
        rigidbody2D.velocity *= 0.75f;
    }
    
    void PlayerRotate()
    {
        //if (Input.GetMouseButton((int) MouseButton.RightMouse))
        {
            var playerPos = (Vector2) Camera.main.WorldToScreenPoint(thisTransform.position);
            var playerToMouseVec = (mouseVec - playerPos).normalized;

            rigidbody2D.rotation = Mathf.Atan2(playerToMouseVec.y, playerToMouseVec.x) * Mathf.Rad2Deg + 270;
        }
        return;
        //else
        {
            if (moveVec.magnitude > 0.2f)
                rigidbody2D.rotation = Mathf.Atan2(moveVec.y, moveVec.x) * Mathf.Rad2Deg + 270;
        }
    }

    protected override void DestroyObject()
    {
        SceneManager.LoadScene("TestScene");
    }
}