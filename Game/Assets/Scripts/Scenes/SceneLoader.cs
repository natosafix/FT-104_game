using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    public string NextSceneToLoad;

    private GameObject player;

    public void Start()
    {
        player = GameObject.Find("Player");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == player && NextSceneToLoad?.Length != 0)
        {
            PlayerMove.LoadNewScene(NextSceneToLoad);
        }
    }
}
