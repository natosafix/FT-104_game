using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private static Entity Player;
    private Enemy NPC;
    
    void Start()
    {
        NPC = gameObject.transform.parent.GetComponent<Enemy>();
    }

    void Update()
    {
        if (!NPC.IsAlive())
            Destroy(gameObject);
    }
    
    void FixedUpdate()
    {
        if (!NPC.IsAlive())
            Destroy(gameObject);
    }

    public static void InitialisePlayer(Entity player)
    {
        Player = player;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!NPC.IsAlive())
            Destroy(gameObject);
        
        var obj = other.gameObject.GetComponent<Entity>();
        
        if (obj == Player)
            Player.SetDamage(1);
    }
}
