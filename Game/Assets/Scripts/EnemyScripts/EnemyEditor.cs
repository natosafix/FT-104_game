/*using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEditor;
using Vector3 = UnityEngine.Vector3;

[CustomEditor(typeof(Spider))]
public class EnemyEditor : Editor
{
    private void OnSceneGUI()
    {
        Spider spider = (Spider)target;
        if (spider.startWayPoint != null)
        {
            Handles.color = Color.magenta;
            var position = spider.thisTransform.position;
            Handles.DrawLine(position + (Vector3)spider.directionVec * 0.3f, position + (Vector3)spider.directionVec * 0.3f 
                                                                             + (Vector3)spider.directionVec * 0.2f);
        }
    }
}
*/