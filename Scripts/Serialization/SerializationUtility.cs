using System;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Unidork.Serialization
{
	/// <summary>
	/// Utility methods for classes that use Unidork serialization.
	/// </summary>
	public static class SerializationUtility
	{
		/// <summary>
		/// Gets the passed save version in a string of format "major.minor.patch version" as integer. Removes leading zeroes.
		/// </summary>
		/// <param name="saveVersion">Save version in string format.</param>
		/// <param name="saveVersionIsValid">Is the save version valid?</param>
		/// <returns>
		/// A parsed integer representing save version or 0 if save version is invalid.
		/// </returns>
		public static int GetSaveVersionAsInteger(string saveVersion, out bool saveVersionIsValid)
		{
			saveVersionIsValid = false;

			if (!SaveVersionIsValid(saveVersion))
			{
				Debug.LogError($"Invalid save version format: {saveVersion}");
				return 0;
			}

			const string zeroString = "0";
			
			string[] versionParts = saveVersion.Split('.');

			if (versionParts.Length == 1 && string.Equals(versionParts[0], "0"))
			{
				Debug.LogError($"Invalid save version format: {saveVersion}");
				return 0;
			}
			
			saveVersionIsValid = true;

			string saveVersionString = string.Empty;

			for (int i = 0; i < versionParts.Length; i++)
			{
				string versionPart = versionParts[i];
				
				if (i == 0 && string.Equals(versionPart, zeroString))
				{
					continue;
				}

				saveVersionString += versionParts[i];
			}
			
			return Int32.Parse(saveVersionString);
		}

		/// <summary>
		/// Checks whether the passed save version matches the "major.minor.patch version" format.
		/// </summary>
		/// <param name="saveVersion"></param>
		/// <returns>
		/// True if save version is of valid format, False otherwise.
		/// </returns>
		private static bool SaveVersionIsValid(string saveVersion)
		{
			Regex checkRegex = new Regex("[0-9\\.]");
			return checkRegex.IsMatch(saveVersion);
		}
	}
}