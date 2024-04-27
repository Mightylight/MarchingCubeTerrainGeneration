using System;
using UnityEngine;

namespace Noise
{
    /// <summary>
    /// Container class for allowing multiple noise generators
    /// </summary>
    public abstract class NoiseGenerator : MonoBehaviour
    {
        public virtual float[] GetNoise(int pWidth, int pHeight, int pDepth) { return null; }
        
    }
}
