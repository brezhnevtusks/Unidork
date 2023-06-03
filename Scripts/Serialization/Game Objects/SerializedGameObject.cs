using UnityEngine;

namespace Unidork.Serialization
{
	/// <summary>
	/// Base class for objects that save game object data that can be deserialized at 
	/// runtime to recreate an object's state.
	/// </summary>
	[System.Serializable]
    public class SerializedGameObject
    {
		#region Properties

		/// <summary>
		/// Object's name.
		/// </summary>
		/// <value>
		/// Gets the value of the string field name.
		/// </value>
		public string Name => name;

		/// <summary>
		/// Is game object active on start?
		/// </summary>
		/// <value>
		/// Gets the value of the boolean field isActive.
		/// </value>
		public bool IsActive => isActive;

		/// <summary>
		/// Game object's position.
		/// </summary>
		/// <value>
		/// Gets the value of the Vector3 field position.
		/// </value>
		public Vector3 Position => position;

		/// <summary>
		/// Game object's rotation as Euler angles.
		/// </summary>
		/// <value>
		/// Gets the value of the Vector3 field eulerAngles.
		/// </value>
		public Vector3 EulerAngles => eulerAngles;

		/// <summary>
		/// Is game object active on start?
		/// </summary>
		/// <value>
		/// Gets the value of the Vector3 field localScale.
		/// </value>
		public Vector3 LocalScale => localScale;

		/// <summary>
		/// Name of the parent object.
		/// </summary>
		/// <value>
		/// Gets the value of the string field parentName.
		/// </value>
		public string ParentName => parentName;

		#endregion

		#region Fields		

		/// <summary>
		/// Object's name.
		/// </summary>
		[SerializeField]
		private string name = "";

		/// <summary>
		/// Is game object active on start?
		/// </summary>
		[SerializeField]
		private bool isActive = false;

		/// <summary>
		/// Game object's position.
		/// </summary>
		[SerializeField]
		private Vector3 position = default;

		/// <summary>
		/// Game object's rotation as Euler angles.
		/// </summary>
		[SerializeField]
		private Vector3 eulerAngles = default;

		/// <summary>
		/// Game object's local scale.
		/// </summary>
		[SerializeField]
		private Vector3 localScale = default;

		/// <summary>
		/// Name of the parent object.
		/// </summary>
		[SerializeField]
		protected string parentName = "";		

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="transform">Game object's transform.</param>
		public SerializedGameObject(Transform transform)
		{
			name = transform.gameObject.name;

			isActive = transform.gameObject.activeSelf;

			position = transform.position;
			eulerAngles = transform.rotation.eulerAngles;
			localScale = transform.localScale;

			if (transform.parent == null)
			{
				return;
			}

			parentName = transform.parent.name;
		}

		#endregion
	}
}