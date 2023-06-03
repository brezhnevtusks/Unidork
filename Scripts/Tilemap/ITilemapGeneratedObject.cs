#if TILEMAP2D && TILEMAP2D_EXTRAS

namespace Unidork.TilemapTools
{
    /// <summary>
    /// Interface for objects that are generated into the editor scene by using data from a tilemap.
    /// </summary>
    /// <typeparam name="T">Type of game object.</typeparam>
	public interface ITilemapGeneratedObject<out T> where T : System.Enum
    {
        /// <summary>
        /// Object type.
        /// </summary>
        T Type { get; }

#if UNITY_EDITOR
        /// <summary>
        /// Sometimes we might place tilemap-generated objects in the scene by hand. 
        /// Use this flag if you need to distinguish between those objects and actual tilemap-generated ones
        /// in the editor.
        /// </summary>
        bool WasGeneratedByTilemap { get; set; }
#endif
    }
}

#endif