using UnityEngine;
using State = UnityEngine.Random.State;

namespace Unidork.Extensions
{
    public class RandomExtensions : MonoBehaviour
    {
		#region Fields

		/// <summary>
		/// Last state saved by <see cref="SetSeed(int, out State)"/> method.
		/// </summary>
		private static State oldState;

		#endregion

		#region Seed

		/// <summary>
		/// Sets the seed for Unity's Random class. Stores the old seed in an out parameter.
		/// </summary>
		/// <param name="seed">Seed to set.</param>
		/// <param name="oldState">Old state.</param>
		public static void SetSeed(int seed, out State oldState)
		{
			oldState = Random.state;
			RandomExtensions.oldState = oldState;
			Random.InitState(seed);
		}
		
		/// <summary>
		/// Restores the old state of Unity's Random class if an old state exists.
		/// </summary>
		public static void RestoreOldState()
		{
			if (oldState.Equals(default(State)))
			{
				Debug.LogWarning("Old random state was not initialized!");
				return;
			}

			Random.state = oldState;
		}

		/// <summary>
		/// Restores the state of Unity's Random class to the passed value.
		/// </summary>
		/// <param name="newState">New random state.</param>
		public static void SetState(State newState)
		{
			if (newState.Equals(default(State)))
			{
				Debug.LogWarning("Trying to set Random state to an unitialized value!");
				return;
			}

			Random.state = newState;
		}

		#endregion
	}
}