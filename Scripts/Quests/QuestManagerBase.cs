using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnderdorkStudios.UnderTools.Extensions;
using Unidork.Serialization;
using UnityEngine;

namespace Unidork.QuestSystem
{
	public class QuestManagerBase<TQuest, TQuestCategory> : MonoBehaviour where TQuest : IQuest<TQuestCategory> where TQuestCategory : System.Enum
	{
		#region Fields
		
		/// <summary>
		/// Objects storing saved quest data.
		/// </summary>
		protected QuestSaveData<TQuestCategory> questSaveData;

		/// <summary>
		/// 
		/// </summary>
		[SerializeField] 
		private QuestDatabase<TQuest, TQuestCategory> questDatabase = null;

		/// <summary>
		/// Dictionary storing quests that are currently active in the game.
		/// </summary>
		/// <remarks>
		/// "Active" here refers to the fact quests have been selected or loaded by the manager from save data
		/// for player to complete, not the actual quest status of active.
		/// </remarks>
		[ShowInInspector]
		private readonly Dictionary<QuestType, List<TQuest>> activeQuestDictionary = new Dictionary<QuestType, List<TQuest>>();

		#endregion
		
		#region Constants
		
		/// <summary>
		/// Key to use when saving/loading with Easy Save.
		/// </summary>
		private const string QuestKeyName = "Quests";
		
		/// <summary>
		/// Relative file path to use when saving/loading with Easy Save.
		/// </summary>
		private const string QuestSavePath = "Quests.und";
		
		#endregion

		#region Init

		/// <summary>
		/// Can be overriden in inheriting classes to migrate save data if necessary when save version changes.
		/// </summary>
		protected virtual void MigrateSaveData() { }
		
		private void Start()
		{
			
			questSaveData = BaseSerializationManager.Load<QuestSaveData<TQuestCategory>>(QuestKeyName, QuestSavePath);

			if (questSaveData != null)
			{
				MigrateSaveData();

				foreach (SavedQuest<TQuestCategory> savedQuest in questSaveData.SavedQuests)
				{
					TQuest quest = questDatabase.GetQuestByIdTypeAndCategory(savedQuest.Id, savedQuest.Type, savedQuest.Category);

					if (quest == null)
					{
						continue;
					}

					QuestType questType = quest.Type;
					
					if (activeQuestDictionary.ContainsKey(questType))
					{
						_ = activeQuestDictionary[questType].AddUnique(quest);
					}
					else
					{
						activeQuestDictionary.Add(questType, new List<TQuest>() {quest});
					}
				}
				
				return;
			}
			
			ResetSaveData();
		}

		#endregion

		#region Serialization

		/// <summary>
		/// Serializes
		/// </summary>
		public void SerializeSaveData()
		{
			BaseSerializationManager.Save(QuestKeyName, questSaveData, QuestSavePath);
		}
		
		/// <summary>
		/// Resets the quest save data.
		/// </summary>
		public void ResetSaveData()
		{
			questSaveData = new QuestSaveData<TQuestCategory>(BaseSerializationManager.SaveVersion);
			SerializeSaveData();
		}

		#endregion
	}
}