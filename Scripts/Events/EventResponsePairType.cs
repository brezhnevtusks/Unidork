namespace Unidork.Events 
{
    /// <summary>
    /// Type of event response pair.
    /// SingleEvent - single event invokes a UnityEvent.
    /// MultipleEvents - multiple events invoke a UnityEvent.
    /// </summary>
    public enum EventResponsePairType
    {
        SingleEvent,
        MultipleEvents
    }
}