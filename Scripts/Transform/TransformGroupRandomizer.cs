using Sirenix.OdinInspector;
using System.Collections.Generic;
using Unidork.Attributes;
using Unidork.Extensions;
using UnityEngine;

namespace Unidork.TransformUtility
{
	using Transform = UnityEngine.Transform;

	/// <summary>
	/// Performs randomization operations on a group of transforms.
	/// </summary>
	public class TransformGroupRandomizer : MonoBehaviour
    {
		#region Enums

		/// <summary>
		/// The way transforms are acquired by this component.
		/// <para>ManualAssignment - transforms are manually assigned in the inspector.</para>
		/// <para>FromHierarchy - transforms are grabbed from hierarchy.</para>
		/// </summary>		
		private enum TransformGrabType
		{
			FromHierarchy,
			ManualAssignment			
		}

		/// <summary>
		/// Way this component is triggered.
		/// <para>DontTriggerAtRuntime - randomization will not be triggered at runtime.
		/// Used when transforms are randomized in the editor.</para>
		/// <para>Start - component is triggered in Unity's Start method.</para>
		/// <para>ExternalCall - component is triggered via an external call, either directly or
		/// using an event listener.</para>
		/// </summary>
		private enum TriggerType
		{
			DontTriggerAtRuntime,
			Start,
			ExternalCall
		}

		/// <summary>
		/// Type of transform to use when randomizing.
		/// <para>UnityTransform - will use Unity's transform component to grab transforms.</para>
		/// <para>RandomizableTransforms = will only search for objects that include 
		/// a <see cref="TransformUtility.RandomizableTransform"/> component.</para>
		/// </summary>		
		private enum TransformType
		{
			UnityTransform,
			RandomizableTransform
		}

		#endregion

		#region Fields

		/// <summary>
		/// The way transforms are acquired by this component.
		/// <para>ManualAssignment - transforms are manually assigned in the inspector.</para>
		/// <para>FromHierarchy - transforms are grabbed from hierarchy.</para>
		/// </summary>
		[Space, SettingsHeader, Space]
		[Tooltip("The way transforms are acquired by this component.\n\n" +
				 "ManualAssignment - transforms are manually assigned in the inspector.\n\n" +
				 "FromHierarchy - transforms are grabbed from hierarchy.")]
		[SerializeField]
		private TransformGrabType transformGrabType = TransformGrabType.FromHierarchy;

		/// <summary>
		/// Way this component is triggered.
		/// <para>Start - component is triggered in Unity's Start method.</para>
		/// <para>ExternalCall - component is triggered via an external call, either directly or
		/// using an event listener.</para>
		/// </summary>
		[Tooltip("Way this component is triggered.\n\n" +
				 "Start - component is triggered in Unity's Start method.\n\n" +
				 "ExternalCall - component is triggered via an external call, either directly or "+
				 "using an event listener.")]
		[SerializeField]
		private TriggerType triggerType = TriggerType.Start;

		/// <summary>
		/// Type of transform to use when randomizing.
		/// <para>UnityTransform - will use Unity's transform component to grab transforms.</para>
		/// <para>RandomizableTransforms = will only search for objects that include 
		/// a <see cref="RandomizableTransform"/> component.</para>
		/// </summary>	
		[Tooltip("Type of transform to use when randomizing.\n\n" +
				 "UnityTransform - will use Unity's transform component to grab transforms.\n\n" +
				 "")]
		[SerializeField]
		private TransformType transformType = TransformType.UnityTransform;

		/// <summary>
		/// Transforms randomized by this component.
		/// </summary>
		[ShowIf("@this.transformGrabType", TransformGrabType.ManualAssignment)]
		[HideIf("@this.transformType", TransformType.RandomizableTransform)]		
		[Tooltip("Transforms randomized by this component.")]
		[SerializeField]
		private List<Transform> transforms = null;

		/// <summary>
		/// Randomizable transforms randomized by this component.
		/// </summary>
		[ShowIf("@this.transformGrabType", TransformGrabType.ManualAssignment)]
		[HideIf("@this.transformType", TransformType.UnityTransform)]
		[Tooltip("Randomizable transforms randomized by this component.")]
		[SerializeField]
		private List<RandomizableTransform> randomizableTransforms = null;

		/// <summary>
		/// Should transform rotations be randomized?
		/// </summary>
		[Space, Title("ROTATION", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false),
		Space]
		[Tooltip("Should transform rotations be randomized?")]
		[SerializeField]
		private bool randomizeRotation = false;

		/// <summary>
		/// Should transform x rotations be randomized?
		/// </summary>
		[ShowIf("@this.randomizeRotation == true")]
		[Tooltip("Should transform x rotations be randomized?")]
		[SerializeField]
		private bool randomizeXRotation = false;

		/// <summary>
		/// Rotation range to randomly pick rotation value around the x axis.
		/// </summary>
		[ShowIf("@this.randomizeRotation == true")]
		[HideIf("@this.randomizeXRotation == false")]
		[Tooltip("Rotation range to randomly pick rotation value around the x axis.")]
		[MinMaxSlider(0f, 360f)]
		[SerializeField]
		private Vector2 xRotationRange = new Vector2(0f, 360f);

		/// <summary>
		/// Should transform y rotations be randomized?
		/// </summary>
		[ShowIf("@this.randomizeRotation == true")]
		[Tooltip("Should transform y rotations be randomized?")]
		[SerializeField]
		private bool randomizeYRotation = false;

