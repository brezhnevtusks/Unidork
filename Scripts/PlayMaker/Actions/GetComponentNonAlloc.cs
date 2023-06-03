#if PLAYMAKER

using HutongGames.PlayMaker;
using Unidork.Extensions;
using UnityEngine;
	
namespace Unidork.PlayMaker
{
	/// <summary>
	/// Custom PlayMaker action that gets a component on an object, and/or its children and parents.
	/// </summary>
	[ActionCategory(ActionCategory.UnityObject)]
	[HutongGames.PlayMaker.Tooltip("Gets a component on an object, and/or its children and parents.")]
	public class GetComponentNonAlloc : FsmStateAction
	{
		#region Fields

		/// <summary>
		/// Game object to search for the component.
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("Game object to search for the component.")]
		public FsmOwnerDefault GameObject;

		/// <summary>
		/// Variable to store the component.
		/// </summary>
		[UIHint(UIHint.Variable)]
		[RequiredField]
		[HutongGames.PlayMaker.Tooltip("Variable to store the component.")]
		public FsmObject StoreComponent;
		
		/// <summary>verlap2d
		/// Should GetComponent be called on the target object itself?
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("Should GetComponent be called on the target object itself?")]
		public bool GetOnSelf;
		
		/// <summary>
		/// Should GetComponent be called on target object's children?
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("Should GetComponent be called on target object's children?")]
		public bool GetInChildren;
		
		/// <summary>
		/// Should GetComponent be called on target object's parents?
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("Should GetComponent be called on target object's parents?")]
		public bool GetInParents;
		
		/// <summary>
		/// Should the action be repeated every frame?
		/// </summary>
		[HutongGames.PlayMaker.Tooltip("Should the action be repeated every frame?")]
		public bool EveryFrame;

		#endregion

		#region Action

		public override void Reset()
		{
			GameObject = null;
			StoreComponent = null;
			GetOnSelf = true;
			GetInChildren = false;
			GetInParents = false;
			EveryFrame = false;
		}

		public override void OnEnter()
		{
			GetComponent();

			if (!EveryFrame)
			{
				Finish();
			}
		}

		public override void OnUpdate()
		{
			GetComponent();
		}
		
		/// <summary>
		/// Gets the component based on action settings.
		/// </summary>
		private void GetComponent()
		{
			if (StoreComponent == null || StoreComponent.IsNone)
			{
				Debug.LogError($"GetComponentNonAlloc action in state {State} on {Owner} doesn't have a variable to store the component!");
				return;
			}

			StoreComponent.Value = null;
			
			var targetObject = Fsm.GetOwnerDefaultTarget(GameObject);
			
			if (targetObject == null)
			{
				Debug.LogError($"GetComponentNonAlloc action in state {State} on {Owner} doesn't have a valid target game object assigned!");
				return;
			}

			if (GetComponentOnSelf())
			{
				return;
			}

			if (GetComponentInChildren())
			{
				return;
			}

			_ = GetComponentInParents();
		}
		
		/// <summary>
		/// Gets the component on the target object itself.
		/// </summary>
		/// <returns>
		/// True if component is successfully retrieved, false otherwise.
		/// </returns>
		private bool GetComponentOnSelf()
		{
			if (!GetOnSelf)
			{
				return false;
			}

			var gameObject = Fsm.GetOwnerDefaultTarget(GameObject);

			if (gameObject == null)
			{
				Debug.LogError($"GetComponentNonAlloc action in state {State} on {Owner} doesn't have a game object assigned!");
				return false;
			}

			var component = gameObject.GetComponentNonAlloc(StoreComponent.ObjectType);

			if (component == null)
			{
				return false;
			}

			StoreComponent.Value = component;
			return true;
		}

		/// <summary>
		/// Gets the component on target object's children.
		/// </summary>
		/// <returns>
		/// True if component is successfully retrieved, false otherwise.
		/// </returns>
		private bool GetComponentInChildren()
		{
			if (!GetInChildren)
			{
				return false;
			}
			
			var gameObject = Fsm.GetOwnerDefaultTarget(GameObject);

			if (gameObject == null)
			{
				Debug.LogError($"GetComponentNonAlloc action in state {State} on {Owner} doesn't have a game object assigned!");
				return false;
			}

			var component = gameObject.GetComponentInChildrenNonAlloc(StoreComponent.ObjectType);

			if (component == null)
			{
				return false;
			}

			StoreComponent.Value = component;
			return true;
		}

		/// <summary>
		/// Gets the component on target object's children.
		/// </summary>
		/// <returns>
		/// True if component is successfully retrieved, false otherwise.
		/// </returns>
		private bool GetComponentInParents()
		{
			if (!GetInParents)
			{
				return false;
			}
			
			var gameObject = Fsm.GetOwnerDefaultTarget(GameObject);

			if (gameObject == null)
			{
				Debug.LogError($"GetComponentNonAlloc action in state {State} on {Owner} doesn't have a game object assigned!");
				return false;
			}

			var component = gameObject.GetComponentInParentsNonAlloc(StoreComponent.ObjectType);

			if (component == null)
			{
				return false;
			}

			StoreComponent.Value = component;
			return true;
		}

#if UNITY_EDITOR
		public override string AutoName()
		{
			return ActionHelpers.AutoName(this, StoreComponent);
		}
#endif

		#endregion
	}
}

#endif