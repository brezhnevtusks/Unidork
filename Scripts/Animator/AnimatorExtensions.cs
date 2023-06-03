using UnityEngine;

namespace Unidork.Animations
{
	public static class AnimatorExtensions
	{
		
		/// <summary>
	    /// Checks whether the animator is in a specific state.
	    /// </summary>
	    /// <param name="animator">Animator.</param>
	    /// <param name="stateName">State name.</param>
	    /// <param name="animatorLayerIndex">Index of the animator layer to check.</param>
	    /// <returns>True if the animator is in a state with the passed name, False otherwise.</returns>
	    public static bool IsInState(this Animator animator, string stateName, int animatorLayerIndex = 0)
	    {
		    return animator.IsInState(Animator.StringToHash(stateName), animatorLayerIndex);
	    }
	    
	    /// <summary>
	    /// Checks whether the animator is in a specific state.
	    /// </summary>
	    /// <param name="animator">Animator.</param>
	    /// <param name="stateNameHash">Hash of the state name.</param>
	    /// <param name="animatorLayerIndex">Index of the animator layer to check.</param>
	    /// <returns>True if the animator is in a state with the passed hash, False otherwise.</returns>
	    public static bool IsInState(this Animator animator, int stateNameHash, int animatorLayerIndex = 0)
	    {
		    return animator.GetCurrentAnimatorStateInfo(animatorLayerIndex).shortNameHash == stateNameHash;
	    }

	    /// <summary>
	    /// Checks whether the animator is in a state that has a specific tag.
	    /// </summary>
	    /// <param name="animator">Animator.</param>
	    /// <param name="tag">Tag name.</param>
	    /// <param name="animatorLayerIndex">Index of the animator layer to check.</param>
	    /// <returns>True if the animator is in a state with the passed tag name, False otherwise.</returns>
	    public static bool IsInStateWithTag(this Animator animator, string tag, int animatorLayerIndex = 0)
	    {
		    return animator.GetCurrentAnimatorStateInfo(animatorLayerIndex).IsTag(tag);
	    }

	    /// <summary>
	    /// Checks whether the animator is in a state that has a specific tag.
	    /// </summary>
	    /// <param name="animator">Animator.</param>
	    /// <param name="tagHash">Tag hash.</param>
	    /// <param name="animatorLayerIndex">Index of the animator layer to check.</param>
	    /// <returns>True if the animator is in a state with the passed tag hash, False otherwise.</returns>
	    public static bool IsInStateWithTag(this Animator animator, int tagHash, int animatorLayerIndex)
	    {
		    return animator.GetCurrentAnimatorStateInfo(animatorLayerIndex).tagHash == tagHash;
	    }

	}
}