using System;
using UnityEngine;

namespace Unidork.Serialization
{
	/// <summary>
	/// Base class for save data objects.
	/// </summary>
	[System.Serializable]
	public abstract class BaseSaveData
	{
		#region Properties

		/// <summary>
		/// Save version for this save data.
		/// </summary>
		/// <value>
		/// Gets and sets the value of the string field saveVersion.
		/// </value>
		public string SaveVersion {	get => saveVersion; set => saveVersion = value; }

		/// <summary>
		/// Save data timestamp.
		/// </summary>
		/// <value>
		/// Gets the value of the field timestamp.
		/// </value>
		public DateTime Timestamp => timestamp;

		#endregion

		#region Fields

		/// <summary>
		/// Save version for this save data.
		/// </summary>
		[SerializeField] 
		private string saveVersion;

		/// <summary>
		/// Save data timestamp.
		/// </summary>
		[SerializeField] 
		private DateTime timestamp;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="saveVersion">Save version for this save data.</param>
		/// <param name="timestamp">Save data timestamp.</param>
		public BaseSaveData(string saveVersion, DateTime timestamp)
		{
			this.saveVersion = saveVersion;
			this.timestamp = timestamp;
		}

		#endregion
	}
}