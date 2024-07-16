using System;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnderdorkStudios.UnderMessages.Editor
{
    [CustomEditor(typeof(UnderMessageSender))]
    public class UnderMessageSenderEditor : UnityEditor.Editor
    {
        #region Constants

        private const string ChannelNamePropertyName = "channelName";
        private const string MessageNamePropertyName = "messageName";

        private const string PayloadTypePropertyName = "payloadType";
        private const string PayloadPropertyName = "payload";

        private VisualElement rootElement;

        #endregion

        #region UI

        public override VisualElement CreateInspectorGUI()
        {
            rootElement = new VisualElement();
            RedrawEditor();
            return rootElement;
        }

        private void RedrawEditor()
        {
            rootElement.Clear();
            
            DrawChannelNamePropertyField(rootElement);
            DrawMessageNamePropertyField(rootElement);

            DrawPayloadTypePropertyField(rootElement);
            DrawPayloadValueField(rootElement);
        }

        private void DrawChannelNamePropertyField(VisualElement rootElement)
        {
            PropertyField channelNamePropertyField = new();
            channelNamePropertyField.BindProperty(serializedObject.FindProperty(ChannelNamePropertyName));
            rootElement.Add(channelNamePropertyField);
        }

        private void DrawMessageNamePropertyField(VisualElement rootElement)
        {
            PropertyField messageNamePropertyField = new();
            messageNamePropertyField.BindProperty(serializedObject.FindProperty(MessageNamePropertyName));
            rootElement.Add(messageNamePropertyField);
        }

        private void DrawPayloadTypePropertyField(VisualElement rootElement)
        {
            PropertyField payloadTypePropertyField = new();
            payloadTypePropertyField.BindProperty(serializedObject.FindProperty(PayloadTypePropertyName));
            payloadTypePropertyField.RegisterValueChangeCallback(OnPayloadTypeChanged);
            rootElement.Add(payloadTypePropertyField);
        }
        
        private void OnPayloadTypeChanged (SerializedPropertyChangeEvent changeEvent)
        {
            var underMessageSender = (UnderMessageSender)serializedObject.targetObject;
                
            switch ((UnderMessageSenderPayloadType)changeEvent.changedProperty.enumValueIndex)
            {
                case UnderMessageSenderPayloadType.None:
                {
                    if (TryGetPayloadComponent(out UnderMessageSenderPayloadBase payloadComponent))
                    {
                        DestroyPayloadComponent(payloadComponent);
                        underMessageSender.SetPayloadComponent(null);
                        serializedObject.ApplyModifiedProperties();
                        serializedObject.Update();
                        RedrawEditor();
                    }
                    break;
                }
                case UnderMessageSenderPayloadType.Integer:
                {
                    SwitchPayloadComponent<UnderMessageSenderPayload_Int>(underMessageSender);
                    break;
                }
                case UnderMessageSenderPayloadType.Float:
                {
                    SwitchPayloadComponent<UnderMessageSenderPayload_Float>(underMessageSender);
                    break;
                }
                case UnderMessageSenderPayloadType.Double:
                {
                    SwitchPayloadComponent<UnderMessageSenderPayload_Double>(underMessageSender);
                    break;
                }
                case UnderMessageSenderPayloadType.String:
                {
                    SwitchPayloadComponent<UnderMessageSenderPayload_String>(underMessageSender);
                    break;
                }
                case UnderMessageSenderPayloadType.Boolean:
                {
                    SwitchPayloadComponent<UnderMessageSenderPayload_Boolean>(underMessageSender);
                    break;
                }
                case UnderMessageSenderPayloadType.GameObject:
                {
                    SwitchPayloadComponent<UnderMessageSenderPayload_GameObject>(underMessageSender);
                    break;
                }
                case UnderMessageSenderPayloadType.Transform:
                {
                    SwitchPayloadComponent<UnderMessageSenderPayload_Transform>(underMessageSender);
                    break;
                }
                case UnderMessageSenderPayloadType.Component:
                {
                    SwitchPayloadComponent<UnderMessageSenderPayload_Component>(underMessageSender);
                    break;
                }
                default:
                {
                    throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void SwitchPayloadComponent<T>(UnderMessageSender underMessageSender) where T : UnderMessageSenderPayloadBase
        {
            if (TryGetPayloadComponent(out UnderMessageSenderPayloadBase payloadComponent))
            {
                if (payloadComponent is not T)
                {
                    DestroyPayloadComponent(payloadComponent);
                    AddPayloadComponent<T>(underMessageSender);
                    RedrawEditor();
                }
            }
            else
            {
                AddPayloadComponent<T>(underMessageSender);
                RedrawEditor();
            }
        }
        
        private bool TryGetPayloadComponent(out UnderMessageSenderPayloadBase payloadComponent)
        {
            var underMessageSender = (UnderMessageSender)serializedObject.targetObject;
            return underMessageSender.TryGetPayloadComponent(out payloadComponent);
        }

        private void DestroyPayloadComponent(UnderMessageSenderPayloadBase payloadComponent)
        {
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                Destroy(payloadComponent);
            }
            else
            {
                DestroyImmediate(payloadComponent);
            }
        }

        private void AddPayloadComponent<T>(UnderMessageSender underMessageSender) where T : UnderMessageSenderPayloadBase
        {
            var payloadComponent = underMessageSender.gameObject.AddComponent<T>();
            payloadComponent.hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector;
            underMessageSender.SetPayloadComponent(payloadComponent);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        private void DrawPayloadValueField(VisualElement rootElement)
        {
            SerializedProperty payloadProperty = serializedObject.FindProperty(PayloadPropertyName);

            if (payloadProperty.objectReferenceValue == null)
            {
                return;
            }
                
            var payloadSerializedObject = new SerializedObject(payloadProperty.objectReferenceValue);
            PropertyField intField = new();
            intField.BindProperty(payloadSerializedObject.FindProperty("value"));
            rootElement.Add(intField);
        }
        
        #endregion
    }
}