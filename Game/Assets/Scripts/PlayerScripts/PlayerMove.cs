using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;


public enum PlayerStates
{
    Katana = 0,
    WithWeapon = 1
};


public class PlayerMove : Entity
{
    public float ShotDelay = 0.2f;
    public float KatanaDelay = 0.8f;
    public float Acceleration = 40f;
    public GameObject Bullet;
    public GameObject StartBulletPos;
    public GameObject Weapon;
    public Animator BodyAnim;
    public PlayerStates State;
    private float coolDown;
    private Vector2 moveVec;
    private Vector2 mouseVec;
    
    private Transform bulletStartPosTransform;
    
    void Start()
    {
        SetUp();
        bulletStartPosTransform = StartBulletPos.transform;
    }

    void Update()
    {
        UpdateAnim();
        moveVec = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        mouseVec = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }

    void FixedUpdate()
    {
        Move();
        PlayerRotate();
        Attack();
    }

    void UpdateAnim()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            State = PlayerStates.Katana;
            BodyAnim.SetInteger("PlayerState", (int) State);
            Weapon.GetComponent<Renderer>().enabled = false;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            State = PlayerStates.WithWeapon;
            Weapon.GetComponent<Renderer>().enabled = true;
            BodyAnim.SetInteger("PlayerState", (int) State);
        }
    }

    void Attack()
    {
        if (coolDown > 0)
            coolDown -= Time.fixedDeltaTime;

        switch (State)
        {
            case PlayerStates.Katana:
                if (!Input.GetMouseButton((int) MouseButton.LeftMouse) || coolDown > 0)
                    break;
                BodyAnim.SetTrigger("KatanaAttack");
                coolDown = KatanaDelay;
                rigidbody2D.AddForce(Vector2.up.Rotate(rigidbody2D.rotation) * 10, ForceMode2D.Impulse);
                break;
            case PlayerStates.WithWeapon:
                if (!Input.GetMouseButton((int) MouseButton.LeftMouse) || coolDown > 0)
                    break;
                Instantiate(Bullet, bulletStartPosTransform.position, Quaternion.Euler(0, 0, rigidbody2D.rotation));
                coolDown = ShotDelay;
                break;
        }
        
    }
    
    void Move()
    {
        rigidbody2D.AddForce(moveVec.normalized * Acceleration);
        rigidbody2D.velocity *= 0.75f;
    }
    
    void PlayerRotate()
    {
        if (Input.GetMouseButton((int) MouseButton.RightMouse))
        {
            var playerPos = (Vector2) Camera.main.WorldToScreenPoint(thisTransform.position);
            var playerToMouseVec = (mouseVec - playerPos).normalized;

            rigidbody2D.rotation = Mathf.Atan2(playerToMouseVec.y, playerToMouseVec.x) * Mathf.Rad2Deg + 270;
        }
        else
        {
            if (moveVec.magnitude > 0.2f)
                rigidbody2D.rotation = Mathf.Atan2(moveVec.y, moveVec.x) * Mathf.Rad2Deg + 270;
        }
    }
}