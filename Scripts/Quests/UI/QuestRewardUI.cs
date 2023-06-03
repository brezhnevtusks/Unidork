using TMPro;
using Unidork.Attributes;
using Unidork.Math;
using UnityEngine;
using UnityEngine.UI;

namespace Unidork.QuestSystem
{
	/// <summary>
	/// Handles updating the visual representations of a quest reward: its icon, number of received rewards, etc.
	/// </summary>
	public class QuestRewardUI : MonoBehaviour
	{
		#region Fields

		/// <summary>
		/// GUI component that displays the reward icon.
		/// </summary>
		[Space, ComponentsHeader, Space]
		[Tooltip("GUI component that displays the reward icon.")]
		[SerializeField]
		private Image icon = null;

		/// <summary>
		/// GUI component that displays the number of received rewards.
		/// </summary>
		[Tooltip("GUI component that displays the number of received rewards.")]
		[SerializeField]
		private TextMeshProUGUI countText = null;

		/// <summary>
		/// Should the "X" multiplier sign be added to the number of received rewards?
		/// </summary>
		[Space, SettingsHeader, Space] 
		[Tooltip("Should the X multiplier sign be added to the number of received rewards?")]
		[SerializeField]
		private bool addMultiplierSign = true;

		#endregion

		#region Setup

		/// <summary>
		/// Sets up the reward UI.
		/// </summary>
		/// <param name="rewardSprite">Reward sprite.</param>
		/// <param name="rewardCount">Number of rewards to give.</param>
		public void Setup(Sprite rewardSprite, int rewardCount)
		{
			icon.sprite = rewardSprite;
			countText.text = CreateRewardCountString(rewardCount);
		}

		/// <summary>
		/// Creates a string that represents the reward count.
		/// </summary>
		/// <param name="rewardCount">Number of rewards.</param>
		/// <returns>
		/// A string formatted according to settings.
		/// </returns>
		protected virtual string CreateRewardCountString(int rewardCount)
		{
			string rewardCountString = MathUtils.GetFormattedNumberWithUnitPrefix(rewardCount);

			if (addMultiplierSign)
			{
				rewardCountString = $"x{rewardCountString}";
			}

			return rewardCountString;
		}

		#endregion
	}
}