using System;
using Unidork.Extensions;

namespace Unidork.Math
{
	/// <summary>
	/// 
	/// </summary>
	public static class MathUtils
	{
		#region Formatting

		/// <summary>
		/// Gets a string that represents the formatted value of the passed number with a respective SI system unit prefix.
		/// </summary>
		/// <remarks>Unit prefix is actually used as a suffix in our case, trailing the number.</remarks>
		/// <param name="number">Number to round.</param>
		/// <param name="minDigitsToAddPrefix">Prefix will only be added if the number of digits is at least as large as this value.</param>
		/// <returns>
		/// A string that is the result of calling <see cref="GetFormattedNumberWithUnitPrefix(long)"/>.
		/// </returns>
		public static string GetFormattedNumberWithUnitPrefix(double number, int minDigitsToAddPrefix = 3)
		{
			return GetFormattedNumberWithUnitPrefix((int) number, minDigitsToAddPrefix);
		}

		/// <summary>
		/// Gets a string that represents the formatted value of the passed number with a respective SI system unit prefix
		/// </summary>
		/// /// <remarks>Unit prefix is actually used as a suffix in our case, trailing the number.</remarks>
		/// <param name="number">Number to round.</param>
		/// <param name="minDigitsToAddPrefix">Prefix will only be added if the number of digits is at least as large as this value.</param>
		/// <returns>
		/// A string that contains of the formatted version of the number and a unit suffix, if any.
		/// </returns>
		public static string GetFormattedNumberWithUnitPrefix(long number, int minDigitsToAddPrefix = 3)
		{
			long numberOfDigits = number.GetNumberOfDigits();

			if (numberOfDigits < minDigitsToAddPrefix)
			{
				return number.ToString();
			}
			
			string unitPrefix = GetUnitPrefix(numberOfDigits);
			
			string formattedNumber = string.Format(new IntegerFormatter(), "{0:K}", number);
			
			return formattedNumber + unitPrefix;
		}

		/// <summary>
		/// Gets the unit suffix for the passed number of digits.
		/// </summary>
		/// <param name="numberOfDigits">Number of digits.</param>
		/// <returns>
		/// An empty string if the number has three or less digits or a string with the respective SI system unit prefix.
		/// </returns>
		private static string GetUnitPrefix(long numberOfDigits)
		{
			var suffix = "";

			if (numberOfDigits > 3)
			{
				numberOfDigits--;
				numberOfDigits /= 3;

				switch (numberOfDigits)
				{
					case 1:
						suffix = "K";
						break;
					case 2:
						suffix = "M";
						break;
					case 3:
						suffix = "B";
						break;
					case 4:
						suffix = "T";
						break;
					case 5:
						suffix = "q";
						break;
					case 6:
						suffix = "Q";
						break;
				}
			}
			
			return suffix;
		}
		
		#region Formatter

		/// <summary>
		/// Custom formatter to used when formatting integers for the <see cref="MathUtils.GetFormattedNumberWithUnitPrefix(int,bool)"/> method.
		/// </summary>
		private class IntegerFormatter : ICustomFormatter, IFormatProvider
		{
			#region Constants

			/// <summary>
			/// Array storing valid unit prefixes.
			/// </summary>
			private readonly string[] unitPrefixes = 
			{
				"K",
				"M",
				"B"
			};

			#endregion
			
			#region Format
			
			/// <inheritdoc />
			public object GetFormat(Type formatType)
			{
				return formatType == typeof(ICustomFormatter) ? this : null;
			}
			
			/// <inheritdoc />
			public string Format(string format, object arg, IFormatProvider formatProvider)
			{
				if (format == null || !FormatIsValid(format)) 
				{
					if (arg is IFormattable formattable) 
					{
						return formattable.ToString(format, formatProvider);
					}
					
					return arg.ToString();
				}

				decimal number = Convert.ToDecimal(arg);

				string numberString = "";

				if (number < 1000)
				{
					numberString = number.ToString();
				}
				else if (number < 1_000_000)
				{
					numberString = (number / 1000).ToString("#.0");
				}
				else if (number < 1_000_000_000)
				{
					numberString = (number / 1_000_000).ToString("#.0");
				}
				else if (number < 1_000_000_000_000)
				{
					numberString = (number / 1_000_000_000).ToString("#.0");
				}
				else if (number < 1_000_000_000_000_000)
				{
					numberString = (number / 1_000_000_000_000).ToString("#.0");
				}
				else if (number < 1_000_000_000_000_000_000)
				{
					numberString = (number / 1_000_000_000_000_000).ToString("#.0");
				}
				else
				{
					numberString = (number / 1_000_000_000_000_000_000).ToString("#.0");
				}

				if (number > 1000 && numberString.EndsWith("0"))
				{
					numberString = numberString.Remove(numberString.Length - 1);
					numberString = numberString.Remove(numberString.Length - 1);
				}
				
				return numberString;
			}

			/// <summary>
			/// Checks whether the passed format string is valid.
			/// </summary>
			/// <param name="format">Format string.</param>
			/// <returns>
			/// True if format string starts with one of the members of the <see cref="unitPrefixes"/> array, False otherwise.
			/// </returns>
			private bool FormatIsValid(string format)
			{
				foreach (string unitPrefix in unitPrefixes)
				{
					if (format.StartsWith(unitPrefix))
					{
						return true;
					}
				}

				return false;
			}
			
			#endregion
		}
		
		#endregion

		#endregion
	}
}