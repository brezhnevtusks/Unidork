using System.Collections.Generic;
using Unidork.Serialization;
using UnityEngine;

namespace Unidork.Unlockables
{
	/// <summary>
	/// Stores save data for
	/// </summary>
	[System.Serializable]
	public class UnlockableItemSaveData : BaseSaveData
	{
		#region Properties

		/// <summary>
		/// IDs of unlockable items already unlocked by the player.
		/// </summary>
		/// <value>
		/// Gets the value of the field unlockedItemIds.
		/// </value>
		public List<int> UnlockedItemIds => unlockedItemIds;

		/// <summary>
		/// IDs of progress-unlockable items already unlocked by the player.
		/// </summary>
		/// <value>
		/// Gets the value of the field unlockedProgressUnlockableItemIds.
		/// </value>
		public List<int> UnlockedProgressUnlockableItemIds => unlockedProgressUnlockableItemIds;

		/// <summary>
		/// Index of current progress-unlockable item.
		/// </summary>
		/// <value>
		/// Gets and sets the value of the integer field currentProgressUnlockableItemIndex.
		/// </value>
		public int CurrentProgressUnlockableItemIndex 
		{
			get => currentProgressUnlockableItemIndex;
			set => currentProgressUnlockableItemIndex = value;
		}

		/// <summary>
		/// Number of levels completed towards receiving current progress-unlockable item.
		/// </summary>
		/// <value>
		/// Gets and sets the value of the integer field levelsCompletedForNextItem.
		/// </value>
		public int LevelsCompletedForCurrentItem 
		{ 
			get => levelsCompletedForCurrentItem; 
			set => levelsCompletedForCurrentItem = value; 
		}

		#endregion

		#region Fields

		/// <summary>
		/// IDs of unlockable items already unlocked by the player.
		/// </summary>
		[SerializeField]
		private List<int> unlockedItemIds;

		/// <summary>
		/// IDs of progress-unlockable items already unlocked by the player.
		/// </summary>
		[SerializeField]
		private List<int> unlockedProgressUnlockableItemIds;	
		

		/// <summary>
		/// Index of the <see cref="ProgressUnlockableItemData"/>
		/// currently used for progress-unlockable item calculations.
		/// </summary>
		[SerializeField]
		private int currentProgressUnlockableItemIndex;

		/// <summary>
		/// Number of levels completed towards receiving current progress-unlockable item.
		/// </summary>
		[SerializeField]
		private int levelsCompletedForCurrentItem;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="saveVersion">Save version.</param>
		public UnlockableItemSaveData(string saveVersion) : base(saveVersion)
		{
			unlockedItemIds = new List<int>();
			unlockedProgressUnlockableItemIds = new List<int>();
			currentProgressUnlockableItemIndex = 0;
			levelsCompletedForCurrentItem = 0;
		}

		#endregion
	}
}