namespace Unidork.UI
{
	/// <summary>
	/// Defines how a <see cref="PendingActionBadge"/> is animated.
	/// </summary>
	public enum PendingActionBadgeAnimationType
	{
		/// <summary>
		/// Badge is shown/hidden instantly and has no loop animations.
		/// </summary>
		None,
		
		/// <summary>
		/// Badge animations are driven by an animator controller.
		/// </summary>
		Animator,
		
		/// <summary>
		/// Badge animations are driven by a tween.
		/// </summary>
		Tween
	}
}