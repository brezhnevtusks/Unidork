using Unity.Collections.LowLevel.Unsafe;
using Unity.Entities;

namespace Unidork.DOTS.Lookups
{
    /// <summary>
    /// A [NativeContainer] that provides access to all instances of shared components of type T, indexed by <see cref="Entity"/>.
    /// </summary>
    /// <typeparam name="T">The type of <see cref="ISharedComponentData"/> to access.</typeparam>
    [NativeContainer] 
    public unsafe struct SharedComponentLookup<T> where T : unmanaged, ISharedComponentData
    {
        [NativeDisableUnsafePtrRestriction]
        private readonly EntityDataAccess* entityDataAccess;
        private readonly TypeIndex typeIndex;
        
#if ENABLE_UNITY_COLLECTIONS_CHECKS
        // This has to be named m_Safety exactly. See:
        // https://docs.unity3d.com/ScriptReference/Unity.Collections.LowLevel.Unsafe.NativeContainerAttribute.html
        private AtomicSafetyHandle m_Safety;
        private readonly byte isReadOnly;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="typeIndex">Type index.</param>
        /// <param name="entityDataAccess">Entity data access.</param>
        /// <param name="isReadOnly">Is this lookup read-only?</param>
        internal SharedComponentLookup(TypeIndex typeIndex, EntityDataAccess* entityDataAccess, bool isReadOnly)
        {
            var safetyHandles = &entityDataAccess->DependencyManager->Safety;
            m_Safety = safetyHandles->GetSafetyHandleForComponentLookup(typeIndex, isReadOnly);
            this.isReadOnly = isReadOnly ? (byte)1 : (byte)0;
            this.entityDataAccess = entityDataAccess;
            this.typeIndex = typeIndex;
        }
#else
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="typeIndex">Type index.</param>
        /// <param name="entityDataAccess">Entity data access.</param>
        internal SharedComponentLookup(TypeIndex typeIndex, EntityDataAccess* entityDataAccess)
        {
            this.entityDataAccess = entityDataAccess;
            this.typeIndex = typeIndex;
        }
#endif

        /// <summary>
        /// Gets the <see cref="IComponentData"/> instance of type T for the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>An <see cref="IComponentData"/> type.</returns>
        /// <remarks>
        /// Normally, you cannot write to components accessed using a ComponentLookup instance
        /// in a parallel Job. This restriction is in place because multiple threads could write to the same component,
        /// leading to a race condition and nondeterministic results. However, when you are certain that your algorithm
        /// cannot write to the same component from different threads, you can manually disable this safety check
        /// by putting the [NativeDisableParallelForRestriction] attribute on the ComponentLookup field in the Job.
        ///
        /// [NativeDisableParallelForRestrictionAttribute]: https://docs.unity3d.com/ScriptReference/Unity.Collections.NativeDisableParallelForRestrictionAttribute.html
        /// </remarks>
        public T this[Entity entity]
        {
            get
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
#endif
                return entityDataAccess->GetSharedComponentData_Unmanaged<T>(entity);
            }
        }

        /// <summary>
        /// Reports whether the specified <see cref="SystemHandle"/> associated <see cref="Entity"/> is valid and contains a
        /// component of type T.
        /// </summary>
        /// <param name="system">The system handle.</param>
        /// <returns>True if the entity associated with the system has a component of type T, and false if it does not. Also returns false if
        /// the system handle refers to a system that has been destroyed.</returns>
        public bool HasComponent(Entity entity)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
#endif
            return entityDataAccess->EntityComponentStore->HasComponent(entity, typeIndex);
        }

        /// <summary>
        /// Retrieves the shared component associated with the specified <see cref="Entity"/>, if it exists.
        /// Then reports if the instance still refers to a valid entity and that it has a component of type T.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// /// <param name="sharedComponentData">The shared component of type T for the given entity, if it exists.</param>
        /// <returns>True if the entity has a component of type T, and false if it does not.</returns>
        public bool TryGetComponent(Entity entity, out T sharedComponentData)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            AtomicSafetyHandle.CheckReadAndThrow(m_Safety);
#endif
            // Not efficient yet, just convenient
            if (!HasComponent(entity))
            {
                sharedComponentData = default;
                return false;
            }

            sharedComponentData = this[entity];
            return true;
        }

        /// <summary>
        /// When a ComponentLookup is cached by a system across multiple system updates, calling this function
        /// inside the system's Update() method performs the minimal incremental updates necessary to make the
        /// type handle safe to use.
        /// </summary>
        /// <param name="system"> The system on which this type handle is cached. </param>
        public void Update(SystemBase system)
        {
            Update(ref *system.m_StatePtr);
        }

        /// <summary>
        /// When a ComponentLookup is cached by a system across multiple system updates, calling this function
        /// inside the system's Update() method performs the minimal incremental updates necessary to make the
        /// type handle safe to use.
        /// </summary>
        /// <param name="systemState"> The SystemState of the system on which this type handle is cached. </param>
        public void Update(ref SystemState systemState)
        {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
            ComponentSafetyHandles* safetyHandles = &entityDataAccess->DependencyManager->Safety;
            m_Safety = safetyHandles->GetSafetyHandleForComponentLookup(typeIndex, isReadOnly != 0);
#endif
        }
    }
}