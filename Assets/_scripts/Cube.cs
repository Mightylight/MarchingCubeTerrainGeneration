using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Vector3 = UnityEngine.Vector3;

public class Cube
{
    public List<Vector3> Points;
    public int Configuration;

    public Cube(List<Vector3> points, int configuration)
    {
        Points = points;
        Configuration = configuration;
    }
    
    public static int GetCubeConfiguration(List<Vector3> points, float isoLevel)
    {
        int configuration = 0;
        for (int i = 0; i < points.Count; i++)
        {
            if (points[i].y < isoLevel)
            {
                configuration |= 1 << i;
            }
        }

        return configuration;
    }
}


