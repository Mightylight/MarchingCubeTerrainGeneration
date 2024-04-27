using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Noise;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class MarchingCubeManager : MonoBehaviour
{
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private int _depth;

    [Tooltip("Controls whether or not the algorithm should smooth the terrain.")]
    [SerializeField] private bool _vectorInterpolate;

    [SerializeField] private int _seed;
    
    [SerializeField] private NoiseGenerator _noiseGenerator;
    
    //TODO: make cubesize actually do something
    [SerializeField] private float _cubeSize = 1f;

    private Camera _camera;
    private MeshFilter _meshFilter;
    private List<MarchingCube> _marchingCubes = new();
    private float[] _weights;
    private MeshBuilder _meshBuilder = new MeshBuilder();
    
    private void Start()
    {
        _camera = Camera.main;
        _meshFilter = GetComponent<MeshFilter>();
        InitializeCubes();
    }

    public void InitializeCubes()
    {
        Random.InitState(_seed);
        Clear();
        _weights = new float[_width * _height * _depth];
        ConfigureCamera();
        _weights = _noiseGenerator.GetNoise(_width, _height,_depth);
        CreateCubes();
        GenerateMesh();
    }

    public void Clear()
    {
        _meshBuilder.Clear();
        _marchingCubes.Clear();
        if (_meshFilter == null)
        {
            _meshFilter = GetComponent<MeshFilter>();
        }
        _meshFilter.mesh = null;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            InitializeCubes();
        }
    }

    private void ConfigureCamera()
    {
        if (_camera == null)
        {
            _camera = Camera.main;
        }
        Vector3 camPos = new(_width / 2, _width, _width / 2);
        _camera.transform.position = camPos;
        _camera.transform.rotation = Quaternion.Euler(90,0,0);
    }

    private void GenerateMesh()
    {
        foreach (MarchingCube marchingCube in _marchingCubes)
        {
            marchingCube.CreateMesh(_meshBuilder);
        }
        
        Mesh mesh = _meshBuilder.CreateMesh();
        _meshFilter.mesh = mesh;
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
                    MarchingCube marchingCube = new();
                    Vector3 worldPos = new (x * _cubeSize, y * _cubeSize, z * _cubeSize);
                    marchingCube.InitializeCube(cubeValues, worldPos,_vectorInterpolate);
                    _marchingCubes.Add(marchingCube);
                }
            }
        }
    }

    private float[] CalculateValues(int pX, int pY, int pZ)
    {
        float[] cubeValues = new float[8];
                    
        cubeValues[0] = _weights[IndexFromCoord(pX, pY, pZ + 1)];
        cubeValues[1] = _weights[IndexFromCoord(pX + 1, pY, pZ + 1)];
        cubeValues[2] = _weights[IndexFromCoord(pX + 1, pY, pZ)];
        cubeValues[3] = _weights[IndexFromCoord(pX, pY, pZ)];
        cubeValues[4] = _weights[IndexFromCoord(pX, pY + 1, pZ + 1)];
        cubeValues[5] = _weights[IndexFromCoord(pX + 1, pY + 1, pZ + 1)];
        cubeValues[6] = _weights[IndexFromCoord(pX + 1, pY + 1, pZ)];
        cubeValues[7] = _weights[IndexFromCoord(pX, pY + 1, pZ)];
        
        return cubeValues;
    }
    
    private int IndexFromCoord(int pX, int pY, int pZ)
    {
        return (_width * _height * pZ) + (_width * pY) + pX;
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
