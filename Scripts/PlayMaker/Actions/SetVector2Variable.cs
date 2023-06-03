#if PLAYMAKER
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Unidork.Variables;
using UnityEngine;
	
namespace Unidork.PlayMaker
{
	/// <summary>
	/// Custom PlayMaker action that allows to set the value of a <see cref="Vector2Variable"/> from a PlayMaker
	/// Vector2 variable or a set of two float variables.
	/// </summary>
	[ActionCategory("Variables")]
	[HutongGames.PlayMaker.Tooltip("Sets the value of a Vector2Variable.")]
	public class SetVector2Variable : BaseUpdateAction
	{
		#region Fields

		/// <summary>
		/// Vector2 variable to set the value on.
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("Vector2 variable to set the value on.")]
		public Vector2Variable Variable = null;

		/// <summary>
		/// PlayMaker variable storing a Vector2 value.
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("PlayMaker variable storing a Vector2 value.")]
		public FsmVector2 Vector2 = null;
		
		/// <summary>
		/// Variable storing the x value.
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("Variable storing the x value.")]
		public FsmFloat X = null;
		
		/// <summary>
		/// Variable storing the y value.
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("Variable storing the y value.")]
		public FsmFloat Y = null;

		#endregion

		#region Action

		public override void Reset()
		{
			Variable = null;
			Vector2 = null;
			X = null;
			Y = null;
		}

		public override void OnEnter()
		{
			if (Vector2 != null && !Vector2.IsNone)
			{
				Variable.Value = Vector2.Value;
			}
			else if (X != null && Y != null && !X.IsNone && !Y.IsNone)
			{
				Variable.Value = new Vector2(X.Value, Y.Value);
			}
			else
			{
				Debug.LogError($"SetVector2Variable action in state {State} on {Owner} doesn't have variable values assigned!");
			}

			if (everyFrame)
			{
				return;
			}
			
			Finish();
		}
		
		public override void OnActionUpdate()
		{
			OnEnter();
		}

		#endregion
	}
}

#endif