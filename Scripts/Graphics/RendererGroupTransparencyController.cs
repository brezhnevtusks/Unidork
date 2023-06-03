#if UNITY_EDITOR
using DG.DOTweenEditor;
#endif
using DG.Tweening;
using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unidork.Attributes;
using Unidork.Extensions;
using Unidork.Tweens;
using UnityEngine;

namespace Unidork.Graphics
{
	/// <summary>
	/// Controls the transparency of a group of renderers that share a transparency value.
	/// </summary>
	public class RendererGroupTransparencyController : MonoBehaviour
    {
		#region Enums

		/// <summary>
		/// Enum that defines how meshes are acquired by this controller.
		/// <para>AllMeshesInHierarchy - all meshes in the hierarchy of the object are grabbed.</para>
		/// <para>ManualAssignment - meshes are assigned manually.</para>
		/// </summary>
		private enum MeshGrabType
		{
			AllMeshesInHierarchy,
			ManualAssignment
		}

		#endregion

		#region Fields

		/// <summary>
		/// Defines how meshes are acquired by this controller.
		/// </summary>
		[Space, SettingsHeader, Space]
		[Tooltip("Defines how meshes are acquired by this controller.")]
		[SerializeField]
		private MeshGrabType meshGrabType = default;

		/// <summary>
		/// Meshes whose transparency this component controls.
		/// </summary>
		[ShowIf("@this.meshGrabType", MeshGrabType.ManualAssignment)]
		[Tooltip("Meshes whose transparency this component controls.")]
		[SerializeField]
		private List<Renderer> renderers = null;

		/// <summary>
		/// Default tween settings to use for fully changing the transparency of a
		/// renderer from 0 to 1 or vice versa.
		/// </summary>
		[Tooltip("Default tween settings to use for fully changing the transparency of a" +
				 "renderer from 0 to 1 or vice versa.")]
		[SerializeField]
		private TweenSettings transparencyTweenSettings = TweenSettings.CreateLinearTweenSettings();

		/// <summary>
		/// Name of the property that stores a renderer's main color.
		/// </summary>
		[Tooltip("Name of the property that stores a renderer's main color.")]
		[SerializeField]
		private string mainColorPropertyName = "_BaseColor";

		/// <summary>
		/// Should renderers be faded out on start?
		/// </summary>
		[Tooltip("Should renderers be faded out on start?")]
		[SerializeField]
		private bool fadeOutOnStart = false;

		/// <summary>
		/// Material property block to use for setting transparency on renderers.
		/// </summary>
		private static MaterialPropertyBlock materialPropertyBlock;

		/// <summary>
		/// Active transparency tweens.
		/// </summary>
		private readonly List<Tween> activeTweens = new List<Tween>();		

		#endregion

		#region Init

		private void Awake()
		{
			if (meshGrabType == MeshGrabType.ManualAssignment)
			{
				return;
			}

			renderers = this.GetComponentsInChildrenNonAlloc<Renderer>();
		}

		private void Start()
		{
			TweenSettings instantTweenSettings = TweenSettings.CreateInstantTweenSettings();

			if (fadeOutOnStart)
			{
				FadeOutRenderers(overrideTweenSettings: instantTweenSettings);
			}
			else
			{
				FadeInRenderers(overrideTweenSettings: instantTweenSettings);
			}
		}

		#endregion

		#region Transparency

		/// <summary>
		/// Fully fades out all renderers.
		/// </summary>
		public void FadeOutRenderers() => FadeOutRenderers(transparencyTweenSettings);

		/// <summary>
		/// Fully fades out all renderers.
		/// </summary>
		/// <param name="overrideTweenSettings">Optional tween data to override
		/// default controller settings.</param>
		/// <param name="useMaterialBlock">Should a material block be used?</param>
		/// <param name="overrideMainColorPropertyName">Override main color shader property name.</param>
		public void FadeOutRenderers(TweenSettings overrideTweenSettings = null, bool useMaterialBlock = true,
									 string overrideMainColorPropertyName = "")
		{
			SetTransparency(0f, overrideTweenSettings, useMaterialBlock, overrideMainColorPropertyName);
		}

