using System;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Noise
{
    public class FlatTerrainNoiseGenerator : NoiseGenerator
    {
        [Range(0,1)]
        [SerializeField] private float _groundLevel;

        [SerializeField] private float _amplitude;

        [SerializeField] private float _frequency;

        private int _width;
        private int _height;
        private int _depth;
    
        public override float[] GetNoise(int pWidth, int pHeight, int pDepth)
        {
            _width = pWidth;
            _height = pHeight;
            _depth = pDepth;
            float[] weights = new float[pWidth * pHeight * pDepth];
            float offset = Random.Range(-10000, 10000);
            for (int x = 0; x < pWidth; x++)
            {
                for (int y = 0; y < pHeight; y++)
                {
                    for (int z = 0; z < pDepth; z++)
                    {

                        //Simplex noise
                        float3 pos = new(x, y, z);
                        pos += offset;
                        float noiseValue = noise.snoise(pos * _frequency);


                        //Ground is used to create a uniform ground level, from which the terrain is build
                        float ground = -y + (_groundLevel * pHeight);

                        weights[IndexFromCoord(x, y, z)] = ground + noiseValue * _amplitude;


                        //Make the top row of points have a weight of 0, to make sure the terrain doesn't have holes at the top of mountains
                        if (y == pHeight - 1)
                        {
                            weights[IndexFromCoord(x, y, z)] = 0f;
                        }
                    }
                }
            }

            return weights;
        }

        private int IndexFromCoord(int pX, int pY, int pZ)
        {
            return (_width * _height * pZ) + (_width * pY) + pX;
        }
    }
}
