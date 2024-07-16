using System;
using UnityEngine;

namespace UnderdorkStudios.UnderMessages
{
    public class UnderMessageSender : MonoBehaviour
    {
        #region Fields

        [SerializeField] private string channelName = "Default";
        [SerializeField] private string messageName;

        [SerializeField] private UnderMessageSenderPayloadType payloadType;
        [SerializeField] private UnderMessageSenderPayloadBase payload;

        #endregion

        #region Send

        public void Send()
        {
            switch (payloadType)
            {
                case UnderMessageSenderPayloadType.None:
                {
                    UnderMessageSystem.SendMessage(channelName, messageName);
                    break;
                }
                case UnderMessageSenderPayloadType.Integer:
                {
                    var intPayload = GetPayloadValue<UnderMessageSenderPayload_Int, int>();
                    UnderMessageSystem.SendMessage(channelName, messageName, intPayload);
                    break;
                }
                case UnderMessageSenderPayloadType.Float:
                {
                    var floatPayload = GetPayloadValue<UnderMessageSenderPayload_Float, float>();
                    UnderMessageSystem.SendMessage(channelName, messageName, floatPayload);
                    break;
                }
                case UnderMessageSenderPayloadType.Double:
                {
                    var doublePayload = GetPayloadValue<UnderMessageSenderPayload_Double, double>();
                    UnderMessageSystem.SendMessage(channelName, messageName, doublePayload);
                    break;
                }
                case UnderMessageSenderPayloadType.String:
                {
                    var stringPayload = GetPayloadValue<UnderMessageSenderPayload_String, string>();
                    UnderMessageSystem.SendMessage(channelName, messageName, stringPayload);
                    break;
                }
                case UnderMessageSenderPayloadType.Boolean:
                {
                    var boolPayload = GetPayloadValue<UnderMessageSenderPayload_Boolean, bool>();
                    UnderMessageSystem.SendMessage(channelName, messageName, boolPayload);
                    break;
                }
                case UnderMessageSenderPayloadType.GameObject:
                {
                    var gameObjectPayload = GetPayloadValue<UnderMessageSenderPayload_GameObject, GameObject>();
                    UnderMessageSystem.SendMessage(channelName, messageName, gameObjectPayload);
                    break;
                }
                case UnderMessageSenderPayloadType.Transform:
                {
                    var transformPayload = GetPayloadValue<UnderMessageSenderPayload_Transform, Transform>();
                    UnderMessageSystem.SendMessage(channelName, messageName, transformPayload);
                    break;
                }
                case UnderMessageSenderPayloadType.Component:
                {
                    var componentPayload = GetPayloadValue<UnderMessageSenderPayload_Component, Component>();
                    UnderMessageSystem.SendMessage(channelName, messageName, componentPayload);
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        private TPayload GetPayloadValue<TComponent, TPayload>() where TComponent : UnderMessageSenderPayloadTyped<TPayload>
        {
            if (payload == null)
            {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
                Debug.LogError($"UnderMessages: {gameObject.name} has an UnderMessageSender with payload type {typeof(TComponent)} but no respective payload component!", gameObject);
#endif
                return default;
            }

            return ((UnderMessageSenderPayloadTyped<TPayload>)payload).Value;
        }
        
        #endregion
        
#if UNITY_EDITOR
        #region Editor

        public bool TryGetPayloadComponent(out UnderMessageSenderPayloadBase payloadComponent)
        {
            payloadComponent = payload;
            return payloadComponent != null;
        }
        
        public void SetPayloadComponent(UnderMessageSenderPayloadBase payloadComponent) => payload = payloadComponent;

        #endregion
#endif
    }
}