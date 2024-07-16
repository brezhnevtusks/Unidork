using System.Collections.Generic;
using Unidork.Extensions;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Unidork.UI
{
	/// <summary>
	/// Utility methods for Unity's GUI system.
	/// </summary>
	public static class UIUtility
	{
		#region Pointer

		/// <summary>
		/// Checks whether pointer is over a UI element.
		/// </summary>
		/// <returns>
		/// True if the pointer is over a UI element, False otherwise.
		/// </returns>
		public static bool IsPointerOverUIElement()
		{
			return IsPositionOverUIElement(GetRaycastResultsAtPointerPosition());
		}

		public static bool IsPositionOverUIElement(Vector3 screenPosition)
		{
			return IsPositionOverUIElement(GetRaycastResultsAtPosition(screenPosition));
		}

		/// <summary>
		/// Checks whether pointer is over a UI element. A layer mask is passed to be able to check against specific layers.
		/// </summary>
		/// <param name="uiLayerMask">UI layer mask.</param>
		/// <returns>
		///	True if the pointer is over a UI element, False otherwise.
		/// </returns>
		public static bool IsPointerOverUIElement(LayerMask uiLayerMask)
		{
			return IsPositionOverUIElement(GetRaycastResultsAtPointerPosition(), uiLayerMask);
		}

		/// <summary>
		/// Checks whether pointer is over a UI element.
		/// </summary>
		/// <param name="raycastResults">Raycast results at pointer position.</param>
		/// <returns></returns>
		private static bool IsPositionOverUIElement(List<RaycastResult> raycastResults)
		{
			int uiLayer = LayerMask.NameToLayer("UI");
			
			foreach (var curRaycastResult in raycastResults)
			{
				if (curRaycastResult.gameObject.layer != uiLayer)
				{
					continue;
				}
				
				return true;
			}

			return false;
		}
		
		/// <summary>
		/// Checks whether pointer is over a UI element.A layer mask is passed to be able to check against specific layers.
		/// </summary>
		/// <param name="raycastResults">Raycast results at pointer position.</param>
		/// <param name="uiLayerMask">UI layer mask.</param>
		/// <returns></returns>
		private static bool IsPositionOverUIElement(List<RaycastResult> raycastResults, LayerMask uiLayerMask)
		{
			foreach (var curRaycastResult in raycastResults)
			{
				if (!uiLayerMask.HasLayer(curRaycastResult.gameObject.layer))
				{
					continue;
				}
				
				return true;
			}

			return false;
		}
		
		/// <summary>
		/// Gets graphical raycast results at current pointer position.
		/// </summary>
		/// <returns>A list of <see cref="RaycastResult"/>.</returns>
		static List<RaycastResult> GetRaycastResultsAtPointerPosition()
		{
			return GetRaycastResultsAtPosition(Input.mousePosition);
		}

		static List<RaycastResult> GetRaycastResultsAtPosition(Vector3 screenPosition)
		{
			PointerEventData eventData = new PointerEventData(EventSystem.current)
			{
				position = screenPosition
			};
			
			var raycastResults = new List<RaycastResult>();
			EventSystem.current.RaycastAll(eventData, raycastResults);
			return raycastResults;
		}

		#endregion
	}
}