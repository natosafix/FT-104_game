using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class DestroyImpactEffect : MonoBehaviour
{
    void Start()
    {
        Destroy(gameObject, 2);
    }
}
