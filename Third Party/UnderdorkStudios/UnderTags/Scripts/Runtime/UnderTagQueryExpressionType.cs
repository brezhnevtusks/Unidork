namespace UnderdorkStudios.UnderTags
{
    /// <summary>
    /// <para>Type of expression used in an <see cref="UnderTagQuery"/>.</para>
    /// <para>None - default uninitialized value.</para>
    /// <para>AnyTagsMatch - At least one <see cref="UnderTag"/> specified in a <see cref="UnderTagQueryExpression"/> must match.</para>
    /// <para>AllTagsMatch - All <see cref="UnderTag"/>s specified in a <see cref="UnderTagQueryExpression"/> must match.</para>
    /// <para>NoTagsMatch - No <see cref="UnderTag"/>s specified in a <see cref="UnderTagQueryExpression"/> can match.</para>
    /// <para>AnyExpressionsMatch - At least one <see cref="UnderTagQueryExpression"/> specified in the parent expression must match.</para>
    /// <para>AllExpressionsMatch - All <see cref="UnderTagQueryExpression"/>s specified in the parent expression must match.</para>
    /// <para>NoExpressionsMatch - No <see cref="UnderTagQueryExpression"/>s specified in the parent expression can match.</para>
    /// </summary>
    public enum UnderTagQueryExpressionType
    {
        /// <summary>
        /// Default uninitialized value.
        /// </summary>
        None,
        /// <summary>
        /// At least one <see cref="UnderTag"/> specified in a <see cref="UnderTagQueryExpression"/> must match.
        /// </summary>
        AnyTagsMatch,
        /// <summary>
        /// All <see cref="UnderTag"/>s specified in a <see cref="UnderTagQueryExpression"/> must match.
        /// </summary>
        AllTagsMatch,
        /// <summary>
        /// No <see cref="UnderTag"/>s specified in a <see cref="UnderTagQueryExpression"/> can match.
        /// </summary>
        NoTagsMatch,
        /// <summary>
        /// At least one <see cref="UnderTagQueryExpression"/> specified in the parent expression must match.
        /// </summary>
        AnyExpressionsMatch,
        /// <summary>
        /// All <see cref="UnderTagQueryExpression"/>s specified in the parent expression must match.
        /// </summary>
        AllExpressionsMatch,
        /// <summary>
        /// No <see cref="UnderTagQueryExpression"/>s specified in the parent expression can match.
        /// </summary>
        NoExpressionsMatch
    }
}