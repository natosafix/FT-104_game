using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WeaponChange : MonoBehaviour
{
    public Sprite[] spriteList;
    private int currentWeaponNum;
    public int weaponNum;
    private Transform CurrentWeapon;
    private Transform IdleWeapon;

    void Start()
    {
        var WeaponPanel = GameObject.Find("WeaponUI").transform.GetChild(0);
        CurrentWeapon = WeaponPanel.transform.GetChild(0);
        IdleWeapon = WeaponPanel.transform.GetChild(1);

    }
    public void ChangeWeapon(int weaponNum)
    {
        var currentNum = (weaponNum == 1) ? 2 : 1;
        IdleWeapon.GetChild(1).GetComponent<Text>().text = currentNum.ToString();
        CurrentWeapon.GetChild(1).GetComponent<Text>().text = weaponNum.ToString();
        IdleWeapon.GetChild(2).GetComponent<Image>().sprite = spriteList[(currentNum - 1) * 2];
        CurrentWeapon.GetChild(2).GetComponent<Image>().sprite = spriteList[(weaponNum - 1) * 2 + 1];


    }
    void Update()
    {
        if (currentWeaponNum != weaponNum)
            currentWeaponNum = weaponNum;
            ChangeWeapon(weaponNum);
    }
}
