using System.Collections.Generic;
using UnityEngine;

namespace Unidork.Unlockables
{
	/// <summary>
	/// Scriptable object storing references to all progress-unlockable items in the game.
	/// </summary>
	[CreateAssetMenu(fileName = "ProgressUnlockableItemDatabase",
                     menuName = "Progress/Progress Unlockable Item Database")]
    public class ProgressUnlockableItemDatabase : ScriptableObject
    {
		#region Properties

		/// <summary>
		/// Total number of items in the database.
		/// </summary>
		public int NumberOfItems => unlockableItems.Count;

		/// <summary>
		/// List storing scriptable object that hold data about unlockable items.
		/// </summary>
		/// <value>
		/// Gets the value of the field unlockableItems.
		/// </value>
		public List<ProgressUnlockableItemData> UnlockableItems => unlockableItems;

		#endregion

		#region Fields

		/// <summary>
		/// List storing scriptable object that hold data about unlockable items.
		/// </summary>
		[Tooltip("List storing scriptable object that hold data about unlockable items.")]
        [SerializeField]
        private List<ProgressUnlockableItemData> unlockableItems = null;

		#endregion
	}
}