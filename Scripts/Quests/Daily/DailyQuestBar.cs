using UniRx;
using UnityEngine;

namespace Unidork.QuestSystem
{
	/// <summary>
	/// Stores data and performs operations with
	/// </summary>
	public class DailyQuestBar : MonoBehaviour, IQuestBar
	{
		#region Properties

		/// <inheritdoc />
		public string Name { get; }

		/// <inheritdoc />
		public bool IsInitialized { get; private set; }

		public ReactiveProperty<int> CurrentPoints { get; }
		
		public ReactiveCollection<QuestBarDivision> DivisionsWithCollectedRewards { get; }
		
		public ReactiveCollection<QuestBarDivision> CompletedDivisions { get; }
		
		/// <inheritdoc />
		public ReactiveProperty<bool> HasReachedMaxFill { get; }

		
		
		#endregion

		#region Fields

		/// <summary>
		/// Name of this quest bar. Should be unique so that quest bars can be located with the <see cref="QuestBarLocator"/>.
		/// </summary>
		[Tooltip("Name of this quest bar. Should be unique so that quest bars can be located with the QuestBarLocator.")]
		[SerializeField]
		private new string name = "QuestBar";
		
		/// <summary>
		/// Divisions on the quest bar.
		/// </summary>
		[Tooltip("Divisions on the quest bar.")]
		[SerializeField]
		private QuestBarDivision[] divisions = null;

		#endregion

		#region Init

		protected virtual void Start()
		{
			IsInitialized = true;
		}

		#endregion
		
		/// <inheritdoc />
        public QuestBarDivision[] GetDivisions() => (QuestBarDivision[])divisions.Clone();

		public void OnCompletedDivisionShown(QuestBarDivision division)
		{
			throw new System.NotImplementedException();
		}
	}
}