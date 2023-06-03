using System;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;

namespace Unidork.Extensions
{
	public static class GameObjectExtensions
	{
		#region Fields

		/// <summary>
		/// List to serve as a component cache to avoid memory allocations.
		/// </summary>
		private static readonly List<Component> componentCache = new List<Component>();

		#endregion

		#region Get

		/// <summary>
		/// Gets a component on a game object without allocating memory.
		/// </summary>
		/// <typeparam name="T">Type of component to get.</typeparam>
		/// <param name="gameObject">Game object to get the component from.</param>
		/// <returns>
		/// An instance of <typeparamref name="T"/> or null if <paramref name="gameObject"/> doesn't have a component of specified type attached.
		/// </returns>
		public static T GetComponentNonAlloc<T>(this GameObject gameObject) where T : Component
		{
			gameObject.GetComponents(typeof(T), componentCache);
			var component = componentCache.Count > 0 ? componentCache[0] : null;
			componentCache.Clear();
			return (T)component;
		}
		
		/// <summary>
		/// Gets a component on a game object without allocating memory.
		/// </summary>
		/// <param name="gameObject">Game object to get the component from.</param>
		/// <param name="componentType">Type of component to get.</param>
		/// <returns>
		/// An instance of <see cref="componentType"/> or null if <paramref name="gameObject"/> doesn't have a component of specified type attached.
		/// </returns>
		public static Component GetComponentNonAlloc(this GameObject gameObject, Type componentType)
		{
			gameObject.GetComponents(componentType, componentCache);
			var component = componentCache.Count > 0 ? componentCache[0] : null;
			componentCache.Clear();
			return component;
		}

		/// <summary>
		/// Gets a component of specified type or adds it to a game object and returns.
		/// </summary>
		/// <typeparam name="T">Type of component to get.</typeparam>
		/// <param name="gameObject">Game object that needs to get checked for a component of type <typeparamref name="T"/>.</param>
		/// <returns>
		/// Existing component of type <typeparamref name="T"/> or a new one if it doesn't exist.
		/// </returns>
		public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
		{
			T componentToReturn = gameObject.GetComponentNonAlloc<T>();

			if (componentToReturn == null)
			{
				componentToReturn = gameObject.AddComponent<T>();
			}

			return componentToReturn;
		}
		
		/// <summary>
		/// Gets a component of specified type or adds it to a game object and returns.
		/// </summary>
		/// <param name="gameObject">Game object that needs to get checked for a component of type <see cref="componentType"/>.</param>
		/// <param name="componentType">Type of component to get.</param>
		/// <returns>
		/// Existing component of type <see cref="componentType"/> or a new one if it doesn't exist.
		/// </returns>
		public static Component GetOrAddComponent(this GameObject gameObject, Type componentType)
		{
			Component componentToReturn = gameObject.GetComponentNonAlloc(componentType);

			if (componentToReturn == null)
			{
				componentToReturn = gameObject.AddComponent(componentType);
			}

			return componentToReturn;
		}

		/// <summary>
		/// Gets the first located component of type <typeparamref name="T"/> in a game object's hierarchy using a non-allocating method. 
		/// Optionally ignores the parent transform.
		/// Children of the object are acquired through a breadth-first search.
		/// </summary>
		/// <typeparam name="T">Type of component to get.</typeparam>
		/// <param name="gameObject">Game object whose hierarchy to search.</param>
		/// <param name="ignoreParent">Should the parent game object be ignored in the search?</param>
		/// <returns>
		/// Component of type <typeparamref name="T"/> or null if game object's hierarchy doesn't contain the component.
		/// </returns>
		public static T GetComponentInChildrenNonAlloc<T>(this GameObject gameObject, bool ignoreParent = false) 
			where T : Component
		{
			List<Transform> transformsInHierarchy = gameObject.transform.GetAllChildren();  

			if (!ignoreParent)
			{
				transformsInHierarchy.AddAt(gameObject.transform, 0);
			}

			T componentToReturn = null;

			foreach (Transform child in transformsInHierarchy)
			{
				componentToReturn = child.GetComponentNonAlloc<T>();

				if (componentToReturn != null)
				{
					return componentToReturn;
				}
			}

			return componentToReturn;
		}
		