		/// <summary>
		/// Fully fades in all renderers.
		/// </summary>
		public void FadeInRenderers() => FadeInRenderers(transparencyTweenSettings);

		/// <summary>
		/// Fully fades in all renderers.
		/// </summary>
		/// <param name="overrideTweenSettings">Optional tween data to override
		/// default controller settings.</param>
		/// /// <param name="useMaterialBlock">Should a material block be used?</param>
		/// /// <param name="overrideMainColorPropertyName">Override main color shader property name.</param>
		public void FadeInRenderers(TweenSettings overrideTweenSettings = null, bool useMaterialBlock = true,
								 string overrideMainColorPropertyName = "")
		{
			SetTransparency(1f, overrideTweenSettings, useMaterialBlock, overrideMainColorPropertyName);
		}

		/// <summary>
		/// Sets the transparency of renderers to the passed value.
		/// </summary>
		/// <param name="targetTransparency">Target transparency value.</param>
		/// <param name="overrideTweenSettings">Optional tween data to override
		/// default controller settings.</param>
		public void SetTransparency(float targetTransparency, TweenSettings overrideTweenSettings = null,
									bool useMaterialBlock = true,
									string overrideMainColorPropertyName = "")
		{
			if (renderers.IsNullOrEmpty())
			{
				Debug.LogWarning($"RendererTransparencyController doesn't have any renderers!", this);
				return;
			}

			TweenSettings tweenSettings = overrideTweenSettings ?? transparencyTweenSettings;

			string mainColorPropertyName = overrideMainColorPropertyName.IsEmpty() ?
										  this.mainColorPropertyName :
										  overrideMainColorPropertyName;

			if (useMaterialBlock)
			{
				SetTransparencyUsingMaterialBlock(targetTransparency, tweenSettings, mainColorPropertyName);
			}
			else
			{
				SetTransparencyUsingMaterial(targetTransparency, tweenSettings);
			}
		}

		/// <summary>
		/// Sets the transparency of renderers to the passed value using a material property block.
		/// </summary>
		/// <param name="targetTransparency">Target transparency value.</param>
		/// <param name="tweenSettings">Tween settings to use for setting the transparency of the renderers.</param>
		/// <param name="mainColorPropertyName">Name of the main color shader property.</param>
		private void SetTransparencyUsingMaterialBlock(float targetTransparency, TweenSettings tweenSettings,
													   string mainColorPropertyName)
		{
			if (materialPropertyBlock == null)
			{
				materialPropertyBlock = new MaterialPropertyBlock();
			}

			Color color = renderers.First().sharedMaterial.GetColor(mainColorPropertyName);

			float currentTransparency = color.a;
			float tweenDuration = GetTweenDuration(tweenSettings.Duration, currentTransparency, targetTransparency);	

			foreach (Renderer renderer in renderers)
			{
				renderer.DOKill();

				float transparency = currentTransparency;

				Tween transparencyTween = DOTween.To(() => transparency,
										  newTransparency => transparency = newTransparency,
										  targetTransparency, tweenDuration);

				transparencyTween.SetEase(tweenSettings.EaseFunction);
				transparencyTween.SetDelay(tweenSettings.Delay);

				transparencyTween.OnUpdate(() =>
				{
					renderer.GetPropertyBlock(materialPropertyBlock);

					color.a = transparency;
					materialPropertyBlock.SetColor(mainColorPropertyName, color);

					renderer.SetPropertyBlock(materialPropertyBlock);
				});

				transparencyTween.OnComplete(() =>
				{					
					renderer.GetPropertyBlock(materialPropertyBlock);

					Color currentColor = materialPropertyBlock.GetColor(mainColorPropertyName);
					currentColor.a = transparency;
					materialPropertyBlock.SetColor(mainColorPropertyName, currentColor);

					renderer.SetPropertyBlock(materialPropertyBlock);

					_ = activeTweens.Remove(transparencyTween);
				});

				transparencyTween.OnKill(() =>
				{
					_ = activeTweens.Remove(transparencyTween);
				});

				activeTweens.Add(transparencyTween);

#if UNITY_EDITOR

				if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
				{
					DOTweenEditorPreview.PrepareTweenForPreview(transparencyTween, clearCallbacks: false);
				}

				continue;
#endif
			}
		}

