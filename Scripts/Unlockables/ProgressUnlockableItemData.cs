using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Unidork.Unlockables
{
	/// <summary>
	/// Scriptable object that stores data about a progress-unlockable item.
	/// </summary>
	[CreateAssetMenu(fileName = "ProgressUnlockableItemData",
					 menuName = "Progress/Progress Unlockable Item Data")]
	public class ProgressUnlockableItemData : ScriptableObject
    {
		#region Properties

		/// <summary>
		/// Item's unique id.
		/// </summary>
		/// <value>
		/// Gets the value of the integer field itemId.
		/// </value>
		public int ItemId => itemId;

		/// <summary>
		/// Item's user-friendly name.
		/// </summary>
		/// <value>
		/// Gets the value of the string field name.
		/// </value>
		public string Name => name;

		/// <summary>
		/// Reference to the progress-unlockable item asset.
		/// </summary>
		/// <value>
		/// Gets the value of the field unlockableAssetReference.
		/// </value>
		public AssetReference UnlockableAssetReference => unlockableAssetReference;

		#endregion

		#region Fields

		/// <summary>
		/// Item's unique id.
		/// </summary>
		[Tooltip("Item's unique id.")]
		[SerializeField]
		private int itemId = 0;

		/// <summary>
		/// Item's user-friendly name.
		/// </summary>
		[Tooltip("Item's user-friendly name.")]
		[SerializeField]
		private new string name = "AwesomeItem";

		/// <summary>
		/// Reference to the progress-unlockable item asset.
		/// </summary>
		[Tooltip("Reference to the progress-unlockable item asset.")]
		[SerializeField]
		private AssetReference unlockableAssetReference = null;    

		#endregion
	}
}