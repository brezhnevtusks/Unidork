using UnityEngine;

namespace Unidork.Serialization
{
	/// <summary>
	/// Interface for game objects that will be serialized into Json.
	/// </summary>
	public interface ISerializableGameObject
    {
		/// <summary>
		/// Game object to be serialized.
		/// </summary>
		GameObject GameObject { get; }

		/// <summary>
		/// Should this game object be serialized?
		/// </summary>
		/// <remarks>
		/// We may want to serialized some child object inside their parent's serialization data,
		/// so we add this flag to be able to ignore those options.
		/// </remarks>
		bool ShouldBeSerialized { get; }

		/// <summary>
		/// Serializes the data that needs to be saved for the game object to be instantiated at runtime.
		/// </summary>
		/// <returns>
		/// A Json storing an instance of <see cref="SerializedGameObject"/>.
		/// </returns>
        string SerializeGameObject();

		/// <summary>
		/// Deserializes the saved game object data and appplies it to the object.
		/// </summary>
		/// <param name="serializedGameObjectJson">Serialized game object Json.</param>
        void DeserializeGameObject(string serializedGameObjectJson);
    }
}