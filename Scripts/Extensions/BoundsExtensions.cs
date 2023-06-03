using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	
namespace Unidork.Extensions
{
	public static class BoundsExtensions
	{
		/// <summary>
		/// Gets the top left corner of a bounds of a 2D collider.
		/// </summary>
		/// <param name="bounds">Bounds bounds.</param>
		/// <returns>
		/// A Vector2.
		/// </returns>
		public static Vector2 GetTopLeftCorner2D(this Bounds bounds)
		{
			return new Vector2(bounds.min.x, bounds.max.y);
		}
		
		/// <summary>
		/// Gets the top right corner of a bounds of a 2D collider.
		/// </summary>
		/// <param name="bounds">Bounds bounds.</param>
		/// <returns>
		/// A Vector2.
		/// </returns>
		public static Vector2 GetTopRightCorner2D(this Bounds bounds)
		{
			return new Vector2(bounds.max.x, bounds.max.y);
		}
		
		/// <summary>
		/// Gets the bottom left corner of a bounds of a 2D collider.
		/// </summary>
		/// <param name="bounds">Bounds bounds.</param>
		/// <returns>
		/// A Vector2.
		/// </returns>
		public static Vector2 GetBottomLeftCorner2D(this Bounds bounds)
		{
			return new Vector2(bounds.min.x, bounds.min.y);
		}
		
		/// <summary>
		/// Gets the bottom right corner of a bounds of a 2D collider.
		/// </summary>
		/// <param name="bounds">Bounds bounds.</param>
		/// <returns>
		/// A Vector2.
		/// </returns>
		public static Vector2 GetBottomRightCorner2D(this Bounds bounds)
		{
			return new Vector2(bounds.max.x, bounds.min.y);
		}
	}
}
	

