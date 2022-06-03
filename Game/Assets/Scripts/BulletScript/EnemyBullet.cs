using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : BulletMove
{
    void Start()
    {
        ignoreLayers = new HashSet<int>{0, 3, 7, 9, 10, 11, 12};
        hitLayers = new HashSet<int> {6};
        SetUp();
    }
}
