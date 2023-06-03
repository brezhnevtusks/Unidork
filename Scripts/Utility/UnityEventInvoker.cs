using UnityEngine;
using UnityEngine.Events;

namespace Unidork.Utility
{
	public class UnityEventInvoker : MonoBehaviour
	{
		[SerializeField]
		private UnityEvent @event = null;

		public void Invoke() => @event?.Invoke();
	}
}