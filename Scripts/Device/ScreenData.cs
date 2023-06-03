using Unidork.Attributes;
using UnityEngine;

namespace Unidork.DeviceUtility
{
	/// <summary>
	/// Stores data about user's device screen.
	/// </summary>
    public class ScreenData : MonoBehaviour
    {
		#region Properties

		/// <summary>
		/// Relation between reference screen size and user's device screen size.
		/// </summary>
		public static float ScreenScaleFactor { get; private set; }

		#endregion

		#region Fields

		/// <summary>
		/// Reference resolution used when making the game in the editor.
		/// </summary>
		[Space, SettingsHeader, Space]
		[Tooltip("Reference resolution used when making the game in the editor.")]
		[SerializeField]
		private Vector2Int referenceResolution = new Vector2Int(1440, 2960);

		#endregion

		#region Init

		private void Start() => CalculateScreenScaleFactor();

		#endregion

		#region Scale

		/// <summary>
		/// Calculates the screen's scale factor.
		/// </summary>
		private void CalculateScreenScaleFactor()
		{
			int screenHeight = Screen.height;
			int screenWidth = Screen.width;

			if (screenHeight < screenWidth && referenceResolution.y > referenceResolution.x)
			{
				int tempValue = screenHeight;
				screenHeight = screenWidth;
				screenWidth = tempValue;
			}

			ScreenScaleFactor = ((screenWidth / (float)referenceResolution.x) + (screenHeight / (float)referenceResolution.y)) * 0.5f;
		}

		#endregion
	}
}