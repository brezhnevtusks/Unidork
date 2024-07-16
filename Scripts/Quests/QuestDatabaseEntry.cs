using System.Collections.Generic;
using UnderdorkStudios.UnderTools.Extensions;
using UnityEngine;

namespace Unidork.QuestSystem
{
	/// <summary>
	/// Stores data a single quest database entry: quest type and all quests of that type.
	/// </summary>
	/// <typeparam name="TQuest">Quest.</typeparam>
	/// <typeparam name="TQuestCategory">Quest category.</typeparam>
	[System.Serializable]
	public class QuestDatabaseEntry<TQuest, TQuestCategory> where TQuest : IQuest<TQuestCategory> where TQuestCategory : System.Enum
	{
		#region Properties

		/// <summary>
		/// Type of quests in this entry.
		/// </summary>
		/// <value>
		/// Gets the value of the field type.
		/// </value>
		public QuestType QuestType => questType;

		#endregion

		#region Fields

		/// <summary>
		/// Type of quests in this entry.
		/// </summary>
		[SerializeField]
		private QuestType questType = default;

		/// <summary>
		/// Quests in this entry.
		/// </summary>
		[SerializeField]
		private TQuest[] quests = null;

		#endregion

		#region Get

		/// <summary>
		/// Gets all quests in this entry.
		/// </summary>
		public TQuest[] Quests() => (TQuest[])quests.Clone();

		/// <summary>
		/// Gets a quest by its id.
		/// </summary>
		/// <param name="id">Quest id.</param>
		/// <returns>
		/// A quest or null if a quest with passed id doesn't exist in this entry.
		/// </returns>
		public TQuest GetQuestById(int id)
		{
			foreach (TQuest quest in quests)
			{
				if (quest.Id != id)
				{
					continue;
				}

				return quest;
			}

			return default;
		}

		/// <summary>
		/// Gets a quest by its id and category.
		/// </summary>
		/// <param name="id">Quest id.</param>
		/// <param name="category">Qiest category.</param>
		/// <returns>
		/// A quest or null if a quest with passed id and category doesn't exist in this entry.
		/// </returns>
		public TQuest GetQuestByIdAndCategory(int id, TQuestCategory category)
		{
			List<TQuest> questsOfCategory = GetQuestsByCategory(category);

			if (questsOfCategory.IsNullOrEmpty())
			{
				return default;
			}

			foreach (TQuest quest in questsOfCategory)
			{
				if (!quest.Category.Equals(category))
				{
					continue;
				}

				return quest;
			}

			return default;
		}
		
		/// <summary>
		/// Gets a random quest that belongs to the passed category.
		/// </summary>
		/// <param name="category">Quest category.</param>
		/// <returns>
		/// A quest that matches the passed quest category or null if such a quest doesn't exist or ques
		/// </returns>
		public TQuest GetQuestByCategory(TQuestCategory category)
		{
			if (quests.IsNullOrEmpty())
			{
				Debug.LogError($"Quest database entry of type {questType} has no quests assigned!");
				return default;
			}

			List<TQuest> questsOfCategory = GetQuestsByCategory(category);

			if (!questsOfCategory.IsEmpty())
			{
				return questsOfCategory.GetRandomOrDefault();
			}
			
			Debug.LogError($"Quest database entry of type {questType} has no quests of category {category}!");
			return default;
		}

		/// <summary>
		/// Gets all quests in this entry that match the passed quest category.
		/// </summary>
		/// <param name="category">Quest category.</param>
		/// <returns>
		/// A list of quests matching the passed category.
		/// </returns>
		private List<TQuest> GetQuestsByCategory(TQuestCategory category)
		{
			var questsOfCategory = new List<TQuest>();

			foreach (TQuest quest in quests)
			{
				if (!quest.Category.Equals(category))
				{
					continue;
				}
				
				questsOfCategory.Add(quest);
			}

			return questsOfCategory;
		}

		#endregion
	}
}