using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    private static Entity Player;
    
    void Start()
    {
    }

    void Update()
    {
    }

    public static void InitialisePlayer(Entity player)
    {
        Player = player;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        var obj = other.gameObject.GetComponent<Entity>();
        
        if (obj == Player)
            Player.SetDamage(1);
    }
}
