using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unidork.Attributes;
using Unidork.Extensions;
using Unidork.Serialization;
using UnityEngine;

namespace Unidork.QuestSystem
{
	public class QuestManagerBase<TQuest, TQuestCategory> : MonoBehaviour where TQuest : IQuest<TQuestCategory> where TQuestCategory : System.Enum
	{
		#region Fields
		
		/// <summary>
		/// Path to the persistent data directory.
		/// </summary>
		protected string persistentDataPath;
		
		/// <summary>
		/// Objects storing saved quest data.
		/// </summary>
		protected QuestSaveData<TQuestCategory> questSaveData;
		
		/// <summary>
		/// Path of the quest save data file relative to <see cref="Application.persistentDataPath"/>.
		/// </summary>
		[Space, BaseHeader, Space]
		[Tooltip("Path of the quest save data file relative to Application.persistentDataPath.")]
		[SerializeField]
		private string questSaveDataRelativePath = "/QuestSaveData.json";

		/// <summary>
		/// 
		/// </summary>
		[SerializeField] 
		private QuestDatabase<TQuest, TQuestCategory> questDatabase = null;

		/// <summary>
		/// Path at which quest save data is stored.
		/// </summary>
		private string questSaveDataPath;

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

		#region Init

		/// <summary>
		/// Can be overriden in inheriting classes to migrate save data if necessary when save version changes.
		/// </summary>
		protected virtual void MigrateSaveData() { }
		
		private void Start()
		{
			persistentDataPath = Application.persistentDataPath;

			questSaveDataPath = persistentDataPath + questSaveDataRelativePath;
			
			questSaveData = BaseSerializationManager.DeserializeSaveDataFromFile<QuestSaveData<TQuestCategory>>(questSaveDataPath);

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
			BaseSerializationManager.SerializeSaveDataToFile(questSaveData, questSaveDataPath);
		}
		
		/// <summary>
		/// Resets the quest save data.
		/// </summary>
		public void ResetSaveData()
		{
			questSaveData = new QuestSaveData<TQuestCategory>(BaseSerializationManager.SaveVersion);
			BaseSerializationManager.SerializeSaveDataToFile(questSaveData, questSaveDataPath);
		}

		#endregion
	}
}