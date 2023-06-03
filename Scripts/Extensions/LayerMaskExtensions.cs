using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Unidork.Extensions
{
	public static class LayerMaskExtensions
	{
		/// <summary>
		/// Adds a layer to a layer mask.
		/// </summary>
		/// <param name="layerMask">Layer mask.</param>
		/// <param name="layerName">Name of the layer.</param>
		/// <returns>
		/// A layer mask.
		/// </returns>
		public static LayerMask AddLayer(this LayerMask layerMask, string layerName) => layerMask | (1 << LayerMask.NameToLayer(layerName));

		/// <summary>
		/// Adds a layer to a layer mask.
		/// </summary>
		/// <param name="layerMask">Layer mask.</param>
		/// <param name="layer">Layer.</param>
		/// <returns>
		/// A layer mask.
		/// </returns>
		public static LayerMask AddLayer(this LayerMask layerMask, int layer) => layerMask | (1 << layer);

		/// <summary>
		/// Removes a layer from a layer mask.
		/// </summary>
		/// <param name="layerMask">Layer mask.</param>
		/// <param name="layerName">Name of the layer.</param>
		/// <returns>
		/// A layer mask.
		/// </returns>
		public static LayerMask RemoveLayer(this LayerMask layerMask, string layerName) => layerMask & ~(1 << LayerMask.NameToLayer(layerName));

		/// <summary>
		/// Removes a layer from a layer mask.
		/// </summary>
		/// <param name="layerMask">Layer mask.</param>
		/// <param name="layer">Layer.</param>
		/// <returns>
		/// A layer mask.
		/// </returns>
		public static LayerMask RemoveLayer(this LayerMask layerMask, int layer) => layerMask | (1 << layer);

		/// <summary>
		/// Checks whether a layer is in a layer mask.
		/// </summary>
		/// <param name="layerMask">Layer mask.</param>
		/// <param name="layerName">Layer name.</param>
		/// <returns>
		/// True if layer is in the layer mask, False otherwise.
		/// </returns>
		public static bool HasLayer(this LayerMask layerMask, string layerName) => layerMask == (layerMask | (1 << LayerMask.NameToLayer(layerName)));
		
		/// <summary>
		/// Checks whether a layer is in a layer mask.
		/// </summary>
		/// <param name="layerMask">Layer mask.</param>
		/// <param name="layer">Layer.</param>
		/// <returns>
		/// True if layer is in the layer mask, False otherwise.
		/// </returns>
		public static bool HasLayer(this LayerMask layerMask, int layer) => layerMask == (layerMask | 1 << layer);

		/// <summary>
		/// Logs all layers in a layer mask to console.
		/// </summary>
		/// <param name="layerMask">LayerMask.</param>
		public static void LogAllLayerNames(this LayerMask layerMask)
		{
			string layerString = string.Empty;
			
			for (int i = 0; i < 32; i++)
			{
				if (layerMask != (layerMask | (1 << i)))
				{
					continue;
				}

				layerString += $"{LayerMask.LayerToName(i)} ";
			}
			
			Debug.Log($"Layers in mask: {layerString}");
		}
	}
}