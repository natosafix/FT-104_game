using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DestroyObject : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2);
    }
}