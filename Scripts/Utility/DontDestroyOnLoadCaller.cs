using UnityEngine;

namespace Unidork.Utility
{
	/// <summary>
	/// Makes a component persistent by calling the DontDestroyOnLoadMethod on the game object it is attached to.
	/// </summary>
	public class DontDestroyOnLoadCaller : MonoBehaviour
	{
		#region MyRegion

		private void Awake() => DontDestroyOnLoad(gameObject);

		#endregion
	}
}