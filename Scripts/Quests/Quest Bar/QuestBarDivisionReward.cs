using Sirenix.OdinInspector;
using UnityEngine;

namespace Unidork.QuestSystem
{
	/// <summary>
	/// Stores data about the reward given for filling a <see cref="QuestBarDivision"/>.
	/// </summary>
	[System.Serializable]
	public class QuestBarDivisionReward
	{
		#region Properties

		/// <summary>
		/// Object that implement the <see cref="IQuestReward"/> interface.
		/// </summary>
		/// <value>
		/// Gets the component on <see cref="@object"/> that implements the <see cref="IQuestReward"/> interface.
		/// </value>
		public IQuestReward RewardObject
		{
			get
			{
				if (@object != null)
				{
					return @object.GetComponent<IQuestReward>();
				}
				
				Debug.LogError($"Quest reward is null!");
				return null;
			}
		}

		/// <summary>
		/// Number of reward objects to give.
		/// </summary>
		/// <value>
		/// Gets the value of the integer field count.
		/// </value>
		public int Count => count;

		#endregion

		#region Fields

		/// <summary>
		/// Reward object. MUST have a component that implement the IQuestReward interface!
		/// </summary>
		[OnValueChanged("ValidateObject")]
		[Tooltip("Reward object. MUST have a component that implement the IQuestReward interface!")]
		[SerializeField]
		private GameObject @object = null;

		/// <summary>
		/// Number of reward objects to give.
		/// </summary>
		[Tooltip("Number of reward objects to give.")]
		[SerializeField]
		private int count = 1;

		#endregion

#if UNITY_EDITOR
		
		#region Editor

		private void ValidateObject()
		{
			if (@object == null)
			{
				return;
			}

			if (@object.GetComponent<IQuestReward>() != null)
			{
				return;
			}
			
			Debug.LogError($"{@object.name} doesn't implement the IQuestReward interface!");
			@object = null;
		}		

		#endregion
		
#endif
	}    
}