		/// <summary>
		/// Gets the first located component of specified type in a game object's hierarchy using a non-allocating method. 
		/// Optionally ignores the parent transform.
		/// Children of the object are acquired through a breadth-first search.
		/// </summary>
		/// <param name="gameObject">Game object whose hierarchy to search.</param>
		/// <param name="componentType">Type of component to get.</param>
		/// <param name="ignoreParent">Should the parent game object be ignored in the search?</param>
		/// <returns>
		/// Component of type <see cref="componentType"/> or null if game object's hierarchy doesn't contain the component.
		/// </returns>
		public static Component GetComponentInChildrenNonAlloc(this GameObject gameObject, Type componentType, bool ignoreParent = false)
		{
			List<Transform> transformsInHierarchy = gameObject.transform.GetAllChildren();  

			if (!ignoreParent)
			{
				transformsInHierarchy.AddAt(gameObject.transform, 0);
			}

			Component componentToReturn = null;

			foreach (Transform child in transformsInHierarchy)
			{
				componentToReturn = child.GetComponentNonAlloc(componentType);

				if (componentToReturn != null)
				{
					return componentToReturn;
				}
			}

			return componentToReturn;
		}
		
		/// <summary>
		/// Gets all components of type <typeparamref name="T"/> in a game object's hierarchy using a non-allocating method.
		/// Optionally ignores the parent transform.
		/// Children of the object are acquired through a breadth-first search.
		/// </summary>
		/// <typeparam name="T">Type of component to get.</typeparam>
		/// <param name="gameObject">Game object whose hierarchy to search.</param>
		/// <param name="ignoreParent">Should the parent game object be ignored in the search?</param>
		/// <returns>
		/// List of components of type <typeparamref name="T"/> that game object's heirarchy contains.
		/// </returns>
		public static List<T> GetComponentsInChildrenNonAlloc<T>(this GameObject gameObject, bool ignoreParent = false) 
			where T : Component
		{
			List<Transform> transformsInHierarchy = gameObject.transform.GetAllChildren();

			if (!ignoreParent)
			{
				transformsInHierarchy.AddAt(gameObject.transform, 0);
			}

			var componentsToReturn = new List<T>();

			foreach (Transform child in transformsInHierarchy)
			{
				T componentToAdd = child.GetComponentNonAlloc<T>();

				if (componentToAdd != null)
				{
					componentsToReturn.Add(componentToAdd);
				}
			}

			return componentsToReturn;
		}

		/// <summary>
		/// Gets all components of specified type in a game object's hierarchy using a non-allocating method.
		/// Optionally ignores the parent transform.
		/// Children of the object are acquired through a breadth-first search.
		/// </summary>
		/// <param name="gameObject">Game object whose hierarchy to search.</param>
		/// <param name="componentType">Type of component to get.</param>
		/// <param name="ignoreParent">Should the parent game object be ignored in the search?</param>
		/// <returns>
		/// List of components of type <see cref="componentType"/> that game object's hierarchy contains.
		/// </returns>
		public static List<Component> GetComponentsInChildrenNonAlloc(this GameObject gameObject, Type componentType, bool ignoreParent = false) 
		{
			List<Transform> transformsInHierarchy = gameObject.transform.GetAllChildren();

			if (!ignoreParent)
			{
				transformsInHierarchy.AddAt(gameObject.transform, 0);
			}

			var componentsToReturn = new List<Component>();

			foreach (Transform child in transformsInHierarchy)
			{
				Component componentToAdd = child.GetComponentNonAlloc(componentType);

				if (componentToAdd != null)
				{
					componentsToReturn.Add(componentToAdd);
				}
			}

			return componentsToReturn;
		}

