#if TILEMAP2D && TILEMAP2D_EXTRAS

using UnityEngine;
using UnityEngine.Tilemaps;

namespace Unidork.TilemapTools
{
    /// <summary>
    /// Tile that stores a game object in a tilemap without instantiating it. Used for instantiating objects
    /// into the scene via a separate generator.
    /// </summary>
    [CreateAssetMenu(fileName = "NewObjectGenerationTile", menuName = "Tilemap/Object Generation Tile")]
    public class ObjectGenerationTile : TileBase
    {
        #region Fields

        /// <summary>
        /// Sprite that represents the connected game object on the tilemap.
        /// </summary>
        [Tooltip("Sprite that represents the connected game object on the tilemap.")]
        [SerializeField]
        private Sprite sprite = null;

        /// <summary>
        /// Game object to store in the tilemap.
        /// </summary>
        [Tooltip("Game object to store in the tilemap.")]
        [SerializeField]
        private GameObject gameObject = null;

        #endregion

        #region Data

        /// <inheritdoc/>
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = sprite;
            tileData.gameObject = gameObject;
            tileData.colliderType = Tile.ColliderType.None;
            tileData.flags = TileFlags.LockTransform | TileFlags.InstantiateGameObjectRuntimeOnly;
            tileData.transform = Matrix4x4.identity;
        }

        #endregion
    }
}

#endif