namespace Unidork.Utility
{
    /// <summary>
    /// Interface for objects that can register with the <see cref="LevelDestroyNotifier"/> and get the level destroy event from it.
    /// </summary>
    public interface ILevelDestroyListener
    {
        /// <summary>
        /// Callback for when level is destroyed, invoked by <see cref="LevelDestroyNotifier"/>.
        /// </summary>
        void OnLevelDestroyed();
    }
}