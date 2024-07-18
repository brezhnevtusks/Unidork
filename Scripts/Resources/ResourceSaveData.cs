using System;
using Unidork.Serialization;
using UnityEngine;

namespace Unidork.Resources
{
	/// <summary>
	/// Stores resource save data.
	/// </summary>
	[System.Serializable]
	public class ResourceSaveData<T> : BaseSaveData where T : Enum
	{
		#region Properties

		/// <summary>
		/// Array that stores data about all resources in the game.
		/// </summary>
		/// <value>
		/// Gets the clone of the value stored in data field.
		/// </value>
		public ResourceData<T>[] Data => (ResourceData<T>[])data.Clone();

		#endregion
		
		#region Fields

		/// <summary>
		/// Array that stores data about all resources in the game.
		/// </summary>
		[SerializeField]
		private ResourceData<T>[] data;

		#endregion
		
		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="resourceDataArray"></param>
		/// <param name="saveVersion"></param>
		public ResourceSaveData(ResourceData<T>[] resourceDataArray, string saveVersion) : base(saveVersion, DateTime.Now)
		{
			data = resourceDataArray;
		}

		#endregion
	}
}