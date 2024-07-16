using Unidork.Attributes;
using Unidork.Events;
using Unidork.Serialization;
using UnityEngine;

namespace Unidork.Progress
{
	/// <summary>
	/// Base class for progress managers.
	/// </summary>
	public abstract class BaseProgressManager : MonoBehaviour
	{
		#region Properties

		/// <summary>
		/// Object that stores settings save data.
		/// </summary>
		public SettingsSaveData SettingsSaveData { get; private set; }

		/// <summary>
		/// Zero-based index of current level.
		/// </summary>
		public abstract int CurrentLevelIndex { get; }

		#endregion

		#region Fields

		/// <summary>
		/// Relative path for the progress save data file.
		/// </summary>
		[Space, BaseHeader, Space]
		[Tooltip("Relative path for the progress save data file.")]
		[SerializeField]
		protected string playerProgressSaveDataRelativePath = "/PlayerProgressSaveData.json";

		/// <summary>
		/// Event that is raised when player progress data is loaded.
		/// </summary>
		[Tooltip("Event that is raised when player progress data is loaded.")]
		[SerializeField]
		protected GameEvent onProgressSaveDataLoaded = null;

		/// <summary>
		/// Entire path for the progress save data file.
		/// </summary>
		protected string progressSaveDataPath;

		/// <summary>
		/// Component that is responsible for serializing and deserializing save data.
		/// </summary>
		protected BaseSerializationManager serializationManager;		

		#endregion

		#region Init

		protected virtual void Start()
		{
			serializationManager = FindAnyObjectByType<BaseSerializationManager>();
			LoadSaveData();
		}

		/// <summary>
		/// Loads the save data on game start. Creates save data objects if they don't exist.
		/// </summary>
		protected virtual void LoadSaveData()
		{	
		}

		#endregion
	}
}