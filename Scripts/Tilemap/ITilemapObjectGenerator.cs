#if TILEMAP2D && TILEMAP2D_EXTRAS

using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Unidork.TilemapTools
{
	/// <summary>
	/// Interface for components that instantiate game objects in the scene using data from a tilemap.
	/// </summary>
	/// <typeparam name="T">Object type.</typeparam>
	public interface ITilemapObjectGenerator<T> where T : System.Enum
	{
		/// <summary>
		/// Instantiates game objects in the scene.
		/// </summary>
		void Generate();

		/// <summary>
		/// Destroys all children of a transform that implement the ITilemapGeneratedObject interface.
		/// Checks the <see cref="ITilemapGeneratedObject{T}.WasGeneratedByTilemap"/> property to make
		/// sure we don't destroy any objects that implement the interface but have been placed
		/// in the level manually.
		/// </summary>
		/// <param name="objectHolder">Transform that holds objects to be destroyed.</param>
		/// <param name="forceDestroy">Should the <see cref="ITilemapGeneratedObject{T}.WasGeneratedByTilemap"/> property
		/// be ignored on the objects we're trying to destroy?</param>
		void DestroyTilemapGeneratedObjects(Transform objectHolder, bool forceDestroy)
		{
			for (int i = objectHolder.childCount - 1; i >= 0; i--)
			{
				GameObject gameObject = objectHolder.GetChild(i).gameObject;

				var tilemapGeneratedObject = gameObject.GetComponent<ITilemapGeneratedObject<T>>();

				if (tilemapGeneratedObject == null)
				{
					Debug.LogError($"{gameObject.name} doesn't have a component that implements ITilemapGeneratedObject!");
					continue;
				}

				if (!forceDestroy && !tilemapGeneratedObject.WasGeneratedByTilemap)
				{
					continue;
				}

				Undo.DestroyObjectImmediate(gameObject);
			}
		}

		/// <summary>
		/// Gets the min and max coordinates of a tilemap's cell bounds.
		/// </summary>
		/// <param name="tilemap">Tilemap.</param>
		/// <returns>
		/// A tuple of type (int, int, int, int) storing min X, max X, min Y, max Y, min Z, and max Z coordinate of the cell bounds respectively.
		/// </returns>
		(int minX, int maxX, int minY, int maxY, int minZ, int maxZ) GetTilemapBoundsCoordinates(Tilemap tilemap)
		{
			BoundsInt railTilemapBounds = tilemap.cellBounds;

			Vector3Int railTilemapBoundsMin = railTilemapBounds.min;
			Vector3Int railTilemapBoundsMax = railTilemapBounds.max;

			int minX = railTilemapBoundsMin.x;
			int maxX = railTilemapBoundsMax.x;

			int minY = railTilemapBoundsMin.y;
			int maxY = railTilemapBoundsMax.y;

			int minZ = railTilemapBoundsMin.z;
			int maxZ = railTilemapBoundsMax.z;

			return (minX, maxX, minY, maxY, minZ, maxZ);
		}

		/// <summary>
		/// Gets the type of a tilemap generated object at a specific coordinates of a tilemap. 
		/// </summary>
		/// <param name="tilemap">Tilemap.</param>
		/// <param name="tilemapCoordinates">Tilemap cell coordinates.</param>
		/// <returns>Type of tilemap generated object at the specified coordinates.</returns>
		T GetObjectType(Tilemap tilemap, Vector3Int tilemapCoordinates)
		{
			GameObject objectToInstantiate = tilemap.GetObjectToInstantiate(tilemapCoordinates);

			if (objectToInstantiate == null)
			{
				return default;
			}

			var tilemapGeneratedObject = objectToInstantiate.GetComponent<ITilemapGeneratedObject<T>>();

			if (tilemapGeneratedObject == null)
			{
				Debug.LogError($"{((Component)tilemapGeneratedObject).gameObject.name} doesn't have a component that implements ITilemapGeneratedObject!");
				return default;
			}
			
			return objectToInstantiate.GetComponent<ITilemapGeneratedObject<T>>().Type;
		}
	}
}


#endif