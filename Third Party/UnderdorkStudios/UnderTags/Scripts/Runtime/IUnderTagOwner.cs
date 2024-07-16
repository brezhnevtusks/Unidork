using System.Collections.Generic;
using UnderdorkStudios.UnderTools.Extensions;

namespace UnderdorkStudios.UnderTags
{
    /// <summary>
    /// Base interface for components that can own tags. Implemented by <see cref="UnderTagContainer"/> but you can also
    /// inherit from this in your own MonoBehaviours or custom data classes. In those cases you will need to implement
    /// the <see cref="GetOwnedTags"/> method with your custom logic for getting tags owned by a component/object. UnderTag
    /// logic will then process those tags the same way it would process tags from an <see cref="UnderTagContainer"/>.
    /// </summary>
    public interface IUnderTagOwner
    {
        /// <summary>
        /// Gets a list of <see cref="UnderTag"/> owned by this object.
        /// </summary>
        /// <returns>
        /// A list of <see cref="UnderTag"/>.
        /// </returns>
        List<UnderTag> GetOwnedTags();

        /// <summary>
        /// Checks if this container has a tag that matches <paramref name="tag"/>.
        /// Tags don't have to match exactly, meaning that they can be expanded when comparing:
        /// if container has a tag A.B.C, it will return True for A, A.B and A.B.C (but not for A.B.C.D!).
        /// For strict matches, use <see cref="HasExact"/>.
        /// </summary>
        /// <param name="tag">Tag to check.</param>
        /// <returns>
        /// True if any of the tags in this container loosely match <paramref name="tag"/>, False otherwise.
        /// </returns>
        bool Has(UnderTag tag)
        {
            List<UnderTag> ownedTags = GetOwnedTags();

            if (ownedTags.IsNullOrEmpty())
            {
                return false;
            }
            
            if (ownedTags.Contains(tag))
            {
                return true;
            }

            UnderTagContainer.CachedTagList ??= new List<UnderTag>();
            UnderTagContainer.CachedTagList.Clear();
            
            UnderTagManager.GetDescendentTags(tag, UnderTagContainer.CachedTagList);

            foreach (UnderTag descendantTag in UnderTagContainer.CachedTagList)
            {
                if (ownedTags.Contains(descendantTag))
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// Checks if this container owns at least one of tags from <paramref name="tags"/>.
        /// Tags don't have to match exactly, meaning that they can be expanded when comparing:
        /// if container has a tag A.B.C, it will return True for A, A.B and A.B.C (but not for A.B.C.D!).
        /// For strict matches, use <see cref="HasAnyExact(System.Collections.Generic.ICollection{UnderTags.UnderTag})"/>.
        /// </summary>
        /// <param name="tags">Tags to check.</param>
        /// <returns>
        /// True if at least 1 tag from <paramref name="tags"/> is owned by this container, False otherwise.
        /// </returns>
        bool HasAny(ICollection<UnderTag> tags)
        {
            foreach (UnderTag tag in tags)
            {
                if (Has(tag))
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// Checks if this container owns at least one tag from <paramref name="tagContainer"/>.
        /// Tags don't have to match exactly, meaning that they can be expanded when comparing:
        /// if container has a tag A.B.C, it will return True for A, A.B and A.B.C (but not for A.B.C.D!).
        /// For strict matches, use <see cref="HasAnyExact(UnderTagContainer)"/>.
        /// </summary>
        /// <param name="tagContainer">Tag container to check.</param>
        /// <returns>
        /// True if at least 1 tag from <paramref name="tagContainer"/> is owned by this container, False otherwise.
        /// </returns>
        bool HasAny(UnderTagContainer tagContainer)
        {
            return HasAny(tagContainer.OwnedTags);
        }
        
        /// <summary>
        /// Checks if this container owns all of tags from <paramref name="tags"/>.
        /// Tags don't have to match exactly, meaning that they can be expanded when comparing:
        /// if container has a tag A.B.C, it will return True for A, A.B and A.B.C (but not for A.B.C.D!).
        /// For strict matches, use <see cref="HasAllExact(System.Collections.Generic.ICollection{UnderTags.UnderTag})"/>.
        /// </summary>
        /// <param name="tags">Tags to check.</param>
        /// <returns>
        /// True if all tags from <paramref name="tags"/> are owned by this container, False otherwise.
        /// </returns>
        bool HasAll(ICollection<UnderTag> tags)
        {
            foreach (UnderTag tag in tags)
            {
                if (!Has(tag))
                {
                    return false;
                }
            }
            
            return true;
        }
        
        /// <summary>
        /// Checks if this container owns all the tags from <paramref name="tagContainer"/>.
        /// Tags don't have to match exactly, meaning that they can be expanded when comparing:
        /// if container has a tag A.B.C, it will return True for A, A.B and A.B.C (but not for A.B.C.D!).
        /// For strict matches, use <see cref="HasAllExact(UnderTagContainer)"/>
        /// </summary>
        /// <param name="tagContainer">Tag container to check.</param>
        /// <returns>
        /// True if all tags from <paramref name="tagContainer"/> are owned by this container, False otherwise.
        /// </returns>
        bool HasAll(UnderTagContainer tagContainer) => HasAll(tagContainer.GetOwnedTags());

        /// <summary>
        /// Checks if this container owns none of tags from <paramref name="tags"/>.
        /// Tags don't have to match exactly, meaning that they can be expanded when comparing:
        /// if container has a tag A.B.C, it will return True for A, A.B and A.B.C (but not for A.B.C.D!).
        /// For strict matches, use <see cref="HasNone(System.Collections.Generic.ICollection{UnderTags.UnderTag})"/>.
        /// </summary>
        /// <param name="tags">Tags to check.</param>
        /// <returns>
        /// True if none of the tags from <paramref name="tags"/> are owned by this container, False otherwise.
        /// </returns> 
        bool HasNone(ICollection<UnderTag> tags)
        {
            foreach (UnderTag tag in tags)
            {
                if (Has(tag))
                {
                    return false;
                }
            }
            
            return true; 
        }
        
        /// <summary>
        /// Checks if this container owns none of the tags from <paramref name="tagContainer"/>.
        /// Tags don't have to match exactly, meaning that they can be expanded when comparing:
        /// if container has a tag A.B.C, it will return True for A, A.B and A.B.C (but not for A.B.C.D!).
        /// For strict matches, use <see cref="HasNone(UnderTagContainer)"/>.
        /// </summary>
        /// <param name="tagContainer">Tag container to check.</param>
        /// <returns>
        /// True if all tags from <paremref name="tagContainer"/> are owned by this container, False otherwise.
        /// </returns>
        bool HasNone(UnderTagContainer tagContainer) => HasNone(tagContainer.GetOwnedTags());

        /// <summary>
        /// Checks if this container owns the passed tag.
        /// Tags must match exactly, meaning they should have the same hierarchical depth. For instance, A.B != A.B.C.
        /// For loose matches, use <see cref="Has"/>.
        /// </summary>
        /// <param name="tag">Tag to check.</param>
        /// <returns>
        /// True if at least 1 tag from <see cref="tag"/> is owned by this container, False otherwise.
        /// </returns>
        public bool HasExact(UnderTag tag)
        {
            List<UnderTag> ownedTags = GetOwnedTags();

            if (ownedTags.IsNullOrEmpty())
            {
                return false;
            }
            
            return ownedTags.Contains(tag);
        }
        
        /// <summary>
        /// Checks if this container owns at least one of the passed tags.
        /// Tags must match exactly, meaning they should have the same hierarchical depth. For instance, A.B != A.B.C.
        /// For loose matches, use <see cref="HasAny(System.Collections.Generic.ICollection{UnderTags.UnderTag})"/>.
        /// </summary>
        /// <param name="tags">Tags to check.</param>
        /// <returns>
        /// True if at least 1 tag from <paramref name="tags"/> is owned by this container, False otherwise.
        /// </returns>
        bool HasAnyExact(ICollection<UnderTag> tags)
        {
            List<UnderTag> ownedTags = GetOwnedTags();

            if (ownedTags.IsNullOrEmpty())
            {
                return false;
            }

            foreach (UnderTag tagToCheck in tags)
            {
                if (ownedTags.Contains(tagToCheck))
                {
                    return true;
                }
            }

            return false;
        }
        
        /// <summary>
        /// Checks if this container owns at least one tag from <paramref name="tagContainer"/>.
        /// Tags must match exactly, meaning they should have the same hierarchical depth. For instance, A.B != A.B.C.
        /// For loose matches, use <see cref="HasAny(UnderTagContainer)"/>.
        /// </summary>
        /// <param name="tagContainer">Tag container to check.</param>
        /// <returns>
        /// True if at least 1 tag from <paramref name="tagContainer"/> is owned by this container, False otherwise.
        /// </returns>
        bool HasAnyExact(UnderTagContainer tagContainer) => HasAnyExact(tagContainer.GetOwnedTags());

        /// <summary>
        /// Checks if this container owns all the passed tags.
        /// Tags must match exactly, meaning they should have the same hierarchical depth. For instance, A.B != A.B.C.
        /// For loose matches, use <see cref="HasAll(System.Collections.Generic.ICollection{UnderTags.UnderTag})"/>.
        /// </summary>
        /// <param name="tags">Tags to check.</param>
        /// <returns>
        /// True if all tags from <paramref name="tags"/> are owned by this container, False otherwise.
        /// </returns>
        bool HasAllExact(ICollection<UnderTag> tags)
        {
            List<UnderTag> ownedTags = GetOwnedTags();

            if (ownedTags.IsNullOrEmpty())
            {
                return false;
            }

            foreach (UnderTag tagToCheck in tags)
            {
                if (!ownedTags.Contains(tagToCheck))
                {
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Checks if this container owns all the tags from the passed container.
        /// Tags must match exactly, meaning they should have the same hierarchical depth. For instance, A.B != A.B.C.
        /// For loose matches, use <see cref="HasAll(UnderTagContainer)"/>.
        /// </summary>
        /// <param name="tags">Tag container to check.</param>
        /// <returns>
        /// True if all tags from <paramref name="tags"/> are owned by this container, False otherwise.
        /// </returns>
        bool HasAllExact(UnderTagContainer tags) => HasAllExact(tags.GetOwnedTags());

        /// <summary>
        /// Checks if this container owns none of the passed tags.
        /// Tags must match exactly, meaning they should have the same hierarchical depth. For instance, A.B != A.B.C.
        /// For loose matches, use <see cref="HasNone(UnderTagContainer)"/>.
        /// </summary>
        /// <param name="tags">Tags to check.</param>
        /// <returns>
        /// True if none of the tags from <paramref name="tags"/> are owned by this container, False otherwise.
        /// </returns>
        bool HasNoneExact(ICollection<UnderTag> tags)
        {
            List<UnderTag> ownedTags = GetOwnedTags();

            if (ownedTags.IsNullOrEmpty())
            {
                return false;
            }

            foreach (UnderTag tagToCheck in tags)
            {
                if (ownedTags.Contains(tagToCheck))
                {
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Checks if this container owns none of the tags from <paramref name="tagContainer"/>.
        /// Tags must match exactly, meaning they should have the same hierarchical depth. For instance, A.B != A.B.C.
        /// For loose matches, use <see cref="HasNone(UnderTagContainer)"/>.
        /// </summary>
        /// <param name="tagContainer">Tag container to check.</param>
        /// <returns>
        /// True if all tags from <paremref name="tagContainer"/> are owned by this container, False otherwise.
        /// </returns>
        bool HasNoneExact(UnderTagContainer tagContainer) => HasNoneExact(tagContainer.GetOwnedTags());
    }
}