using UnityEngine;

namespace Unidork.QuestSystem
{
	/// <summary>
	/// Interface that must be implemented by objects that serve as quest rewards.
	/// </summary>
	public interface IQuestReward
	{
		/// <summary>
		/// Display name of the reward.
		/// </summary>
		string Name { get; }
		
		/// <summary>
		/// Icon that represents the reward.
		/// </summary>
		Sprite Icon { get; }
		
		/// <summary>
		/// Called when a reward is collected.
		/// </summary>
		void OnRewardCollected();
	}
}