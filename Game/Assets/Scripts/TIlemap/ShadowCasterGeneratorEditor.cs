/*using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShadowCaster2DTilemap))]
public class ShadowCastersGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        var generator = (ShadowCaster2DTilemap)target;
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();


        if (GUILayout.Button("Generate"))
            generator.Generate();

        EditorGUILayout.Space();
        if (GUILayout.Button("Destroy All Children"))
            generator.DestroyAllChildren();
    }

}*/