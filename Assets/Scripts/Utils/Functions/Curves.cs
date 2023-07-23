using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public static class Curves
    {
        public static float EaseIn(float x)
        {
            return x * x;
        }
        public static float EaseOut(float x)
        {
            x = x - 1;
            return 1 - x * x;
        }
        public static float EaseInOut(float x)
        {
            return x < 0.5 ? 16 * x * x * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 5) / 2;
        }
        public static float Linear(float x)
        {
            return x;
        }
        public static float Constant(float x)
        {
            return 1;
        }
    }

}