		/// <summary>
		/// Gets the first located component of type <typeparamref name="T"/> in game object's parents using a non-allocating method.
		/// Optionally ignores the transform on the calling game object.
		/// </summary>
		/// <typeparam name="T">Type of component to get.</typeparam>
		/// <param name="gameObject">Game object whose parents to search.</param>
		/// <param name="ignoreCallerGo">Should the calling game object be ignored?</param>
		/// <returns>
		/// Component of type <typeparamref name="T"/> or null if game object's hierarchy doesn't contain the component.
		/// </returns>
		public static T GetComponentInParentsNonAlloc<T>(this GameObject gameObject, bool ignoreCallerGo = false) 
			where T : Component
		{
			Transform currentTransform = ignoreCallerGo ? gameObject.transform.parent : gameObject.transform;

			while (currentTransform != null)
			{
				T componentToReturn = currentTransform.GetComponentNonAlloc<T>();

				if (componentToReturn != null)
				{
					return componentToReturn;
				}
				
				currentTransform = currentTransform.parent;
			}

			return null;
		}
		
		/// <summary>
		/// Gets the first located component of specified type in game object's parents using a non-allocating method.
		/// Optionally ignores the transform on the calling game object.
		/// </summary>
		/// <param name="gameObject">Game object whose parents to search.</param>
		/// <param name="componentType">Type of component to get.</param>
		/// <param name="ignoreCallerGo">Should the calling game object be ignored?</param>
		/// <returns>
		/// Component of type <see cref="componentType"/> or null if game object's hierarchy doesn't contain the component.
		/// </returns>
		public static Component GetComponentInParentsNonAlloc(this GameObject gameObject, Type componentType, bool ignoreCallerGo = false)
		{
			Transform currentTransform = ignoreCallerGo ? gameObject.transform.parent : gameObject.transform;

			while (currentTransform != null)
			{
				Component componentToReturn = currentTransform.GetComponentNonAlloc(componentType);

				if (componentToReturn != null)
				{
					return componentToReturn;
				}
				
				currentTransform = currentTransform.parent;
			}

			return null;
		}

		/// <summary>
		/// Gets all components of type <typeparamref name="T"/> in game object's upwards hierarchy. 
		/// Optionally ignores the transform on the calling object.
		/// </summary>
		/// <typeparam name="T">Type of component to get.</typeparam>
		/// <param name="gameObject">Game object whose parents to search.</param>
		/// <param name="ignoreCallerGo">Should the calling game object?</param>
		/// <returns>
		/// List of components of type <typeparamref name="T"/> that game object's hierarchy contains.
		/// </returns>
		public static List<T> GetComponentsInParentsNonAlloc<T>(this GameObject gameObject, bool ignoreCallerGo = false) 
			where T : Component
		{
			var componentsToReturn = new List<T>();

			Transform currentTransform = ignoreCallerGo ? gameObject.transform.parent : gameObject.transform;

			while (currentTransform != null)
			{
				T componentToAdd = currentTransform.GetComponentNonAlloc<T>();

				if (componentToAdd != null)
				{
					componentsToReturn.Add(componentToAdd);
				}
				
				currentTransform = currentTransform.parent;
			}

			return componentsToReturn;
		}
		
		/// <summary>
		/// Gets all components of specified type in game object's upwards hierarchy. 
		/// Optionally ignores the transform on the calling object.
		/// </summary>
		/// <param name="gameObject">Game object whose parents to search.</param>
		/// <param name="componentType">Type of component to get.</param>
		/// <param name="ignoreCallerGo">Should the calling game object?</param>
		/// <returns>
		/// List of components of type <see cref="componentType"/> that game object's hierarchy contains.
		/// </returns>
		public static List<Component> GetComponentsInParentsNonAlloc(this GameObject gameObject, Type componentType, bool ignoreCallerGo = false)
			
