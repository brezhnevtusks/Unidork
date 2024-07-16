using System.Collections.Generic;
using UnderdorkStudios.UnderTools.Extensions;
using Unidork.Progress;
using Unidork.Serialization;
using UniRx;
using UnityEngine;

namespace Unidork.Unlockables
{
    /// <summary>
    /// Base class for unlockable item managers.
    /// </summary>
    public class BaseUnlockableItemManager : MonoBehaviour
    {
		#region Properties

		public ReactiveProperty<float> CurrentProgress { get; private set; }

		/// <summary>
		/// Index of current progress-unlockable item.
		/// </summary>
		/// <value>
		/// Gets and sets the value of the integer 
		/// property <see cref="UnlockableItemSaveData.CurrentProgressUnlockableItemIndex"/>
		/// of <see cref="unlockableItemSaveData"/>.
		/// </value>
		protected int CurrentProgressUnlockableItemIndex
		{
			get => unlockableItemSaveData.CurrentProgressUnlockableItemIndex;
			set => unlockableItemSaveData.CurrentProgressUnlockableItemIndex = value;
		}

		/// <summary>
		/// Number of levels completed towards receiving current progress-unlockable item.
		/// </summary>
		/// <value>
		/// Gets and sets the value of the integer 
		/// property <see cref="UnlockableItemSaveData.LevelsCompletedForCurrentItem"/>
		/// of <see cref="unlockableItemSaveData"/>.
		/// </value>
		protected int LevelsCompletedForCurrentItem
		{
			get => unlockableItemSaveData.LevelsCompletedForCurrentItem;
			set => unlockableItemSaveData.LevelsCompletedForCurrentItem = value;
		}

		#endregion

		#region Fields		

		/// <summary>
		/// Delegate to be used for the event invoked when unlockable item save data is loaded on game's start.
		/// </summary>
		/// <param name="saveData">Unlockable item save data.</param>
		public delegate void UnlockableItemSaveDataLoadHandler(UnlockableItemSaveData saveData);

		/// <summary>
		/// Event that is invoked when unlockable item save data is loaded on game's start.
		/// </summary>
		public static event UnlockableItemSaveDataLoadHandler OnSaveDataLoaded;	

		/// <summary>
		/// Object storing data about unlocked items.
		/// </summary>
		protected UnlockableItemSaveData unlockableItemSaveData;

		/// <summary>
		/// Component that is responsible for serializing and deserializing save data.
		/// </summary>
		protected BaseSerializationManager serializationManager;

		/// <summary>
		/// Component that manages operations with player's progress.
		/// </summary>
		protected BaseProgressManager progressManager;

		/// <summary>
		/// Scriptable object that stores data about items that are 
		/// unlocked by progressing through levels.
		/// </summary>
		[Tooltip("Scriptable object that stores data about items that are " +
				 "unlocked by progressing through levels.")]
		[SerializeField]
		private ProgressUnlockableItemProgression progressUnlockableItemProgression = null;

		/// <summary>
		/// Scriptable object storing references to all progress-unlockable items in the game.
		/// </summary>
		[Tooltip("Scriptable object storing references to all progress-unlockable items in the game.")]
		[SerializeField]
		private ProgressUnlockableItemDatabase progressUnlockableItemDatabase = null;

		/// <summary>
		/// Current entry in the <see cref="ProgressUnlockableItemProgression"/>.
		/// </summary>
		private ProgressUnlockableItemProgressionEntry currentProgressionEntry;

		#endregion
		
		#region Constants
		
		/// <summary>
		/// Key to use when saving/loading with Easy Save.
		/// </summary>
		private const string UnlockableKeyName = "Ublockables";
		
		/// <summary>
		/// Relative file path to use when saving/loading with Easy Save.
		/// </summary>
		private const string UnlockableSavePath = "Unlockables.und";
		
		#endregion

		#region Init

		protected virtual void Start()
		{
			progressManager = FindAnyObjectByType<BaseProgressManager>();

			serializationManager = FindAnyObjectByType<BaseSerializationManager>();

			CurrentProgress = new ReactiveProperty<float>();

			LoadSaveData();
		}

		#endregion

		#region Item IDs

		/// <summary>
		/// Adds an unlocked item ID to save file and serializes it.
		/// </summary>
		/// <param name="itemId">Unlocked item ID.</param>
		public void AddUnlockedItem(int itemId)
		{
			bool itemWasAdded = unlockableItemSaveData.UnlockedProgressUnlockableItemIds.AddUnique(itemId);

			if (!itemWasAdded)
			{
				return;
			}

			SaveData();
		}

		/// <summary>
		/// Gets the IDs of the items already unlocked by the player.
		/// </summary>
		/// <returns>
		/// A list of integers that represent IDs of items already unlocked by the player
		/// stored in <see cref="unlockableItemSaveData"/>'s UnlockedItemIds property.
		/// </returns>
		public List<int> GetUnlockedItemIds() => unlockableItemSaveData.UnlockedProgressUnlockableItemIds;

		#endregion

		#region Progress Unlockable Items

		/// <summary>
		/// Increases the progress towards the current unlockable item.
		/// </summary>
		public void IncreaseUnlockableItemProgress()
		{
			LevelsCompletedForCurrentItem++;

			float currentProgress = Mathf.Clamp01(
				LevelsCompletedForCurrentItem / (float)currentProgressionEntry.LevelsToUnlockItem);			

			if (Mathf.Approximately(currentProgress, 1f)) 
			{
				currentProgress = 1f;
			}

			CurrentProgress.Value = currentProgress;
		}

