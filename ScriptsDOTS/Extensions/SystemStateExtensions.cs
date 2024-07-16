using Unidork.DOTS.Lookups;
using Unity.Entities;

namespace Unidork.DOTS.Extensions
{
    public static class SystemStateExtensions
    {
        /// <summary>
        /// Gets a shared component lookup from system state.
        /// </summary>
        /// <param name="systemState">System state.</param>
        /// <param name="isReadOnly">Is the lookup read-only?</param>
        /// <typeparam name="T">Type of elements in the lookup.</typeparam>
        /// <returns>
        /// A new instance of a shared component lookup of type <typeparam name="T"></typeparam>.
        /// </returns>
        public static SharedComponentLookup<T> GetSharedComponentLookup<T>(
            ref this SystemState systemState, bool isReadOnly = false) where T : unmanaged, ISharedComponentData
        {
            systemState.AddReaderWriter(isReadOnly ? ComponentType.ReadOnly<T>() : ComponentType.ReadWrite<T>());
            return systemState.EntityManager.GetSharedComponentLookup<T>(isReadOnly);
        }
    }
}


