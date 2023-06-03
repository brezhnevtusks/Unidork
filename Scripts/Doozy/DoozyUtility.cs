#if dUI_MANAGER
using Doozy.Engine.UI;
using System.Collections.Generic;
using UnityEngine.UI;

namespace Unidork.DoozyUI
{
	/// <summary>
	/// Utility methods for Doozy UI asset.
	/// </summary>
	public static class DoozyUtility
    {
		#region Fields

		/// <summary>
		/// Disables raycasters on all active UI views.
		/// Views that don't toggle raycasters on show/hide are ignored.
		/// </summary>
		public static void DisableRaycastersOnActiveViews() => ToggleRaycastersOnActiveViews(toggleValue: false);

		/// <summary>
		/// Enables raycasters on all active UI views.
		/// Views that don't toggle raycasters on show/hide are ignored.
		/// </summary>
		public static void EnableRaycastersOnActiveViews() => ToggleRaycastersOnActiveViews(toggleValue: true);

		/// <summary>
		/// Toggles raycasters on all active UI views.
		/// Views that don't toggle raycasters on show/hide are ignored.
		/// </summary>
		/// <param name="toggleValue">Toggle value.</param>
		private static void ToggleRaycastersOnActiveViews(bool toggleValue)
		{
			List<UIView> visibleViews = UIView.VisibleViews;

			foreach (UIView visibleView in visibleViews)
			{
				if (!visibleView.DisableGraphicRaycasterWhenHidden)
				{
					continue;
				}

				GraphicRaycaster currentRaycaster = visibleView.GraphicRaycaster;
				currentRaycaster.enabled = toggleValue;
			}
		}

		#endregion
	}
}

#endif