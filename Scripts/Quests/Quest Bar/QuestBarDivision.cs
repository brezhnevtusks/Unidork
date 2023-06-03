using System;
using UniRx;
using UnityEngine;

namespace Unidork.QuestSystem
{
	/// <summary>
	/// Stores data about a single division on a <see cref="IQuestBar"/>: number of points required
	/// to fully fill the division and the rewards acquired for doing so. 
	/// </summary>
	[Serializable]
	public class QuestBarDivision
	{
		#region Properties

		/// <summary>
		/// Index of this division on the progress bar.
		/// </summary>
		public int DivisionIndex { get; private set; }
		
		/// <summary>
		/// Value of the fill on the quest bar that represents max fill (completion) for this division in [0..1] range.
		/// </summary>
		public float DivisionFillNormalized { get; private set; }

		/// <summary>
		/// Has this division been completed?
		/// </summary>
		public ReactiveProperty<bool> IsCompleted { get; } = new ReactiveProperty<bool>(false);

		/// <summary>
		/// Number of points required to fill this division.
		/// </summary>
		/// <value>
		/// Gets the value of the integer field pointsRequired.
		/// </value>
		public int PointsRequired => pointsRequired;

		/// <summary>
		/// Rewards received for fully filling the division.
		/// </summary>
		/// <value>
		/// Gets the clone of the array stored in rewards or an empty array if rewards are null.
		/// </value>
		public QuestBarDivisionReward[] Rewards
		{
			get
			{
				if (rewards != null)
				{
					return (QuestBarDivisionReward[])rewards.Clone();
				}
				
				Debug.LogError($"Division has no rewards!");
				return Array.Empty<QuestBarDivisionReward>();
			}
		}

		#endregion

		#region Fields

		/// <summary>
		/// Number of points required to fill this division.
		/// </summary>
		[Tooltip("Number of points required to fill this division.")]
		[SerializeField]
		private int pointsRequired = 1;
		
		/// <summary>
		/// Rewards received for fully filling the division.
		/// </summary>
		[UnityEngine.Tooltip("Rewards received for fully filling the division.")]
		[SerializeField]
		private QuestBarDivisionReward[] rewards = null;

		#endregion

		#region Setup

		/// <summary>
		/// Sets up the division.
		/// </summary>
		/// <param name="divisionIndex">Index of this division on the progress bar.</param>
		/// <param name="divisionFillNormalized">Value of the fill on the quest bar that represents max fill (completion) for this division in [0..1] range.</param>
		public void Setup(int divisionIndex, float divisionFillNormalized)
		{
			DivisionIndex = divisionIndex;
			DivisionFillNormalized = divisionFillNormalized;
		}

		#endregion

		#region State

		/// <summary>
		/// Called when player fully fills a division.
		/// </summary>
		public void OnDivisionCompleted()
		{
			IsCompleted.Value = true;
		}

		#endregion
	}    
}