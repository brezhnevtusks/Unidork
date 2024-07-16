using UnityEngine;

namespace UnderdorkStudios.UnderMessages
{
    [AddComponentMenu("")]
    public class UnderMessageSenderPayloadTyped<T> : UnderMessageSenderPayloadBase
    {
        public T Value { get => value; set => this.value = value; }
        [SerializeField, HideInInspector] private T value;
    }
}