		/// <summary>
		/// Sets the transparency of renderers to the passed value using the renderer's material.
		/// </summary>
		/// <param name="transparencyValue">Target transparency value.</param>
		/// <param name="tweenSettings">Tween settings to use for setting the transparency of the renderers.</param>
		private void SetTransparencyUsingMaterial (float transparencyValue, TweenSettings tweenSettings)
		{
			foreach (Renderer renderer in renderers)
			{
				renderer.DOKill();

				Tween transparencyTween = renderer.material.DOFade(transparencyValue, tweenSettings.Duration);
				transparencyTween.SetEase(tweenSettings.EaseFunction);

				transparencyTween.SetDelay(tweenSettings.Delay);

				transparencyTween.OnKill(() =>
				{
					activeTweens.Remove(transparencyTween);
				});

				activeTweens.Add(transparencyTween);
			}
		}

		/// <summary>
		/// Gets the actual duration of a tween based on default duration and target transparency.
		/// </summary>
		/// <param name="defaultDuration">Duration of a default tween that performs a full fade in/out.</param>
		/// <param name="currentAlpha">Current transparency value.</param>
		/// <param name="targetAlpha">Target transparency value.</param>
		/// <returns>
		/// A float representing the actual duration of a transparency tween.
		/// </returns>
		private float GetTweenDuration(float defaultDuration, float currentAlpha, float targetAlpha)
		{
			float durationFactor = Mathf.Abs(targetAlpha - currentAlpha);
			return defaultDuration * durationFactor;
		}

		#endregion

		#region Pause

		/// <summary>
		/// Pauses all active transparency tweens.
		/// </summary>
		public void PauseAllTweens()
		{
			if (activeTweens.IsEmpty())
			{
				return;
			}

			foreach (Tween activeTween in activeTweens)
			{
				activeTween.Pause();
			}
		}

		/// <summary>
		/// Unpauses all active transparency tweens.
		/// </summary>
		public void UnpauseAllTweens()
		{
			if (activeTweens.IsEmpty())
			{
				return;
			}

			foreach (Tween activeTween in activeTweens)
			{
				activeTween.Play();
			}
		}

		#endregion

		#region Stop

		/// <summary>
		/// Stops all active transparency tweens.
		/// </summary>
		public void StopAllTweens()
		{
			if (activeTweens.IsEmpty())
			{
				return;
			}

			foreach (Tween activeTween in activeTweens)
			{
				activeTween.Kill();
			}
		}

		#endregion

#if UNITY_EDITOR

		#region Editor

		[ButtonGroup]
		[Button("Fade In Renderers", ButtonSizes.Large)]
		[UsedImplicitly]
		private void FadeInRenderersInEditor()
		{
			if (meshGrabType == MeshGrabType.AllMeshesInHierarchy)
			{
				renderers = this.GetComponentsInChildrenNonAlloc<Renderer>();
			}
			
			FadeInRenderers(transparencyTweenSettings);

			DOTweenEditorPreview.Start();
		}

		[ButtonGroup]		
		[Button("Fade Out Renderers", ButtonSizes.Large)]
		[UsedImplicitly]
		private void FadeOutRendererInEditor()
		{
			if (meshGrabType == MeshGrabType.AllMeshesInHierarchy)
			{
				renderers = this.GetComponentsInChildrenNonAlloc<Renderer>();
			}

			FadeOutRenderers(transparencyTweenSettings);

			DOTweenEditorPreview.Start();
		}

		#endregion

#endif
	}
}