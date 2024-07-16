using System;

namespace UnderdorkStudios.UnderMessages
{
    public struct UnderMessageReceiverHandle : IEquatable<UnderMessageReceiverHandle>
    {
        #region Properties

        public int Id { get; private set; }
        public bool IsValid => Id > 0 && ReceiverObject != null;
        public string ChannelName { get; }
        public string MessageName { get; }
        public object ReceiverObject { get; }
        
        #endregion

        #region Constructor

        public UnderMessageReceiverHandle(int id, string channelName, string messageName, object receiverObject)
        {
            Id = id;
            ChannelName = channelName;
            MessageName = messageName;
            ReceiverObject = receiverObject;

            UnderMessageSystem.OnHandleInvalidated += OnHandleInvalidated;
        }

        #endregion

        #region Invalidate

        private void Invalidate()
        {
            Id = 0;
            UnderMessageSystem.OnHandleInvalidated -= OnHandleInvalidated;
        }

        private void OnHandleInvalidated(int handleId)
        {
            if (handleId == Id)
            {
                Invalidate();
            }
        }
        
        #endregion

        #region Equality

        public static bool operator ==(UnderMessageReceiverHandle handle1, UnderMessageReceiverHandle handle2)
        {
            return handle1.Id == handle2.Id;
        }
        
        public static bool operator !=(UnderMessageReceiverHandle handle1, UnderMessageReceiverHandle handle2)
        {
            return handle1.Id != handle2.Id;
        }
        
        public bool Equals(UnderMessageReceiverHandle other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return obj is UnderMessageReceiverHandle other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        #endregion
    }
}