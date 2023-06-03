using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace Unidork.SceneManagement
{
	/// <summary>
	/// Loads a scene from an Addressable asset reference.
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
		
		/// <summary>
		/// Scene Addressable asset reference.
		/// </summary>
		[Tooltip("Scene Addressable asset reference.")]
		[SerializeField]
		private AssetReference sceneAssetReference = null;

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
			_ = SceneManager.LoadSceneAsync(sceneAssetReference, LoadSceneMode.Additive, activateOnLoad: true, setAsActiveScene: true, sceneLoadCallback: null);
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