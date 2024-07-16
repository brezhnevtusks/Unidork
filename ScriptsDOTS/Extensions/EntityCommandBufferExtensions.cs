using Unity.Entities;

namespace Unidork.DOTS.Extensions
{
    public static class EntityCommandBufferExtensions
    {
        /// <summary>
        /// Plays back an <see cref="EntityCommandBuffer"/> with the entity manager from the passed <see cref="SystemState"/>.
        /// </summary>
        /// <param name="entityCommandBuffer">Entity command buffer.</param>
        /// <param name="systemState">System state.</param>
        public static void Playback(this EntityCommandBuffer entityCommandBuffer, SystemState systemState)
        {
            entityCommandBuffer.Playback(systemState.EntityManager);
        }
    }
}