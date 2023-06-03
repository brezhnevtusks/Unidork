using UnityEngine;

namespace Unidork.Extensions
{
	public static class MaterialPropertyBlockExtensions
	{
		#region Color

		/// <summary>
		/// Sets the alpha value of a color property with the given name to the passed value.
		/// </summary>
		/// <param name="materialPropertyBlock">Material property block to change.</param>
		/// <param name="colorPropertyName">Name of the color property.</param>
		/// <param name="targetValue">Value to the set the alpha to.</param>
		public static void SetColorAlpha(this MaterialPropertyBlock materialPropertyBlock, string colorPropertyName, float targetValue)
		{
			SetColorAlpha(materialPropertyBlock, Shader.PropertyToID(colorPropertyName), targetValue);
		}

		/// <summary>
		/// Sets the alpha value of a color property with the given name to the passed value.
		/// </summary>
		/// <param name="materialPropertyBlock">Material property block to change.</param>
		/// <param name="colorPropertyID">Id of the color property.</param>
		/// <param name="targetValue">Value to the set the alpha to.</param>
		public static void SetColorAlpha(this MaterialPropertyBlock materialPropertyBlock, int colorPropertyID, float targetValue)
		{
			Color currentColor = materialPropertyBlock.GetColor(colorPropertyID);
			currentColor.a = targetValue;
			materialPropertyBlock.SetColor(colorPropertyID, currentColor);
		}

		#endregion
	}
}