using UnityEngine;

namespace UnderdorkStudios.UnderMessages
{
    public readonly struct NoPayload
    {
    }
    
    [AddComponentMenu("UnderMessages/UnderMessage No Payload Receiver")]
    public class UnderMessageReceiver_NoPayload : UnderMessageReceiverComponent<NoPayload>
    {
    }
}