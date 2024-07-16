using System;
using Unity.Collections;
using Unity.Entities;
using Random = Unity.Mathematics.Random;

namespace Unidork.DOTS.Collections
{
    public static class CollectionExtensions
    {
        #region Contains

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array">Array.</param>
        /// <param name="value">Value.</param>
        /// <typeparam name="T">Type of values inside array.</typeparam>
        /// <returns>
        /// True if the array contains the value, False otherwise.
        /// </returns>
        public static bool ContainsValue<T>(this NativeArray<T> array, T value) where T : struct, IComparable<T>
        {
            for (int i = 0, length = array.Length; i < length; i++)
            {
                T arrayValue = array[i];

                if (arrayValue.CompareTo(value) == 0)
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// Checks if a DynamicBuffer contains the passed value.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="value">Value.</param>
        /// <returns>
        /// True if the passed value is present in the buffer, False otherwise.
        /// </returns>
        public static bool ContainsValue<T, U>(this DynamicBuffer<T> buffer, U value) where T : unmanaged, IEquatable<U>
        {
            return buffer.AsNativeArray().Contains(value);
        }

        /// <summary>
        /// Checks if a blob array contains the passed value.
        /// </summary>
        /// <param name="blobArray">Blob array.</param>
        /// <param name="value">Value.</param>
        /// <typeparam name="T">Type of values inside blob array.</typeparam>
        /// <returns>
        /// True if blob array contains the passed value, False otherwise.
        /// </returns>
        public static bool Contains<T>(this ref BlobArray<T> blobArray, T value) where T : struct, IEquatable<T>
        {
            int arrayLength = blobArray.Length;

            if (arrayLength == 0)
            {
                return false;
            }

            for (var i = 0; i < arrayLength; i++)
            {
                if (blobArray[i].Equals(value))
                {
                    return true;
                }
            }

            return false;
        }
        
        #endregion

        #region Random

        /// <summary>
        /// Gets a random element of a native array.
        /// </summary>
        /// <param name="array">Array.</param>
        /// <param name="random">Random.</param>
        /// <typeparam name="T">Element type.</typeparam>
        /// <returns>
        /// An instance of <typeparam name="T"></typeparam> or default value if the array is empty.
        /// </returns>
        public static T GetRandomOrDefault<T>(this NativeArray<T> array, Random random) where T : unmanaged
        {
            return array.Length == 0 ? default : array[random.NextInt(array.Length)];
        }

        /// <summary>
        /// Gets a random element of a native list.
        /// </summary>
        /// <param name="list">List.</param>
        /// <param name="random">Random.</param>
        /// <typeparam name="T">Element type.</typeparam>
        /// <returns>
        /// An instance of <typeparam name="T"></typeparam> or default value if the list is empty.
        /// </returns>
        public static T GetRandomOrDefault<T>(this NativeList<T> list, Random random) where T : unmanaged
        {
            return list.Length == 0 ? default : list[random.NextInt(list.Length)];
        }

        /// <summary>
        /// Gets a random element of a dynamic buffer.
        /// </summary>
        /// <param name="buffer">Buffer.</param>
        /// <param name="random">Random.</param>
        /// <typeparam name="T">Element type.</typeparam>
        /// <returns>
        /// An instance of <typeparam name="T"></typeparam> or default value if the buffer is empty.
        /// </returns>
        public static T GetRandomOrDefault<T>(this DynamicBuffer<T> buffer, Random random) where T : unmanaged
        {
            return buffer.Length == 0 ? default : buffer[random.NextInt(buffer.Length)];
        }
        
        /// <summary>
        /// Gets a random element of a blob array.
        /// </summary>
        /// <param name="blobArray">Blob array.</param>
        /// <param name="random">Random.</param>
        /// <typeparam name="T">Element type.</typeparam>
        /// <returns>
        /// An instance of <typeparam name="T"></typeparam> or default value if the blob array is empty.
        /// </returns>
        public static T GetRandomOrDefault<T>(this ref BlobArray<T> blobArray, Random random) where T : unmanaged
        {
            return blobArray.Length == 0 ? default : blobArray[random.NextInt(blobArray.Length)];
        }

        #endregion
    }
}