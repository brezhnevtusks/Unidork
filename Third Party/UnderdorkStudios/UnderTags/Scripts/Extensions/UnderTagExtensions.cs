using System.Collections.Generic;
using UnityEngine;

namespace UnderdorkStudios.UnderTags.Extensions
{
    public static class UnderTagExtensions
    {
        public static bool HasTag(this Component component, UnderTag tag)
        {
            return HasTag(component.gameObject, tag);
        }
        
        public static bool HasTag(this GameObject gameObject, UnderTag tag)
        {
            List<IUnderTagOwner> underTagOwners = new();
            gameObject.GetComponents(underTagOwners);

            foreach (IUnderTagOwner underTagOwner in underTagOwners)
            {
                if (underTagOwner.Has(tag))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasTagInChildren(this Component component, UnderTag tag)
        {
            return HasExactTagInChildren(component.gameObject, tag);
        }
        
        public static bool HasTagInChildren(this GameObject gameObject, UnderTag tag)
        {
            List<IUnderTagOwner> underTagOwners = new();
            gameObject.GetComponentsInChildren(false, underTagOwners);

            foreach (IUnderTagOwner underTagOwner in underTagOwners)
            {
                if (underTagOwner.Has(tag))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasTagInParents(this Component component, UnderTag tag)
        {
            return HasTagInParents(component.gameObject, tag);
        }
        
        public static bool HasTagInParents(this GameObject gameObject, UnderTag tag)
        {
            List<IUnderTagOwner> underTagOwners = new();
            gameObject.GetComponentsInParent(false, underTagOwners);

            foreach (IUnderTagOwner underTagOwner in underTagOwners)
            {
                if (underTagOwner.Has(tag))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasAnyTag(this Component component, ICollection<UnderTag> tags)
        {
            return HasAnyTag(component.gameObject, tags);
        }

        public static bool HasAnyTag(this GameObject gameObject, ICollection<UnderTag> tags)
        {
            List<IUnderTagOwner> underTagOwners = new();
            gameObject.GetComponents(underTagOwners);

            foreach (IUnderTagOwner underTagOwner in underTagOwners)
            {
                if (underTagOwner.HasAny(tags))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasAnyTag(this Component component, UnderTagContainer tagContainer)
        {
            return HasAnyTag(component.gameObject, tagContainer);
        }

        public static bool HasAnyTag(this GameObject gameObject, UnderTagContainer tagContainer)
        {
            return HasAnyTag(gameObject, tagContainer.OwnedTags);
        }

        public static bool HasAnyTagInChildren(this Component component, ICollection<UnderTag> tags)
        {
            return HasAnyTagInChildren(component.gameObject, tags);
        }

        public static bool HasAnyTagInChildren(this GameObject gameObject, ICollection<UnderTag> tags)
        {
            List<IUnderTagOwner> underTagOwners = new();
            gameObject.GetComponentsInChildren(false, underTagOwners);

            foreach (IUnderTagOwner underTagOwner in underTagOwners)
            {
                if (underTagOwner.HasAny(tags))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasAnyTagInChildren(this Component component, UnderTagContainer tagContainer)
        {
            return HasAnyTagInChildren(component.gameObject, tagContainer);
        }

        public static bool HasAnyTagInChildren(this GameObject gameObject, UnderTagContainer tagContainer)
        {
            return HasAnyTagInChildren(gameObject, tagContainer.OwnedTags);
        }

        public static bool HasAnyTagInParents(this Component component, ICollection<UnderTag> tags)
        {
            return HasAnyTagInParents(component.gameObject, tags);
        }
        
        public static bool HasAnyTagInParents(this GameObject gameObject, ICollection<UnderTag> tags)
        {
            List<IUnderTagOwner> underTagOwners = new();
            gameObject.GetComponentsInParent(false, underTagOwners);

            foreach (IUnderTagOwner underTagOwner in underTagOwners)
            {
                if (underTagOwner.HasAny(tags))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasAnyTagInParents(this Component component, UnderTagContainer tagContainer)
        {
            return HasAnyTagInParents(component.gameObject, tagContainer);
        }

        public static bool HasAnyTagInParents(this GameObject gameObject, UnderTagContainer tagContainer)
        {
            return HasAnyTagInParents(gameObject, tagContainer.OwnedTags);
        }

        public static bool HasAllTags(this Component component, ICollection<UnderTag> tags)
        {
            return HasAllTags(component.gameObject, tags);
        }
        
        public static bool HasAllTags(this GameObject gameObject, ICollection<UnderTag> tags)
        {
            List<IUnderTagOwner> underTagOwners = new();
            gameObject.GetComponents(underTagOwners);

            foreach (IUnderTagOwner underTagOwner in underTagOwners)
            {
                if (underTagOwner.HasAll(tags))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasAllTags(this Component component, UnderTagContainer tagContainer)
        {
            return HasAllTags(component.gameObject, tagContainer);
        }

        public static bool HasAllTags(this GameObject gameObject, UnderTagContainer tagContainer)
        {
            return HasAllTags(gameObject, tagContainer.OwnedTags);
        }

        public static bool HasAllTagsInChildren(this Component component, ICollection<UnderTag> tags)
        {
            return HasAllTagsInChildren(component.gameObject, tags);
        }
        
        public static bool HasAllTagsInChildren(this GameObject gameObject, ICollection<UnderTag> tags)
        {
            List<IUnderTagOwner> underTagOwners = new();
            gameObject.GetComponentsInChildren(false, underTagOwners);

            foreach (IUnderTagOwner underTagOwner in underTagOwners)
            {
                if (underTagOwner.HasAll(tags))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasAllTagsInChildren(this Component component, UnderTagContainer tagContainer)
        {
            return HasAllTagsInChildren(component.gameObject, tagContainer);
        }

        public static bool HasAllTagsInChildren(this GameObject gameObject, UnderTagContainer tagContainer)
        {
            return HasAllTagsInChildren(gameObject, tagContainer.OwnedTags);
        }

        public static bool HasAllTagsInParents(this Component component, ICollection<UnderTag> tags)
        {
            return HasAllTags(component.gameObject, tags);
        }

        public static bool HasAllTagsInParents(this GameObject gameObject, ICollection<UnderTag> tags)
        {
            List<IUnderTagOwner> underTagOwners = new();
            gameObject.GetComponentsInParent(false, underTagOwners);

            foreach (IUnderTagOwner underTagOwner in underTagOwners)
            {
                if (underTagOwner.HasAll(tags))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasAllTagsInParents(this Component component, UnderTagContainer tagContainer)
        {
            return HasAllTagsInParents(component.gameObject, tagContainer);
        }

        public static bool HasAllTagsInParents(this GameObject gameObject, UnderTagContainer tagContainer)
        {
            return HasAllTagsInParents(gameObject, tagContainer.OwnedTags);
        }

        public static bool HasNoneOfTags(this Component component, ICollection<UnderTag> tags)
        {
            return HasNoneOfTags(component.gameObject, tags);
        }
        
        public static bool HasNoneOfTags(this GameObject gameObject, ICollection<UnderTag> tags)
        {
            return !HasAnyTag(gameObject, tags);
        }

        public static bool HasNoneOfTags(this Component component, UnderTagContainer tagContainer)
        {
            return HasNoneOfTags(component.gameObject, tagContainer);
        }

        public static bool HasNoneOfTags(this GameObject gameObject, UnderTagContainer tagContainer)
        {
            return HasNoneOfTags(gameObject, tagContainer.OwnedTags);
        }

        public static bool HasNoneOfTagsInChildren(this Component component, ICollection<UnderTag> tags)
        {
            return HasNoneOfTagsInChildren(component.gameObject, tags);
        }

        public static bool HasNoneOfTagsInChildren(this GameObject gameObject, ICollection<UnderTag> tags)
        {
            return !HasAnyTagInChildren(gameObject, tags);
        }

        public static bool HasNoneOfTagsInChildren(this Component component, UnderTagContainer tagContainer)
        {
            return HasNoneOfTagsInChildren(component.gameObject, tagContainer);
        }

        public static bool HasNoneOfTagsInChildren(this GameObject gameObject, UnderTagContainer tagContainer)
        {
            return HasNoneOfTagsInChildren(gameObject, tagContainer.OwnedTags);
        }

        public static bool HasNoneOfTagsInParents(this Component component, ICollection<UnderTag> tags)
        {
            return HasNoneOfTagsInParents(component.gameObject, tags);
        }

        public static bool HasNoneOfTagsInParents(this GameObject gameObject, ICollection<UnderTag> tags)
        {
            return !HasAnyTagInParents(gameObject, tags);
        }

        public static bool HasNoneOfTagsInParents(this Component component, UnderTagContainer tagContainer)
        {
            return HasNoneOfTagsInParents(component.gameObject, tagContainer);
        }

        public static bool HasNoneOfTagsInParents(this GameObject gameObject, UnderTagContainer tagContainer)
        {
            return HasNoneOfTagsInParents(gameObject, tagContainer.OwnedTags);
        }

        /// <summary>
        /// Checks if component's game object has an exact tag. Exact matches have the same hierarchical depth: A.B == A.B, but
        /// A.B != A.B.C Use <see cref="HasTag(UnityEngine.Component,UnderTags.UnderTag)"/> for non-strict queries.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="tag">Tag to check.</param>
        /// <returns>
        /// True if the component's game object has a <see cref="UnderTagContainer"/> and that container contains a tag that
        /// matches <see cref="tag"/> exactly, False otherwise.
        /// </returns>
        public static bool HasExactTag(this Component component, UnderTag tag)
        {
            return HasExactTag(component.gameObject, tag);
        }
        
        /// <summary>
        /// Checks if a game object has an exact tag. Exact matches have the same hierarchical depth: A.B == A.B, but
        /// A.B. != A.B.C Use <see cref="HasTag(UnityEngine.GameObject,UnderTags.UnderTag)"/> for non-strict queries.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="tag">Tag to check.</param>
        /// <returns>
        /// True if the game object has a <see cref="UnderTagContainer"/> and that container contains a tag that
        /// matches <see cref="tag"/> exactly, False otherwise.
        /// </returns>
        public static bool HasExactTag(this GameObject gameObject, UnderTag tag)
        {
            List<IUnderTagOwner> tagOwners = new();
            gameObject.GetComponents(tagOwners);

            foreach (IUnderTagOwner tagOwner in tagOwners)
            {
                if (tagOwner.HasExact(tag))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a component's game object or any of its children have an exact tag.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasTagInChildren(UnityEngine.Component,UnderTags.UnderTag)"/> for non-strict queries.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="tag">Tag to check.</param>
        /// <returns>
        /// True if the component's game object or any of its children have a <see cref="UnderTagContainer"/> and that container
        /// contains a tag that matches <see cref="tag"/> exactly, False otherwise.
        /// </returns>
        public static bool HasExactTagInChildren(this Component component, UnderTag tag)
        {
            return HasExactTagInChildren(component.gameObject, tag);
        }
        
        /// <summary>
        /// Checks if a game object or any of its children have an exact tag.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasTagInChildren(UnityEngine.GameObject,UnderTags.UnderTag)"/> for non-strict queries.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="tag">Tag to check.</param>
        /// <returns>
        /// True if the game object or any of its children have a <see cref="UnderTagContainer"/> and that container
        /// contains a tag that matches <see cref="tag"/> exactly, False otherwise.
        /// </returns>
        public static bool HasExactTagInChildren(this GameObject gameObject, UnderTag tag)
        {
            List<IUnderTagOwner> underTagOwners = new();
            gameObject.GetComponentsInChildren(underTagOwners);

            foreach (IUnderTagOwner underTagOwner in underTagOwners)
            {
                if (underTagOwner.HasExact(tag))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a component's game object or any of its parents have an exact tag.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasTagInParents(UnityEngine.Component,UnderTags.UnderTag)"/> for non-strict queries.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="tag">Tag to check.</param>
        /// <returns>
        /// True if the component's game object or any of its parents have a <see cref="UnderTagContainer"/> and that container
        /// contains a tag that matches <see cref="tag"/> exactly, False otherwise.
        /// </returns>
        public static bool HasExactTagInParents(this Component component, UnderTag tag)
        {
            return HasExactTagInParents(component.gameObject, tag);
        }
        
        /// <summary>
        /// Checks if a game object or any of its parents have an exact tag.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasTagInParents(UnityEngine.GameObject,UnderTags.UnderTag)"/> for non-strict queries.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="tag">Tag to check.</param>
        /// <returns>
        /// True if the game object or any of its parents have a <see cref="UnderTagContainer"/> and that container
        /// contains a tag that matches <see cref="tag"/> exactly, False otherwise.
        /// </returns>
        public static bool HasExactTagInParents(this GameObject gameObject, UnderTag tag)
        {
            List<IUnderTagOwner> underTagOwners = new();
            gameObject.GetComponentsInParent(false, underTagOwners);

            foreach (IUnderTagOwner underTagOwner in underTagOwners)
            {
                if (underTagOwner.HasExact(tag))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a component's game object has any of the passed tags exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAnyTag(UnityEngine.Component,System.Collections.Generic.ICollection{UnderTags.UnderTag})"/> for non-strict queries.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="tags">Tags to check.</param>
        /// <returns>
        /// True if the component's game object has a <see cref="UnderTagContainer"/> and that container contains a tag that
        /// matches any of <see cref="tags"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAnyExactTag(this Component component, ICollection<UnderTag> tags)
        {
            return HasAnyExactTag(component.gameObject, tags);
        }
        
        /// <summary>
        /// Checks if a game object has any of the passed tags exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAnyTag(UnityEngine.GameObject,System.Collections.Generic.ICollection{UnderTags.UnderTag})"/> for non-strict queries.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="tags">Tags to check.</param>
        /// <returns>
        /// True if the game object has a <see cref="UnderTagContainer"/> and that container contains a tag that
        /// matches any of <see cref="tags"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAnyExactTag(this GameObject gameObject, ICollection<UnderTag> tags)
        {
            List<IUnderTagOwner> underTagOwners = new();
            gameObject.GetComponents(underTagOwners);

            foreach (IUnderTagOwner underTagOwner in underTagOwners)
            {
                if (underTagOwner.HasAnyExact(tags))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a component's game object matches any of tags contained in the passed container exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAnyTag(UnityEngine.Component,UnderTagContainer)"/> for non-strict queries.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="tagContainer">Tag container to check.</param>
        /// <returns>
        /// True if the component's game object has a <see cref="UnderTagContainer"/> and that container contains a tag that
        /// matches any of tags in <see cref="tagContainer"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAnyExactTag(this Component component, UnderTagContainer tagContainer)
        {
            return HasAnyExactTag(component.gameObject, tagContainer);
        }
        
        /// <summary>
        /// Checks if a game object matches any of tags contained in the passed container exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAnyTag(UnityEngine.GameObject,UnderTagContainer)"/> for non-strict queries.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="tagContainer">Tag container to check.</param>
        /// <returns>
        /// True if the game object has a <see cref="UnderTagContainer"/> and that container contains a tag that
        /// matches any of tags in <see cref="tagContainer"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAnyExactTag(this GameObject gameObject, UnderTagContainer tagContainer)
        {
            return HasAnyExactTag(gameObject, tagContainer.OwnedTags);
        }

        /// <summary>
        /// Checks if a component's game object or any of its children have any of the passed tags exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAnyTagInChildren(UnityEngine.Component,System.Collections.Generic.ICollection{UnderTags.UnderTag})"/> for non-strict queries.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="tags">Tag to check.</param>
        /// <returns>
        /// True if component's game object or any of its children have a <see cref="UnderTagContainer"/> and that container
        /// contains a tag that matches any of <see cref="tags"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAnyExactTagInChildren(this Component component, ICollection<UnderTag> tags)
        {
            return HasAnyExactTagInChildren(component.gameObject, tags);
        }
        
        /// <summary>
        /// Checks if a game object or any of its children have any of the passed tags exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAnyTagInChildren(UnityEngine.Component,System.Collections.Generic.ICollection{UnderTags.UnderTag})"/> for non-strict queries.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="tags">Tag to check.</param>
        /// <returns>
        /// True if the game object or any of its children have a <see cref="UnderTagContainer"/> and that container
        /// contains a tag that matches any of <see cref="tags"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAnyExactTagInChildren(this GameObject gameObject, ICollection<UnderTag> tags)
        {
            List<IUnderTagOwner> underTagOwners = new();
            gameObject.GetComponentsInChildren(underTagOwners);

            foreach (IUnderTagOwner underTagOwner in underTagOwners)
            {
                if (underTagOwner.HasAnyExact(tags))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a component's game object or any of its children have any of the tags contained in the passed container exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAnyTagInChildren(UnityEngine.GameObject,UnderTagContainer)"/> cref="HasTag"/> for non-strict queries.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="tagContainer">Tag container to check.</param>
        /// <returns>
        /// True if component's game object has a <see cref="UnderTagContainer"/> and that container contains a tag that
        /// matches any of tags in <see cref="tagContainer"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAnyExactTagInChildren(this Component component, UnderTagContainer tagContainer)
        {
            return HasAnyExactTagInChildren(component.gameObject, tagContainer);
        }

        /// <summary>
        /// Checks if a game object or any of its children have any of the tags contained in the passed container exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAnyTagInChildren(UnityEngine.GameObject,UnderTagContainer)"/> cref="HasTag"/> for non-strict queries.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="tagContainer">Tag container to check.</param>
        /// <returns>
        /// True if the game object has a <see cref="UnderTagContainer"/> and that container contains a tag that
        /// matches any of tags in <see cref="tagContainer"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAnyExactTagInChildren(this GameObject gameObject, UnderTagContainer tagContainer)
        {
            return HasAnyExactTagInChildren(gameObject, tagContainer.OwnedTags);
        }

        /// <summary>
        /// Checks if a component's game object or any of its parents have any of the passed tags exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAnyTagInParents(UnityEngine.Component,System.Collections.Generic.ICollection{UnderTags.UnderTag})"/> for non-strict queries.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="tags">Tags to check.</param>
        /// <returns>
        /// True if component's game object or any of its parents have a <see cref="UnderTagContainer"/> and that container
        /// contains a tag that matches any of <see cref="tags"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAnyExactTagInParents(this Component component, ICollection<UnderTag> tags)
        {
            return HasAnyExactTagInParents(component.gameObject, tags);
        }
        
        /// <summary>
        /// Checks if a game object or any of its parents have any of the passed tags exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAnyTagInParents(UnityEngine.GameObject,System.Collections.Generic.ICollection{UnderTags.UnderTag})"/> for non-strict queries.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="tags">Tags to check.</param>
        /// <returns>
        /// True if the game object or any of its parents have an <see cref="UnderTagContainer"/> and that container
        /// contains a tag that matches any of <see cref="tags"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAnyExactTagInParents(this GameObject gameObject, ICollection<UnderTag> tags)
        {
            List<IUnderTagOwner> underTagOwners = new();
            gameObject.GetComponentsInParent(false, underTagOwners);

            foreach (IUnderTagOwner underTagOwner in underTagOwners)
            {
                if (underTagOwner.HasAnyExact(tags))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a component's game object or any of its parents have any of the tags contained in the passed container exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAnyTagInParents(UnityEngine.Component,UnderTagContainer)"/> cref="HasTag"/> for non-strict queries.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="tagContainer">Tag container to check.</param>
        /// <returns>
        /// True if component's game object or any of its parents have an <see cref="UnderTagContainer"/> and that container
        /// contains a tag that matches any of tags in <see cref="tagContainer"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAnyExactTagInParents(this Component component, UnderTagContainer tagContainer)
        {
            return HasAnyExactTagInParents(component.gameObject, tagContainer);
        }

        /// <summary>
        /// Checks if a game object or any of its parents have any of the tags contained in the passed container exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAnyTagInParents(UnityEngine.GameObject,UnderTagContainer)"/> cref="HasTag"/> for non-strict queries.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="tagContainer">Tag container to check.</param>
        /// <returns>
        /// True if the game object or any of its parents have a <see cref="UnderTagContainer"/> and that container
        /// contains a tag that matches any of tags in <see cref="tagContainer"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAnyExactTagInParents(this GameObject gameObject, UnderTagContainer tagContainer)
        {
            return HasAnyExactTagInParents(gameObject, tagContainer.OwnedTags);
        }

        /// <summary>
        /// Checks if a component's game object owns all of the passed tags exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAllTags(UnityEngine.Component,System.Collections.Generic.ICollection{UnderTags.UnderTag})"/> for non-strict queries.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="tags">Tags to check.</param>
        /// <returns>
        /// True if component's game object has a <see cref="UnderTagContainer"/> and that container contains owns all tags
        /// from <see cref="tags"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAllExactTags(this Component component, ICollection<UnderTag> tags)
        {
            return HasAllExactTags(component.gameObject, tags);
        } 
        
        /// <summary>
        /// Checks if a game object owns all of the passed tags exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAllTags(UnityEngine.GameObject,System.Collections.Generic.ICollection{UnderTags.UnderTag})"/> for non-strict queries.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="tags">Tags to check.</param>
        /// <returns>
        /// True if the game object has a <see cref="UnderTagContainer"/> and that container contains owns all tags
        /// from <see cref="tags"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAllExactTags(this GameObject gameObject, ICollection<UnderTag> tags)
        {
            List<IUnderTagOwner> underTagOwners = new();
            gameObject.GetComponents(underTagOwners);

            foreach (IUnderTagOwner underTagOwner in underTagOwners)
            {
                if (underTagOwner.HasAllExact(tags))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a component's game object owns all tags from the passed container exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAllTags(UnityEngine.Component, UnderTags.UnderTagContainer)"/> for non-strict queries.
        /// </summary>
        /// <param name="component">Component.</param>
        /// <param name="tagContainer">Tag container to check.</param>
        /// <returns>
        /// True if component's game object has a <see cref="UnderTagContainer"/> and that container contains owns all tags
        /// from <see cref="tagContainer"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAllExactTags(this Component component, UnderTagContainer tagContainer)
        {
            return HasAllExactTags(component.gameObject, tagContainer);
        }
        
        /// <summary>
        /// Checks if a game object owns all tags from the passed container exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAllTags(UnityEngine.GameObject, UnderTags.UnderTagContainer)"/> for non-strict queries.
        /// </summary>
        /// <param name="gameObject">Game object.</param>
        /// <param name="tagContainer">Tag container to check.</param>
        /// <returns>
        /// True if the game object has a <see cref="UnderTagContainer"/> and that container contains owns all tags
        /// from <see cref="tagContainer"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAllExactTags(this GameObject gameObject, UnderTagContainer tagContainer)
        {
            return HasAnyExactTag(gameObject, tagContainer.OwnedTags);
        }

        /// <summary>
        /// Checks if a component's game object or any of its children have all the passed tags exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAllTagsInChildren(UnityEngine.Component,System.Collections.Generic.ICollection{UnderTags.UnderTag})"/> for non-strict queries.
        /// </summary>
        /// <remarks>
        /// This checks matches for each tag container in the hierarchy separately! Containers ARE NOT combined into one
        /// "mega-container", so in the case tags are distributed across several containers and none of them have the
        /// entire collection, this will return False.
        /// </remarks>
        /// <param name="component">Component.</param>
        /// <param name="tags">Tags to check.</param>
        /// <returns>
        /// True if component's game object or any of its children have a <see cref="UnderTagContainer"/> and that container
        /// contains all tags from <see cref="tags"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAllExactTagsInChildren(this Component component, ICollection<UnderTag> tags)
        {
            return HasAllExactTagsInChildren(component.gameObject, tags);
        }
        
        /// <summary>
        /// Checks if a game object or any of its children have all the passed tags exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAllTagsInChildren(UnityEngine.GameObject,System.Collections.Generic.ICollection{UnderTags.UnderTag})"/> for non-strict queries.
        /// </summary>
        /// <remarks>
        /// This checks matches for each tag container in the hierarchy separately! Containers ARE NOT combined into one
        /// "mega-container", so in the case tags are distributed across several containers and none of them have the
        /// entire collection, this will return False.
        /// </remarks>
        /// <param name="gameObject">Game object.</param>
        /// <param name="tags">Tags to check.</param>
        /// <returns>
        /// True if the game object or any of its children have a <see cref="UnderTagContainer"/> and that container
        /// contains all tags from <see cref="tags"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAllExactTagsInChildren(this GameObject gameObject, ICollection<UnderTag> tags)
        {
            List<IUnderTagOwner> underTagOwners = new();
            gameObject.GetComponentsInChildren(underTagOwners);

            foreach (IUnderTagOwner underTagOwner in underTagOwners)
            {
                if (((Component)underTagOwner).HasAllExactTagsInChildren(tags))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a component's game object or any of its children have all of the tags contained in the passed container exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAllTagsInChildren(UnityEngine.Component,UnderTagContainer)"/> cref="HasTag"/> for non-strict queries.
        /// </summary>
        /// <remarks>
        /// This checks matches for each tag container in the hierarchy separately! Containers ARE NOT combined into one
        /// "mega-container", so in the case tags are distributed across several containers and none of them have the
        /// entire collection, this will return False.
        /// </remarks>
        /// <param name="component">Component.</param>
        /// <param name="tagContainer">Tags to check.</param>
        /// <returns>
        /// True if component's game object has a <see cref="UnderTagContainer"/> and that container contains all tags
        /// from <see cref="tagContainer"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAllExactTagsInChildren(this Component component, UnderTagContainer tagContainer)
        {
            return HasAllExactTagsInChildren(component.gameObject, tagContainer);
        }
        
        /// <summary>
        /// Checks if a game object or any of its children have all of the tags contained in the passed container exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAllTagsInChildren(UnityEngine.GameObject,UnderTagContainer)"/> cref="HasTag"/> for non-strict queries.
        /// </summary>
        /// <remarks>
        /// This checks matches for each tag container in the hierarchy separately! Containers ARE NOT combined into one
        /// "mega-container", so in the case tags are distributed across several containers and none of them have the
        /// entire collection, this will return False.
        /// </remarks>
        /// <param name="gameObject">Game object.</param>
        /// <param name="tagContainer">Tags to check.</param>
        /// <returns>
        /// True if the game object has a <see cref="UnderTagContainer"/> and that container contains all tags
        /// from <see cref="tagContainer"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAllExactTagsInChildren(this GameObject gameObject, UnderTagContainer tagContainer)
        {
            return HasAllExactTagsInChildren(gameObject, tagContainer.OwnedTags);
        }
        
        /// <summary>
        /// Checks if a component's game object or any of its parents have aall of the passed tags exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAllTagsInParents(UnityEngine.Component,System.Collections.Generic.ICollection{UnderTags.UnderTag})"/> for non-strict queries.
        /// </summary>
        /// <remarks>
        /// This checks matches for each tag container in the hierarchy separately! Containers ARE NOT combined into one
        /// "mega-container", so in the case tags are distributed across several containers and none of them have the
        /// entire collection, this will return False.
        /// </remarks>
        /// <param name="component">Component.</param>
        /// <param name="tags">Tags to check.</param>
        /// <returns>
        /// True if component's game object or any of its parents have a <see cref="UnderTagContainer"/> and that container
        /// contains all tags from <see cref="tags"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAllExactTagsInParents(this Component component, ICollection<UnderTag> tags)
        {
            return HasAllExactTagsInParents(component.gameObject, tags);
        }
        
        /// <summary>
        /// Checks if a game object or any of its parents have aall of the passed tags exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAllTagsInParents(UnityEngine.GameObject,System.Collections.Generic.ICollection{UnderTags.UnderTag})"/> for non-strict queries.
        /// </summary>
        /// <remarks>
        /// This checks matches for each tag container in the hierarchy separately! Containers ARE NOT combined into one
        /// "mega-container", so in the case tags are distributed across several containers and none of them have the
        /// entire collection, this will return False.
        /// </remarks>
        /// <param name="gameObject">Game object.</param>
        /// <param name="tags">Tags to check.</param>
        /// <returns>
        /// True if the game object or any of its parents have a <see cref="UnderTagContainer"/> and that container
        /// contains all tags from <see cref="tags"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAllExactTagsInParents(this GameObject gameObject, ICollection<UnderTag> tags)
        {
            List<IUnderTagOwner> underTagOwners = new();
            gameObject.GetComponentsInChildren(false, underTagOwners);

            foreach (IUnderTagOwner underTagOwner in underTagOwners)
            {
                if (((Component)underTagOwner).HasAllExactTagsInParents(tags))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if a component's game object or any of its parents have all of the tags contained in the passed container exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAllTagsInParents(UnityEngine.Component,UnderTagContainer)"/> cref="HasTag"/> for non-strict queries.
        /// </summary>
        /// <remarks>
        /// This checks matches for each tag container in the hierarchy separately! Containers ARE NOT combined into one
        /// "mega-container", so in the case tags are distributed across several containers and none of them have the
        /// entire collection, this will return False.
        /// </remarks>
        /// <param name="component">Component.</param>
        /// <param name="tagContainer">Tags to check.</param>
        /// <returns>
        /// True if component's game object or any of its parents have a <see cref="UnderTagContainer"/> and that container
        /// contains all tags from <see cref="tagContainer"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAllExactTagsInParents(this Component component, UnderTagContainer tagContainer)
        {
            return HasAllExactTagsInParents(component.gameObject, tagContainer);
        }

        /// <summary>
        /// Checks if a game object or any of its parents have all of the tags contained in the passed container exactly.
        /// Exact matches have the same hierarchical depth: A.B == A.B, but A.B != A.B.C.
        /// Use <see cref="HasAllTagsInParents(UnityEngine.GameObject,UnderTagContainer)"/> cref="HasTag"/> for non-strict queries.
        /// </summary>
        /// <remarks>
        /// This checks matches for each tag container in the hierarchy separately! Containers ARE NOT combined into one
        /// "mega-container", so in the case tags are distributed across several containers and none of them have the
        /// entire collection, this will return False.
        /// </remarks>
        /// <param name="gameObject">Game object.</param>
        /// <param name="tagContainer">Tags to check.</param>
        /// <returns>
        /// True if the game object or any of its parents have a <see cref="UnderTagContainer"/> and that container
        /// contains all tags from <see cref="tagContainer"/> exactly, False otherwise.
        /// </returns>
        public static bool HasAllExactTagsInParents(this GameObject gameObject, UnderTagContainer tagContainer)
        {
            return HasAllExactTagsInParents(gameObject, tagContainer.OwnedTags);
        }
        
        // TODO: Add HasNoneOfExactTags() methods
    }
}