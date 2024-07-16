using UnityEngine;
using UnityEngine.Events;

namespace UnderdorkStudios.UnderMessages
{
    [AddComponentMenu("")]
    [DefaultExecutionOrder(-5000)]
    public class UnderMessageReceiverComponent<T> : MonoBehaviour
    {
        [SerializeField] private string channelName = "Default";
        [SerializeField] private string messageName;
        [SerializeField] private UnityEvent<T> callback;
        [SerializeField] private UnityEvent parameterlessCallback;

        private void Awake()
        {
            if (typeof(T) == typeof(NoPayload))
            {
                UnderMessageSystem.RegisterReceiver(channelName, messageName, this, parameterlessCallback.Invoke);
            }
            else
            {
                UnderMessageSystem.RegisterReceiver<T>(channelName, messageName, this, callback.Invoke);
            }
        }
    }
}