using System.Collections.Generic;
using Unidork.Serialization;

namespace Unidork.QuestSystem
{
	/// <summary>
	/// Stores save data about active quests.
	/// </summary>
	[System.Serializable]
	public class QuestSaveData<TQuestCategory> : BaseSaveData where TQuestCategory : System.Enum
	{
		#region Properties

		/// <summary>
		/// List of all saved quests.
		/// </summary>
		/// <value>
		/// Gets the value of the field savedQuests.
		/// </value>
		public List<SavedQuest<TQuestCategory>> SavedQuests => savedQuests;

		#endregion
		
		#region Fields

		/// <summary>
		/// List of all saved quests.
		/// </summary>
		private readonly List<SavedQuest<TQuestCategory>> savedQuests;

		#endregion
		
		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="saveVersion">Save version.</param>
		public QuestSaveData(string saveVersion) : base(saveVersion)
		{
			savedQuests = new List<SavedQuest<TQuestCategory>>();
		}

		#endregion
	}
}