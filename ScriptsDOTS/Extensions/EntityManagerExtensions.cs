using Unidork.DOTS.Lookups;
using Unity.Entities;

namespace Unidork.DOTS.Lookups
{
    public static unsafe class EntityManagerExtensions
    {
        /// <summary>
        /// Gets a shared component lookup from an entity manager.
        /// </summary>
        /// <param name="entityManager">Entity manager.</param>
        /// <param name="isReadOnly">Is the lookup read-only?</param>
        /// <typeparam name="T">Type of elements in the lookup.</typeparam>
        /// <returns>
        /// A new instance of a shared component lookup of type <typeparam name="T"></typeparam>.
        /// </returns>
        public static SharedComponentLookup<T> GetSharedComponentLookup<T>(
            this EntityManager entityManager, bool isReadOnly = true) where T : unmanaged, ISharedComponentData
        {
            var entityDataAccess = entityManager.GetCheckedEntityDataAccess();
            var typeIndex = TypeManager.GetTypeIndex<T>();
            
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            return new SharedComponentLookup<T>(typeIndex, entityDataAccess, isReadOnly);
#else
            return new SharedComponentLookup<T>(typeIndex, entityDataAccess);
#endif
        }
    }
}