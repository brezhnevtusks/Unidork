using System.IO;
using UnityEngine;

namespace Unidork.FileUtility
{
	/// <summary>
	/// Collection of utility I/O methods.
	/// </summary>
	public static class FileUtils
	{
		/// <summary>
		/// Gets a string of bytes from a file.
		/// </summary>
		/// <param name="filePath">File path.</param>
		/// <returns>
		/// A string of bytes at the specified path or an empty string if file at that path doesn't exist.
		/// </returns>
		public static string GetByteStringFromFile(string filePath)
		{
			var saveDataBytes = string.Empty; 
        
			if (File.Exists(filePath))
			{
				saveDataBytes = File.ReadAllText(filePath);
			}

			return saveDataBytes;
		}
	}
}