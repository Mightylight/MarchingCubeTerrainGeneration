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
    
    //TODO: make cubesize actually do something
    [SerializeField] private float _cubeSize = 1f;

    private Camera _camera;
    
    
    private List<MarchingCubes> _marchingCubes = new();
    private float[] _weights;
    
    MeshBuilder _meshBuilder = new MeshBuilder();
    
    private void Start()
    {
        _weights = new float[_width * _height * _depth];
        _camera = Camera.main;
        ConfigureCamera();
        GetNoise();
        CreateCubes();
        GenerateMesh();
    }

    private void ConfigureCamera()
    {
        Vector3 camPos = new Vector3(_width / 2, _width, _width / 2);
        _camera.transform.position = camPos;
        _camera.transform.rotation = Quaternion.Euler(90,0,0);
    }

    private void GetNoise()
    {
        //TODO: Make a improved version of the noise function
        //TODO: Add a random offset to the noise to make the terrain random
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int z = 0; z < _depth; z++)
                {
                    float noiseValue = Mathf.PerlinNoise(x * .1f, z * .1f);
                    
                    //Calculate the height cutoff point to get a semi-smooth terrain
                    int heightCutoff = Mathf.RoundToInt(noiseValue * _height);
                    _weights[IndexFromCoord(x, y, z)] = y > heightCutoff ? 0f : 1f;
                    
                    //Make the top row of points have a weight of 0, to make sure the terrain doesn't have holes
                    if (y == _height - 1)
                    {
                        _weights[IndexFromCoord(x, y, z)] = 0f;
                    }
                }
            }
        }
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
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int z = 0; z < _depth; z++)
                {
                    if(x == _width - 1 || y == _height - 1 || z == _depth - 1) continue;
                    float[] cubeValues = CalculateValues(x, y, z);
                    MarchingCubes marchingCubes = new();
                    Vector3 worldPos = new (x * _cubeSize, y * _cubeSize, z * _cubeSize);
                    marchingCubes.InitializeCube(cubeValues, worldPos);
                    _marchingCubes.Add(marchingCubes);
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
        
        return cubeValues;
    }
    
    private int IndexFromCoord(int x, int y, int z)
    {
        return (_width * _height * z) + (_width * y) + x;
    }

    private void OnDrawGizmos()
    {
        if(_weights == null || _weights.Length == 0) return;
        
        for (int x = 0; x < _width; x++) {
            for (int y = 0; y < _height; y++) {
                for (int z = 0; z < _depth; z++)
                {
                    int index = IndexFromCoord(x, y, z);
                    float noiseValue = _weights[index];
                    Gizmos.color = noiseValue < 0.5f ? Color.black : Color.white;
                    Gizmos.DrawCube(new Vector3(x, y, z), Vector3.one * .2f);
                }
            }
        }
    }
}
