using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using System;

public class EyeBoss : Entity
{
    public Camera camera;
    public GameObject grabPrefab;
    public GameObject circlePrefab;
    public GameObject angelicEyePrefab;
    public GameObject globalLight;
    private GameObject player;
    private int phase;
    private Light2D light;
    private bool changingLight = false;
    private bool attackedVertical = false;
    private bool attackedHorizontal = false;
    private bool circle1 = false;
    private bool circle2 = false;
    private bool circle3 = false;
    // Start is called before the first frame update
    void Start()
    {
        invincible = true;
        SetUp(50);
        player = GameObject.Find("Player");
        phase = 0;
        light = globalLight.GetComponent<Light2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && phase == 0)
        {
            StartCoroutine(makeIntensityHigher());
            StartCoroutine(bossAwake());
        }
        if (collision.name == "KatanaRange" && !invincible)
        {
            SetDamage(1);
            Debug.Log(HP);
            if (HP <= 40 && !attackedVertical)
            {
                grabVertical();
                attackedVertical = true;
            }

            if (HP <= 30 && !attackedHorizontal)
            {
                grabVertical();
                grabHorizontal();
                attackedHorizontal = true;
            }

            if (HP <= 25 && !circle1)
            {
                phase = 2;
                Animator.SetTrigger("Phase2");
                circleAttack(new Vector3(0, -6, -3));
                circle1 = true;
            }

            if (HP <= 15 && !circle2)
            {
                circleAttack(new Vector3(0, 6, -3));
                circle2 = true;
            }

            if (HP <= 10 && !circle3)
            {
                circleAttack(new Vector3(-3, 0, -3));
                circle3 = true;
            }
        }
    }

    private void grabVertical()
    {
        for (int i = 3; i >= -3; i--)
        {
            if (i != 0)
            {
                GameObject grab = Instantiate(grabPrefab) as GameObject;
                grab.transform.position = new Vector3(0, i * 3, -3);
            }
        }
    }

    private void grabHorizontal()
    {
        for (int i = 3; i >= -3; i--)
        {
            if (i != 0)
            {
                GameObject grab = Instantiate(grabPrefab) as GameObject;
                grab.transform.position = new Vector3(i * 3, 0, -3);
            }
        }
    }

    private void circleAttack(Vector3 position)
    {
        GameObject circle= Instantiate(circlePrefab) as GameObject;
        circle.transform.position = position;
    }

    IEnumerator makeIntensityHigher()
    {
        changingLight = true;
        while (light.intensity < 1)
        {
            light.intensity += 0.05f;
            yield return new WaitForSeconds(0.1f);
        }
        changingLight = false;
    }

    IEnumerator bossAwake()
    {
        while (changingLight)
        {
            yield return new WaitForSeconds(0.01f);
        }
        Animator.SetTrigger("Awake");
        phase = 1;
        StartCoroutine(grabAttack());
        invincible = false;
    }

    IEnumerator grabAttack()
    {
        while (phase == 1)
        {
            GameObject grab = Instantiate(grabPrefab) as GameObject;
            grab.transform.position = player.transform.position;
            yield return new WaitForSeconds(1);
            
        }
    }
}
