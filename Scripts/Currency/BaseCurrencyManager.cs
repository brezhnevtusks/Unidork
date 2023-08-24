using Unidork.Attributes;
using Unidork.Serialization;
using UnityEngine;

namespace Unidork.Currency
{
	/// <summary>
	/// Base class for currency managers.
	/// </summary>
	public abstract class BaseCurrencyManager : MonoBehaviour
	{
		#region Field

		/// <summary>
		/// Instance of this class.
		/// </summary>
		protected static BaseCurrencyManager instance;
		
		/// <summary>
		/// Current version of save data.
		/// </summary>
		protected string saveVersion;

		/// <summary>
		/// Path to the persistent data directory.
		/// </summary>
		protected string persistentDataPath;
		
		/// <summary>
		/// Object that stores currency save data.
		/// </summary>
		protected CurrencySaveData currencySaveData;

		/// <summary>
		/// Path of the currency save data file relative to <see cref="Application.persistentDataPath"/>.
		/// </summary>
		[Space, BaseHeader, Space]
		[Tooltip("Path of the currency save data file relative to Application.persistentDataPath.")]
		[SerializeField]
		private string currencySaveDataRelativePath = "/CurrencySaveData.json";

		/// <summary>
		/// Path at which currency save data is stored.
		/// </summary>
		private string currencySaveDataPath;

		#endregion

		#region Init

		protected virtual void Awake()
		{
			if (instance != null)
			{
				Destroy(instance.gameObject);
				return;
			}

			instance = this;
					
			saveVersion = BaseSerializationManager.SaveVersion;
			persistentDataPath = Application.persistentDataPath;

			currencySaveDataPath = $"{persistentDataPath}{currencySaveDataRelativePath}";
			
			currencySaveData = BaseSerializationManager.DeserializeSaveDataFromFile<CurrencySaveData>(currencySaveDataPath);

			if (currencySaveData == null)
			{
				ResetSaveDataInternal();
			}
		}

		#endregion

		#region Currency

		/// <summary>
		/// Checks whether the amount of currency player has with specified ID is equal to or bigger that the passed amount.
		/// </summary>
		/// <param name="currencyId">Currency ID></param>
		/// <param name="amount">Amount to check against.</param>
		/// <returns>
		/// True if the player owns at least as much specified currency as the passed amount, False otherwise.
		/// </returns>
		public static bool HasEnoughCurrency(int currencyId, double amount)
		{
			return instance.currencySaveData.GetCurrencyById(currencyId).Amount >= amount;
		}

		/// <summary>
		/// Gets the amount of currency with specified ID that player owns.
		/// </summary>
		/// <param name="currencyId">Currency ID.</param>
		/// <returns>
		/// A double value that represents the amount of owned currency for passed currency ID.
		/// </returns>
		public static double GetCurrencyAmountById(int currencyId)
		{			
			return instance.currencySaveData.GetCurrencyById(currencyId).Amount;
		}

		/// <summary>
		/// Decreases the amount of currency with passed id by the specified value.
		/// </summary>
		/// <param name="currencyId">Currency Id.</param>
		/// <param name="amount">Amount to spend.</param>
		public static void SpendCurrency(int currencyId, double amount)
		{
			instance.currencySaveData.GetCurrencyById(currencyId).Amount -= amount;
			SaveDataStatic();
		}

		/// <summary>
		/// Increases the amount of currency with passed id by the specified value.
		/// </summary>
		/// <param name="currencyId">Currency Id.</param>
		/// <param name="amount">Amount to spend.</param>
		public static void AddCurrency(int currencyId, double amount)
		{
			instance.currencySaveData.GetCurrencyById(currencyId).Amount += amount;
			SaveDataStatic();
		}

		/// <summary>
		/// Creates a currency data array.
		/// </summary>
		/// <returns>
		/// An array of <see cref="CurrencyData"/> objects.
		/// </returns>
		protected abstract CurrencyData[] CreateCurrencyDataArray();

		#endregion

		#region Save

		/// <summary>
		/// Calls non-static SaveData() on singleton instance of the save manager.
		/// </summary>
		protected static void SaveDataStatic()
		{
			instance.SaveData();
		}

		/// <summary>
		/// Tells the serialization manager to save currency data to disk. Can be overrided by inheriting classes.
		/// </summary>
		protected virtual void SaveData()
		{
			BaseSerializationManager.SerializeSaveDataToFile(instance.currencySaveData, instance.currencySaveDataPath);
		}

		#endregion

		#region Reset

		/// <summary>
		/// Resets the currency save data by calling <see cref="ResetSaveDataInternal"/>.
		/// </summary>
		public static void ResetSaveData()
		{
			if (instance == null)
			{
				return;
			}

			instance.ResetSaveDataInternal();
		}

		/// <summary>
		/// Resets the currency save data.
		/// </summary>
		private void ResetSaveDataInternal()
		{
			currencySaveData = new CurrencySaveData(CreateCurrencyDataArray(), saveVersion);
			BaseSerializationManager.SerializeSaveDataToFile(currencySaveData, currencySaveDataPath);
		}

		#endregion
		
	}
}