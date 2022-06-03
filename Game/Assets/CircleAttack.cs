using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CircleAttack : MonoBehaviour
{
    public Entity player;
    private Collider2D collider;
    private bool enlarging = false;
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        player = GameObject.Find("Player").GetComponent<Entity>();

    }

    // Update is called once per frame
    void Update()
    {
        if (!enlarging)
        {
            StartCoroutine(enlarge());
        }
    }

    IEnumerator enlarge()
    {
        enlarging = true;
        var k = 0;
        while(k < 200)
        {
            transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
            k++;
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            if (player.invincible)
            {
                collider.enabled = false;
            }
            else
            {
                SceneManager.LoadScene("BossLevel");
            }
        }
    }
}

