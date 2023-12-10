using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingCubes : MonoBehaviour
{
    [SerializeField] private int _rows;       
    [SerializeField] private int _columns;    
    [SerializeField] private int _depth;      
    [SerializeField] private int _increment;  
    

    private void Start()
    {
        Generate();
    }

    private void Generate()
    {
        
    }
}
