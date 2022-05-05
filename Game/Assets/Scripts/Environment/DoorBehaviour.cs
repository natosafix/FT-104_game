using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public Animator doorAnimator;
    private int playerLayer = 6;
    private int enemyLayer = 7;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer || collision.gameObject.layer == enemyLayer)
        {
            doorAnimator.SetBool("IsOpen", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == playerLayer || collision.gameObject.layer == enemyLayer)
        {
            doorAnimator.SetBool("IsOpen", false);
        }
    }
}
