using System.Runtime.CompilerServices;
using UnityEngine;

namespace Jam
{
    public static class MathUtil
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithY(this Vector3 vec, float value)
        {
            vec.y = value;
            return vec;
        }
        
    }
}
