using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualMarchingCube : MonoBehaviour
{

    [SerializeField] private int _cubeConfig;
    
    private void Start()
    {
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateMesh();
        }
    }

    private void CreateMesh()
    {
        MeshBuilder meshBuilder = new MeshBuilder();
        MarchingCubes marchingCubes = new MarchingCubes();
        marchingCubes._cubeConfig = _cubeConfig;
        marchingCubes.CreateMesh(meshBuilder);
        Mesh mesh = meshBuilder.CreateMesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
