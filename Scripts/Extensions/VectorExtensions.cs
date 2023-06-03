using UnityEngine;

namespace Unidork.Extensions
{
    public static class VectorExtensions
    {
        #region Set

        /// <summary>
        /// Sets the x value of a Vector3 and returns the new vector.
        /// </summary>
        /// <param name="vector">Vector3 to change.</param>
        /// <param name="newX">New x value.</param>
        /// <returns>
        /// A Vector3 with a new x value.
        /// </returns>
        public static Vector3 SetX(this Vector3 vector, float newX)
        {
            vector.x = newX;
            return vector;
        }

        /// <summary>
        /// Sets the y value of a Vector3 and returns the new vector.
        /// </summary>
        /// <param name="vector">Vector3 to change.</param>
        /// <param name="newY">New y value.</param>
        /// <returns>
        /// A Vector3 with a new y value.
        /// </returns>
        public static Vector3 SetY(this Vector3 vector, float newY)
        {
            vector.y = newY;
            return vector;
        }

        /// <summary>
        /// Sets the z value of a Vector3 and returns the new vector.
        /// </summary>
        /// <param name="vector">Vector3 to change.</param>
        /// <param name="newZ">New z value.</param>
        /// <returns>
        /// A Vector3 with a new z value.
        /// </returns>
        public static Vector3 SetZ(this Vector3 vector, float newZ)
        {
            vector.z = newZ;
            return vector;
        }

        /// <summary>
        /// Sets the x value of a Vector2 and returns the new vector.
        /// </summary>
        /// <param name="vector">Vector2 to change.</param>
        /// <param name="newX">New x value.</param>
        /// <returns>
        /// A Vector2 with a new x value.
        /// </returns>
        public static Vector2 SetX(this Vector2 vector, float newX)
        {
            vector.x = newX;
            return vector;
        }

        /// <summary>
        /// Sets the y value of a Vector2 and returns the new vector.
        /// </summary>
        /// <param name="vector">Vector2 to change.</param>
        /// <param name="newY">New y value.</param>
        /// <returns>
        /// A Vector2 with a new y value.
        /// </returns>
        public static Vector2 SetY(this Vector2 vector, float newY)
        {
            vector.y = newY;
            return vector;
        }

		#endregion

		#region Random

		/// <summary>
		/// Gets a random float value in the range between a Vector2's x and y.
		/// </summary>
		/// <param name="vector">Vector2 to get the random value from.</param>
		/// <returns>
		/// A float between Vector2's x and y components.
		/// </returns>
		public static float GetRandomBetweenXAndY(this Vector2 vector) => Random.Range(vector.x, vector.y);

        /// <summary>
		/// Gets a random integer value in the range between a Vector2Int's x and y.
		/// </summary>
		/// <param name="vector">Vector2Int to get the random value from.</param>
		/// <returns>
		/// A float between Vector2Int's x and y components.
		/// </returns>
        public static int GetRandomBetweenXAndY(this Vector2Int vector) => Random.Range(vector.x, vector.y + 1);

		#endregion

		#region Product

        /// <summary>
        /// Gets the product of multiplying Vector2Int's components.
        /// </summary>
        /// <param name="vector">Vector.</param>
        /// <returns>
        /// An integer that is the result of multiplying vector's x and y.
        /// </returns>
		public static int GetComponentMultiplicationProduct(this Vector2Int vector) => vector.x * vector.y;

        /// <summary>
        /// Gets the product of multiplying Vector3Int's components.
        /// </summary>
        /// <param name="vector">Vector.</param>
        /// <returns>
        /// An integer that is the result of multiplying vector's x, y, and z.
        /// </returns>
		public static int GetComponentMultiplicationProduct(this Vector3Int vector) => vector.x * vector.y * vector.z;

        #endregion

	    #region Clamp

	    /// <summary>
	    /// Clamps the value of a Vector3.
	    /// </summary>
	    /// <param name="value">Vector3 to clamp.</param>
	    /// <param name="min">Min vector value.</param>
	    /// <param name="max">Max vector value.</param>
	    public static Vector3 Clamp(this Vector3 value, Vector3 min, Vector3 max)
	    {
		    return new Vector3(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y),Mathf.Clamp(value.z, min.z, max.z));
	    }

	    /// <summary>
	    /// Clamps the value of a Vector2.
	    /// </summary>
	    /// <param name="value">Vector2 to clamp.</param>
	    /// <param name="min">Min vector value.</param>
	    /// <param name="max">Max vector value.</param>
	    public static Vector2 Clamp(this Vector2 value, Vector2 min, Vector2 max)
	    {
		    return new Vector2(Mathf.Clamp(value.x, min.x, max.x), Mathf.Clamp(value.y, min.y, max.y));
	    }

	    #endregion

	    #region Swap

	    /// <summary>
	    /// Swaps x and y components of a Vector2.
	    /// </summary>
	    /// <param name="vector">Vector.</param>
	    /// <returns>
	    /// A Vector2Int whose x is equal to original vector's y and y to x.
	    /// </returns>
	    public static Vector2 SwapComponents(this Vector2 vector) => new Vector2(vector.y, vector.x);
	    
	    /// <summary>
	    /// Swaps x and y components of a Vector2Int.
	    /// </summary>
	    /// <param name="vector">Vector.</param>
	    /// <returns>
	    /// A Vector2Int whose x is equal to original vector's y and y to x.
	    /// </returns>
	    public static Vector2Int SwapComponents(this Vector2Int vector) => new Vector2Int(vector.y, vector.x);

	    #endregion
    }
}