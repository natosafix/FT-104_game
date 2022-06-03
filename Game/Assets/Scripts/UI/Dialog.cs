using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    private bool isActive;
    public string[] Dialogs;
    private Transform dialogs;
    private Transform playerDialogs;
    private Transform enemyDialogs;
    public Sprite[] PlayerSprites;
    public Sprite[] EnemySprites;
    public string Speaker = "enemy";
    
    void Start()
    {
        isActive = true;
        dialogs = GameObject.Find("Canvas").transform;
        playerDialogs = dialogs.GetChild(1);
        enemyDialogs = dialogs.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6 && isActive)
        {
            isActive = false;
            other.gameObject.GetComponent<PlayerMove>().isLocked = true;
            StartCoroutine(ShowDialog(other));
        }
    }

    IEnumerator ShowDialog(Collider2D other)
    {
        foreach (var dialog in Dialogs)
        {
            if (Speaker == "player")
            {
                playerDialogs.GetChild(0).GetComponent<Image>().sprite = PlayerSprites[1];
                playerDialogs.GetChild(1).gameObject.SetActive(true);
                playerDialogs.GetChild(1).GetChild(0).GetComponent<Text>().text = dialog;
                enemyDialogs.GetChild(0).GetComponent<Image>().sprite = EnemySprites[0];
                enemyDialogs.GetChild(1).gameObject.SetActive(false);
            }
            else
            {
                var tmp = playerDialogs.GetChild(0);
                var tmp2 = playerDialogs.GetChild(0).GetComponent<Image>().sprite;
                playerDialogs.GetChild(0).GetComponent<Image>().sprite = PlayerSprites[0];
                playerDialogs.GetChild(1).gameObject.SetActive(false);
                enemyDialogs.GetChild(0).GetComponent<Image>().sprite = EnemySprites[1];
                enemyDialogs.GetChild(1).gameObject.SetActive(true);
                enemyDialogs.GetChild(1).GetChild(0).GetComponent<Text>().text = dialog;
            }

            Speaker = Speaker == "player" ? "enemy" : "player"; 
            yield return new WaitForSeconds(3);
        }
        other.gameObject.GetComponent<PlayerMove>().isLocked = false;
        playerDialogs.GetChild(1).gameObject.SetActive(false);
        enemyDialogs.GetChild(1).gameObject.SetActive(false);
        playerDialogs.GetChild(0).GetComponent<Image>().sprite = PlayerSprites[0];
        enemyDialogs.GetChild(0).GetComponent<Image>().sprite = EnemySprites[0];
        yield return new WaitForSeconds(0.01f);
    }
}
