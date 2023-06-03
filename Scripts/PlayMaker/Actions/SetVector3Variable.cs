#if PLAYMAKER
using System.Diagnostics.CodeAnalysis;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using Unidork.Variables;
using UnityEngine;
	
namespace Unidork.PlayMaker
{
	/// <summary>
	/// Custom PlayMaker action that allows to set the value of a <see cref="Vector3Variable"/> from a PlayMaker
	/// Vector3 variable or a set of two float variables.
	/// </summary>
	[ActionCategory("Variables")]
	[HutongGames.PlayMaker.Tooltip("Sets the value of a Vector3Variable.")]
	public class SetVector3Variable : BaseUpdateAction
	{
		#region Fields

		/// <summary>
		/// Vector3 variable to set the value on.
		/// </summary>
		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("Vector3 variable to set the value on.")]
		public Vector3Variable Variable = null;

		/// <summary>
		/// PlayMaker variable storing a Vector3 value.
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("PlayMaker variable storing a Vector3 value.")]
		public FsmVector3 Vector3 = null;
		
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
		
		/// <summary>
		/// Variable storing the z value.
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("Variable storing the z value.")]
		public FsmFloat Z = null;

		#endregion

		#region Action

		public override void Reset()
		{
			Variable = null;
			Vector3 = null;
			X = null;
			Y = null;
			Z = null;
		}

		public override void OnEnter()
		{
			if (Vector3 != null && !Vector3.IsNone)
			{
				Variable.Value = Vector3.Value;
			}
			else if (X != null && Y != null && Z != null && !X.IsNone && !Y.IsNone && !Z.IsNone)
			{
				Variable.Value = new Vector3(X.Value, Y.Value, Z.Value);
			}
			else
			{
				Debug.LogError($"SetVector3Variable action in state {State} on {Owner} doesn't have variable values assigned!");
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