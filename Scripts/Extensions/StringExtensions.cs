using System.Text.RegularExpressions;
using UnityEngine;

namespace Unidork.Extensions
{
	public static class StringExtensions
	{

		/// <summary>
		/// Splits a string by capital letters.
		/// </summary>
		/// <param name="string">String to split.</param>
		/// <returns>
		/// <paramref name="string"/> with spaces inserted before capital letters.
		/// </returns>
		public static string SplitByCapitalLetters(this string @string)
		{
			var regex = new Regex(@"(?<=[A-Z])(?=[A-Z][a-z]) | (?<=[^A-Z])(?=[A-Z]) | (?<=[A-Za-z])(?=[^A-Za-z])",
								  RegexOptions.IgnorePatternWhitespace);

			return regex.Replace(@string, " ");
		}

		/// <summary>
		/// Adds color tag to a string with specified color value.
		/// </summary>
		/// <param name="string">String to which color tag is added.</param>
		/// <param name="color">Color value of the color tag.</param>
		/// <returns>
		/// <paramref name="string"/> with added color tag.
		/// </returns>
		public static string AddColorTag(this string @string, Color color) => $"<color={GetHexColorFromUnityColor(color)}>{@string}</color>";

		private static string GetHexColorFromUnityColor(this Color unityColor) => $"#{ColorUtility.ToHtmlStringRGBA(unityColor)}";
	}
}