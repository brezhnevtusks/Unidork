using System;
using System.Collections.Generic;
using UnderdorkStudios.UnderTools.Extensions;
using Unidork.Attributes;
using Unidork.Serialization;
using UniRx;
using UnityEngine;

namespace Unidork.Resources
{
	/// <summary>
	/// Base class for resource managers.
	/// </summary>
	public class BaseResourceManager<T> : MonoBehaviour where T : Enum
	{
		#region Properties

		/// <summary>
		/// Has the resource manager been initialized?
		/// </summary>
		/// <returns>
		/// True if there is an instance of the manager with a valid resource dictionary, False otherwise.
		/// </returns>
		public static bool IsInitialized => instance != null && Resources != null;

		/// <summary>
		/// Reactive dictionary where key is the resource id and value is the owned amount.
		/// </summary>
		public static ReactiveDictionary<int, double> Resources { get; protected set; }

		/// <summary>
		/// Event that is raised when resource manager data is reset.
		/// </summary>
		public static event EventHandler OnReset; 
		
		#endregion
		
		#region Field
		
		/// <summary>
		/// Instance of this class.
		/// </summary>
		protected static BaseResourceManager<T> instance;
		
		/// <summary>
		/// Current version of save data.
		/// </summary>
		protected string saveVersion;

		/// <summary>
		/// Reference to the scriptable objects that stores configuration data about in-game resource.
		/// </summary>
		[Space, BaseHeader, Space]
		[Tooltip("Reference to the scriptable objects that stores configuration data about in-game resource.")]
		[SerializeField] 
		protected ResourceSettings<T> resourceSettings;

		/// <summary>
		/// Dictionary where keys are resource ids and values are user-friendly names.
		/// </summary>
		protected Dictionary<int, string> resourceNames = new();

		/// <summary>
		/// Is there currently any data that needs to be saved?
		/// </summary>
		private bool hasDataToSave;

		#endregion
		
		#region Constants
		
		/// <summary>
		/// Key to use when saving/loading with Easy Save.
		/// </summary>
		private const string ResourceKeyName = "Resources";
		
		/// <summary>
		/// Relative file path to use when saving/loading with Easy Save.
		/// </summary>
		private const string ResourceSavePath = "Resource.und";
		
		#endregion

		#region Init

		protected virtual void Awake()
		{
			if (instance != null)
			{
				Destroy(instance.gameObject);
				return;
			}

			instance = this;
		}

		private void Start()
		{
			if (resourceSettings == null)
			{
				Debug.LogError("ResourceManager doesn't have a reference to resource settings!", this);
				return;
			}
			
			saveVersion = BaseSerializationManager.SaveVersion;
			InitResources();
		}

		#endregion

		#region Resources
		
		/// <summary>
		/// Checks whether the amount of resource player has with specified ID is equal to or bigger that the passed amount.
		/// </summary>
		/// <param name="resourceId">Resource ID></param>
		/// <param name="amount">Amount to check against.</param>
		/// <returns>
		/// True if the player owns at least as much specified resource as the passed amount, False otherwise.
		/// </returns>
		public static bool HasEnoughResource(int resourceId, double amount)
		{
			if (Resources.TryGetValue(resourceId, out double ownedAmount))
			{
				return ownedAmount >= amount;
			}

			Debug.LogWarning($"ResourceManager doesn't have an entry for Resource with id {resourceId}!");
			return false;
		}

		/// <summary>
		/// Gets the amount of resource with specified ID that player owns.
		/// </summary>
		/// <param name="resourceId">Resource ID.</param>
		/// <returns>
		/// A double value that represents the amount of owned resource.
		/// </returns>
		public static double GetResourceAmount(int resourceId)
		{
			if (Resources.TryGetValue(resourceId, out double amount))
			{
				return amount;
			}

			Debug.LogWarning($"ResourceManager doesn't have an entry for Resource with id {resourceId}!");
			return 0d;
		}

		/// <summary>
		/// Decreases the amount of resource with passed id by the specified value.
		/// </summary>
		/// <param name="resourceId">Resource Id.</param>
		/// <param name="amount">Amount to spend.</param>
		public static void SpendResource(int resourceId, double amount)
		{
			AddResource(resourceId, -amount);
		}

		/// <summary>
		/// Increases the amount of resource with passed id by the specified value.
		/// </summary>
		/// <param name="resourceId">Resource Id.</param>
		/// <param name="amount">Amount to spend.</param>
		/// <param name="saveData"></param>
		public static void AddResource(int resourceId, double amount, bool saveData = true)
		{
			if (Resources.ContainsKey(resourceId))
			{
				Resources[resourceId] += amount;
				instance.hasDataToSave = true;
				return;
			}
			
			Debug.LogError($"ResourceManager doesn't have an entry for Resource with id {resourceId}!");
		}

		/// <summary>
		/// Creates a resource data array.
		/// </summary>
		/// <returns>
		/// An array of <see cref="ResourceData"/> objects.
		/// </returns>
		protected virtual void InitResources()
		{
			Resources = new ReactiveDictionary<int, double>();
            
			ResourceSaveData<T> resourceSaveData = BaseSerializationManager.Load<ResourceSaveData<T>>(ResourceKeyName, ResourceSavePath);

			if (resourceSaveData == null || resourceSaveData.Data.IsNullOrEmpty())
			{
				foreach (ResourceData<T> resourceData in resourceSettings.Resources)
				{
					Resources.Add(resourceData.Id, resourceData.Amount);
				}
				
				SaveData();
			}
			else
			{
				foreach (ResourceData<T> resourceData in resourceSaveData.Data)
				{
					Resources.Add(resourceData.Id, resourceData.Amount);

				}
			}
			
			resourceNames.Clear();
			
			foreach (ResourceData<T> resourceData in resourceSettings.Resources)
			{
				resourceNames.Add(resourceData.Id, resourceData.Name);
			}
		}

		#endregion

		#region Save

		/// <summary>
		/// Calls non-static SaveData() on singleton instance of the save manager.
		/// </summary>
		protected static void SaveDataStatic()
		{
			instance.SaveData();
		}

		/// <summary>
		/// Tells the serialization manager to save resource data to disk. Can be overriden by inheriting classes.
		/// </summary>
		protected virtual void SaveData()
		{
			ResourceData<T>[] resources = new ResourceData<T>[Resources.Count];

			var index = 0;
			
			foreach (KeyValuePair<int, double> kvp in Resources)
			{
				// Don't waste space saving user-friendly resource names
				resources[index] = new ResourceData<T>((T)(object)kvp.Key, "", kvp.Value);
				index++;
			}
			
			ResourceSaveData<T> resourceSaveData = new ResourceSaveData<T>(resources, saveVersion);
			BaseSerializationManager.Save(ResourceKeyName, resourceSaveData, ResourceSavePath);
		}

		private void LateUpdate()
		{
			if (hasDataToSave)
			{
				SaveData();
			}

			hasDataToSave = false;
		}

		#endregion

		#region Reset

		/// <summary>
		/// Resets the resource save data by calling <see cref="ResetDataInternal"/>.
		/// </summary>
		public static void ResetData()
		{
			if (instance != null)
			{
				instance.ResetDataInternal();
			}
		}

		/// <summary>
		/// Resets the resource save data.
		/// </summary>
		protected virtual void ResetDataInternal()
		{
			BaseSerializationManager.Save(ResourceKeyName, new ResourceSaveData<T>(null, saveVersion), ResourceSavePath);
			InitResources();
			SaveData();
			OnReset?.Invoke(this, EventArgs.Empty);
		}

		#endregion
	}
}