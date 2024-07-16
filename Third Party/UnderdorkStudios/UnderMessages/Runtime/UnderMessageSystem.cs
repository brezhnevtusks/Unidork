using System;
using System.Collections.Generic;
using UnderdorkStudios.UnderTools.Extensions;
using UnityEngine;

namespace UnderdorkStudios.UnderMessages
{
    public static class UnderMessageSystem
    {
        #region Constants
        
        private const string DefaultChannelName = "Default";

        #endregion

        #region Properties

        public delegate void OnHandleInvalidatedEvent(int handleId);
        public static event OnHandleInvalidatedEvent OnHandleInvalidated;

        #endregion
        
        #region Fields

        private static readonly Dictionary<(string, string), HashSet<UnderMessageReceiver>> underMessageReceivers;
        private static readonly Dictionary<object, Dictionary<(string, string), HashSet<UnderMessageReceiverHandle>>> underMessageReceiverHandles;
        private static readonly HashSet<UnderMessageReceiverHandle> invalidReceiverHandles;
        
        private static int handleId;
        
        #endregion

        #region Constructor

        static UnderMessageSystem()
        {
            underMessageReceivers = new Dictionary<(string, string), HashSet<UnderMessageReceiver>>();
            underMessageReceiverHandles = new Dictionary<object, Dictionary<(string, string), HashSet<UnderMessageReceiverHandle>>>();
            invalidReceiverHandles = new HashSet<UnderMessageReceiverHandle>();
            
            handleId = 0;
        }

        #endregion

        #region Register

        public static UnderMessageReceiverHandle RegisterReceiver(string messageName, object receiverObject, Action callback)
        {
            return RegisterReceiver(DefaultChannelName, messageName, receiverObject, callback);
        }
        
        public static UnderMessageReceiverHandle RegisterReceiver(string channelName, string messageName, object receiverObject, Action callback)
        {
            UnderMessageReceiverHandle receiverHandle = CreateReceiverHandle(channelName, messageName, receiverObject);
            var receiver = new UnderMessageReceiver_Empty(receiverObject, callback, receiverHandle);
            
            if (underMessageReceivers.TryGetValue((channelName, messageName), out HashSet<UnderMessageReceiver> receivers))
            {
                receivers.Add(receiver);
            }
            else
            {
                underMessageReceivers.Add((channelName, messageName), new HashSet<UnderMessageReceiver> { receiver });
            }

            return receiverHandle;
        }

        public static UnderMessageReceiverHandle RegisterReceiver<TPayload>(string messageName, object receiverObject, Action<TPayload> callback)
        {
            return RegisterReceiver(DefaultChannelName, messageName, receiverObject, callback);
        }
        
        public static UnderMessageReceiverHandle RegisterReceiver<TPayload>(string channelName, string messageName, object receiverObject, Action<TPayload> callback)
        {
            UnderMessageReceiverHandle receiverHandle = CreateReceiverHandle(channelName, messageName, receiverObject);
            var receiver = new UnderMessageReceiver_Payload<TPayload>(receiverObject, callback, receiverHandle);

            if (underMessageReceivers.TryGetValue((channelName, messageName), out HashSet<UnderMessageReceiver> receivers))
            {
                receivers.Add(receiver);
            }
            else
            {
                underMessageReceivers.Add((channelName, messageName), new HashSet<UnderMessageReceiver> { receiver });
            }

            return receiverHandle;
        }

        private static UnderMessageReceiverHandle CreateReceiverHandle(string channelName, string messageName, object receiverObject)
        {
            var receiverHandle = new UnderMessageReceiverHandle(++handleId, channelName, messageName, receiverObject);

            if (underMessageReceiverHandles.TryGetValue(receiverObject, out Dictionary<(string, string), HashSet<UnderMessageReceiverHandle>> handleDict))
            {
                if (handleDict.TryGetValue((channelName, messageName), out HashSet<UnderMessageReceiverHandle> receivedHandles))
                {
                    receivedHandles.Add(receiverHandle);
                }
                else
                {
                    handleDict.Add((channelName, messageName), new HashSet<UnderMessageReceiverHandle> { receiverHandle });
                }
            }
            else
            {
                underMessageReceiverHandles.Add(receiverObject, new Dictionary<(string, string), HashSet<UnderMessageReceiverHandle>>
                {
                    { (channelName, messageName), new HashSet<UnderMessageReceiverHandle> { receiverHandle } }
                });
            }

            return receiverHandle;
        }

        #endregion

        #region Unregister

        public static void UnregisterReceiver(UnderMessageReceiverHandle receiverHandle)
        {
            if (!receiverHandle.IsValid)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogError("UnderMessages: UnderMessageReceiverHandle is invalid!");
#endif
                return;
            }

