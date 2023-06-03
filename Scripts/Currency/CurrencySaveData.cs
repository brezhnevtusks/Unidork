using Unidork.Currency;
using UnityEngine;

namespace Unidork.Serialization
{
	/// <summary>
	/// Stores currency save data.
	/// </summary>
	[System.Serializable]
	public class CurrencySaveData : BaseSaveData
	{
		#region Fields

		/// <summary>
		/// Array that stores data about all currencies in the game.
		/// </summary>
		[SerializeField]
		private CurrencyData[] currencyDataArray;

		/// <summary>
		/// Last queried currency data. Used so we don't have to request the same currency data multiple times
		/// in consequent calls to currency manager.
		/// </summary>
		private CurrencyData cachedCurrencyData;

		#endregion
		
		#region Constructor

		/// <summary>
		/// 
		/// </summary>
		/// <param name="currencyDataArray"></param>
		/// <param name="saveVersion"></param>
		public CurrencySaveData(CurrencyData[] currencyDataArray, string saveVersion) : base(saveVersion)
		{
			this.currencyDataArray = currencyDataArray;
		}

		#endregion

		#region Get

		/// <summary>
		/// Gets currency data with specified id.
		/// </summary>
		/// <param name="currencyId">Currency ID></param>
		/// <returns>
		/// <see cref="CurrencyData"/> from <see cref="currencyDataArray"/> that corresponds with
		/// the passed ID or null if such currency data doesn't exist.
		/// </returns>
		public CurrencyData GetCurrencyById(int currencyId)
		{
			if (cachedCurrencyData != null && cachedCurrencyData.CurrencyId == currencyId)
			{
				return cachedCurrencyData;
			}

			foreach (CurrencyData currencyData in currencyDataArray)
			{
				if (currencyData.CurrencyId == currencyId)
				{
					cachedCurrencyData = currencyData;
					return currencyData;
				}
			}

			Debug.LogError($"Failed to find currency with id {currencyId}! Make sure one is created when currencies are initialized.");
			return null;
		}

		#endregion
	}
}