		/// <summary>
		/// Selects the next unlockable item if previous item's progression has been completed
		/// and there are locked items available in progress-unlockable item database.
		/// </summary>
		public void SelectNextUnlockableItem()
		{
			if (AllProgressUnlockableItemsAreUnlocked())
			{
				return;
			}

			if (currentProgressionEntry.SelectionType == ProgressUnlockableItemSelectionType.Manual)
            {
				CurrentProgressUnlockableItemIndex++;

				for (; CurrentProgressUnlockableItemIndex < progressUnlockableItemDatabase.NumberOfItems - 1; CurrentProgressUnlockableItemIndex++)
				{
					ProgressUnlockableItemProgressionEntry progressionEntry =
						progressUnlockableItemProgression.GetProgressionEntryAtIndex(CurrentProgressUnlockableItemIndex);
					
					int itemId = progressionEntry.UnlockableItemData.ItemId;

					bool itemIsAlreadyUnlocked = unlockableItemSaveData.UnlockedProgressUnlockableItemIds.Contains(itemId);

					if (!itemIsAlreadyUnlocked)
					{
						break;
					}
				}
			}

			currentProgressionEntry =
				progressUnlockableItemProgression.GetProgressionEntryAtIndex(CurrentProgressUnlockableItemIndex);

			CurrentProgress.Value = 0f;

			LevelsCompletedForCurrentItem = 0;

			SaveData();
		}

		/// <summary>
		/// Have all progress-unlockable items in the game been unlocked by the player?
		/// </summary>
		/// <returns>
		/// True if all progress-unlockable items have been unlocked, False otherwise.
		/// </returns>
		public bool AllProgressUnlockableItemsAreUnlocked()
		{
			return progressUnlockableItemDatabase.NumberOfItems == unlockableItemSaveData.UnlockedProgressUnlockableItemIds.Count;
		}

		/// <summary>
		/// Gets a progress-unlockable item from the passed <see cref="ProgressUnlockableItemProgressionEntry"/>.
		/// </summary>
		/// <param name="unlockedItemIds">IDs of progress-unlockable items that have already been unlocked.</param>
		/// <returns>
		/// A <see cref="ProgressUnlockableItemData"/> from the array
		/// matching the passed index or the last element of the array 
		/// if the index exceeds array's length.
		/// </returns>
		public ProgressUnlockableItemData GetNewUnlockableItem()
		{
			return currentProgressionEntry.SelectionType switch
			{
				ProgressUnlockableItemSelectionType.Manual => currentProgressionEntry.UnlockableItemData,
				ProgressUnlockableItemSelectionType.Random => GetRandomUnlockableItem(unlockableItemSaveData.UnlockedProgressUnlockableItemIds),
				_ => throw new System.NotImplementedException($"Unhandled item selection type: {currentProgressionEntry.SelectionType}")
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="unlockedItemIds">IDs of progress-unlockable items that have already been unlocked.</param>
		/// <returns>
		/// A random <see cref="ProgressUnlockableItemData"/> from the 
		/// <see cref="ProgressUnlockableItemDatabase"/> that hasn't been unlocked yet.
		/// </returns>
		private ProgressUnlockableItemData GetRandomUnlockableItem(List<int> unlockedItemIds)
		{
			List<ProgressUnlockableItemData> allItems = progressUnlockableItemDatabase.UnlockableItems;

			List<ProgressUnlockableItemData> lockedItems =
				allItems.FindAll(data => !unlockedItemIds.Contains(data.ItemId));

			return lockedItems.GetRandomOrDefault();
		}

		#endregion

		#region Serialization

		/// <summary>
		/// Serializes unlockable item data to disk.
		/// </summary>
		public void SaveData()
		{
			BaseSerializationManager.Save(UnlockableKeyName, unlockableItemSaveData, UnlockableSavePath);
		}

		/// <summary>
		/// Resets the unlockable item save data, creating a file with default values.
		/// </summary>
		public void ResetSaveData()
		{
			unlockableItemSaveData = new UnlockableItemSaveData(BaseSerializationManager.SaveVersion);

			if (serializationManager == null)
			{
				serializationManager = FindAnyObjectByType<BaseSerializationManager>();
			}

			SaveData();
		}

		/// <summary>
		/// Loads the save data on game start. Creates save data objects if they don't exist.
		/// </summary>
		protected virtual void LoadSaveData()
		{
			unlockableItemSaveData = BaseSerializationManager.Load<UnlockableItemSaveData>(UnlockableKeyName, UnlockableSavePath);

			if (unlockableItemSaveData == null)
			{
				ResetSaveData();
			}

			currentProgressionEntry = 
				progressUnlockableItemProgression.GetProgressionEntryAtIndex(CurrentProgressUnlockableItemIndex);

			float currentProgress = Mathf.Clamp01(
				LevelsCompletedForCurrentItem / (float)currentProgressionEntry.LevelsToUnlockItem);

			if (Mathf.Approximately(currentProgress, 1f))
			{
				currentProgress = 1f;
			}

			CurrentProgress.Value = currentProgress;

			OnSaveDataLoaded?.Invoke(unlockableItemSaveData);
		}		

		#endregion
	}
}