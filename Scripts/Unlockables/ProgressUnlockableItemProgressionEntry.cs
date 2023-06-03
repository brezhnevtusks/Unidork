using Sirenix.OdinInspector;
using Unidork.Attributes;
using UnityEngine;

namespace Unidork.Unlockables
{
	/// <summary>
	/// Stores an entry in the <see cref="ProgressUnlockableItemProgression"/>.
	/// </summary>
	[System.Serializable]
	public class ProgressUnlockableItemProgressionEntry
    {
		#region Properties

		/// <summary>
		/// Way the progress-unlockable item is selected.
		/// </summary>
		/// <value>
		/// Gets the value of the enum field selectionType.
		/// </value>
		public ProgressUnlockableItemSelectionType SelectionType => selectionType;

		/// <summary>
		/// Unlockable item data.
		/// </summary>
		/// <value>
		/// Gets the value of the field unlockableItem.
		/// </value>
		public ProgressUnlockableItemData UnlockableItemData => unlockableItemData;

		/// <summary>
		/// Number of leves (stages) player has to beat to unlock the item.
		/// </summary>
		/// <value>
		/// Gets the value of the integer field levelsToUnlockItem.
		/// </value>
		public int LevelsToUnlockItem => levelsToUnlockItem;

		#endregion

		#region Fields

		/// <summary>
		/// Way the progress-unlockable item is selected.
		/// </summary>
		[Space, SettingsHeader, Space]
		[Tooltip("Way the progress-unlockable item is selected.")]
		[SerializeField]
		private ProgressUnlockableItemSelectionType selectionType = default;

		/// <summary>
		/// Unlockable item data.
		/// </summary>
		[ShowIf("@this.selectionType", ProgressUnlockableItemSelectionType.Manual)]
		[Tooltip("Unlockable item data.")]
		[SerializeField]
		private ProgressUnlockableItemData unlockableItemData = null;

		/// <summary>
		/// Number of leves (stages) player has to beat to unlock the item.
		/// </summary>
		[Tooltip("Number of leves (stages) player has to beat to unlock the item.")]
		[SerializeField]
		private int levelsToUnlockItem = 4;

		#endregion
	}
}