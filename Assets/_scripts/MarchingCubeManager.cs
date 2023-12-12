using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MarchingCubeManager : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int _depth;
    [SerializeField] private float _cubeSize = 1f;
    
    private List<MarchingCubes> _marchingCubes = new();
    private float[] _weights;
    
    MeshBuilder _meshBuilder = new MeshBuilder();
    
    //Create points with width,height and depth
    private void Start()
    {
        Debug.Log("hoi?");
        _weights = new float[_width * _height * _depth];
        // for (int i = 0; i < _weights.Length; i++)
        // {
        //     _weights[i] = UnityEngine.Random.Range(0f, 2f);
        //     _weights[i] = Mathf.PerlinNoise();
        //     Debug.Log(_weights[i]);
        // }

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int z = 0; z < _depth; z++)
                {
                    float noiseValue = Mathf.PerlinNoise(x * .3f, z * .3f);
                    int pY = Mathf.RoundToInt(noiseValue * _height/2);
                    //_weights[IndexFromCoord(x, y, z)] = 1f;
                    _weights[IndexFromCoord(x, y, z)] = y > pY ? 0f : 1f;
                }
            }
        }
        CreateCubes();
        GenerateMesh();
    }

    private void GenerateMesh()
    {
        foreach (MarchingCubes marchingCube in _marchingCubes)
        {
            marchingCube.CreateMesh(_meshBuilder);
        }
        Mesh mesh = _meshBuilder.CreateMesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    private void CreateCubes()
    {
        Debug.Log("Creating Cubes");
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int z = 0; z < _depth; z++)
                {
                    //Debug.Log(x + " " + y + " " + z);
                    if(x == _width - 1 || y == _height - 1 || z == _depth - 1) continue;
                    float[] cubeValues = CalculateValues(x, y, z);
                    // float[] cubeValues = new float[8]
                    // {
                    //     1,1,1,1,1,1,1,0
                    // };
                    MarchingCubes marchingCubes = new();
                    Vector3 worldPos = new (x * _cubeSize, y * _cubeSize, z * _cubeSize);
                    marchingCubes.InitializeCube(cubeValues, worldPos);
                    _marchingCubes.Add(marchingCubes);
                    //Debug.Log($"CubeConfig: {marchingCubes._cubeConfig}");
                }
            }
        }
    }

    private float[] CalculateValues(int x, int y, int z)
    {
        float[] cubeValues = new float[8];
                    
        cubeValues[0] = _weights[IndexFromCoord(x, y, z + 1)];
        cubeValues[1] = _weights[IndexFromCoord(x + 1, y, z + 1)];
        cubeValues[2] = _weights[IndexFromCoord(x + 1, y, z)];
        cubeValues[3] = _weights[IndexFromCoord(x, y, z)];
        cubeValues[4] = _weights[IndexFromCoord(x, y + 1, z + 1)];
        cubeValues[5] = _weights[IndexFromCoord(x + 1, y + 1, z + 1)];
        cubeValues[6] = _weights[IndexFromCoord(x + 1, y + 1, z)];
        cubeValues[7] = _weights[IndexFromCoord(x, y + 1, z)];
        
        Debug.Log(cubeValues[0] + " " + cubeValues[1] + " " + cubeValues[2] + " " + cubeValues[3] + " " + cubeValues[4] + " " + cubeValues[5] + " " + cubeValues[6] + " " + cubeValues[7]);
        
        
        return cubeValues;
    }
    
    private int IndexFromCoord(int x, int y, int z)
    {
        return x + _width * (y + _depth * z);
    }

    private void OnDrawGizmos()
    {
        if(_weights == null || _weights.Length == 0) return;
        
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                for (int z = 0; z < _depth; z++) {
                    int index = x + _width * (y + _depth * z);
                    float noiseValue = _weights[index];
                    //Gizmos.color = Color.Lerp(Color.black, Color.white, noiseValue);
                    Gizmos.color = noiseValue < 0.5f ? Color.black : Color.white;
                    Gizmos.DrawCube(new Vector3(x, y, z), Vector3.one * .2f);
                }
            }
        }
    }
}
