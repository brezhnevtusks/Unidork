using UnityEngine;

namespace Unidork.OffScreenTargets
{
	/// <summary>
	/// Stores data about an off screen target: reference to the target and its viewport position, so we don't have to recalculate it multiple times.
	/// </summary>
	public class OffScreenTargetData
	{
		/// <summary>
		/// Reference to an off screen target.
		/// </summary>
		public IOffScreenTarget Target;
		
		/// <summary>
		/// Off screen target's viewport position.
		/// </summary>
		public Vector2 ViewportPosition;
	}
}