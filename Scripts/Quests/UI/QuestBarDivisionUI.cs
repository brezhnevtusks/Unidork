using System;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using TMPro;
using Unidork.Attributes;
using UnityEngine;
using UnityEngine.UI;

namespace Unidork.QuestSystem
{
	/// <summary>
	/// Handles the visual representation of a <see cref="QuestBarDivision"/>.
	/// </summary>
	public class QuestBarDivisionUI : MonoBehaviour
	{
		#region Fields

		/// <summary>
		/// Does the UI division have an image that marks the amount of fill player has to reach to complete the division.
		/// </summary>
		[Space, ComponentsHeader, Space]
		[Tooltip("Does the UI division have an image that marks the amount of fill player has to reach to complete the division.")]
		[SerializeField]
		private bool hasGoalImage = true;
		
		/// <summary>
		/// Image that marks the amount of fill player has to reach to complete this division.
		/// </summary>
		[ShowIf("@this.hasGoalImage == true")]
		[Tooltip("Image that marks the amount of fill player has to reach to complete this division.")]
		[SerializeField]
		private Image goalImage = null;

		/// <summary>
		/// Image that marks the amount of fill player has to reach to complete this division. Used after a division
		/// is completed and can be different from <see cref="goalImage"/>.
		/// </summary>
		[Tooltip("Image that marks the amount of fill player has to reach to complete this division. Used after a division" +
		         "is completed and can be different from Goal Image.")]
		[SerializeField]
		private Image goalReachedImage = null;

		/// <summary>
		/// Does the UI division have a GUI component that shows the progress player has to reach to complete the division or
		/// the index of the division?
		/// </summary>
		[Tooltip("Does the UI division have a GUI component that shows the progress player has to reach to complete the division or" +
		         "the index of the division?")]
		[SerializeField]
		private bool hasGoalNumber = true;
		
		/// <summary>
		/// GUI component that shows the progress player has to reach to complete the division or the index of the division?
		/// </summary>
		[ShowIf("@this.hasGoalNumber == true")]
		[Tooltip("GUI component that shows the progress player has to reach to complete the division or the index of the division?")]
		[SerializeField]
		private TextMeshProUGUI goalNumber = null;

		#endregion

		#region Setup
		
		/// <summary>
		/// Sets up the UI with the data from the passed <see cref="QuestBarDivision"/>.
		/// </summary>
		/// <param name="questBarDivision">Quest bar division.</param>
		public void Setup(QuestBarDivision questBarDivision)
		{
			QuestBarDivisionReward[] rewards = questBarDivision.Rewards;

			if (questBarDivision.IsCompleted.Value)
			{
				return;
			}
			
			
		}

		#endregion

		#region Completion

		/// <summary>
		/// Plays the completion sequence asynchronously. Should be overriden in child classes.
		/// </summary>
		protected virtual async UniTask PlayCompletionSequence()
		{
		} 

		#endregion
	}
}