		{
			var componentsToReturn = new List<Component>();

			Transform currentTransform = ignoreCallerGo ? gameObject.transform.parent : gameObject.transform;

			while (currentTransform != null)
			{
				Component componentToAdd = currentTransform.GetComponentNonAlloc(componentType);

				if (componentToAdd != null)
				{
					componentsToReturn.Add(componentToAdd);
				}
				
				currentTransform = currentTransform.parent;
			}

			return componentsToReturn;
		}

		/// <summary>
		/// Gets the first located component of type <typeparamref name="T"/> in game object's hierarchy
		/// using a non-allocating method.
		/// Optionally ignores the calling game object.
		/// </summary>
		/// <typeparam name="T">Type of component to get.</typeparam>
		/// <param name="gameObject">Game object whose hierarchy to search.</param>
		/// <param name="searchChildrenFirst">Should children of the game object be searched before parents?</param>
		/// <param name="ignoreCallerGo">Should the calling game object be ignored?</param>
		/// <returns>
		/// Component of type <typeparamref name="T"/> or null if game object's hierarchy doesn't contain the component.
		/// </returns>
		public static T GetComponentInHierarchyNonAlloc<T>(this GameObject gameObject, bool searchChildrenFirst = true,
														 bool ignoreCallerGo = false)
			where T : Component
		{
			T component;

			if (searchChildrenFirst)
			{
				component = gameObject.GetComponentInChildrenNonAlloc<T>(ignoreCallerGo);

				if (component == null)
				{
					component = gameObject.GetComponentInParentsNonAlloc<T>(ignoreCallerGo);
				}
			}
			else
			{
				component = gameObject.GetComponentInParentsNonAlloc<T>(ignoreCallerGo);

				if (component == null)
				{
					component = gameObject.GetComponentInChildrenNonAlloc<T>(ignoreCallerGo);
				}
			}

			return component;
		}
		
		/// <summary>
		/// Gets the first located component of specified type in game object's hierarchy
		/// using a non-allocating method.
		/// Optionally ignores the calling game object.
		/// </summary>
		/// <param name="gameObject">Game object whose hierarchy to search.</param>
		/// <param name="componentType">Type of component to get.</param>
		/// <param name="searchChildrenFirst">Should children of the game object be searched before parents?</param>
		/// <param name="ignoreCallerGo">Should the calling game object be ignored?</param>
		/// <returns>
		/// Component of type <see cref="componentType"/> or null if game object's hierarchy doesn't contain the component.
		/// </returns>
		public static Component GetComponentInHierarchyNonAlloc(this GameObject gameObject, Type componentType, bool searchChildrenFirst = true,
		                                                  bool ignoreCallerGo = false)
		{
			Component component;

			if (searchChildrenFirst)
			{
				component = gameObject.GetComponentInChildrenNonAlloc(componentType, ignoreCallerGo);

				if (component == null)
				{
					component = gameObject.GetComponentInParentsNonAlloc(componentType, ignoreCallerGo);
				}
			}
			else
			{
				component = gameObject.GetComponentInParentsNonAlloc(componentType, ignoreCallerGo);

				if (component == null)
				{
					component = gameObject.GetComponentInChildrenNonAlloc(componentType, ignoreCallerGo);
				}
			}

			return component;
		}	

		/// <summary>
		/// Gets all components of type <typeparamref name="T"/> in game object's hierarchy (both parents and children). 
		/// Optionally ignores the calling game object.
		/// </summary>
		/// <typeparam name="T">Type of component to get.</typeparam>
		/// <param name="gameObject">Game object whose parents and children to search.</param>
		/// <param name="ignoreCallingTransform">Should the transform on the calling object be ignored?</param>
		/// <returns>
		/// List of components of type <typeparamref name="T"/> that game object's hierarchy contains.
		/// </returns>
		public static List<T> GetComponentsInHierarchyNonAlloc<T>(this GameObject gameObject, 
																 bool ignoreCallingTransform = false) where T : Component
		{
			var componentsInHierarchy = new List<T>();

			componentsInHierarchy.AddRange(GetComponentsInChildrenNonAlloc<T>(gameObject, ignoreCallingTransform));
			componentsInHierarchy.AddRange(GetComponentsInParentsNonAlloc<T>(gameObject, ignoreCallingTransform));			

			return componentsInHierarchy;
		}
		
