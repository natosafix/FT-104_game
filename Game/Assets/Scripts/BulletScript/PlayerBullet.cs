using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : BulletMove
{
    void Start()
    {
        ignoreLayers = new HashSet<int>{0, 3, 6, 9, 10, 11, 12};
        hitLayers = new HashSet<int> {7};
        SetUp();
    }
}