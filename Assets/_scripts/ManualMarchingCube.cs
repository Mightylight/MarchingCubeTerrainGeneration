using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to help with checking if the marching cube algorithm is implemented correctly
/// </summary>
public class ManualMarchingCube : MonoBehaviour
{

    [SerializeField] private int _cubeConfig;
    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateMesh();
        }
    }

    private void CreateMesh()
    {
        MeshBuilder meshBuilder = new();
        MarchingCube marchingCube = new();
        marchingCube.SetCubeConfig(_cubeConfig);
        marchingCube.CreateMesh(meshBuilder);
        Mesh mesh = meshBuilder.CreateMesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
