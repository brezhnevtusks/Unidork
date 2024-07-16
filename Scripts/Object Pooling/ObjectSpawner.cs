using System;
using Sirenix.Utilities;
using System.Collections.Generic;
using UnderdorkStudios.UnderTools.Extensions;
using Unidork.Attributes;
using Unidork.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Unidork.ObjectPooling
{
	/// <summary>
	/// Spawns objects on request. Objects are acquired from an <see cref="objectPooler"/> that this component creates on start.
	/// </summary>
	public class ObjectSpawner : MonoBehaviour
    {
        #region Properties

        /// <summary>
        /// Spawner's name.
        /// </summary>
        /// <value>
        /// Gets the value of the string field name.
        /// </value>
        public string Name => name;

        /// <summary>
        /// Can this spawner receive requests from other objects? We wait until the connected pooler creates
        /// its pool before allowing the first request.
        /// </summary>
        /// <value>
        /// Gets the value of the <see cref="ObjectPooler.IsInitialized"/> property on the connected object pooler.
        /// </value>
        public bool CanSpawnObjects => objectPooler != null && objectPooler.IsInitialized;

        #endregion
        
        #region Fields

        /// <summary>
        /// List of all spawners currently present in the game.
        /// </summary>
        private static List<ObjectSpawner> objectSpawners;

        /// <summary>
        /// Spawner's name.
        /// </summary>
        [Space, SettingsHeader, Space]
        [Tooltip("Spawner's name.")]
        [SerializeField]
        private new string name = "NewSpawner";
        
        /// <summary>
        /// Settings to use when creating the pooler that will be tied to this spawner.
        /// </summary>
        [Tooltip("Settings to use when creating the pooler that will be tied to this spawner.")]
        [SerializeField]
        private ObjectPoolerSettings poolerSettings = null;

        /// <summary>
        /// Transform that serves as a holder for spawned objects in case no other transform is provided by calling scripts.
        /// </summary>
        [Tooltip("Transform that serves as a holder for spawned objects in case no other transform is provided by calling scripts.")]
        [SerializeField]
        private Transform defaultSpawnedObjectHolder = null;

        /// <summary>
        /// Should this spawner be initialized in Start() method?
        /// </summary>
        [Tooltip("Should this spawner be initialized in Start() method?")]
        [SerializeField]
        private bool initOnStart = false;

        /// <summary>
        /// Object pooler that stores objects spawned by this spawner.
        /// </summary>
        private ObjectPooler objectPooler;

        /// <summary>
        /// List of active spawned objects.
        /// </summary>
        private readonly List<IPooledObject> spawnedObjects = new List<IPooledObject>();
        
        #endregion

        #region Start

        public virtual void Init() => objectPooler = ObjectPooler.CreatePooler(poolerSettings);

		private void Start()
		{
			if (!initOnStart)
			{
                return;
			}

            Init();
		}

		#endregion

		#region Spawn

        /// <summary>
        /// Spawns a pooled object at the specified position, rotation, and scale.
        /// </summary>
        /// <param name="assetReference">Reference to the asset of the addressable object that needs to be spawned.</param>
        /// <param name="position">Position.</param>
        /// <param name="rotation">Rotation.</param>
        /// <param name="parent">Transform to use as a parent for the spawned object.</param>
        /// <param name="autoActivate">Should the object be auto-activated upon spawning?</param>
        /// <returns>
        /// An instance of <see cref="IPooledObject"/> or null if no valid object was acquired from the connected pooler.
        /// </returns>
        public virtual IPooledObject Spawn(AssetReference assetReference, Vector3 position, Quaternion rotation,
                                   bool overrideScale = false, Vector3 scale = default, Transform parent = null, bool autoActivate = true)
        {
            objectPooler ??= ObjectPooler.FindPoolerWithName(poolerSettings.Name) ?? ObjectPooler.CreatePooler(poolerSettings);

			IPooledObject spawnedObject = objectPooler.Get(assetReference);            

            if (spawnedObject == null)
            {
                return null;
            }

            if (parent == null)
            {
                parent = defaultSpawnedObjectHolder;
            }

            spawnedObject.SetUpTransform(parent, position, rotation, overrideScale, scale);

            if (autoActivate)
            {
                spawnedObject.Activate();
            }

            spawnedObjects.Add(spawnedObject);

            return spawnedObject;
        }

        /// <summary>
        /// Deactivates a pooled object and parents it to the pooled object holder.
        /// </summary>
        /// <param name="pooledObject">Pooled object to despawn.</param>
        public void Despawn(IPooledObject pooledObject)
        {
	        pooledObject.Deactivate();
            pooledObject.SetParent(poolerSettings.PooledObjectHolder);

            _ = spawnedObjects.Remove(pooledObject);
        }

        /// <summary>
        /// Despawns all objects that are currently active.
        /// </summary>
        public void DespawnAllObjects()
		{
            foreach (IPooledObject spawnedObject in spawnedObjects)
			{
                spawnedObject.Deactivate();
                spawnedObject.SetParent(poolerSettings.PooledObjectHolder);
            }

            spawnedObjects.Clear();
		}

        #endregion

        #region Destroy

        /// <summary>
        ///  Destroys the specified spawner.
        /// </summary>
        /// <param name="spawner">Spawner to destroy.</param>
        public static void DestroySpawner(ObjectSpawner spawner)
        {
	        if (spawner == null)
	        {
		        throw new ArgumentNullException($"Trying to destroy a spawner that doesn't exist! Make sure something else hasn't destroyed it!");
	        }
	        
	        spawner.Destroy();
        }
        
        /// <summary>
        /// Destroys the spawner's game object and the connected pooler.
        /// </summary>
        public void Destroy()
		{
			ObjectPooler.DestroyPooler(objectPooler);
			
			objectSpawners.Remove(this);
			Destroy(gameObject);
		}

        #endregion

        #region Static

        /// <summary>
        /// Gets a spawner with the passed name.
        /// </summary>
        /// <param name="spawnerName">Spawner name.</param>
        /// <returns>
        /// An instance of <see cref="ObjectSpawner"/> that matches the passed name, null otherwise.
        /// </returns>
        public static ObjectSpawner GetSpawnerWithName(string spawnerName)
        {
            if (objectSpawners.IsNullOrEmpty())
            {
                UpdateSpawnerList();
            }

            var updateSpawnerList = false;

            foreach (ObjectSpawner objectSpawner in objectSpawners)
            {
                if (objectSpawner == null)
                {
                    updateSpawnerList = true;
                    break;
                }
            }

            if (updateSpawnerList)
			{
                UpdateSpawnerList();
			}

            foreach (ObjectSpawner objectSpawner in objectSpawners)
            {
                if (objectSpawner.Name == spawnerName)
                {
                    return objectSpawner;
                }
            }
                
            return null;
        }

		/// <summary>
		/// Updates the static list of all spawners.
		/// </summary>
		public static void UpdateSpawnerList()
		{
            ObjectSpawner[] objectSpawnerArray = FindObjectsByType<ObjectSpawner>(FindObjectsSortMode.None);
            objectSpawners = objectSpawnerArray.ToList(objectSpawnerArray.Length);
		}

		/// <summary>
		/// Clears the static list of all spawners.
		/// </summary>
		public static void ClearSpawnerArray()
		{
            if (objectSpawners.IsNullOrEmpty())
			{
                return;
			}

			objectSpawners.Clear();
		}

		/// <summary>
		/// Destroys spawner with the passed name.
		/// </summary>
		/// <param name="spawnerName">Spawner name.</param>
		public static void DestroySpawnerWithName(string spawnerName)
		{
            ObjectSpawner spawnerToDestroy = GetSpawnerWithName(spawnerName);

            if (spawnerToDestroy == null)
			{
                return;
			}            

            for (var i = objectSpawners.Count - 1; i >= 0; i--)
			{
                if (objectSpawners[i].Name == spawnerName)
				{
                    objectSpawners.RemoveAt(i);
                    break;
				}
			}

            spawnerToDestroy.Destroy();
        }

        /// <summary>
        /// Destroys all members of the static list of active spawners and clears the list.
        /// </summary>
        public static void DestroyAllSpawners()
		{
            if (objectSpawners.IsNullOrEmpty())
			{
                return;
			}

            for (var i = objectSpawners.Count - 1; i >= 0; i--)
			{
                objectSpawners[i].Destroy();
			}

            objectSpawners.Clear();
		}

        #endregion
    }
}