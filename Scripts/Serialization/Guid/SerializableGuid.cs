using System;
using UnityEngine;

namespace Unidork.Serialization
{
	/// <summary>
	/// Serializable wrapper for System.Guid.
	/// Can be implicitly converted to/from System.Guid.
	/// </summary>
	[Serializable]
	public struct SerializableGuid : ISerializationCallbackReceiver, IEquatable<SerializableGuid>
	{
		#region Fields

		/// <summary>
		/// Guid wrapped by this object.
		/// </summary>
		private Guid guid;
		
		/// <summary>
		/// Byte array representation of this object.
		/// </summary>
		[SerializeField] private byte[] serializedGuid;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="guid">Guid to wrap.</param>
		public SerializableGuid(Guid guid)
		{
			this.guid = guid;
			serializedGuid = null;
		}
		
		#endregion

		#region Object

		/// <inheritdoc />
		public override bool Equals(object obj) => obj is SerializableGuid other && Equals(other);

		/// <inheritdoc />
		public bool Equals(SerializableGuid other) => guid.Equals(other.guid);

		/// <inheritdoc />
		public override int GetHashCode() => HashCode.Combine(guid, serializedGuid);

		#endregion

		#region Serialization

		/// <inheritdoc />
		public void OnAfterDeserialize()
		{
			try
			{
				guid = new Guid(serializedGuid);
			}
			catch
			{
				guid = Guid.Empty;
				Debug.LogWarning($"Attempted to parse invalid GUID string '{serializedGuid}'. GUID will set to System.Guid.Empty");
			}
		}

		/// <inheritdoc />
		public void OnBeforeSerialize()
		{
			serializedGuid = guid.ToByteArray();
		}

		#endregion
		
		#region Operators
		
		public static bool operator ==(SerializableGuid a, SerializableGuid b) => a.guid.Equals(b.guid);

		public static bool operator !=(SerializableGuid a, SerializableGuid b) => !a.guid.Equals(b.guid);
		
		public static implicit operator SerializableGuid(Guid guid) => new SerializableGuid(guid);
		public static implicit operator Guid(SerializableGuid serializable) => serializable.guid;

		public static implicit operator SerializableGuid(byte[] serializedGuid) => new SerializableGuid(new Guid(serializedGuid));
		public static implicit operator byte[](SerializableGuid guid) => guid.guid.ToByteArray();

		#endregion
	}
}