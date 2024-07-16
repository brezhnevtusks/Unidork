using UnderdorkStudios.UnderTools.Extensions;
using UnityEngine;

namespace Unidork.QuestSystem
{
	/// <summary>
	/// Scriptable object storing entries with quest types and quests corresponding with those types.
	/// </summary>
	/// <typeparam name="TQuest">Quest.</typeparam>
	/// <typeparam name="TQuestCategory">Quest category.</typeparam>
	public class QuestDatabase<TQuest, TQuestCategory> : ScriptableObject where TQuest : IQuest<TQuestCategory> where TQuestCategory : System.Enum
	{
		#region Fields

		/// <summary>
		/// List of entries stored in this database.
		/// </summary>
		[SerializeField]
		private QuestDatabaseEntry<TQuest, TQuestCategory>[] entries = null;

		#endregion

		#region Get

		/// <summary>
		/// Gets an entry that matches the passed <see cref="QuestType"/>.
		/// </summary>
		/// <param name="questType">Quest type.</param>
		/// <returns>
		/// A quest database entry matching the passed type or null if such entry doesn't exists or entry array is null or empty.
		/// </returns>
		public QuestDatabaseEntry<TQuest, TQuestCategory> GetEntryOfType(QuestType questType)
		{
			if (entries.IsNullOrEmpty())
			{
				Debug.LogError($"Quest database {name} is empty!");
				return null;
			}

			foreach (QuestDatabaseEntry<TQuest, TQuestCategory> entry in entries)
			{
				if (entry.QuestType != questType)
				{
					continue;
				}

				return entry;
			}

			Debug.LogError($"Failed to find quest database entry of type {questType}!");
			return null;
		}

		/// <summary>
		/// Gets a random quest of specified type and category.
		/// </summary>
		/// <param name="questType">Quest type.</param>
		/// <param name="questCategory">Quest category.</param>
		/// <returns>
		/// A <see cref="TQuest"/> that matches the passed quest type and category or null if such quest doesn't exist.
		/// </returns>
		public TQuest GetQuestByTypeAndCategory(QuestType questType, TQuestCategory questCategory)
		{
			QuestDatabaseEntry<TQuest, TQuestCategory> databaseEntry = GetEntryOfType(questType);

			if (databaseEntry != null)
			{
				return databaseEntry.GetQuestByCategory(questCategory);
			}
			
			Debug.LogError($"Quest database {name} doesn't have an entry of type {questType}!");
			return default;
		}

		/// <summary>
		/// Gets a quest by its id.
		/// </summary>
		/// <param name="id">Quest id.</param>
		/// <returns>
		/// A quest or null if quest with passed id doesn't exist.
		/// </returns>
		public TQuest GetQuestById(int id)
		{
			if (entries.IsNullOrEmpty())
			{
				Debug.LogError($"Quest database {name} is empty!");
				return default;
			}

			foreach (QuestDatabaseEntry<TQuest, TQuestCategory> entry in entries)
			{
				TQuest quest = entry.GetQuestById(id);

				if (quest == null)
				{
					continue;
				}

				return quest;
			}

			Debug.LogError($"Quest database {name} doesn't contain quest with id {id}!");
			return default;
		}

		/// <summary>
		/// Gets a quest by its id and quest type.
		/// </summary>
		/// <param name="id">Quest id.</param>
		/// <param name="type">Quest type.</param>
		/// <returns>
		/// A quest or null if quest with passed id and quest type doesn't exist.
		/// </returns>
		public TQuest GetQuestByIdAndType(int id, QuestType type)
		{
			if (entries.IsNullOrEmpty())
			{
				Debug.LogError($"Quest database {name} is empty!");
				return default;
			}

			QuestDatabaseEntry<TQuest, TQuestCategory> entry = GetEntryOfType(type);

			if (entry == null)
			{
				Debug.LogError($"Quest database {name} doesn't have an entry of type {type}!");
				return default;
			}

			return entry.GetQuestById(id);
		}

		
		/// <summary>
		/// Gets a quest by its id, quest type, and category..
		/// </summary>
		/// <param name="id">Quest id.</param>
		/// <param name="type">Quest type.</param>
		/// <param name="category">Quest category.</param>
		/// <returns>
		/// A quest or null if quest with passed id, quest type, and category doesn't exist.
		/// </returns>
		public TQuest GetQuestByIdTypeAndCategory(int id, QuestType type, TQuestCategory category)
		{
			if (entries.IsNullOrEmpty())
			{
				Debug.LogError($"Quest database {name} is empty!");
				return default;
			}

			QuestDatabaseEntry<TQuest, TQuestCategory> entry = GetEntryOfType(type);

			if (entry == null)
			{
				Debug.LogError($"Quest database {name} doesn't have an entry of type {type}!");
				return default;
			}

			return entry.GetQuestByIdAndCategory(id, category);
		}

		#endregion
	}
}