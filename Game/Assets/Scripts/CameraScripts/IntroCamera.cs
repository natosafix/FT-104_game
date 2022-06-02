using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCamera : MonoBehaviour
{
    public float waitTime;
    
    void Start()
    {
        StartCoroutine(WaitForLevel());
    }

    private IEnumerator WaitForLevel()
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("FirstLevel");
    }
}
