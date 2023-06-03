using Sirenix.OdinInspector;
using Unidork.Attributes;
using UnityEngine;

namespace Unidork.Utility
{
	/// <summary>
	/// Copies transform settings from one transform to transform that this component is atttached to.
	/// </summary>
	public class TransformSettingsCopier : MonoBehaviour
	{
		#region Fields

		/// <summary>
		/// Transform to copy settings from.
		/// </summary>
		[Space, SettingsHeader, Space]
		[Tooltip("Transform to copy settings from.")]
		[SerializeField]
		private Transform transformToCopy = null;		

		/// <summary>
		/// Should position be copied?
		/// </summary>
		[Tooltip("Should position be copied?")]
		[SerializeField]
		private bool copyPosition = false;

		/// <summary>
		/// Offset for transform's position settings.
		/// </summary>
		[Space, Title("POSITION", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false), Space]
		[ShowIf("@this.copyPosition == true")]
		[Tooltip("Offset for transform's position settings.")]
		[SerializeField]
		private Vector3 positionOffset = Vector3.zero;

		/// <summary>
		/// Should the position offset be added in object's local space?
		/// </summary>
		[Tooltip("Should the position offset be added in object's local space?")]
		[SerializeField]
		private bool offsetIsLocal = false;

		/// <summary>
		/// Should x coordinate be ignored?
		/// </summary>	
		[ShowIf("@this.copyPosition == true")]
		[Tooltip("Should x coordinate be ignored?")]
		[SerializeField]
		private bool ignoreXCoordinate = false;

		/// <summary>
		/// Should x coordinate be inverted?
		/// </summary>
		[ShowIf("@this.copyPosition == true")]
		[HideIf("@this.ignoreXCoordinate == true")]
		[Tooltip("Should x coordinate be inverted?")]
		[SerializeField]
		private bool invertXCoordinate = false;

		/// <summary>
		/// Should y coordinate be ignored?
		/// </summary>
		[ShowIf("@this.copyPosition == true")]
		[Tooltip("Should y coordinate be ignored?")]
		[SerializeField]
		private bool ignoreYCoordinate = false;

		/// <summary>
		/// Should y coordinate be inverted?
		/// </summary>
		[ShowIf("@this.copyPosition == true")]
		[HideIf("@this.ignoreYCoordinate == true")]
		[Tooltip("Should y coordinate be inverted?")]
		[SerializeField]
		private bool invertYCoordinate = false;

		/// <summary>
		/// Should z coordinate be ignored?
		/// </summary>
		[ShowIf("@this.copyPosition == true")]
		[Tooltip("Should z coordinate be ignored?")]
		[SerializeField]
		private bool ignoreZCoordinate = false;

		/// <summary>
		/// Should z coordinate be inverted?
		/// </summary>
		[ShowIf("@this.copyPosition == true")]
		[HideIf("@this.ignoreZCoordinate == true")]
		[Tooltip("Should z coordinate be inverted?")]
		[SerializeField]
		private bool invertZCoordinate = false;

		/// <summary>
		/// Should rotation be copied?
		/// </summary>
		[Space, Space]
		[Tooltip("Should rotation be copied?")]
		[SerializeField]
		private bool copyRotation = false;

		/// <summary>
		/// Should rotation around the x axis be inverted?
		/// </summary>
		[Space, Title("ROTATION", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false), Space]
		[ShowIf("@this.copyRotation == true")]
		[Tooltip("Should rotation around the x axis be inverted?")]
		[SerializeField]
		private bool invertXRotation = false;

		/// <summary>
		/// Should rotation around the y axis be inverted?
		/// </summary>		
		[ShowIf("@this.copyRotation == true")]
		[Tooltip("Should rotation around the y axis be inverted?")]
		[SerializeField]
		private bool invertYRotation = false;

		/// <summary>
		/// Should rotation around the z axis be inverted?
		/// </summary>
		[ShowIf("@this.copyRotation == true")]
		[Tooltip("Should rotation around the z axis be inverted?")]
		[SerializeField]
		private bool invertZRotation = false;

		/// <summary>
		/// Should scale be copied?
		/// </summary>
		[Space, Space]
		[Tooltip("Should scale be copied?")]
		[SerializeField]
		private bool copyScale = false;

		/// <summary>
		/// Should x scale be ignored?
		/// </summary>
		[ShowIf("@this.copyScale == true")]
		[Space, Title("SCALE", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false), Space]
		[SerializeField]
		private bool ignoreXScale = false;

		/// <summary>
		/// Should y scale be ignored?
		/// </summary>
		[ShowIf("@this.copyScale == true")]
		[Tooltip("Should y scale be ignored?")]
		[SerializeField]
		private bool ignoreYScale = false;

		/// <summary>
		/// Should z scale be ignored?
		/// </summary>
		[ShowIf("@this.copyScale == true")]
		[Tooltip("Should z scale be ignored?")]
		[SerializeField]
		private bool ignoreZScale = false;

		#endregion

		#region Init

		private void Awake() => Update();

		#endregion

		#region Follow

		private void Update()
		{
			CopyPosition();
			CopyRotation();
			CopyScale();
		}

		/// <summary>
		/// Copies other transform's position if <see cref="copyPosition"/> is True.
		/// </summary>
		private void CopyPosition()
		{
			if (!copyPosition)
			{
				return;
			}

			Vector3 currentPosition = transform.position;

			Vector3 targetPosition = transformToCopy.position;
			
			if (positionOffset != Vector3.zero)
			{
				if (offsetIsLocal)
				{
					targetPosition += transform.right * positionOffset.x;
					targetPosition.y += positionOffset.y;
					targetPosition += transform.forward * positionOffset.z;
				}
				else
				{
					targetPosition += positionOffset;
				}
			}

			if (ignoreXCoordinate)
			{
				targetPosition.x = currentPosition.x;
			}
			else if (invertXCoordinate)
			{
				targetPosition.x *= -1f;
			}

			if (ignoreYCoordinate)
			{
				targetPosition.y = currentPosition.y;
			}
			else if (invertYCoordinate)
			{
				targetPosition.y *= -1;
			}			

			if (ignoreZCoordinate)
			{
				targetPosition.z = currentPosition.z;
			}
			else if (invertZCoordinate)
			{
				targetPosition.z *= -1f;
			}			

			transform.position = targetPosition;
		}

		/// <summary>
		/// Copies other transform's rotation if <see cref="copyRotation"/> is True.
		/// </summary>
		private void CopyRotation()
		{
			if (!copyRotation) 
			{ 
				return;
			}

			Vector3 targetRotation = transformToCopy.eulerAngles;

			if (invertXRotation)
			{
				targetRotation.x *= -1f;
			}

			if (invertYRotation)
			{
				targetRotation.y *= -1f;
			}

			if (invertZRotation)
			{
				targetRotation.z *= -1f;
			}

			transform.rotation = Quaternion.Euler(targetRotation);
		}

		/// <summary>
		/// Copies other transform's scale if <see cref="copyScale"/> is set to True.
		/// </summary>
		private void CopyScale()
		{
			if (!copyScale)
			{
				return;
			}

			Vector3 currentScale = transform.localScale;
			Vector3 targetScale = transformToCopy.localScale;

			if (ignoreXScale)
			{
				targetScale.x = currentScale.x;
			}

			if (ignoreYScale)
			{
				targetScale.y = currentScale.y;
			}

			if (ignoreZScale)
			{
				targetScale.z = currentScale.z;
			}

			transform.localScale = targetScale;
		}

		#endregion
	}
}