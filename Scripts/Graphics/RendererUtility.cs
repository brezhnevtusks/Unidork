using DG.Tweening;
using Unidork.Extensions;
using Unidork.Tweens;
using UnityEngine;

namespace Unidork.GraphicsUtility
{
	/// <summary>
	/// Various utility methods for Unity's renderers.
	/// </summary>
	public static class RendererUtility
    {
		#region Fields

		/// <summary>
		/// Material property block to reuse for renderer operations.
		/// </summary>
		private static MaterialPropertyBlock materialPropertyBlock;

		#endregion

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		static RendererUtility()
		{
			materialPropertyBlock = new MaterialPropertyBlock();
		}

		#endregion

		#region Color

		/// <summary>
		/// Creates a tween that changes the color of the passed renderer to the target value.
		/// </summary>
		/// <param name="renderer">Renderer to fade.</param>
		/// <param name="colorPropertyName">Property name of the renderer's color to be changed.</param>
		/// <param name="colorTweenSettings">Settings for the tween used to change the renderer's color.</param>		/// 
		/// <param name="subMeshIndex">Index of the sub mesh.</param>
		/// <param name="loopType">Number of loops.</param>
		/// <param name="numberOfLoops">Loop type.</param>
		/// <returns>
		/// A <see cref="Tween"/>.
		/// </returns>
		public static Tween CreateRendererColorTween(Renderer renderer, string colorPropertyName,
													 ColorTweenSettings colorTweenSettings,
													 int subMeshIndex = 0,
													 int numberOfLoops = 0,
													 LoopType loopType = LoopType.Restart)
		{
			return CreateRendererColorTween(renderer, Shader.PropertyToID(colorPropertyName), colorTweenSettings,
											subMeshIndex, numberOfLoops, loopType);
		}

		/// <summary>
		/// Creates a tween that changes the color of the passed renderer to the target value.
		/// </summary>
		/// <param name="renderer">Renderer to fade.</param>
		/// <param name="colorPropertyId">Property id of the renderer's color to be changed.</param>
		/// <param name="colorTweenSettings">Settings for the tween used to change the renderer's color.</param>
		/// <param name="subMeshIndex">Index of the sub mesh.</param>
		/// <param name="loopType">Number of loops.</param>
		/// <param name="numberOfLoops">Loop type.</param>
		/// <returns>
		/// A <see cref="Tween"/>.
		/// </returns>
		public static Tween CreateRendererColorTween(Renderer renderer, int colorPropertyId,
													 ColorTweenSettings colorTweenSettings,
													 int subMeshIndex = 0,
													 int numberOfLoops = 0,
													 LoopType loopType = LoopType.Restart)
		{
			renderer.GetPropertyBlock(materialPropertyBlock, subMeshIndex);

			Color startColor = Color.white;

			if (materialPropertyBlock.isEmpty)
			{
				startColor = renderer.sharedMaterials[subMeshIndex].GetColor(colorPropertyId);

				materialPropertyBlock.SetColor(colorPropertyId, startColor);

				renderer.SetPropertyBlock(materialPropertyBlock, subMeshIndex);
			}
			else
			{
				startColor = materialPropertyBlock.GetColor(colorPropertyId);
			}

			Color endColor = colorTweenSettings.TargetColor;

			var colorLerpValue = 0f;

			Tween colorTween = DOTween.To(() => colorLerpValue, newLerpValue => colorLerpValue = newLerpValue,
								   1f, colorTweenSettings.Duration).SetLoops(numberOfLoops, loopType);

			colorTween.OnUpdate(SetRendererColor);
			colorTween.OnComplete(SetRendererColor);

			void SetRendererColor()
			{
				renderer.GetPropertyBlock(materialPropertyBlock, subMeshIndex);
				materialPropertyBlock.SetColor(colorPropertyId, Color.Lerp(startColor, endColor, colorLerpValue));
				renderer.SetPropertyBlock(materialPropertyBlock, subMeshIndex);
			}

			colorTween.SetDelay(colorTweenSettings.Delay);

			if (colorTweenSettings.UseCustomEaseCurve)
			{
				colorTween.SetEase(colorTweenSettings.CustomEaseCurve);
			}
			else
			{
				colorTween.SetEase(colorTweenSettings.EaseFunction);
			}

			return colorTween;
		}


		#endregion

		#region Alpha

		/// <summary>
		/// Creates a tween that fades the alpha of the passed renderer to the target value.
		/// </summary>
		/// <param name="renderer">Renderer to fade.</param>
		/// <param name="colorPropertyName">Name of the renderer's color to be faded.</param>
		/// <param name="alphaTweenSettings">Settings for the tween used to fade the renderer.</param>
		/// <returns>
		/// A <see cref="Tween"/>.
		/// </returns>
		public static Tween CreateRendererAlphaTween(Renderer renderer, string colorPropertyName,
													 FloatTweenSettings alphaTweenSettings)
		{
			return CreateRendererAlphaTween(renderer, Shader.PropertyToID(colorPropertyName), alphaTweenSettings);
		}

		/// <summary>
		/// Creates a tween that fades the alpha of the passed renderer to the target value.
		/// </summary>
		/// <param name="renderer">Renderer to fade.</param>
		/// <param name="colorPropertyId">Property id of the renderer's color to be faded.</param>
		/// <param name="alphaTweenSettings">Settings for the tween used to fade the renderer.</param>
		/// <returns>
		/// A <see cref="Tween"/>.
		/// </returns>
		public static Tween CreateRendererAlphaTween(Renderer renderer, int colorPropertyId, 
													 FloatTweenSettings alphaTweenSettings)
		{
			renderer.GetPropertyBlock(materialPropertyBlock);

			float currentAlpha;						

			if (materialPropertyBlock.isEmpty)
			{
				Color materialColor = renderer.sharedMaterial.GetColor(colorPropertyId);

				materialPropertyBlock.SetColor(colorPropertyId, materialColor);
				currentAlpha = materialColor.a;

				renderer.SetPropertyBlock(materialPropertyBlock);
			}
			else
			{
				currentAlpha = materialPropertyBlock.GetColor(colorPropertyId).a;
			}

			float endAlpha = alphaTweenSettings.TargetValue;

			Tween alphaTween = DOTween.To(() => currentAlpha, newAlpha => currentAlpha = newAlpha,
								   endAlpha, alphaTweenSettings.Duration);

			alphaTween.OnUpdate(SetRendererAlpha);
			alphaTween.OnComplete(SetRendererAlpha);

			void SetRendererAlpha()
			{
				renderer.GetPropertyBlock(materialPropertyBlock);
				materialPropertyBlock.SetColorAlpha(colorPropertyId, currentAlpha);
				renderer.SetPropertyBlock(materialPropertyBlock);
			}

			alphaTween.SetDelay(alphaTweenSettings.Delay);

			if (alphaTweenSettings.UseCustomEaseCurve)
			{
				alphaTween.SetEase(alphaTweenSettings.CustomEaseCurve);
			}
			else
			{
				alphaTween.SetEase(alphaTweenSettings.EaseFunction);
			}

			return alphaTween;
		}

		#endregion
	}
}