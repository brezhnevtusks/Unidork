using Sirenix.OdinInspector;
using UnityEngine;

namespace Unidork.Currency
{
	/// <summary>
	/// Stores data about a type of in-game currency: ID, type, amount player has etc.
	/// </summary>
	[System.Serializable]
	public class CurrencyData
	{
		#region Properties

		/// <summary>
		/// Unique currency ID.
		/// </summary>
		/// <value>
		/// Gets the value of the int field currencyId.
		/// </value>
		public int CurrencyId => id;

		/// <summary>
		/// User-friendly currency name for easier data readability.
		/// </summary>
		/// <value>
		/// Gets the value of the string field currencyName.
		/// </value>
		public string CurrencyName => name;
		
		/// <summary>
		/// Currency type.
		/// </summary>
		/// <value>
		/// Gets the value of the field currencyType.
		/// </value>
		public CurrencyType CurrencyType => type;

		/// <summary>
		/// Amount of currency the player owns.
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
		/// Unique currency ID.
		/// </summary>
		[SerializeField]
		private int id;

		/// <summary>
		/// User-friendly currency name for easier data readability.
		/// </summary>
		[ShowInInspector]
		private string name;
		
		/// <summary>
		/// Currency type.
		/// </summary>
		[SerializeField]
		private CurrencyType type;

		/// <summary>
		/// Amount of currency the player owns.
		/// </summary>
		[SerializeField]
		private double amount;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="id">Unique currency ID.</param>
		/// <param name="name">User-friendly currency name for easier data readability.</param>
		/// <param name="type">Currency type.</param>
		/// <param name="amount">Amount of currency the player owns.</param>
		public CurrencyData(int id, string name, CurrencyType type, double amount = 0d)
		{
			this.id = id;
			this.name = name;
			this.type = type;
			this.amount = amount;
		}		

		#endregion
	}
}