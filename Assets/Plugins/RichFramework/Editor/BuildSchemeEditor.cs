using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildScheme))]
public class BuildSchemeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        BuildScheme buildScheme = target as BuildScheme;
        base.OnInspectorGUI();
        if (GUILayout.Button("Build"))
        {
            buildScheme.PerformBuild();   
        }
    }
}