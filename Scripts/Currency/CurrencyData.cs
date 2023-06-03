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
		public int CurrencyId => currencyId;

		/// <summary>
		/// User-friendly currency name for easier data readability.
		/// </summary>
		/// <value>
		/// Gets the value of the string field currencyName.
		/// </value>
		public string CurrencyName => currencyName;
		
		/// <summary>
		/// Currency type.
		/// </summary>
		/// <value>
		/// Gets the value of the field currencyType.
		/// </value>
		public CurrencyType CurrencyType => currencyCurrencyType;

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
		private int currencyId;

		/// <summary>
		/// User-friendly currency name for easier data readability.
		/// </summary>
		[SerializeField]
		private string currencyName;
		
		/// <summary>
		/// Currency type.
		/// </summary>
		[SerializeField]
		private CurrencyType currencyCurrencyType;

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
		/// <param name="currencyId">Unique currency ID.</param>
		/// <param name="currencyName">User-friendly currency name for easier data readability.</param>
		/// <param name="currencyCurrencyType">Currency type.</param>
		/// <param name="amount">Amount of currency the player owns.</param>
		public CurrencyData(int currencyId, string currencyName, CurrencyType currencyCurrencyType, double amount = 0d)
		{
			this.currencyId = currencyId;
			this.currencyName = currencyName;
			this.currencyCurrencyType = currencyCurrencyType;
			this.amount = amount;
		}		

		#endregion
	}
}