using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnderdorkStudios.UnderTags
{
    public static class UnderTagManager
    {
        #region Fields

        /// <summary>
        /// Reference to the tag database scriptable object.
        /// </summary>
        private static readonly UnderTagDatabase database;

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor.
        /// </summary>
        static UnderTagManager()
        {
            database = UnderTagDatabase.GetOrCreateUnderTagDatabase();
        }
        
        #endregion

        #region Tags

        /// <summary>
        /// Gets a list of <paramref name="tag"/>'s parents up to the root tag.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <returns>
        /// A list of <see cref="UnderTag"/> with all parents in the hierarchy.
        /// </returns>
        public static List<UnderTag> GetParentTags(UnderTag tag)
        {
            return database.GetParentTags(tag);
        }
        
        /// <summary>
        /// Recursively gets all descendant tags: children, children of children, etc.
        /// </summary>
        /// <param name="tag">Tag.</param>
        /// <param name="childTags">List of child tags to fill.</param>
        /// <returns>
        /// A list of <see cref="UnderTag"/> containing all child tags, if any.
        /// </returns>
        public static void GetDescendentTags(UnderTag tag, List<UnderTag> childTags)
        {
            database.GetDescendentTags(tag, childTags);
        }

        #endregion
    }
}