using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCube
{
    private int _cubeConfig;

    private float[] _cubeValues;
    
    private Vector3 _worldPos;

    private bool _vectorInterpolate;

    /// <summary>
    /// Reads the cube configuration as a binary number,
    /// converting it into a decimal number for use in the tri table.
    /// </summary>
    private void CalculateCubeConfiguration()
    {
        int cubeConfig = 0;
        if (_cubeValues[0] < 0.5f) cubeConfig |= 1;
        if (_cubeValues[1] < 0.5f) cubeConfig |= 2;
        if (_cubeValues[2] < 0.5f) cubeConfig |= 4;
        if (_cubeValues[3] < 0.5f) cubeConfig |= 8;
        if (_cubeValues[4] < 0.5f) cubeConfig |= 16;
        if (_cubeValues[5] < 0.5f) cubeConfig |= 32;
        if (_cubeValues[6] < 0.5f) cubeConfig |= 64;
        if (_cubeValues[7] < 0.5f) cubeConfig |= 128;
        _cubeConfig = cubeConfig;
    }

    public void InitializeCube(float[] pCubeValues, Vector3 pWorldPos, bool pVectorInterpolate)
    {
        _vectorInterpolate = pVectorInterpolate;
        _cubeValues = pCubeValues;
        _worldPos = pWorldPos;
        CalculateCubeConfiguration();
    }

    public void SetCubeConfig(int pCubeConfig)
    {
        _cubeConfig = pCubeConfig;
    }

    public void CreateMesh(MeshBuilder pMeshBuilder)
    {
        int[] edges = MarchingCubesTables.triTable[_cubeConfig];
        
        //The tri table returns 3 edges per triangle.
        //-1 in the tri table indicates that no further triangles need to be made for this configuration.
        for (int i = 0; edges[i] != -1; i += 3)
        {
            int firstEdgePoint1 = MarchingCubesTables.edgeConnections[edges[i]][0];
            int firstEdgePoint2 = MarchingCubesTables.edgeConnections[edges[i]][1];

            
            int secondEdgePoint1 = MarchingCubesTables.edgeConnections[edges[i + 1]][0];
            int secondEdgePoint2 = MarchingCubesTables.edgeConnections[edges[i + 1]][1];
        
            
            int thirdEdgePoint1 = MarchingCubesTables.edgeConnections[edges[i + 2]][0];
            int thirdEdgePoint2 = MarchingCubesTables.edgeConnections[edges[i + 2]][1];

            //Construct the triangle points using the cube corners
            
             Vector3 trianglePoint1 =
                InterpolateVertex(MarchingCubesTables.cubeCorners[firstEdgePoint1], 
                    MarchingCubesTables.cubeCorners[firstEdgePoint2],
                    _cubeValues[firstEdgePoint1], 
                    _cubeValues[firstEdgePoint2]) + _worldPos;
             Vector3 trianglePoint2 =
                InterpolateVertex(MarchingCubesTables.cubeCorners[secondEdgePoint1], 
                    MarchingCubesTables.cubeCorners[secondEdgePoint2],
                    _cubeValues[secondEdgePoint1],
                    _cubeValues[secondEdgePoint2]) + _worldPos;
             Vector3 trianglePoint3 =
                InterpolateVertex(MarchingCubesTables.cubeCorners[thirdEdgePoint1], 
                    MarchingCubesTables.cubeCorners[thirdEdgePoint2],
                    _cubeValues[thirdEdgePoint1], 
                    _cubeValues[thirdEdgePoint2]) + _worldPos;
             
             int index = pMeshBuilder.AddVertex(trianglePoint1);
             pMeshBuilder.AddVertex(trianglePoint2);
             pMeshBuilder.AddVertex(trianglePoint3);
             pMeshBuilder.AddTriangle(index,index + 1,index + 2);
        }
    }

    /// <summary>
    /// Returns an interpolated vertex by taking 2 Vectors
    /// </summary>
    /// <param name="pEdgeVertex1"></param>
    /// <param name="pEdgeVertex2"></param>
    /// <returns></returns>
    private Vector3 InterpolateVertex(Vector3 pEdgeVertex1, Vector3 pEdgeVertex2, float pEdgeVertex1Value, float pEdgeVertex2Value)
    {
        Vector3 interpolatedVertex;
        float t = 0.5f;
        
        if (_vectorInterpolate)
        {
             t = pEdgeVertex1Value / (pEdgeVertex1Value - pEdgeVertex2Value);
        }
        
        interpolatedVertex = Vector3.Lerp(pEdgeVertex1, pEdgeVertex2, t);

        return interpolatedVertex;
    }
}
