using System.Collections.Generic;
using UnderdorkStudios.UnderTools.Extensions;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace UnderdorkStudios.UnderTags
{
    /// <summary>
    /// Component storing tags that belong to a game object. Use any of the Has...() methods to check if this
    /// component has any/all/none of a collection of game tags.
    /// </summary>
    [DisallowMultipleComponent]
    public class UnderTagContainer : MonoBehaviour, IUnderTagOwner
    {
        #region Properties

        /// <summary>
        /// Tags belonging to this container.
        /// </summary>
        /// <value>
        /// Gets the value of <see cref="ownedTags"/>.
        /// </value>
        public List<UnderTag> OwnedTags => ownedTags;

        #endregion
        
        #region Fields

        /// <summary>
        /// Cached list to use for various operations with tags.
        /// </summary>
        public static List<UnderTag> CachedTagList;
        
        /// <summary>
        /// Tags belonging to this container.
        /// </summary>
        [SerializeField] private List<UnderTag> ownedTags = new();

        #endregion

        #region Tags

        /// <summary>
        /// Removes all tags owned by this container.
        /// </summary>
        public void Clear() => ownedTags.Clear();
        
        /// <summary>
        /// Checks if this container has any tags assigned.
        /// </summary>
        /// <returns>
        /// True if the container has at least 1 tag, False otherwise.
        /// </returns>
        public bool IsEmpty() => ownedTags.Count == 0;

        /// <summary>
        /// Adds a tag to this container.
        /// </summary>
        /// <param name="tagToAdd">Tag to add.</param>
        public void AddTag(UnderTag tagToAdd)
        {
            if (ownedTags.AddUnique(tagToAdd))
            {
                List<UnderTag> tagsInHierarchy = UnderTagDatabase
                    .GetOrCreateUnderTagDatabase()
                    .GetAllTagsInHierarchy(tagToAdd, true);

                foreach (UnderTag tagInHierarchy in tagsInHierarchy)
                {
                    _ = ownedTags.Remove(tagInHierarchy);
                }
#if UNITY_EDITOR
                EditorUtility.SetDirty(this);
#endif
            }
        }

        /// <summary>
        /// Adds a collection of <see cref="UnderTag"/> to an <see cref="UnderTagContainer"/>.
        /// </summary>
        /// <param name="tagsToAdd">Tags to add.</param>
        public void AddTags(ICollection<UnderTag> tagsToAdd)
        {
            if (tagsToAdd == null)
            {
                Debug.LogError("UnderTags: Trying to add a null tag collection to UnderTagContainer!", this);
                return;
            }

            foreach (UnderTag tagToAdd in tagsToAdd)
            {
                AddTag(tagToAdd);
            }
        }
        
        /// <summary>
        /// Removes the passed tag from <see cref="ownedTags"/>.
        /// </summary>
        /// <param name="tagToRemove">Tag to remove.</param>
        public void RemoveTag(UnderTag tagToRemove)
        {
            if (!tagToRemove.IsValid())
            {
                return;
            }

            if (ownedTags.Remove(tagToRemove))
            {
                UnderTagDatabase tagDatabase = UnderTagDatabase.GetOrCreateUnderTagDatabase();
                List<UnderTag> childTags = new();
                tagDatabase.GetDescendentTags(tagToRemove, childTags);
                foreach (UnderTag parentTag in childTags)
                {
                    _ = ownedTags.Remove(parentTag);
                }
#if UNITY_EDITOR
                if (!EditorApplication.isPlaying)
                {
                    EditorUtility.SetDirty(this);
                }
#endif
            }
        }

        /// <summary>
        /// Removes a collection of <see cref="UnderTag"/> from an <see cref="UnderTagContainer"/>.
        /// </summary>
        /// <param name="tagsToRemove">Tags to remove.</param>
        public void RemoveTags(ICollection<UnderTag> tagsToRemove)
        {
            if (tagsToRemove == null)
            {
                Debug.LogError("UnderTags: Trying to remove a null tag collection to UnderTagContainer!", this);
                return;
            }

            foreach (UnderTag tagToRemove in tagsToRemove)
            {
                RemoveTag(tagToRemove);
            }
        }

        public List<UnderTag> GetOwnedTags() => ownedTags;
        
        #endregion
    }
}