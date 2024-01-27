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
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithX(this Vector3 vec, float value)
        {
            vec.x = value;
            return vec;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 WithZ(this Vector3 vec, float value)
        {
            vec.z = value;
            return vec;
        }
    }
}