		/// <summary>
		/// Gets all components of specified type in game object's hierarchy (both parents and children). 
		/// Optionally ignores the calling game object.
		/// </summary>
		/// <param name="gameObject">Game object whose parents and children to search.</param>
		/// <param name="componentType">Type of component to get.</param>
		/// <param name="ignoreCallingTransform">Should the transform on the calling object be ignored?</param>
		/// <returns>
		/// List of components of type <see cref="componentType"/> that game object's hierarchy contains.
		/// </returns>
		public static List<Component> GetComponentsInHierarchyNonAlloc(this GameObject gameObject, Type componentType,
		                                                          bool ignoreCallingTransform = false)
		{
			var componentsInHierarchy = new List<Component>();

			componentsInHierarchy.AddRange(GetComponentsInChildrenNonAlloc(gameObject, componentType, ignoreCallingTransform));
			componentsInHierarchy.AddRange(GetComponentsInParentsNonAlloc(gameObject, componentType, ignoreCallingTransform));			

			return componentsInHierarchy;
		}

		#endregion

		#region Tweens

		#region Position

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOMove"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose position will be tweened.</param>
	    /// <param name="targetPosition">Target position.</param>
	    /// <param name="duration">Tween duration.</param>
	    /// <param name="snapping">When set to True, will snap all values to integers.</param>
	    public static TweenerCore<Vector3, Vector3, VectorOptions> DOMove(this GameObject target, Vector3 targetPosition, float duration, bool snapping = false)
		{
			return target.transform.DOMove(targetPosition, duration, snapping);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOMoveX"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose position will be tweened.</param>
		/// <param name="targetX">Target X position.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="snapping">When set to True, will snap all values to integers.</param>
	    public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveX(this GameObject target, float targetX, float duration, bool snapping = false)
	    {
		    return target.transform.DOMoveX(targetX, duration, snapping);
	    }

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOMoveY"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose position will be tweened.</param>
		/// <param name="targetY">Target Y position.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="snapping">When set to True, will snap all values to integers.</param>
	    public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveY(this GameObject target, float targetY, float duration, bool snapping = false)
	    {
		    return target.transform.DOMoveY(targetY, duration, snapping);
	    }

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOMoveZ"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose position will be tweened.</param>
		/// <param name="targetZ">Target Z position.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="snapping">When set to True, will snap all values to integers.</param>
	    public static TweenerCore<Vector3, Vector3, VectorOptions> DOMoveZ(this GameObject target, float targetZ, float duration, bool snapping = false)
	    {
		    return target.transform.DOMoveZ(targetZ, duration, snapping);
	    }

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOLocalMove"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose position will be tweened.</param>
		/// <param name="targetPosition">Target position.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="snapping">When set to True, will snap all values to integers.</param>
	    public static TweenerCore<Vector3, Vector3, VectorOptions> DOLocalMove(this GameObject target, Vector3 targetPosition, float duration, bool snapping = false)
		{
			return target.transform.DOLocalMove(targetPosition, duration, snapping);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOLocalMoveX"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose position will be tweened.</param>
		/// <param name="targetX">Target X position.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="snapping">When set to True, will snap all values to integers.</param>
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOLocalMoveX(this GameObject target, float targetX, float duration, bool snapping = false)
		{
			return target.transform.DOLocalMoveX(targetX, duration, snapping);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOLocalMoveY"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose position will be tweened.</param>
		/// <param name="targetY">Target Y position.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="snapping">When set to True, will snap all values to integers.</param>
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOLocalMoveY(this GameObject target, float targetY, float duration, bool snapping = false)
		{
			return target.transform.DOLocalMoveY(targetY, duration, snapping);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOLocalMoveZ"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose position will be tweened.</param>
		/// <param name="targetZ">Target Z position.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="snapping">When set to True, will snap all values to integers.</param>
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOLocalMoveZ(this GameObject target, float targetZ, float duration, bool snapping = false)
		{
			return target.transform.DOLocalMoveZ(targetZ, duration, snapping);
		}

		#endregion

		#region Rotation

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DORotate"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetRotation">Target rotation.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="mode">Rotation mode.</param>
	    public static TweenerCore<Quaternion, Vector3, QuaternionOptions> DORotate(this GameObject target, Vector3 targetRotation, float duration,
																				   RotateMode mode = RotateMode.Fast)
		{
			return target.transform.DORotate(targetRotation, duration, mode);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DORotateQuaternion"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetRotation">Target rotation.</param>
		/// <param name="duration">Tween duration.</param>
		public static TweenerCore<Quaternion, Quaternion, NoOptions> DORotateQuaternion(this GameObject target, Quaternion targetRotation, float duration)
		{
			return target.transform.DORotateQuaternion(targetRotation, duration);
		}
		
		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOLocalRotate"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetRotation">Target rotation.</param>
		/// <param name="duration">Tween duration.</param>
		/// <param name="mode">Rotation mode.</param>
		public static TweenerCore<Quaternion, Vector3, QuaternionOptions> DOLocalRotate(this GameObject target, Vector3 targetRotation, float duration,
			RotateMode mode = RotateMode.Fast)
		{
			return target.transform.DOLocalRotate(targetRotation, duration, mode);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOLocalRotateQuaternion"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetRotation">Target rotation.</param>
		/// <param name="duration">Tween duration.</param>
		public static TweenerCore<Quaternion, Quaternion, NoOptions> DOLocalRotateQuaternion(this GameObject target, Quaternion targetRotation, float duration)
		{
			return target.transform.DOLocalRotateQuaternion(targetRotation, duration);
		}

		#endregion

		#region Scale

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOScale(Transform, Vector3, float)"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetScale">Target scale.</param>
		/// <param name="duration">Tween duration.</param>
	    public static TweenerCore<Vector3, Vector3, VectorOptions> DOScale(this GameObject target, Vector3 targetScale, float duration)
		{
			return target.transform.DOScale(targetScale, duration);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOScale(Transform, float, float)"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetScale">Target scale.</param>
		/// <param name="duration">Tween duration.</param>
	    public static TweenerCore<Vector3, Vector3, VectorOptions> DOScale(this GameObject target, float targetScale, float duration)
		{
			return target.transform.DOScale(targetScale, duration);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOScaleX"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetXScale">Target X scale.</param>
		/// <param name="duration">Tween duration.</param>
	    public static TweenerCore<Vector3, Vector3, VectorOptions> DOScaleX(this GameObject target, float targetXScale, float duration)
		{
			return target.transform.DOScaleX(targetXScale, duration);
		}

		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOScaleY"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetYScale">Target Y scale.</param>
		/// <param name="duration">Tween duration.</param>
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOScaleY(this GameObject target, float targetYScale, float duration)
		{
			return target.transform.DOScaleY(targetYScale, duration);
		}
		
		/// <summary>
		/// Calls the <see cref="ShortcutExtensions.DOScaleZ"/> method on the transform of this game object.
		/// </summary>
		/// <param name="target">Game object whose rotation will be tweened.</param>
		/// <param name="targetZScale">Target Z scale.</param>
		/// <param name="duration">Tween duration.</param>
		public static TweenerCore<Vector3, Vector3, VectorOptions> DOScaleZ(this GameObject target, float targetZScale, float duration)
		{
			return target.transform.DOScaleZ(targetZScale, duration);
		}

		#endregion

		#endregion
	}
}