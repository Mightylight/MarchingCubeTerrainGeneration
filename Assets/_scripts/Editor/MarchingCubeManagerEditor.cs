using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MarchingCubeManager))]
public class MarchingCubeManagerEditor : Editor
{

    private MarchingCubeManager _marchingCubeManager;

    private void OnEnable()
    {
        _marchingCubeManager = (MarchingCubeManager) target;
    }

    public override void OnInspectorGUI()
    {
        
        base.OnInspectorGUI();
        
        if (GUILayout.Button("Generate"))
        {
            _marchingCubeManager.InitializeCubes();
        }

        if (GUILayout.Button("Clear"))
        {
            _marchingCubeManager.Clear();
        }
    }
}
