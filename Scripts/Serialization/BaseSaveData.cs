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

		#endregion

		#region Fields

		/// <summary>
		/// Save version for this save data.
		/// </summary>
		[SerializeField] 
		private string saveVersion;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="saveVersion">Save version for this save data.</param>
		public BaseSaveData(string saveVersion)
		{
			this.saveVersion = saveVersion;
		}

		#endregion
	}
}