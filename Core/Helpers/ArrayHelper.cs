using System;
using System.Collections.Generic;
using System.Text;

namespace Pici.Core.Helpers
{
    public static class ArrayHelper
    {
        
        /// <summary>
        /// Copies an array to another array
        /// </summary>
        /// <param name="original"></param>
        /// <param name="startIndex">The starting index of the array</param>
        /// <returns>An copied array form the start index</returns>
        public static E[] copyArray<E>(E[] original, int startIndex)
        {
            E[] returnArray = new E[original.Length - startIndex];
            for (int i = startIndex; i < original.Length; i++)
                returnArray[i - startIndex] = original[i];

            return returnArray;
        }

        /// <summary>
        /// Combines 2 ararys into 1 major array
        /// </summary>
        /// <param name="a">Array 1</param>
        /// <param name="b">Array 2</param>
        /// <returns></returns>
        public static byte[] combineArrays(byte[] a, byte[] b)
        {
            byte[] c = new byte[a.Length + b.Length];
            System.Buffer.BlockCopy(a, 0, c, 0, a.Length);
            System.Buffer.BlockCopy(b, 0, c, a.Length, b.Length);
            return c;
        }

    }
}
