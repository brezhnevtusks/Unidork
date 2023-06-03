using UnityEngine;

namespace Unidork.Serialization
{
	/// <summary>
	/// Component that can be attached to game objects to serialized them in 
	/// case they don't have any other components implementing serialization logic.
	/// </summary>
	public class SerializableGameObject : MonoBehaviour, ISerializableGameObject
	{
		#region Properties
		
		/// <inheritdoc/>		
		public GameObject GameObject => gameObject;

		/// <inheritdoc/>		
		public bool ShouldBeSerialized => true;

		#endregion

		#region Serialization		

		/// <inheritdoc/>		
		public string SerializeGameObject()
		{
			var serializedGameObject = new SerializedGameObject(transform);
			return JsonUtility.ToJson(serializedGameObject);
		}

		/// <inheritdoc/>		
		public void DeserializeGameObject(string serializedGameObjectJson)
		{
			var serializedGameObject = JsonUtility.FromJson<SerializedGameObject>(serializedGameObjectJson);

			gameObject.name = serializedGameObject.Name;
			gameObject.SetActive(serializedGameObject.IsActive);

			transform.position = serializedGameObject.Position;
			transform.rotation = Quaternion.Euler(serializedGameObject.EulerAngles);
			transform.localScale = serializedGameObject.LocalScale;
		}

		#endregion
	}
}