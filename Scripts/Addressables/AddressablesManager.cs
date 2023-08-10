using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace Unidork.AddressableAssetsUtility
{
    /// <summary>
    /// Manager that handles loading/unloading and storing operation handles for Addressables.
    /// </summary>
    public static class AddressablesManager
    {
        #region Init

        /// <summary>
        /// Constructor.
        /// </summary>
        static AddressablesManager()
        {
            loadedAssetDictionary = new Dictionary<IResourceLocation, AsyncOperationHandle>();
        }

        /// <summary>
        /// Initializes the Addressables Manager.
        /// </summary>
        public static void Initialize()
        {
            InitializeAsync().Forget();
        }
        
        /// <summary>
        /// Initializes the Addressables Manager asynchronously. Manager is considered initializes after Addressables initialization.
        /// </summary>
        private static async UniTaskVoid InitializeAsync()
        {
            IsInitialized.Value = false;

            await Addressables.InitializeAsync().ToUniTask();
            
            IsInitialized.Value = true;
        }

        #endregion

        #region Properties

        public static ReactiveProperty<bool> IsInitialized { get; } = new(false);

        #endregion

        #region Fields

        /// <summary>
        /// Dictionary storing loaded assets where key is an Addressable resource location and the value is the async operation handle.
        /// </summary>
        private static readonly Dictionary<IResourceLocation, AsyncOperationHandle> loadedAssetDictionary;

        #endregion

        #region Validation

        /// <summary>
        /// Checks whether a passed asset address is not null and not empty.
        /// </summary>
        /// <param name="assetAddress">Asset address.</param>
        /// <returns>
        /// True if the asset address is valid, False otherwise.
        /// </returns>
        private static bool AssetAddressIsValid(string assetAddress)
        {
            return !string.IsNullOrEmpty(assetAddress);
        }

        /// <summary>
        /// Checks whether a passed asset reference is not null and has a valid runtime key.
        /// </summary>
        /// <param name="assetReference">Asset reference.</param>
        /// <returns>
        /// True if the asset reference is valid, False otherwise.
        /// </returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static bool AssetReferenceIsValid(AssetReference assetReference)
        {
            if (assetReference == null)
            {
                throw new ArgumentNullException(nameof(assetReference));
            }

            return assetReference.RuntimeKeyIsValid();
        }

        #endregion

        #region Get
        
        /// <summary>
        /// Tries to get an asset load result that matches the passed asset address.
        /// </summary>
        /// <param name="assetAddress">Asset address.</param>
        /// <param name="assetLoadResult">Asset load result.</param>
        /// /// <typeparam name="T">Asset type.</typeparam>
        /// <returns>
        /// True if <see cref="loadedAssetDictionary"/> has an asset load result matching the asset address, False otherwise.
        /// </returns>
        public static bool TryGetAssetLoadResult<T>(string assetAddress, out AddressableLoadOperationResult<T> assetLoadResult)
        {
            if (AssetAddressIsValid(assetAddress))
            {
                return TryGetAssetLoadResult(AddressablesUtility.GetResourceLocationFromAssetAddress(assetAddress), out assetLoadResult);
            }

            Debug.LogError("Trying to load an invalid asset address!");
            assetLoadResult = AddressableLoadOperationResult<T>.Failed();
            return false;
        }

        /// <summary>
        /// Tries to get an asset load result that matches the passed asset reference.
        /// </summary>
        /// <param name="assetReference">Asset reference.</param>
        /// <param name="assetLoadResult">Asset load result.</param>
        ///  <typeparam name="T">Asset type.</typeparam>
        /// <returns>
        /// True if <see cref="loadedAssetDictionary"/> has an asset load result matching the asset reference, False otherwise.
        /// </returns>
        public static bool TryGetAssetLoadResult<T>(AssetReference assetReference, out AddressableLoadOperationResult<T> assetLoadResult)
        {
            if (AssetReferenceIsValid(assetReference))
            {
                return TryGetAssetLoadResult(AddressablesUtility.GetResourceLocationFromAssetReference(assetReference), out assetLoadResult);    
            }

            Debug.Log("Trying to load an invalid asset reference!");
            assetLoadResult = AddressableLoadOperationResult<T>.Failed();
            return false;
        }
        
        /// <summary>
        /// Tries to get an asset load result that matches the passed resource location.
        /// </summary>
        /// <param name="resourceLocation">Resource location.</param>
        /// <param name="assetLoadResult">Asset load result.</param>
        /// <typeparam name="T">Asset type.</typeparam>
        /// <returns>
        /// True if <see cref="loadedAssetDictionary"/> has an asset load result the resource location0, False otherwise.
        /// </returns>
        private static bool TryGetAssetLoadResult<T>(IResourceLocation resourceLocation, out AddressableLoadOperationResult<T> assetLoadResult)
        {
            if (loadedAssetDictionary.TryGetValue(resourceLocation, out AsyncOperationHandle loadedAssetHandle))
            {
                assetLoadResult = new AddressableLoadOperationResult<T>
                {
                    Succeeded = true,
                    Key = resourceLocation,
                    Value = (T)loadedAssetHandle.Result
                };

                return true;
            }

            assetLoadResult = AddressableLoadOperationResult<T>.Failed();
            return false;
        }

        #endregion

        #region Load

        /// <summary>
        /// Loads an asset by address using an asynchronous operation.
        /// </summary>
        /// <param name="assetAddress">Asset address.</param>
        /// <param name="storeLoadedHandle">Should the handle of the loaded asset be stored in a dictionary for faster future access?</param>
        /// <typeparam name="T">Asset type.</typeparam>
        /// <returns>
        /// A handle for the load operation.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public static AsyncOperationHandle<T> LoadAssetByAddressAsync<T>(string assetAddress, bool storeLoadedHandle = true)
        {
            if (!AssetAddressIsValid(assetAddress))
            {
                throw new ArgumentException("Passed asset address is not valid!");
            }
            
            IResourceLocation resourceLocation = AddressablesUtility.GetResourceLocationFromAssetAddress(assetAddress);
                
            if (loadedAssetDictionary.TryGetValue(resourceLocation, out AsyncOperationHandle loadAssetHandle))
            {
                // If we already have a valid handle, it will not fire its complete callback, so the user can't rely on it for their logic.
                // We assign a new handle as a workaround. Ideally, user should call TryGetLoadResult() before calling this method,
                // in which case we won't end up in this branch.
                AsyncOperationHandle<T> newLoadHandle = Addressables.LoadAssetAsync<T>(assetAddress);
                loadedAssetDictionary[resourceLocation] = loadAssetHandle;

                return newLoadHandle;
            }
                
            AsyncOperationHandle<T> loadHandle = Addressables.LoadAssetAsync<T>(assetAddress);

            if (storeLoadedHandle)
            {
                AddLoadHandleToDictionary(resourceLocation, loadHandle);    
            }

            return loadHandle;
        }

        /// <summary>
        /// Loads an asset by asset reference using an asynchronous operation.
        /// </summary>
        /// <param name="assetReference">Asset reference.</param>
        /// <param name="storeLoadedHandle">Should the handle of the loaded asset be stored in a dictionary for faster future access?</param>
        /// <typeparam name="T">Asset type.</typeparam>
        /// <returns>
        /// A handle for the load operation.
        /// </returns>
        /// <exception cref="ArgumentException"></exception>
        public static AsyncOperationHandle<T> LoadAssetByReferenceAsync<T>(AssetReference assetReference, bool storeLoadedHandle = true)
        {
            if (!AssetReferenceIsValid(assetReference))
            {
                throw new ArgumentException("Passed asset reference is not valid!");   
            }
                
            IResourceLocation resourceLocation = AddressablesUtility.GetResourceLocationFromAssetReference(assetReference);
                
            if (loadedAssetDictionary.TryGetValue(resourceLocation, out AsyncOperationHandle loadAssetHandle))
            {
                // If we already have a valid handle, it will not fire its complete callback, so the user can't rely on it for their logic.
                // We assign a new handle as a workaround. Ideally, user should call TryGetLoadResult() before calling this method,
                // in which case we won't end up in this branch.
                AsyncOperationHandle<T> newLoadHandle = Addressables.LoadAssetAsync<T>(assetReference);
                loadedAssetDictionary[resourceLocation] = loadAssetHandle;

                return newLoadHandle;
            }

            AsyncOperationHandle<T> loadHandle = Addressables.LoadAssetAsync<T>(assetReference);
            
            if (storeLoadedHandle)
            {
                AddLoadHandleToDictionary(resourceLocation, loadHandle);    
            }
            
            return loadHandle;
        }

        private static void AddLoadHandleToDictionary<T>(IResourceLocation resourceLocation, AsyncOperationHandle<T> loadHandle)
        {
            loadHandle.Completed += _ =>
            {
                if (loadHandle.Status != AsyncOperationStatus.Succeeded)
                {
                    return;
                }

                loadedAssetDictionary.TryAdd(resourceLocation, loadHandle);
            };
        }

        /// <summary>
        /// Loads an asset by address asynchronously using UniTask.
        /// </summary>
        /// <param name="assetAddress">Asset address.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="storeLoadedHandle">Should the handle of the loaded asset be stored in a dictionary for faster future access?</param>
        /// <typeparam name="T">Asset type.</typeparam>
        /// <returns>
        /// A UniTask storing an <see cref="AddressableLoadOperationResult{T}"/>. 
        /// </returns>
        public static async UniTask<AddressableLoadOperationResult<T>> LoadAssetByAddressAsyncWithTask<T>(string assetAddress, bool storeLoadedHandle = true,
                                                                                                         CancellationToken cancellationToken = default)
        {
            if (!AssetAddressIsValid(assetAddress))
            {
                Debug.LogError("Trying to load an invalid asset address!");
                return AddressableLoadOperationResult<T>.Failed();
            }
            
            if (TryGetAssetLoadResult(assetAddress, out AddressableLoadOperationResult<T> loadResult))
            {
                return loadResult;
            }

            AsyncOperationHandle<T> assetLoadHandle = Addressables.LoadAssetAsync<T>(assetAddress);

            await assetLoadHandle.ToUniTask(cancellationToken: cancellationToken);

            if (cancellationToken.CanBeCanceled && cancellationToken.IsCancellationRequested)
            {
                Addressables.Release(assetLoadHandle);
                return AddressableLoadOperationResult<T>.Failed();
            }

            if (assetLoadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                var result = new AddressableLoadOperationResult<T> 
                {
                    Succeeded = true,
                    Key = assetAddress,
                    Value = assetLoadHandle.Result
                };

                if (!storeLoadedHandle)
                {
                    Addressables.Release(assetLoadHandle);
                }
                else
                {
                    IResourceLocation resourceLocation = AddressablesUtility.GetResourceLocationFromAssetAddress(assetAddress);

                    loadedAssetDictionary.TryAdd(resourceLocation, assetLoadHandle);
                }
                
                return result;
            }
            
            Debug.LogError($"Failed to load asset at address: {assetAddress}!");
            Addressables.Release(assetLoadHandle);
            return AddressableLoadOperationResult<T>.Failed();
        }
        
        /// <summary>
        /// Loads an asset by reference asynchronously using UniTask.
        /// </summary>
        /// <param name="assetReference">Asset reference.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <param name="storeLoadedHandle">Should the handle of the loaded asset be stored in a dictionary for faster future access?</param>
        /// <typeparam name="T">Asset type.</typeparam>
        /// <returns>
        /// A UniTask storing an <see cref="AddressableLoadOperationResult{T}"/>. 
        /// </returns>
        public static async UniTask<AddressableLoadOperationResult<T>> LoadAssetByReferenceAsyncWithTask<T>(AssetReference assetReference,
                                                                                                          bool storeLoadedHandle = true,
                                                                                                          CancellationToken cancellationToken = default)
        {
            if (!AssetReferenceIsValid(assetReference))
            {
                Debug.LogError("Trying to load an invalid asset reference!");
                return AddressableLoadOperationResult<T>.Failed();
            }
            
            if (TryGetAssetLoadResult(assetReference, out AddressableLoadOperationResult<T> loadResult))
            {
                return loadResult;
            }

            AsyncOperationHandle<T> assetLoadHandle = Addressables.LoadAssetAsync<T>(assetReference);

            await assetLoadHandle.ToUniTask(cancellationToken: cancellationToken);

            if (cancellationToken.CanBeCanceled && cancellationToken.IsCancellationRequested)
            {
                Addressables.Release(assetLoadHandle);
                return AddressableLoadOperationResult<T>.Failed();
            }

            if (assetLoadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                var result = new AddressableLoadOperationResult<T>
                {
                    Succeeded = true,
                    Key = assetReference,
                    Value = assetLoadHandle.Result
                };

                if (!storeLoadedHandle)
                {
                    Addressables.Release(assetLoadHandle);
                }
                else
                {
                    IResourceLocation resourceLocation = AddressablesUtility.GetResourceLocationFromAssetReference(assetReference);

                    loadedAssetDictionary.TryAdd(resourceLocation, assetLoadHandle);
                }
                
                return result;
            }
            
            Debug.LogError($"Failed to load asset reference: {assetReference.AssetGUID}!");
            Addressables.Release(assetLoadHandle);
            return AddressableLoadOperationResult<T>.Failed();
        }

        #endregion

        #region Unload

        /// <summary>
        /// Releases all asset load handles currently stored in <see cref="loadedAssetDictionary"/>.
        /// </summary>
        public static void UnloadAllAssets()
        {
            foreach (KeyValuePair<IResourceLocation, AsyncOperationHandle> kvp in loadedAssetDictionary)
            {
                AsyncOperationHandle handle = kvp.Value;

                if (!handle.IsValid())
                {
                    continue;
                }
                
                Addressables.Release(handle);
            }
            
            loadedAssetDictionary.Clear();
        }
        
        /// <summary>
        /// Releases a loaded Addressable asset by asset address.
        /// </summary>
        /// <param name="assetAddress">Asset address.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void UnloadAddressableByAddress(string assetAddress)
        {
            if (!AssetAddressIsValid(assetAddress))
            {
                throw new ArgumentNullException(nameof(assetAddress));
            }

            IResourceLocation resourceLocation = AddressablesUtility.GetResourceLocationFromAssetAddress(assetAddress);
            
            RemoveDictionaryKvp(resourceLocation);
        }
        
        /// <summary>
        /// Releases a loaded Addressable asset by asset reference.
        /// </summary>
        /// <param name="assetReference">Asset reference.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void UnloadAddressableByReference(AssetReference assetReference)
        {
            if (!AssetReferenceIsValid(assetReference))
            {
                throw new ArgumentNullException(nameof(assetReference));
            }

            IResourceLocation resourceLocation = AddressablesUtility.GetResourceLocationFromAssetReference(assetReference);
            
            RemoveDictionaryKvp(resourceLocation);
        }

        /// <summary>
        /// Removes a key-value pair from loaded dictionary that matches the passed key.
        /// </summary>
        /// <param name="key">Dictionary key to remove.</param>
        private static void RemoveDictionaryKvp(IResourceLocation key)
        {
            if (!loadedAssetDictionary.TryGetValue(key, out AsyncOperationHandle loadedAssetHandle))
            {
                return;
            }

            if (loadedAssetHandle.IsValid())
            {
                Addressables.Release(loadedAssetHandle);    
            }

            loadedAssetDictionary.Remove(key);
        }

        #endregion
    }
}