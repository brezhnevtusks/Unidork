using Cysharp.Threading.Tasks;
using Unidork.Attributes;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Unidork.QuestSystem
{
	/// <summary>
	/// Handles updating GUI that shows player's progress with a specific <see cref="IQuestBar"/>:
	/// daily quests, seasons, etc. 
	/// </summary>
	public abstract class QuestBarUI : MonoBehaviour
	{
		#region Fields

		/// <summary>
		/// Name of the connected quest bar.
		/// </summary>
		[Space, SettingsHeader, Space]
		[Tooltip("Name of the quest bar that is connected to this UI.")]
		[SerializeField]
		private string questBarName = "QuestBar";

		/// <summary>
		/// GUI component that displays the quest bar.
		/// </summary>
		[Space, ComponentsHeader, Space]
		[Tooltip("GUI component that displays the quest bar.")]
		[SerializeField]
		private Image barImage = null;
		
		/// <summary>
		/// Connected quest bar.
		/// </summary>
		private IQuestBar questBar;

		/// <summary>
		/// Object storing UniRx subscriptions associated with this component.
		/// </summary>
		private readonly CompositeDisposable subscriptions = new CompositeDisposable();

		/// <summary>
		/// Has this UI been initialized?
		/// </summary>
		protected bool isInitialized;
		
		#endregion

		#region Init

		private void Start()
		{
			questBar = QuestBarLocator.GetQuestBar(questBarName);

			if (questBar == null)
			{
				Debug.LogError($"Quest bar with name {questBarName} doesn't exist!");
				return;
			}
			
			InitAsync().Forget();
		}

		/// <summary>
		/// Initializes the quest bar UI asynchronously.
		/// </summary>
		protected virtual async UniTaskVoid InitAsync()
		{
			isInitialized = true;
		}

		#endregion

		#region Show

		

		#endregion
	}    
}