using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

using static UnityEngine.AddressableAssets.Addressables;

namespace Unidork.AddressableAssetsUtility
{
	/// <summary>
	/// Async operation that loads an Addressable game object and then returns a component of a specific type on that game object as operation's result.
	/// </summary>
	public class ComponentAsyncLoadOperation<T> : AsyncOperationBase<T> where T : Component
	{
		#region Properties

		/// <summary>
		/// Reference to the Addressable game object that will be loaded with this operation.
		/// </summary>
		private AssetReference GameObjectAssetReference { get; }

		#endregion

		#region Fields

		/// <summary>
		/// Handle for the operation that loads the Addressable game object.
		/// </summary>
		private AsyncOperationHandle<GameObject> gameObjectLoadHandle;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="gameObjectAssetReference">Reference to the Addressable game object that will be loaded with this operation.</param>
		public ComponentAsyncLoadOperation(AssetReference gameObjectAssetReference)
		{
			GameObjectAssetReference = gameObjectAssetReference;
		}

		#endregion

		#region Operation

		/// <inheritdoc />
		protected override void Execute()
		{
			gameObjectLoadHandle = LoadAssetAsync<GameObject>(GameObjectAssetReference);

			gameObjectLoadHandle.Completed += completionHandle =>
			{
				if (completionHandle.Status == AsyncOperationStatus.Succeeded)
				{
					T component = gameObjectLoadHandle.Result.GetComponent<T>();

					if (component == null)
					{
						Complete(null, false,
						         $"Loaded Addressable game object doesn't have a component of type {typeof(T)}!", true);
					}
					else
					{
						Complete(component, true, "", true);
					}

					return;
				}

				Complete(null, false,
				         $"Failed to load game object asset with GUID: {GameObjectAssetReference.AssetGUID}", true);
			};
		}

		/// <inheritdoc />
		protected override void Destroy()
		{
			base.Destroy();
			Release(gameObjectLoadHandle);
		}

		#endregion
	}
}