using System.Collections.Generic;
using UnityEngine;

namespace UnderdorkStudios.UnderTags
{
    /// <summary>
    /// Class that allows to evaluate an <see cref="UnderTagContainer"/> or a collection of <see cref="UnderTag"/>s
    /// against a set of expressions. Contains the root expression which in turn can contain sub expressions of its own.
    /// </summary>
    [System.Serializable]
    public class UnderTagQuery
    {
        #region Fields
#if UNITY_EDITOR
        [SerializeField, Multiline] private string expressionTree = "";
#endif
        
        /// <summary>
        /// Query's root expression.
        /// </summary>
        [Tooltip("Query's root expression.`")]
        [SerializeField] private UnderTagQueryExpression rootExpression;

        /// <summary>
        /// All expressions in this query. To avoid serialization issues, we don't store reference to expressions
        /// inside other expressions. Instead, we store indices and retrieve expressions from this array.
        /// </summary>
        private UnderTagQueryExpression[] expressions;
        
        #endregion

        #region Evaluate

        /// <summary>
        /// Evaluates the query against the passed <see cref="UnderTagContainer"/>.
        /// </summary>
        /// <param name="container">Container.</param>
        /// <returns>
        /// True if all expressions within the query are satisfied by tags in <paramref name="container"/>, False otherwise.
        /// </returns>
        public bool Evaluate(UnderTagContainer container)
        {
            return rootExpression.IsSatisfied(container);
        }

        /// <summary>
        /// Evaluates the query against the passed collection of <see cref="UnderTag"/>s.
        /// </summary>
        /// <param name="tags">Tags.</param>
        /// <returns>
        /// True if all expressions within the query are satisfied by <paramref name="tags"/>, False otherwise.
        /// </returns>
        public bool Evaluate(ICollection<UnderTag> tags)
        {
            return rootExpression.IsSatisfied(tags);
        }

        #endregion

        #region UI

        public void UpdateExpressionTree()
        {
            
        }

        #endregion
    }
}