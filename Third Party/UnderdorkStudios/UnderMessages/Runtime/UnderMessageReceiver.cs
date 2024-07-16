using System;

namespace UnderdorkStudios.UnderMessages
{
    public abstract class UnderMessageReceiver
    {
        public UnderMessageReceiverHandle Handle { get; }
        public object ReceiverObject { get; }

        protected UnderMessageReceiver(object receiverObject, UnderMessageReceiverHandle handle)
        {
            Handle = handle;
            ReceiverObject = receiverObject;
        }

        public abstract bool IsValid();
        public abstract void OnMessageReceived(object payload);
    }

    public class UnderMessageReceiver_Empty : UnderMessageReceiver 
    {
        public UnderMessageReceiver_Empty(object receiverObject, Action callback, UnderMessageReceiverHandle handle) : base(receiverObject, handle)
        {
            this.callback = callback;
        }

        private readonly Action callback;

        public override bool IsValid()
        {
            return ReceiverObject != null && callback != null;
        }

        public override void OnMessageReceived(object _)
        {
            callback?.Invoke();
        }
    }
    
    public class UnderMessageReceiver_Payload<TPayload> : UnderMessageReceiver
    {
        protected readonly Action<TPayload> callback;
        
        public UnderMessageReceiver_Payload(object receiverObject, Action<TPayload> callback, UnderMessageReceiverHandle handle) : base(receiverObject, handle)
        {
            this.callback = callback;
        }

        public override bool IsValid()
        {
            return ReceiverObject != null && callback != null;
        }

        public override void OnMessageReceived(object payload)
        {
            if (payload is TPayload castPayload)
            {
                callback?.Invoke(castPayload);
            }
        }
    }
}