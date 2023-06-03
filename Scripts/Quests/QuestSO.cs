using JetBrains.Annotations;
using Sirenix.OdinInspector;
using Unidork.Attributes;
using UniRx;
using UnityEngine;

namespace Unidork.QuestSystem
{
	/// <summary>
	/// Base class for objects storing data about a quest.
	/// </summary>
	public class QuestSO<T> : ScriptableObject, IQuest<T> where T : System.Enum
	{
		#region Properties

		/// <inheritdoc />
		public int Id { get => id; set => id = value; }

		/// <inheritdoc />
		public QuestType Type => questType;

		/// <inheritdoc />
		public T Category => category;

		/// <inheritdoc />
		public string Name => name;

		/// <inheritdoc />
		public string Description => description;

		/// <inheritdoc />
		public ReactiveProperty<bool> IsCompleted { get; private set; }

		/// <inheritdoc />
		public QuestStatus Status { get; set; }
		
		/// <inheritdoc />
		public QuestProgressValueType ProgressValueType => progressValueType;

		/// <inheritdoc />
		public float Progress
		{
			get => progress;
			set
			{
				progress = value;
				NormalizedProgress = Mathf.Clamp01(progress / MaxProgress);

				if (!Mathf.Approximately(NormalizedProgress, 1f))
				{
					return;
				}
				
				IsCompleted.SetValueAndForceNotify(true);
			}
		}

		/// <inheritdoc />
		public float MaxProgress => maxProgress;

		/// <inheritdoc />
		public float NormalizedProgress { get; private set; }

		/// <inheritdoc />
		public int QuestBarPoints => questBarPoints;

		#endregion

		#region Fields

		/// <summary>
		/// Quest's unique id.
		/// </summary>
		[Space, SettingsHeader, Space]
		[Tooltip("Quest's unique id.")]
		[SerializeField]
		private int id = -1;

		/// <summary>
		/// Type of quest.
		/// </summary>
		[Tooltip("Type of quest.")]
		[SerializeField]
		private QuestType questType = default;

		/// <summary>
		/// Quest category.
		/// </summary>
		[Tooltip("Quest category.v")]
		[SerializeField]
		private T category = default;
		
		/// <summary>
		/// Quest's name.
		/// </summary>
		[Tooltip("Quest's name.Quest's name.")]
		[SerializeField]
		private string name = "Quest name";

		/// <summary>
		/// Full description of the quest.
		/// </summary>
		[Tooltip("Full description of the quest.")]
		[SerializeField]
		private string description = "Quest description";

		/// <summary>
		/// Type of value used to measure quest progress.
		/// </summary>
		[Tooltip("Type of value used to measure quest progress.")] 
		[SerializeField]
		private QuestProgressValueType progressValueType = default;

		/// <summary>
		/// Max quest progress value as a float.
		/// For quests that have integer progress values pass floats without decimal part and cast to int when getting this value.
		/// </summary>
		[Tooltip("Max quest progress value as a float." +
		          "For quests that have integer progress values pass floats without decimal part and cast to int when getting this value.")]
		[SerializeField]
		private float maxProgress = 1f;

		/// <summary>
		/// Is this quest used to fill a <see cref="IQuestBar"/>?
		/// </summary>
		[UsedImplicitly]
		[Tooltip("Is this quest used to fill a quest bar?")]
		[SerializeField]
		bool usedInQuestBar = false;

		/// <summary>
		/// Number of points towards filling an <see cref="IQuestBar"/> when this quest is completed.
		/// </summary>
		[ShowIf("@this.usedInQuestBar == true")]
		[Tooltip("Number of points towards filling a quest bar when this quest is completed.")]
		[SerializeField]
		private int questBarPoints = 10;

		/// <summary>
		/// Current quest progress value as a float.
		/// </summary>
		private float progress;

		#endregion

		#region Init

		/// <inheritdoc />
		public void Init(QuestStatus status)
		{
			Status = status;
			IsCompleted = new ReactiveProperty<bool>(status == QuestStatus.Completed || status == QuestStatus.CompletedRewardCollected);
		}

		#endregion
	}
}