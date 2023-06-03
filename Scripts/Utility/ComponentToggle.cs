using JetBrains.Annotations;
using Unidork.Attributes;
using UnityEngine;

namespace Unidork.Utility
{
	/// <summary>
	/// Toggles a component on and off.
	/// </summary>
	public class ComponentToggle : MonoBehaviour
	{
		#region Fields

		/// <summary>
		/// Component to toggle.
		/// </summary>
		[Space, SettingsHeader, Space]
		[Tooltip("Component to toggle.")]
		[SerializeField]
		private Behaviour componentToToggle = null;

		#endregion
		
		#region Toggle

		/// <summary>
		/// Gets the value of <see cref="MonoBehaviour.enabled"/>
		/// property of <see cref="componentToToggle"/> to the opposite value.
		/// </summary>
		[UsedImplicitly]
		public void Toggle()
		{
			componentToToggle.enabled = !componentToToggle.enabled;
		}

		/// <summary>
		/// Gets the value of <see cref="MonoBehaviour.enabled"/>
		/// property of <see cref="componentToToggle"/> to the passed value.
		/// </summary>
		/// <param name="toggleValue">Value to set.</param>
		[UsedImplicitly]
		public void Toggle(bool toggleValue)
		{
			componentToToggle.enabled = toggleValue;
		}

		#endregion
	}
}