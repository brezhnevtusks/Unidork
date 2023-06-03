using UnityEngine;

namespace Unidork.Extensions
{
	public static class AnimatorExtensions
    {
		#region Reset

		/// <summary>
		/// Resets all triggers in an animator.
		/// </summary>
		/// <param name="animator"></param>
		public static void ResetAllTriggers(this Animator animator)
		{
			foreach (AnimatorControllerParameter parameter in animator.parameters)
			{
				if (parameter.type == AnimatorControllerParameterType.Trigger)
				{
					animator.ResetTrigger(parameter.nameHash);
				}
			}
		}

		#endregion
	}
}