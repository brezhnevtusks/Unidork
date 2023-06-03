using UnityEngine;

namespace Unidork.Math
{	public static class IntegerExtensions
    {
		#region Odd/Even

		/// <summary>
		/// Checks whether an integer number is even.
		/// </summary>
		/// <param name="number">Number.</param>
		/// <returns>
		/// True if the number is even, False otherwise.
		/// </returns>
		public static bool IsEvenNumber(this int number) => number % 2 == 0;

		/// <summary>
		/// Checks whether an integer number if odd.
		/// </summary>
		/// <param name="number">Number.</param>
		/// <returns>
		/// True if the number is odd, False otherwise.
		/// </returns>
		public static bool IsOddNumber(this int number) => !IsEvenNumber(number);

		#endregion

		#region Prime

		/// <summary>
		/// Checks whether an integer number is a prime number.
		/// </summary>
		/// <param name="number"></param>
		/// <returns>
		/// True if the number is as prime, False otherwise.
		/// </returns>
		public static bool IsPrime(int number)
		{
			if (number <= 1)
			{
				return false;
			}

			if (number % 2 == 0)
			{
				return false;
			}

			if (number == 2)
			{
				return true;
			}

			var boundary = (int)Mathf.Floor(Mathf.Sqrt(number));

			for (int i = 3; i <= boundary; i += 2)
			{
				if (number % i == 0)
				{
					return false;
				}
			}

			return true;
		}

		#endregion

		#region Remainder

		/// <summary>
		/// Checks whether one integer number can be divided by another without remainder.
		/// </summary>
		/// <param name="dividend">Dividend.</param>
		/// <param name="divisor">Divisor.</param>
		/// <returns>
		/// True if dividend can be divided by divisor with no remainder, False otherwise.
		/// </returns>
		public static bool HasNoRemainderWhenDividedBy(this int dividend, int divisor) => GetRemainderWhenDividedBy(dividend, divisor) == 0;

		/// <summary>
		/// Gets the remainder of divison of two integer numbers.
		/// </summary>
		/// <param name="dividend">Dividend.</param>
		/// <param name="divisor">Divisor.</param>
		/// <returns>
		/// An integer that is the result of dividing dividend by divisor.
		/// </returns>
		public static int GetRemainderWhenDividedBy(this int dividend, int divisor) => dividend % divisor;

		#endregion
	}
}