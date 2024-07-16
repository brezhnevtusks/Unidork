using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using MoreMountains.Feedbacks;
using Unidork.Attributes;
using UnityEngine;
	
namespace Unidork.Feedbacks
{
	/// <summary>
	/// Feedback that sets a game object's layer to a specific value.
	/// </summary>
	[System.Serializable]
	[UsedImplicitly]
	[FeedbackPath("GameObject/Set Layer")]
	[FeedbackHelp("Sets a game object's layer to a specific value.")]
	public class SetLayerFeedback : MMF_Feedback
	{
		#region Properties      

#if UNITY_EDITOR

		/// <inheritdoc/>
		public override Color FeedbackColor => FeedbacksEditorColors.SetLayerFeedbackColor;

#endif
		#endregion

		#region Fields

		/// <summary>
		/// Game object to set layer on.
		/// </summary>
		[MMFInspectorGroup("Set Layer", true)]

		[Tooltip("Game object to set layer on.")]
		[SerializeField]
		private GameObject targetGameObject = null;
		
		/// <summary>
		/// Layer to set.
		/// </summary>
		[SingleLayer]
		[Tooltip("Layer to set.")]
		[SerializeField]
		private LayerMask layerMask = default;

		#endregion

		#region Play

		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1)
		{
			if (!Active)
			{
				return;
			}
			
			targetGameObject.layer = layerMask;
		}

		#endregion

#if UNITY_EDITOR
		
		#region Editor

		[UsedImplicitly]
		private IEnumerable GetAllLayerMaskValues()
		{
			var sequence = new List<LayerMask>();

			for (int i = 0; i < 32; i++)
			{
				sequence.Add((LayerMask)i);
			}

			return sequence;
		}

		#endregion

#endif
	}
}