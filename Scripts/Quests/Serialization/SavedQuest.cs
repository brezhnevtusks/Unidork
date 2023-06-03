using UnityEngine;

namespace Unidork.QuestSystem
{
	/// <summary>
	/// Objects storing save data for a quest.
	/// </summary>
	[System.Serializable]
	public class SavedQuest<TQuestCategory> where TQuestCategory : System.Enum
	{
		#region Properties

		/// <summary>
		/// Unique quest id.
		/// </summary>
		/// <value>
		/// Gets and sets the value of the integer field id.
		/// </value>
		public int Id { get => id; set => id = value; }

		/// <summary>
		/// Quest type.
		/// </summary>
		/// <value>
		/// Gets the value of the field type.
		/// </value>
		public QuestType Type => type;

		/// <summary>
		/// Quest category.
		/// </summary>
		/// <value>
		/// Gets the value of the field category.
		/// </value>
		public TQuestCategory Category => category;

		/// <summary>
		/// Quest progress at time of saving.
		/// </summary>
		/// <value>
		/// Gets and sets the value of the float value currentProgress.
		/// </value>
		public float CurrentProgress { get => currentProgress; set => currentProgress = value; }

		#endregion
		
		#region Fields

		/// <summary>
		/// Unique quest id.
		/// </summary>
		[SerializeField]
		private int id = 0;

		/// <summary>
		/// Quest type.
		/// </summary>
		[SerializeField]
		private QuestType type = default;

		/// <summary>
		/// Quest category.
		/// </summary>
		[SerializeField]
		private TQuestCategory category = default;

		/// <summary>
		/// Quest progress at time of saving.
		/// </summary>
		[SerializeField]
		private float currentProgress = 0f;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">Unique quest id.Unique quest id.</param>
		/// <param name="type">Quest type.</param>
		/// <param name="category">Quest category.</param>
		/// <param name="currentProgress">Quest progress at time of saving.</param>
		public SavedQuest(int id, QuestType type, TQuestCategory category, float currentProgress)
		{
			this.id = id;
			this.type = type;
			this.category = category;
			this.currentProgress = currentProgress;
		}

		#endregion
	}
}