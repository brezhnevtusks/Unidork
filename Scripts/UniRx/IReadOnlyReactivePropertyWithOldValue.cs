using UniRx;

namespace Unidork.UniRx
{
    /// <summary>
    /// Interface for read-only Reactive properties that keeps tracks of its value before latest change.
    /// </summary>
    /// <typeparam name="T">Type of property.</typeparam>
    public interface IReadOnlyReactivePropertyWithOldValue<T> : IReadOnlyReactiveProperty<T>
    {
        /// <summary>
        /// Property's value before latest change.
        /// </summary>
        public T OldValue { get; set; }
    }
}