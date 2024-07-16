using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnderdorkStudios.UnderTags
{
    /// <summary>
    /// Expression used in <see cref="UnderTagQuery"/>. Consists of a type and, based on that type,
    /// a set of <see cref="UnderTag"/>s or a set of sub-expressions.
    /// </summary>
    [Serializable]
    public class UnderTagQueryExpression
    {
        #region Fields

        /// <summary>
        /// Expression type.
        /// </summary>
        [Tooltip("Expression type.")]
        [SerializeField] private UnderTagQueryExpressionType type;
        
        /// <summary>
        /// Tags used to check if this condition is satisfied when its type is AnyTagsMatch, AllTagsMatch, NoTagsMatch.
        /// </summary>
        [Tooltip("Tags used to check if this condition is satisfied when its type is AnyTagsMatch, AllTagsMatch, NoTagsMatch.")]
        [SerializeField] private List<UnderTag> tags;
        
        /// <summary>
        /// Sub expressions used to check if this condition is satisfied when its type is AnyExpressionsMatch, AllExpressionsMatch, NoExpressionsMatch.
        /// </summary>
        [Tooltip("Sub expressions used to check if this condition is satisfied when its type is AnyExpressionsMatch, AllExpressionsMatch, NoExpressionsMatch.")]
        [SerializeField] private List<UnderTagQueryExpression> subExpressions;
        
        #endregion

        #region Check
        
        /// <summary>
        /// Checks if this expression is satisfied with the passed <see cref="UnderTagContainer"/>.
        /// </summary>
        /// <param name="tagContainer">Tag container.</param>
        /// <returns>
        /// True if this expression is satisfied by <paramref name="tagContainer"/>, False otherwise.
        /// </returns>
        public bool IsSatisfied(UnderTagContainer tagContainer)
        {
            return IsSatisfied(tagContainer.OwnedTags);
        }
        
        /// <summary>
        /// Checks if this expression is satisfied with the passed collection of <see cref="UnderTag"/>s.
        /// </summary>
        /// <param name="tagsToCheck">Tags.</param>
        /// <returns>
        /// True if this expression is satisfied by <paramref name="tagsToCheck"/>, False otherwise.
        /// </returns>
        public bool IsSatisfied(ICollection<UnderTag> tagsToCheck)
        {
            switch (type)
            {
                case UnderTagQueryExpressionType.AnyTagsMatch:
                {
                    foreach (UnderTag tagToCheck in tagsToCheck)
                    {
                        if (tags.Contains(tagToCheck))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                case UnderTagQueryExpressionType.AllTagsMatch:
                {
                    foreach (UnderTag tagToCheck in tagsToCheck)
                    {
                        if (!tags.Contains(tagToCheck))
                        {
                            return false;
                        }
                    }

                    return true;
                }
                case UnderTagQueryExpressionType.NoTagsMatch:
                {
                    foreach (UnderTag tagToCheck in tagsToCheck)
                    {
                        if (tags.Contains(tagToCheck))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                case UnderTagQueryExpressionType.AnyExpressionsMatch:
                {
                    if (subExpressions == null || subExpressions.Count == 0)
                    {
                        Debug.LogError("UnderTags: Query has type AnyExpressionsMatch but not valid expressions!");
                        return false;
                    }

                    foreach (UnderTagQueryExpression childExpression in subExpressions)
                    {
                        if (childExpression.IsSatisfied(tagsToCheck))
                        {
                            return true;
                        }
                    }
                    return false;
                }
                case UnderTagQueryExpressionType.AllExpressionsMatch:
                {
                    if (subExpressions == null || subExpressions.Count == 0)
                    {
                        Debug.LogError("UnderTags: Query has type AllExpressionsMatch but not valid expressions!");
                        return false;
                    }
                    
                    foreach (UnderTagQueryExpression childExpression in subExpressions)
                    {
                        if (!childExpression.IsSatisfied(tagsToCheck))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                case UnderTagQueryExpressionType.NoExpressionsMatch:
                {
                    if (subExpressions == null || subExpressions.Count == 0)
                    {
                        Debug.LogError("UnderTags: Query has type NoExpressionsMatch but no valid expressions!");
                        return false;
                    }
                    foreach (UnderTagQueryExpression childExpression in subExpressions)
                    {
                        if (childExpression.IsSatisfied(tagsToCheck))
                        {
                            return false;
                        }
                    }
                    return true;
                }
                case UnderTagQueryExpressionType.None:
                {
                    Debug.LogError("UnderTags: Query expressions has type of None!");
                    return false;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }
        
        #endregion
        
#if UNITY_EDITOR
        #region Editor

        /// <summary>
        /// Clears all tags in this expression.
        /// </summary>
        public void ClearTags()
        {
            tags?.Clear();
        }
        
        /// <summary>
        /// Clears all subexpressions.
        /// </summary>
        public void ClearSubExpressions()
        {
            subExpressions?.Clear();
        }

        public string ToReadableString()
        {
            string tagString = "...";
            
            if (tags is { Count: > 0 })
            {
                for (var i = 0; i < tags.Count; i++)
                {
                    UnderTag tag = tags[i];
                    tagString += $"{tag.Value}";

                    if (i < tags.Count - 1)
                    {
                        tagString += ", ";
                    }
                }
            }
            
            switch (type)
            {
                case UnderTagQueryExpressionType.None:
                {
                    return "NONE";
                }
                case UnderTagQueryExpressionType.AnyTagsMatch:
                {
                    return $"ANY TAGS: {tagString}";
                }
                case UnderTagQueryExpressionType.AllTagsMatch:
                {
                    return $"ALL TAGS: {tagString}";
                }
                case UnderTagQueryExpressionType.NoTagsMatch:
                {
                    return $"NO TAGS: {tagString}";
                }
                case UnderTagQueryExpressionType.AnyExpressionsMatch:
                {
                    return "ANY EXPRESSIONS";
                }
                case UnderTagQueryExpressionType.AllExpressionsMatch:
                {
                    return "ALL EXPRESSIONS";
                }
                case UnderTagQueryExpressionType.NoExpressionsMatch:
                {
                    return "NO EXPRESSIONS";
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        #endregion
#endif
    }
}