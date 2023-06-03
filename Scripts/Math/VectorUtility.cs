using UnityEngine;
	
namespace Unidork.Math
{
	/// <summary>
	/// Utility methods for Unity's vectors.
	/// </summary>
	public static class VectorUtility
	{
		#region Fields

		/// <summary>
		/// Array storing all cardinal and ordinal directions of a compass as Vector 2 values.
		/// </summary>
		private static readonly Vector2[] cardinalOrdinalDirections;

		#endregion
		
		#region Constructor

		static VectorUtility()
		{
			cardinalOrdinalDirections = new Vector2[] {
				Vector2.right,
				Vector2.one,
				Vector2.up,
				new Vector2(-1f, 1f),
				Vector2.left,
				-Vector2.one,
				Vector2.down,
				new Vector2(1f, -1f)
			};
		}

		#endregion
		
		#region Direction

		/// <summary>
		/// Rounds a passed vector to the closest cardinal or ordinal direction of a compass where Vector2.up is North, Vector2.right is east and so on.
		/// </summary>
		/// <param name="originalVector">Original vector.</param>
		/// <returns>A Vector2 representing a cardinal or ordinal direction of a compass closest to the passed vector.</returns>
		public static Vector2 GetCardinalOrOrdinalDirection(Vector2 originalVector)
		{
			int directionIndex = ((int) Mathf.Round(Mathf.Atan2(originalVector.y, originalVector.x) / (2 * Mathf.PI / 8)) + 8) % 8;
			return cardinalOrdinalDirections[directionIndex];
		}

		#endregion
	}
}