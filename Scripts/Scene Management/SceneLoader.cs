using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
#if UNIDORK_ADDRESSABLES
using UnityEngine.AddressableAssets;
#endif
using UnityEngine.SceneManagement;

namespace Unidork.SceneManagement
{
	/// <summary>
	/// Loads a scene from an Addressable asset reference or a scene name (using default scene manager).
	/// </summary>
	public class SceneLoader : MonoBehaviour
	{
		#region Enums

		/// <summary>
		/// Defines when the scene is loaded.
		/// </summary>
		private enum SceneLoadType
		{
			/// <summary>
			/// Scene is loaded when Start() is called on this component
			/// </summary>
			Start,
			
			/// <summary>
			/// Scene is loaded when LoadMainScene() is called from an external script
			/// </summary>
			ExternalCall
		} 

		#endregion
		
		#region Fields

		/// <summary>
		/// Defines when the scene is loaded.
		/// Start - Scene is loaded when Start() is called on this component.
		/// External Call - Scene is loaded when LoadMainScene() is called from an external script.
		/// </summary>
		[Tooltip("Defines when the scene is loaded:" +
		         "Start - Scene is loaded when Start() is called on this component" +
		         "External Call - Scene is loaded when LoadMainScene() is called from an external script")]
		[SerializeField]
		private SceneLoadType loadType;
		
	#if UNIDORK_ADDRESSABLES	
		/// <summary>
		/// Scene Addressable asset reference.
		/// </summary>
		[Tooltip("Scene Addressable asset reference.")]
		[SerializeField]
		private AssetReference sceneAssetReference = null;
#endif
		/// <summary>
		/// Name of the scene to load using default scene manager.
		/// </summary>
		[Tooltip("Name of the scene to load using default scene manager.")]
		[SerializeField] 
		private string sceneName = "";
		
		/// <summary>
		/// Time to wait before loading the scene.
		/// </summary>
		[SerializeField]
		private float delayBeforeLoad = 0f;

		#endregion

		#region Init

		private void Start()
		{
			if (loadType != SceneLoadType.Start)
			{
				return;
			}
			
			LoadScene();
		}

		#endregion

		#region Load

		/// <summary>
		/// Starts the asynchronous load of the scene.
		/// </summary>
		public void LoadScene()
		{
			SplashSceneLoader.AlreadyLoadedSplash = true;
			LoadSceneAsync().Forget();
		}

		private async UniTaskVoid LoadSceneAsync()
		{
			await UniTask.Delay(new TimeSpan(0, 0, 0, 0, (int)(delayBeforeLoad * 1000f)));
#if UNIDORK_ADDRESSABLES
			_ = SceneManager.LoadSceneAsync(sceneAssetReference, LoadSceneMode.Additive, activateOnLoad: true, setAsActiveScene: true, sceneLoadCallback: null);
#endif
			SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive, true).completed += _ =>
			{
				UnityEngine.SceneManagement.SceneManager.SetActiveScene(UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName));
			};
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
			await UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(0);
		}

		#endregion
	}
}