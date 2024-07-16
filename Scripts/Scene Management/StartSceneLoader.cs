using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
#if UNIDORK_START_SCENE_ADDRESSABLE
using UnityEngine.AddressableAssets;
#endif
using UnityEngine.SceneManagement;

namespace Unidork.SceneManagement
{
	/// <summary>
	/// Loads game's start scene from an Addressable asset reference or a scene name (using default scene manager).
	/// </summary>
	public class StartSceneLoader : MonoBehaviour
	{
		#region Fields
		
#if UNIDORK_START_SCENE_ADDRESSABLE	
		/// <summary>
		/// Scene Addressable asset reference.
		/// </summary>
		[Tooltip("Scene Addressable asset reference.")]
		[SerializeField]
		private AssetReference sceneAssetReference = null;
#else
		/// <summary>
		/// Name of the scene to load using default scene manager.
		/// </summary>
		[Tooltip("Name of the scene to load using default scene manager.")]
		[SerializeField] 
		private string sceneName = "";
#endif
		
		/// <summary>
		/// Time to wait before loading the scene.
		/// </summary>
		[SerializeField]
		private float delayBeforeLoad = 0f;

		#endregion

		#region Load

		/// <summary>
		/// Loads the Scene asynchronously.
		/// </summary>
		/// <returns>
		/// Async load task.
		/// </returns>
		public async UniTask LoadSceneAsync()
		{
			SplashSceneLoader.AlreadyLoadedSplash = true;
			
			await UniTask.Delay(new TimeSpan(0, 0, 0, 0, (int)(delayBeforeLoad * 1000f)));
#if UNIDORK_START_SCENE_ADDRESSABLE
			await SceneManager.LoadSceneAsync(sceneAssetReference, LoadSceneMode.Additive, activateOnLoad: true, setAsActiveScene: true, sceneLoadCallback: null);
#else
			AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive, true);
			loadOperation.completed +=  _ =>
			{
				UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName));
			};
			await loadOperation.ToUniTask();
#endif
		}

		#endregion

		#region Unload
		
		/// <summary>
		/// Starts the asynchronous unload of the splash scene.
		/// </summary>
		public void StartSplashSceneUnload(float delayInSeconds) => UnloadSplashSceneAsync(delayInSeconds).Forget();

		/// <summary>
		/// Unloads the splash scene asynchronously after a delay.
		/// </summary>
		/// <returns>
		/// <see cref="UniTaskVoid"/>.
		/// </returns>
		private async UniTaskVoid UnloadSplashSceneAsync(float delay)
		{
			await UniTask.Delay(new TimeSpan(0, 0, 0, 0, (int)(delay * 1000f)));
			await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(0).ToUniTask();
		}

		#endregion
	}
}