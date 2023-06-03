using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Sirenix.Utilities;
using Unidork.Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Unidork.ObjectPooling
{
    /// <summary>
    /// Manages a pool of objects that can be used by an <see cref="ObjectSpawner/>.
    /// </summary>
    public class ObjectPooler
    {
        #region Properties

        /// <summary>
        /// Pooler's name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Has this pooler finished creating the pool?
        /// </summary>
        public bool IsInitialized { get; private set; } = false;

        #endregion
        
        #region Fields

        /// <summary>
        /// All object poolers that are currently active.
        /// </summary>
        private static List<ObjectPooler> objectPoolers;

        /// <summary>
        /// Settings for this object pooler.
        /// </summary>
        private readonly ObjectPoolerSettings settings;
        
        /// <summary>
        /// Transform that serves as a holder for pooled objects.
        /// </summary>
        private readonly Transform pooledObjectHolder;
        
        /// <summary>
        /// Dictionary where an asset guid is the key and a list of pooled objects corresponding with that asset are the value.
        /// </summary>
        private readonly Dictionary<string, List<IPooledObject>> pooledObjectLists;

        #endregion
        
        #region Constructor

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="settings">Object pooler settings.</param>
        public ObjectPooler(ObjectPoolerSettings settings)
        {
            Name = settings.Name;

            this.settings = settings;
            
            pooledObjectHolder = settings.PooledObjectHolder;
            
            pooledObjectLists = new Dictionary<string, List<IPooledObject>>();
            CreatePoolAsync(settings.GetPoolItemSettings()).Forget();
        }

        #endregion

        #region Pool

        /// <summary>
        /// Creates the object pool from the passed settings. A separate list if created for each item in the passed array
        /// and stored as a value in a dictionary.
        /// </summary>
        /// <param name="itemSettingsArray">Object pool item settings.</param>
        /// <exception cref="ArgumentException">Thrown when passed settings array is null or empty.</exception>
        private async UniTaskVoid CreatePoolAsync(ObjectPoolItemSettings[] itemSettingsArray)
        {
            await Addressables.InitializeAsync().ToUniTask();
            
            if (itemSettingsArray.IsNullOrEmpty())
            {
                throw new ArgumentException($"{Name} has received an invalid array of pooled item settings!");
            }

            if (pooledObjectHolder != null)
			{
                int childCount = pooledObjectHolder.childCount;

                if (childCount > 0)
				{
                    for (int i = childCount - 1; i >= 0; i--)
					{
                        UnityEngine.Object.Destroy(pooledObjectHolder.GetChild(i).gameObject);
                    }
				}
			}

            List<UniTask> poolTasks = new List<UniTask>();
            
            foreach (ObjectPoolItemSettings itemSettings in itemSettingsArray)
            {
                poolTasks.Add(CreatePooledObjectListAsync(itemSettings));
            }

            await UniTask.WhenAll(poolTasks);

            IsInitialized = true;
        }
        
        /// <summary>
        /// Instantiates a pool of objects from the passed item handle and settings and adds it to the pooler dictionary.
        /// </summary>
        /// <param name="itemSettings">Object pool item settings.</param>
        /// <exception cref="ArgumentException">Thrown when the pool already contains items matching the passed handle.</exception>
        private async UniTask CreatePooledObjectListAsync(ObjectPoolItemSettings itemSettings)
        {
            if (pooledObjectLists.ContainsKey(itemSettings.AssetReference.AssetGUID))
            {
                throw new ArgumentException($"{Name} already contains a pool for asset with address {itemSettings.AssetReference.AssetGUID}");
            }
            
            AsyncOperationHandle loadHandle = Addressables.LoadAssetAsync<GameObject>(itemSettings.AssetReference);

            await loadHandle.ToUniTask();
            
            if (loadHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"{Name} failed to load asset with GUID {itemSettings.AssetReference.AssetGUID}");
                return;
            }

            var pooledObjectList = new List<IPooledObject>();
            
            for (var i = 1; i <= itemSettings.NumberToPool; i++)
            {
                var pooledObject = (IPooledObject)Addressables.InstantiateAsync(itemSettings.AssetReference).Result.GetComponentInChildrenNonAlloc(typeof(IPooledObject));
                pooledObject.SetParent(pooledObjectHolder);
                pooledObject.Deactivate(deactivateOnStart: true);
                pooledObjectList.Add(pooledObject);
            }

            pooledObjectLists.Add(itemSettings.AssetReference.AssetGUID, pooledObjectList);
            
            if (itemSettings.PoolCanExpand)
			{
                return;
			}

            Addressables.Release(loadHandle);
        }

        /// <summary>
        /// Destroys all objects in the pool. ONLY USE when the pool is no longer needed.
        /// </summary>
        private void Destroy()
        {
            foreach (KeyValuePair<string, List<IPooledObject>> kvp in pooledObjectLists)
            {
                List<IPooledObject> pooledObjects = kvp.Value;

                while (pooledObjects.Count > 0)
                {
                    pooledObjects[0].Destroy();
                    pooledObjects.RemoveAt(0);
                }
            }
            
            pooledObjectLists.Clear();
        }

        #endregion

        #region Items

        /// <summary>
        /// Gets an item from the pool that matches the passed asset reference.
        /// </summary>
        /// <param name="assetReference">Addressable asset reference.</param>
        /// <returns>
        /// The result of calling <see cref="Get(string)"/>.
        /// </returns>
        public IPooledObject Get(AssetReference assetReference)
		{
            return Get(assetReference.AssetGUID);
		}

        /// <summary>
        /// Gets an item from the pool that matches the passed asset address.
        /// </summary>
        /// <param name="assetAddress">Asset address.</param>
        /// <returns>
        /// An <see cref="IPooledObject"/> or null if there are no available objects with 
        /// that address and the pool isn't allowed to expand.
        /// </returns>
        /// <exception cref="ArgumentException">Thrown when the pool dictionary doesn't 
        /// contain any items matching the passed asset address.</exception>
        public IPooledObject Get(string assetAddress)
        {
            if (!pooledObjectLists.TryGetValue(assetAddress, out List<IPooledObject> pooledObjects))
            {
                throw new ArgumentException($"{Name} doesn't contain items that match asset with gui {assetAddress}");
            }

            foreach (IPooledObject pooledObject in pooledObjects)
            {
                if (!pooledObject.IsActive)
                {
                    return pooledObject;
                }
            }

            foreach (ObjectPoolItemSettings objectPoolItemSettings in settings.GetPoolItemSettings())
			{
                if (string.Equals(objectPoolItemSettings.AssetReference.AssetGUID, assetAddress))
				{
                    if (!objectPoolItemSettings.PoolCanExpand)
					{
                        return null;
					}
                    else
					{
                        var pooledObject = Addressables.InstantiateAsync(assetAddress, pooledObjectHolder).Result.GetComponentInChildren<IPooledObject>();
                        pooledObject.Deactivate(deactivateOnStart: true);

                        pooledObjectLists[assetAddress].Add(pooledObject);

                        return pooledObject;
                    }
				}
			}

            return null;            
        }

        #endregion
        
        #region Static

        /// <summary>
        /// Create a new object pooler with provided settings and adds it to the list of currently active poolers.
        /// </summary>
        /// <param name="settings">Pooler settings.</param>
        /// <returns>
        /// The newly created <see cref="ObjectPooler"/>.
        /// </returns>
        public static ObjectPooler CreatePooler(ObjectPoolerSettings settings)
        {
            objectPoolers ??= new List<ObjectPooler>();

            var newPooler = new ObjectPooler(settings);
            
            objectPoolers.Add(newPooler);
            
            return newPooler;
        }

        /// <summary>
        /// Destroys the passed pooler.
        /// </summary>
        /// <param name="objectPooler">Pooler to destroy.</param>
        /// <exception cref="ArgumentNullException">Thrown when puller is null.</exception>
        public static void DestroyPooler(ObjectPooler objectPooler)
        {
            if (objectPooler == null)
            {
                throw new ArgumentNullException($"Trying to destroy a pooler that doesn't exist! Make sure something else hasn't destroyed it!");
            }
            
            objectPoolers.Remove(objectPooler);
            objectPooler.Destroy();
        }

        /// <summary>
        /// Locates a pooler with the passed name.
        /// </summary>
        /// <param name="poolerName">Pooler name.</param>
        /// <returns>
        /// An instance of <see cref="ObjectPooler"/> if a pooler with queried name exists, null otherwise.
        /// </returns>
        public static ObjectPooler FindPoolerWithName(string poolerName)
        {
            foreach (ObjectPooler pooler in objectPoolers)
            {
                if (pooler.Name.Equals(poolerName))
                {
                    return pooler;
                }
            }

            Debug.LogError($"Failed to find pooler with name {poolerName}");
            return null;
        } 

        #endregion
    }
}