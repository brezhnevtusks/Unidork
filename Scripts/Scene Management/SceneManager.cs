using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Unidork.SceneManagement
{
	/// <summary>
	/// Loads and unloads game scenes by using the Addressables system.
	/// </summary>
	public static class SceneManager
    {
        #region Load/Unload

        /// <summary>
        /// Loads a scene asynchronously from the passed Addressables asset reference.
        /// </summary>
        /// <param name="sceneToLoad">Asset reference to the scene object.</param>
        /// <param name="loadSceneMode">Scene load mode.</param>
        /// <param name="activateOnLoad">Should the scene be activated on load?</param>
        /// <param name="setAsActiveScene">Should the scene be set as current active scene on load?</param>
        /// <param name="sceneLoadCallback">Optional callback to invoke when the scene is loaded.</param>
        /// <returns>
        /// Async operation handle for the scene load.
        /// </returns>
        public static AsyncOperationHandle LoadSceneAsync(AssetReference sceneToLoad, LoadSceneMode loadSceneMode, bool activateOnLoad, bool setAsActiveScene,
                                                     Action sceneLoadCallback = null)
        {
            AsyncOperationHandle sceneLoadHandle = Addressables.LoadSceneAsync(sceneToLoad, loadSceneMode, activateOnLoad);

            sceneLoadHandle.Completed += _ =>
            {
                if (sceneLoadHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    sceneLoadCallback?.Invoke();

                    if (!setAsActiveScene)
                    {
                        return;
                    }

                    var sceneInstance = (SceneInstance)sceneLoadHandle.Result;
                    UnityEngine.SceneManagement.SceneManager.SetActiveScene(sceneInstance.Scene);
                }
                else
                {
                    Debug.LogError($"Failed to load scene {sceneToLoad.Asset.name}");
                }
            };

            return sceneLoadHandle;
        }
        
        /// <summary>
        /// Loads a scene asynchronously from the passed scene asset address.
        /// </summary>
        /// <param name="sceneAssetAddress">Address of the scene asset.</param>
        /// <param name="loadSceneMode">Scene load mode.</param>
        /// <param name="activateOnLoad">Should the scene be activated on load?</param>
        /// <param name="setAsActiveScene">Should the scene be set as current active scene on load?</param>
        /// <param name="sceneLoadCallback">Optional callback to invoke when the scene is loaded.</param>
        /// <returns>
        /// Async operation handle for the scene load.
        /// </returns>
        public static AsyncOperationHandle LoadSceneAsync(string sceneAssetAddress, LoadSceneMode loadSceneMode, bool activateOnLoad, bool setAsActiveScene,
                                                     Action sceneLoadCallback = null)
        {
            AsyncOperationHandle sceneLoadHandle = Addressables.LoadSceneAsync(sceneAssetAddress, loadSceneMode, activateOnLoad);

            sceneLoadHandle.Completed += _ =>
            {
                if (sceneLoadHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    sceneLoadCallback?.Invoke();

                    if (!setAsActiveScene)
                    {
                        return;
                    }

                    var sceneInstance = (SceneInstance)sceneLoadHandle.Result;
                    UnityEngine.SceneManagement.SceneManager.SetActiveScene(sceneInstance.Scene);
                }
                else
                {
                    Debug.LogError($"Failed to load scene {sceneAssetAddress}");
                }
            };

            return sceneLoadHandle;
        }

        /// <summary>
        /// Unloads a scene connected to the passed async operation handle.
        /// </summary>
        /// <param name="sceneHandle">Async operation handle.</param>
        /// <param name="sceneUnloadCallback">Optional callback to invoke when the scene is unloaded.</param>
        /// <param name="autoReleaseHandle">Should the handle be automatically released after the unload?</param>
        public static AsyncOperationHandle UnloadSceneAsync(in AsyncOperationHandle sceneHandle, Action sceneUnloadCallback = null, bool autoReleaseHandle = false)
        {
            if (!sceneHandle.IsValid())
            {
                Debug.LogError("Trying to unload a scene handle that is invalid!");
                return new AsyncOperationHandle();
            }

            if (sceneHandle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError("Trying to unload a scene handle that didn't complete successfully!");
                return new AsyncOperationHandle();
            }

            AsyncOperationHandle unloadHandle = Addressables.UnloadSceneAsync(sceneHandle);
            
            unloadHandle.Completed += _ =>
            {
                if (unloadHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    sceneUnloadCallback?.Invoke();

                    if (autoReleaseHandle)
                    {
                        Addressables.Release(unloadHandle);
                    }
                }
                else
                {
                    Debug.LogError($"Failed to unload scene!");
                }
            };

            return unloadHandle;
        }

        #endregion

        #region Activate

        /// <summary>
        /// Activates a scene connected to the passed async operation handle.
        /// </summary>
        /// <param name="sceneHandle">Scene's handle.</param>
        /// <returns>
        /// Async operation handle for scene activation.
        /// </returns>
        public static AsyncOperation ActivateSceneAsync(AsyncOperationHandle sceneHandle)
        {
            var sceneInstance = (SceneInstance)sceneHandle.Result;
            return sceneInstance.ActivateAsync();
        }

        #endregion
    }
}