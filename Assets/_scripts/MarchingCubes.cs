using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubes
{
    public int _cubeConfig;

    private float[] _cubeValues;
    
    public Vector3 _worldPos;

    private void CalculateCubeConfiguration()
    {
        int cubeConfig = 0;
        for (int i = 0; i < _cubeValues.Length; i++)
        {
            if (_cubeValues[i] > 1f)
            {
                cubeConfig |= 1 << i;
            }
        }
        _cubeConfig = cubeConfig;
    }

    public void InitializeCube(float[] pCubeValues, Vector3 pWorldPos)
    {
        _cubeValues = pCubeValues;
        _worldPos = pWorldPos;
        CalculateCubeConfiguration();
    }

    public void CreateMesh(MeshBuilder pMeshBuilder)
    {
        int[] edges = MarchingCubesTables.triTable[_cubeConfig];
        for (int i = 0; edges[i] != -1; i += 3)
        {
            // First edge lies between vertex e00 and vertex e01
            int e00 = MarchingCubesTables.edgeConnections[edges[i]][0];
            int e01 = MarchingCubesTables.edgeConnections[edges[i]][1];

            // Second edge lies between vertex e10 and vertex e11
            int e10 = MarchingCubesTables.edgeConnections[edges[i + 1]][0];
            int e11 = MarchingCubesTables.edgeConnections[edges[i + 1]][1];
        
            // Third edge lies between vertex e20 and vertex e21
            int e20 = MarchingCubesTables.edgeConnections[edges[i + 2]][0];
            int e21 = MarchingCubesTables.edgeConnections[edges[i + 2]][1];

            //Construct the trianglepoints using the cube corners
            //TODO: add worldposition to the trianglepoint
            List<Vector3> trianglePoints = new List<Vector3>();
             Vector3 trianglePoint1 =
                InterpolateVertex(MarchingCubesTables.cubeCorners[e00], MarchingCubesTables.cubeCorners[e01]) + _worldPos;
             Vector3 trianglePoint2 =
                InterpolateVertex(MarchingCubesTables.cubeCorners[e10], MarchingCubesTables.cubeCorners[e11]) + _worldPos;
             Vector3 trianglePoint3 =
                InterpolateVertex(MarchingCubesTables.cubeCorners[e20], MarchingCubesTables.cubeCorners[e21]) + _worldPos;
             
             int index = pMeshBuilder.AddVertex(trianglePoint1);
             pMeshBuilder.AddVertex(trianglePoint2);
             pMeshBuilder.AddVertex(trianglePoint3);
             pMeshBuilder.AddTriangle(index,index + 1,index + 2);
             
        }
        // Mesh mesh = pMeshBuilder.CreateMesh();
        // _meshFilter.mesh = mesh;
        
        
    }

    private Vector3 InterpolateVertex(Vector3 pEdgeVertex1, Vector3 pEdgeVertex2)
    {
        //TODO: Interpolate with cube values
        Vector3 interpolatedVertex = (pEdgeVertex2 - pEdgeVertex1) * 0.5f;
        interpolatedVertex += pEdgeVertex1;
        return interpolatedVertex;
    }
}
