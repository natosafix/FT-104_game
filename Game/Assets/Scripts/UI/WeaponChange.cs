using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class WeaponChange : MonoBehaviour
{
    public Sprite[] spriteList;
    public Sprite[] backgrounds;
    private int currentWeaponNum;
    private int otherWeaponNum;
    private Transform currentWeapon;
    private Transform idleWeapon;
    private Transform ammoLeft;

    void Start()
    {
        var weaponPanel = GameObject.Find("Canvas").transform.GetChild(0);
        currentWeapon = weaponPanel.transform.GetChild(0);
        idleWeapon = weaponPanel.transform.GetChild(1);
        ammoLeft = weaponPanel.transform.GetChild(2);
        currentWeaponNum = 1;
        otherWeaponNum = 2;
    }
    public void ChangeWeapon(int weaponNum)
    {
        if (currentWeaponNum == weaponNum)
            return;
        currentWeaponNum = weaponNum;
        otherWeaponNum = currentWeaponNum == 1 ? 2 : 1;
        idleWeapon.GetChild(0).GetComponent<Image>().sprite = backgrounds[otherWeaponNum - 1];
        currentWeapon.GetChild(0).GetComponent<Image>().sprite = backgrounds[currentWeaponNum - 1];
        ammoLeft.gameObject.SetActive(currentWeaponNum == 2);
        /*idleWeapon.GetChild(1).GetComponent<Text>().text = otherWeaponNum.ToString();
        currentWeapon.GetChild(1).GetComponent<Text>().text = currentWeaponNum.ToString();
        idleWeapon.GetChild(2).GetComponent<Image>().sprite = spriteList[otherWeaponNum];
        currentWeapon.GetChild(2).GetComponent<Image>().sprite = spriteList[currentWeaponNum];*/
    }

    public void ChangeAmmoLeft(int ammoCount)
    {
        ammoLeft.GetChild(1).GetComponent<Text>().text = ammoCount.ToString();
    }
}
