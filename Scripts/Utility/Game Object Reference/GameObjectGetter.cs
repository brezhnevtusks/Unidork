using System;
using Sirenix.OdinInspector;
using Unidork.Attributes;
using UnityEngine;

namespace Unidork.Utility
{
	/// <summary>
	/// Gets and stores a reference to a game object based on settings.
	/// </summary>
	[Serializable]
	public class GameObjectGetter
	{
		#region Properties

		/// <summary>
		/// Game object acquired by this getter.
		/// </summary>
		/// <value>
		/// Gets the value of the field gameObject.
		/// </value>
		public GameObject GameObject
		{
			get
			{
				if (gameObject == null)
				{
					gameObject = gameObjectReferenceType switch
					{
						GameObjectReferenceType.DirectReference => null,
						GameObjectReferenceType.Tag => GameObject.FindWithTag(tag),
						GameObjectReferenceType.Name => GameObject.Find(name),
						_ => throw new ArgumentOutOfRangeException()
					};
				}

				return gameObject;
			}
		}

		#endregion

		#region Fields

		/// <summary>
		/// Defines the way to get the game object.
		/// </summary>
		[Tooltip("Defines the way to get the game object.")]
		[SerializeField]
		private GameObjectReferenceType gameObjectReferenceType = default;

		/// <summary>
		/// Game object acquired by this getter.
		/// </summary>
		[ShowIf("@this.gameObjectReferenceType", GameObjectReferenceType.DirectReference)]
		[SerializeField]
		private GameObject gameObject;
		
		/// <summary>
		/// Tag by which to get the game object.
		/// </summary>
		[ShowIf("@this.gameObjectReferenceType", GameObjectReferenceType.Tag)]
		[Tooltip("Tag by which to get the game object.")]
		[SerializeField]
		private string tag = "";

		/// <summary>
		/// Name by which to get the game object.
		/// </summary>
		[ShowIf("@this.gameObjectReferenceType", GameObjectReferenceType.Name)]
		[Tooltip("Name by which to get the game object.")]
		[SerializeField]
		private string name = "";

		#endregion
	}
}