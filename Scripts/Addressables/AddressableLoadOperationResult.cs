namespace Unidork.AddressableAssetsUtility
{
    /// <summary>
    /// Stores the result of an addressable load operation.
    /// </summary>
    /// <typeparam name="T">Type of loaded Addressable asset.</typeparam>
    public struct AddressableLoadOperationResult<T>
    {
        public static AddressableLoadOperationResult<T> Failed()
        {
            return new AddressableLoadOperationResult<T>
            {
                Succeeded = false
            };
        }

        public readonly bool Succeeded { get; init; }
        public readonly object Key { get; init; }
        public readonly T Value { get; init; }
    }
}