            string channelName = receiverHandle.ChannelName;
            string messageName = receiverHandle.MessageName;
            
            if (underMessageReceivers.TryGetValue((channelName, messageName), out HashSet<UnderMessageReceiver> receivers))
            {
                receivers.RemoveWhere(receiver => receiver.Handle == receiverHandle);
            }

            RemoveHandle(receiverHandle);
        }

        public static void UnregisterReceivers(object receiverObject)
        {
            var handlesToUnregister = new HashSet<UnderMessageReceiverHandle>();
            
            if (underMessageReceiverHandles.TryGetValue(receiverObject, out Dictionary<(string, string), HashSet<UnderMessageReceiverHandle>> handles))
            {
                foreach (KeyValuePair<(string, string), HashSet<UnderMessageReceiverHandle>> keyValuePair in handles)
                {
                    handlesToUnregister.AddRange(keyValuePair.Value);
                }
            }

            foreach (UnderMessageReceiverHandle handleToUnregister in handlesToUnregister)
            {
                UnregisterReceiver(handleToUnregister);
            }
        }

        public static void UnregisterReceivers(object receiverObject, string channelName)
        {
            var handlesToUnregister = new HashSet<UnderMessageReceiverHandle>();
            
            if (underMessageReceiverHandles.TryGetValue(receiverObject, out Dictionary<(string, string), HashSet<UnderMessageReceiverHandle>> handles))
            {
                foreach (KeyValuePair<(string, string), HashSet<UnderMessageReceiverHandle>> keyValuePair in handles)
                {
                    if (keyValuePair.Key.Item1 == channelName)
                    {
                        handlesToUnregister.AddRange(keyValuePair.Value);
                    }
                }
            }

            foreach (UnderMessageReceiverHandle handleToUnregister in handlesToUnregister)
            {
                UnregisterReceiver(handleToUnregister);
            }
        }

        public static void UnregisterReceivers(object receiverObject, string channelName, string messageName)
        {
            var handlesToUnregister = new HashSet<UnderMessageReceiverHandle>();
            
            if (underMessageReceiverHandles.TryGetValue(receiverObject, out Dictionary<(string, string), HashSet<UnderMessageReceiverHandle>> handles))
            {
                foreach (KeyValuePair<(string, string), HashSet<UnderMessageReceiverHandle>> keyValuePair in handles)
                {
                    if (keyValuePair.Key == (channelName, messageName))
                    {
                        handlesToUnregister.AddRange(keyValuePair.Value);
                    }
                }
            }

            foreach (UnderMessageReceiverHandle handleToUnregister in handlesToUnregister)
            {
                UnregisterReceiver(handleToUnregister);
            }
        }

        private static void RemoveHandle(UnderMessageReceiverHandle handleToRemove)
        {
            if (!handleToRemove.IsValid)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogError("UnderMessages: Trying to remove an invalid UnderMessageReceiverHandle!");
#endif
                return;
            }

            if (underMessageReceiverHandles.TryGetValue(handleToRemove.ReceiverObject, out Dictionary<(string, string), HashSet<UnderMessageReceiverHandle>> handleDict))
            {
                if (handleDict.TryGetValue((handleToRemove.ChannelName, handleToRemove.MessageName), out HashSet<UnderMessageReceiverHandle> handles))
                {
                    handles.RemoveWhere(handle => handle.Id == handleToRemove.Id);
                    OnHandleInvalidated?.Invoke(handleToRemove.Id);
                }
            }
        }

        #endregion

        #region Messages

        public static void SendMessage(string messageName)
        {
            SendMessage(DefaultChannelName, messageName);
        }
        
        public static void SendMessage(string channelName, string messageName)
        {
            SendMessage<object>(channelName, messageName, null);
        }

        public static void SendMessage<TPayload>(string messageName, TPayload payload)
        {
            SendMessage(DefaultChannelName, messageName, payload);
        }
        
        public static void SendMessage<TPayload>(string channelName, string messageName, TPayload payload)
        {
            invalidReceiverHandles.Clear();
            
            if (underMessageReceivers.TryGetValue((channelName, messageName), out HashSet<UnderMessageReceiver> receivers))
            {
                
                foreach (UnderMessageReceiver receiver in receivers)
                {
                    if (receiver.IsValid())
                    {
                        receiver.OnMessageReceived(payload);
                    }
                    else
                    {
                        invalidReceiverHandles.Add(receiver.Handle);
                    }
                }
            }

            if (!receivers.IsNullOrEmpty() && invalidReceiverHandles.Count > 0)
            {
                receivers.RemoveWhere(receiver => invalidReceiverHandles.Contains(receiver.Handle));
            }
        }
        
        #endregion
    }
}