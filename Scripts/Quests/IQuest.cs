using UniRx;

namespace Unidork.QuestSystem 
{
	/// <summary>
	/// Base interface for all objects storing quest data.
	/// </summary>
	/// <typeparam name="TCategory">Category of quest.</typeparam>
	public interface IQuest<out TCategory> where TCategory : System.Enum
	{
		/// <summary>
		/// Quest's unique id.
		/// </summary>
		int Id { get; set; }
		
		/// <summary>
		/// Quest type (daily, weekly, main story, achievement, etc).
		/// </summary>
		QuestType Type { get; }
		
		/// <summary>
		/// Quest category.
		/// </summary>
		TCategory Category { get; }
		
		/// <summary>
		/// Quest's name.
		/// </summary>
		string Name { get; }
		
		/// <summary>
		/// Full description of the quest.
		/// </summary>
		string Description { get; }

		/// <summary>
		/// Quest's current status.
		/// </summary>
		QuestStatus Status { get; }
		
		/// <summary>
		/// Type of value used to measure quest progress.
		/// </summary>
		QuestProgressValueType ProgressValueType { get; }
		
		/// <summary>
		/// Current quest progress value as a float.
		/// For quests that have integer progress values pass floats without decimal part and cast to int when getting this value.
		/// </summary>
		float Progress { get; set; }
		
		/// <summary>
		/// Max quest progress value as a float.
		/// For quests that have integer progress values pass floats without decimal part and cast to int when getting this value.
		/// </summary>
		float MaxProgress { get; }

		/// <summary>
		/// Quest progress as a float in the range [0..1]
		/// </summary>
		float NormalizedProgress { get; }
		
		/// <summary>
		/// Has this quest been completed?
		/// </summary>
		ReactiveProperty<bool> IsCompleted { get; }

		/// <summary>
		/// Number of points towards filling an <see cref="IQuestBar"/> when this quest is completed.
		/// </summary>
		int QuestBarPoints { get; }
		
		/// <summary>
		/// Initializes the quest.
		/// <param name="status">Quest status.</param>
		/// </summary>
		void Init(QuestStatus status);
	}
}