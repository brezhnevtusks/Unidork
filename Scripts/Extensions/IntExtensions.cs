using System;

namespace Unidork.Extensions
{
	public static class IntExtensions
	{
		/// <summary>
        /// Gets the number of digits in an integer number using a binary search.
        /// </summary>
        /// <param name="number">Number.</param>
        /// <see cref="https://stackoverflow.com/questions/4483886/how-can-i-get-a-count-of-the-total-number-of-digits-in-a-number"/>
        /// <returns>
        /// An integer representing the number of digits
        /// </returns>
		public static int GetNumberOfDigits(this long number)
        {
            number = Math.Abs(number);
	        
            var numberOfDigits = 1;

	        while ((number /= 10) >= 1)
	        {
		        numberOfDigits++;
	        }
	        
	        return numberOfDigits;
        }
	}
}