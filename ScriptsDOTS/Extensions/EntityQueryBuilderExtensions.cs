using Unity.Entities;

namespace Unidork.DOTS.Extensions
{
    public static class EntityQueryBuilderExtensions
    {
        /// <summary>
        /// Builds a query from data stored in <see cref="EntityQueryBuilder"/> and resets the builder
        /// before returning the query.
        /// </summary>
        /// <param name="builder">Builder.</param>
        /// <param name="state">System state.</param>
        /// <returns>
        /// An <see cref="EntityQuery"/>.
        /// </returns>
        public static EntityQuery BuildAndReset(this EntityQueryBuilder builder, ref SystemState state)
        {
            return BuildAndReset(builder, state.EntityManager);
        }

        /// <summary>
        /// Builds a query from data stored in <see cref="EntityQueryBuilder"/> and resets the builder
        /// before returning the query.
        /// </summary>
        /// <param name="builder">Builder.</param>
        /// <param name="entityManager">Entity manager.</param>
        /// <returns>
        /// An <see cref="EntityQuery"/>.
        /// </returns>
        public static EntityQuery BuildAndReset(this EntityQueryBuilder builder, EntityManager entityManager)
        {
            EntityQuery query = builder.Build(entityManager);
            builder.Reset();
            return query;
        }
    }
}