		/// <summary>
		/// Rotation range to randomly pick rotation value around the y axis.
		/// </summary>
		[ShowIf("@this.randomizeRotation == true")]
		[HideIf("@this.randomizeYRotation == false")]
		[Tooltip("Rotation range to randomly pick rotation value around the y axis.")]
		[MinMaxSlider(0f, 360f)]
		[SerializeField]
		private Vector2 yRotationRange = new Vector2(0f, 360f);

		/// <summary>
		/// Should transform z rotations be randomized?
		/// </summary>
		[ShowIf("@this.randomizeRotation == true")]
		[Tooltip("Should transform z rotations be randomized?")]
		[SerializeField]
		private bool randomizeZRotation = false;

		/// <summary>
		/// Rotation range to randomly pick rotation value around the z axis.
		/// </summary>
		[ShowIf("@this.randomizeRotation == true")]
		[HideIf("@this.randomizeZRotation == false")]
		[Tooltip("Rotation range to randomly pick rotation value around the z axis.")]
		[MinMaxSlider(0f, 360f)]
		[SerializeField]
		private Vector2 zRotationRange = new Vector2(0f, 360f);

		/// <summary>
		/// Should transform scales be randomized?
		/// </summary>
		[Space, Title("SCALE", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false),
		Space]
		[Tooltip("Should transform scales be randomized?")]
		[SerializeField]		
		private bool randomizeScale = false;

		/// <summary>
		/// Should scale be randomized uniformly, as opposed to separately on each axis?
		/// </summary>
		[ShowIf("@this.randomizeScale == true")]
		[Tooltip("Should scale be randomized uniformly, as opposed to separately on each axis?")]
		[SerializeField]
		private bool scaleUniformly = true;

		/// <summary>
		/// Scale range to randomly pick a uniform scale value from.
		/// </summary>
		[ShowIf("@this.randomizeScale == true")]
		[HideIf("@this.scaleUniformly == false")]
		[Tooltip("Scale range to randomly pick a uniform scale value from.")]
		[SerializeField]
		private Vector2 uniformScaleRange = new Vector2(0.1f, 100f);

		#endregion

		#region Editor

		[Button("RANDOMIZE", ButtonSizes.Large), GUIColor(0f, 1f, 0f)]
		private void RandomizeInEditor()
		{
			Awake();
			Randomize();
		}

		#endregion

		#region Init

		private void Awake()
		{
			if (transformGrabType == TransformGrabType.ManualAssignment)
			{
				return;
			}

			if (transformType == TransformType.UnityTransform)
			{
				transforms = transform.GetAllChildren();
			} 
			else
			{
				randomizableTransforms =
					this.GetComponentsInChildrenNonAlloc<RandomizableTransform>(ignoreParent: true);
			}			
		}

		private void Start()
		{
			if (triggerType != TriggerType.Start)
			{
				return;
			}

			Randomize();
		}

		#endregion

		#region Randomize

		/// <summary>
		/// Randomizes transforms according to the settings in the inspector.
		/// </summary>
		public void Randomize()
		{
			List<Transform> transformsToRandomize = GetTransformsToRandomize();

			if (transformsToRandomize.IsNullOrEmpty())
			{
				Debug.LogError($"Transfrom randomizer doesn't have any transforms assigned!", this);
				return;
			}

			RandomizeRotation(transformsToRandomize);
			RandomizeScale(transformsToRandomize);
		}

		/// <summary>
		/// Gets transforms to randomize based on which type of transforms 
		/// is set in <see cref="transformType"/>.
		/// </summary>
		/// <returns>A list of <see cref="Transform"/></returns>.
		private List<Transform> GetTransformsToRandomize()
		{
			if (transformType == TransformType.UnityTransform)
			{
				return transforms;
			}

			if (randomizableTransforms.IsNullOrEmpty())
			{
				return null;
			}

			List<Transform> transformsToRandomize = new List<Transform>();

			foreach (RandomizableTransform randomizableTransform in randomizableTransforms)
			{
				transformsToRandomize.Add(randomizableTransform.transform);
			}

			return transformsToRandomize;
		}

		/// <summary>
		/// Randomizes transform rotations.
		/// </summary>
		private void RandomizeRotation(List<Transform> transformsToRandomize)
		{
			if (!randomizeRotation)
			{
				return;
			}

			foreach (Transform transform in transformsToRandomize)
			{
				Vector3 randomRotation = Vector3.zero;

				if (randomizeXRotation)
				{
					randomRotation.x = xRotationRange.GetRandomBetweenXAndY();
				}

				if (randomizeYRotation)
				{
					randomRotation.y = yRotationRange.GetRandomBetweenXAndY();
				}

				if (randomizeZRotation)
				{
					randomRotation.z = zRotationRange.GetRandomBetweenXAndY();
				}

				transform.rotation = Quaternion.Euler(randomRotation);
			}
		}
		
		/// <summary>
		/// Randomizes transform scales.
		/// </summary>
		private void RandomizeScale(List<Transform> transformsToRandomize)
		{
			if (!randomizeScale)
			{
				return;
			}

			foreach (Transform transform in transformsToRandomize)
			{
				if (scaleUniformly)
				{
					transform.localScale = Vector3.one * uniformScaleRange.GetRandomBetweenXAndY();
				}
			}
		}

		#endregion
	}
}