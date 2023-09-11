#if UNIDORK_ADDRESSABLES
using System;
using UnityEngine.AddressableAssets;
#endif
using System;
using UnityEngine;
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
 #if UNIDORK_ADDRESSABLES
        #region Load/Unload - Addressables

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
#endif
        
        #region Load/Unload

        /// <summary>
        /// Loads a scene asynchronously by name using default Unity scene manager. Can auto-activate..
        /// </summary>
        /// <param name="sceneName">Scene name.</param>
        /// <param name="loadSceneMode">Load scene mode.</param>
        /// <param name="allowSceneActivation">Should the scene be activated on load?</param>
        /// <param name="callback">Optional callback.</param>
        /// <returns>An async load operation.</returns>
        public static AsyncOperation LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode, bool allowSceneActivation, Action<AsyncOperation> callback = null)
        {
            return LoadSceneAsync(sceneName, new LoadSceneParameters(loadSceneMode, LocalPhysicsMode.None), allowSceneActivation, callback);
        }
        
        /// <summary>
        /// Loads a scene asynchronously by name using default Unity scene manager. Can auto-activate..
        /// </summary>
        /// <param name="sceneName">Scene name.</param>
        /// <param name="loadSceneParameters">Load scene parameters that include load scene mode and local physics mode..</param>
        /// <param name="allowSceneActivation">Should the scene be activated on load?</param>
        /// <param name="callback">Optional callback.</param>
        /// <returns>An async load operation.</returns>
        public static AsyncOperation LoadSceneAsync(string sceneName, LoadSceneParameters loadSceneParameters, bool allowSceneActivation, Action<AsyncOperation> callback = null)
        {
            AsyncOperation loadOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadSceneParameters);
            loadOperation.allowSceneActivation = allowSceneActivation;
            if (callback != null)
            {
                loadOperation.completed += callback;
            }
            return loadOperation;
        }

        /// <summary>
        /// Loads a scene asynchronously by build index using default Unity scene manager. Can auto-activate.
        /// </summary>
        /// <param name="sceneBuildIndex">Scene's build index.</param>
        /// <param name="loadSceneMode">Load scene mode.</param>
        /// <param name="allowSceneActivation">Should the scene be activated on load?</param>
        /// <param name="callback">Optional callback.</param>
        /// <returns>An async load operation.</returns>
        public static AsyncOperation LoadSceneAsync(int sceneBuildIndex, LoadSceneMode loadSceneMode, bool allowSceneActivation, Action<AsyncOperation> callback = null)
        {
            return LoadSceneAsync(sceneBuildIndex, new LoadSceneParameters(loadSceneMode, LocalPhysicsMode.None), allowSceneActivation, callback);
        }

        /// <summary>
        /// Loads a scene asynchronously by build index using default Unity scene manager. Can auto-activate.
        /// </summary>
        /// <param name="sceneBuildIndex">Scene's build index.</param>
        /// <param name="loadSceneParameters">Load scene parameters that include load scene mode and local physics mode..</param>
        /// <param name="allowSceneActivation">Should the scene be activated on load?</param>
        /// <param name="callback">Optional callback.</param>
        /// <returns>An async load operation.</returns>
        public static AsyncOperation LoadSceneAsync(int sceneBuildIndex, LoadSceneParameters loadSceneParameters, bool allowSceneActivation, Action<AsyncOperation> callback = null)
        {
            AsyncOperation loadOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneBuildIndex, loadSceneParameters);
            loadOperation.allowSceneActivation = allowSceneActivation;
            if (callback != null)
            {
                loadOperation.completed += callback;
            }
            return loadOperation;
        }

        /// <summary>
        ///  Unloads a scene asynchronously by direct reference using the default scene manager.
        /// </summary>
        /// <param name="scene">Scene.</param>
        /// <param name="unloadSceneOptions">Optional unload options.</param>
        /// <param name="callback">Optional callback.</param>
        /// <returns>An async unload operation.</returns>
        public static AsyncOperation UnloadSceneAsync(Scene scene, UnloadSceneOptions unloadSceneOptions = default, Action<AsyncOperation> callback = null)
        {
            AsyncOperation unloadOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(scene, unloadSceneOptions);
            if (callback != null)
            {
                unloadOperation.completed += callback;
            }
            return unloadOperation;
        }
        
        /// <summary>
        ///  Unloads a scene asynchronously by name using the default scene manager.
        /// </summary>
        /// <param name="sceneName">Scene name.</param>
        /// <param name="unloadSceneOptions">Optional unload options.</param>
        /// <param name="callback">Optional callback.</param>
        /// <returns>An async unload operation.</returns>
        
        public static AsyncOperation UnloadSceneAsync(string sceneName, UnloadSceneOptions unloadSceneOptions = default, Action<AsyncOperation> callback = null)
        {
            AsyncOperation unloadOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName, unloadSceneOptions);
            if (callback != null)
            {
                unloadOperation.completed += callback;
            }
            return unloadOperation;
        }
        
        /// <summary>
        ///  Unloads a scene asynchronously by build index using the default scene manager.
        /// </summary>
        /// <param name="sceneBuildIndex">Scene build index.</param>
        /// <param name="unloadSceneOptions">Optional unload options.</param>
        /// <param name="callback">Optional callback.</param>
        /// <returns>An async unload operation.</returns>
        
        public static AsyncOperation UnloadSceneAsync(int sceneBuildIndex, UnloadSceneOptions unloadSceneOptions = default, Action<AsyncOperation> callback = null)
        {
            AsyncOperation unloadOperation = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneBuildIndex, unloadSceneOptions);
            if (callback != null)
            {
                unloadOperation.completed += callback;
            }
            return unloadOperation;
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