using System;
using UnityEngine;

namespace Unidork.Resources
{
	/// <summary>
	/// Stores data about a type of in-game resource: ID, type, amount player has etc.
	/// </summary>
	[System.Serializable]
	public class ResourceData<T> where T : Enum
	{
		#region Properties

		/// <summary>
		/// Resource type.
		/// </summary>
		/// <value>
		/// Gets the value of the field type.
		/// </value>
		public T Type => type;

		/// <summary>
		/// Unique resource ID.
		/// </summary>
		public int Id => Convert.ToInt32(type);

		/// <summary>
		/// User-friendly resource name.
		/// </summary>
		/// <value>
		/// Gets the value of the string field name.
		/// </value>
		public string Name => name;

		/// <summary>
		/// Amount of resource the player owns.
		/// </summary>
		/// <value>
		/// Gets and sets the value of the double field amount.
		/// </value>
		public double Amount
		{
			get => amount;
			set => amount = value;
		}

		#endregion
		
		#region Fields

		/// <summary>
		/// Resource type.
		/// </summary>
		[SerializeField] 
		private T type;

		/// <summary>
		/// User-friendly resource name.
		/// </summary>
		[SerializeField]
		private string name;

		/// <summary>
		/// Amount of resource the player owns.
		/// </summary>
		[SerializeField]
		private double amount;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="type">Resource type.</param>
		/// <param name="name">User-friendly resource name.</param>
		/// <param name="amount">Amount of resource the player owns.</param>
		public ResourceData(T type, string name, double amount = 0d)
		{
			this.type = type;
			this.name = name;
			this.amount = amount;
		}		

		#endregion
	}
}