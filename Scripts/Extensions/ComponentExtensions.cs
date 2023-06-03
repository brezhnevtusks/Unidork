using System;
using System.Collections.Generic;
using UnityEngine;

namespace Unidork.Extensions
{
	public static class ComponentExtensions
	{
		#region Get

		/// <summary>
		/// Gets a component on a game object component is attached to without allocating memory.
		/// </summary>
		/// <typeparam name="T">Type of component to get.</typeparam>
		/// <param name="component">Component from whose game object to get another component.</param>
		/// <returns>
		/// An instance of <typeparamref name="T"/> or null if component's game object doesn't have a component of specified type attached.
		/// </returns>
		public static T GetComponentNonAlloc<T>(this Component component) where T : Component => component.gameObject.GetComponentNonAlloc<T>();

		/// <summary>
		/// Gets a component on a game object component is attached to without allocating memory.
		/// </summary>
		/// <param name="type">Type of component to get.</param>
		/// <param name="component">Component from whose game object to get another component.</param>
		/// <returns>
		/// An instance of <paramref name="type"/> or null if component's game object doesn't have a component of specified type attached.
		/// </returns>
		public static Component GetComponentNonAlloc(this Component component, System.Type type) => component.gameObject.GetComponentNonAlloc(type);
		
		/// <summary>
		/// Gets a component of specified type or adds it to a game object component is attached to and returns.
		/// </summary>
		/// <typeparam name="T">Type of component to get.</typeparam>
		/// <param name="component">Component whose game object needs to get checked for a component of type <typeparamref name="T"/>.</param>
		/// <returns>
		/// Existing component of type <typeparamref name="T"/> or a new one if it doesn't exist.
		/// </returns>
		public static T GetOrAddComponent<T>(this Component component) where T : Component
		{
			T componentToReturn = component.GetComponentNonAlloc<T>();

			if (componentToReturn == null)
			{
				componentToReturn = component.gameObject.AddComponent<T>();
			}

			return componentToReturn;
		}
		
		/// <summary>
		/// Gets a component of specified type or adds it to a game object and returns.
		/// </summary>
		/// <param name="component">Component whose game object needs to get checked for a component of type <see cref="componentType"/>.</param>
		/// <param name="componentType">Type of component to get.</param>
		/// <returns>
		/// Existing component of type <see cref="componentType"/> or a new one if it doesn't exist.
		/// </returns>
		public static Component GetOrAddComponent(this Component component, Type componentType)
		{
			Component componentToReturn = component.GetComponentNonAlloc(componentType);

			if (componentToReturn == null)
			{
				componentToReturn = component.gameObject.AddComponent(componentType);
			}

			return componentToReturn;
		}

		/// <summary>
		/// Gets the first located component of type <typeparamref name="T"/> in the children of the game object
		/// component is attached to using a non-allocating method. 
		/// Optionally ignores the parent transform.
		/// Children of the object are acquired through a breadth-first search.
		/// </summary>
		/// <typeparam name="T">Type of component to get.</typeparam>
		/// <param name="component">Component whose hierarchy to search.</param>
		/// <param name="ignoreParent">Should the parent transform be ignored in the search?</param>
		/// <returns>
		/// Component of type <typeparamref name="T"/> or null if none of component's game object children have the component.
		/// </returns>
		public static T GetComponentInChildrenNonAlloc<T>(this Component component, bool ignoreParent = false) where T : Component
		{
			return component.gameObject.GetComponentInChildrenNonAlloc<T>(ignoreParent);
		}
		
		/// <summary>
		/// Gets the first located component of specified type in children of the game object
		/// component is attached to using a non-allocating method. 
		/// Optionally ignores the parent transform.
		/// Children of the object are acquired through a breadth-first search.
		/// </summary>
		/// <param name="component">Component whose hierarchy to search.</param>
		/// <param name="componentType">Type of component to get.</param>
		/// <param name="ignoreParent">Should the parent transform be ignored in the search?</param>
		/// <returns>
		/// Component of type <see cref="componentType"/> or null if none of component's game object children have the component.
		/// </returns>
		public static Component GetComponentInChildrenNonAlloc(this Component component, Type componentType, bool ignoreParent = false)
		{
			return component.gameObject.GetComponentInChildrenNonAlloc(componentType, ignoreParent);
		}

		/// <summary>
		/// Gets all components of type <typeparamref name="T"/> in children of the game object
		/// component is attached to using a non-allocating method.
		/// Optionally ignores the parent transform.
		/// Children of the object are acquired through a breadth-first search.
		/// </summary>
		/// <typeparam name="T">Type of component to get.</typeparam>
		/// <param name="component">Component whose hierarchy to search.</param>
		/// <param name="ignoreParent">Should the parent transform be ignored in the search?</param>
		/// <returns>
		/// List of components of type <typeparamref name="T"/> that game object's children have.
		/// </returns>
		public static List<T> GetComponentsInChildrenNonAlloc<T>(this Component component, bool ignoreParent = false) where T : Component
		{
			return component.gameObject.GetComponentsInChildrenNonAlloc<T>(ignoreParent);
		}
		
		/// <summary>
		/// Gets all components of specified type in in children of the game object
		/// component is attached to using a non-allocating method.
		/// Optionally ignores the parent transform.
		/// Children of the object are acquired through a breadth-first search.
		/// </summary>
		/// <param name="component">Component whose hierarchy to search.</param>
		/// <param name="componentType">Type of component to get.</param>
		/// <param name="ignoreParent">Should the parent transform be ignored in the search?</param>
		/// <returns>
		/// List of components of type <see cref="componentType"/> that game object's children have.
		/// </returns>
		public static List<Component> GetComponentsInChildrenNonAlloc(this Component component, Type componentType, bool ignoreParent = false)
		{
			return component.gameObject.GetComponentsInChildrenNonAlloc(componentType, ignoreParent);
		}

		/// <summary>
		/// Gets the first located component of type <typeparamref name="T"/> in parents of the game object component is attached to
		/// using a non-allocating method.
		/// Optionally ignores the transform on the calling game object.
		/// </summary>
		/// <typeparam name="T">Type of component to get.</typeparam>
		/// <param name="component">Component whose game object's parents to search.</param>
		/// <param name="ignoreFirstTransform">Should the transform on the calling object be ignored?</param>
		/// <returns>
		/// Component of type <typeparamref name="T"/> or null if component's upwards hierarchy doesn't contain component.
		/// </returns>
		public static T GetComponentInParentsNonAlloc<T>(this Component component, bool ignoreFirstTransform = false) where T : Component
		{
			return component.gameObject.GetComponentInParentsNonAlloc<T>(ignoreFirstTransform);
		}
		
		/// <summary>
		/// Gets the first located component of specified type in parents of the game object component is attached to
		/// using a non-allocating method.
		/// Optionally ignores the transform on the calling game object.
		/// </summary>
		/// <param name="component">Component whose game object's parents to search.</param>
		/// <param name="componentType">Type of component to get.</param>
		/// <param name="ignoreFirstTransform">Should the transform on the calling object be ignored?</param>
		/// <returns>
		/// Component of type <see cref="componentType"/> or null if component's upwards hierarchy doesn't contain component.
		/// </returns>
		public static Component GetComponentInParentsNonAlloc(this Component component, Type componentType, bool ignoreFirstTransform = false)
		{
			return component.gameObject.GetComponentInParentsNonAlloc(componentType, ignoreFirstTransform);
		}

		/// <summary>
		/// Gets all components of type <typeparamref name="T"/> in parents of the game object component is attached to. 
		/// Optionally ignores the transform on the calling object.
		/// </summary>
		/// <typeparam name="T">Type of component to get.</typeparam>
		/// <param name="component">Component whose game object's parents to search.</param>
		/// <param name="ignoreFirstTransform">Should the transform on the calling object be ignored?</param>
		/// <returns>
		/// List of components of type <typeparamref name="T"/> that component's upwards heirarchy contains.
		/// </returns>
		public static List<T> GetComponentsInParentsNonAlloc<T>(this Component component, bool ignoreFirstTransform = false) where T : Component
		{
			return component.gameObject.GetComponentsInParentsNonAlloc<T>(ignoreFirstTransform);
		}
		
		/// <summary>
		/// Gets all components of specified type in parents of the game object component is attached to. 
		/// Optionally ignores the transform on the calling object.
		/// </summary>
		/// <param name="component">Component whose game object's parents to search.</param>
		/// <param name="componentType">Type of component to get.</param>
		/// <param name="ignoreFirstTransform">Should the transform on the calling object be ignored?</param>
		/// <returns>
		/// List of components of type <see cref="componentType"/> that component's upwards hierarchy contains.
		/// </returns>
		public static List<Component> GetComponentsInParentsNonAlloc(this Component component, Type componentType, bool ignoreFirstTransform = false)
		{
			return component.gameObject.GetComponentsInParentsNonAlloc(componentType, ignoreFirstTransform);
		}
		
		/// <summary>
		/// Gets the first located component of type <typeparamref name="T"/> in the hierarchy of a transform using a non-allocating method.
		/// Optionally ignores the calling transform.
		/// </summary>
		/// <typeparam name="T">Type of component to get.</typeparam>
		/// <param name="transform">Transform whose hierarchy to search.</param>
		/// <param name="searchChildrenFirst">Should children of the transform be searched before parents?</param>
		/// <param name="ignoreCallerTransform">Should the calling transform be ignored?</param>
		/// <returns>
		/// Component of type <typeparamref name="T"/> or null if game object's hierarchy doesn't contain the component.
		/// </returns>
		public static T GetComponentInHierarchyNonAlloc<T>(this Transform transform, bool searchChildrenFirst = true,
														 bool ignoreCallerTransform = false)
			where T : Component
		{
			return transform.gameObject.GetComponentInHierarchyNonAlloc<T>(searchChildrenFirst, ignoreCallerTransform);
		}
		
		/// <summary>
		/// Gets the first located component of specified type in the hierarchy of a transform using a non-allocating method.
		/// Optionally ignores the calling transform.
		/// </summary>
		/// <param name="transform">Transform whose hierarchy to search.</param>
		/// <param name="componentType">Type of component to get.</param>
		/// <param name="searchChildrenFirst">Should children of the transform be searched before parents?</param>
		/// <param name="ignoreCallerTransform">Should the calling transform be ignored?</param>
		/// <returns>
		/// Component of type <see cref="componentType"/> or null if game object's hierarchy doesn't contain the component.
		/// </returns>
		public static Component GetComponentInHierarchyNonAlloc(this Transform transform, Type componentType, bool searchChildrenFirst = true,
		                                                   bool ignoreCallerTransform = false)
		{
			return transform.gameObject.GetComponentInHierarchyNonAlloc(componentType, searchChildrenFirst, ignoreCallerTransform);
		}

		/// <summary>
		/// Gets all components of type <typeparamref name="T"/> in a transform's hierarchy (both parents and children). 
		/// Optionally ignores the calling transform.
		/// </summary>
		/// <typeparam name="T">Type of component to get.</typeparam>
		/// <param name="transform">Transform whose parents and children to search.</param>
		/// <param name="ignoreCallingTransform">Should the calling transform be ignored?</param>
		/// <returns>
		/// List of components of type <typeparamref name="T"/> that transform's hierarchy contains.
		/// </returns>
		public static List<T> GetComponentsInHierarchyNonAlloc<T>(this Transform transform,
																 bool ignoreCallingTransform = false) where T : Component
		{
			return transform.gameObject.GetComponentsInHierarchyNonAlloc<T>(ignoreCallingTransform);
		}
		
		/// <summary>
		/// Gets all components of specified type in a transform's hierarchy (both parents and children). 
		/// Optionally ignores the calling transform.
		/// </summary>
		/// <param name="transform">Transform whose parents and children to search.</param>
		/// <param name="componentType">Type of component to get.</param>
		/// <param name="ignoreCallingTransform">Should the calling transform be ignored?</param>
		/// <returns>
		/// List of components of type <see cref="componentType"/> that transform's hierarchy contains.
		/// </returns>
		public static List<Component> GetComponentsInHierarchyNonAlloc(this Transform transform, Type componentType, 
		                                                          bool ignoreCallingTransform = false)
		{
			return transform.gameObject.GetComponentsInHierarchyNonAlloc(componentType, ignoreCallingTransform);
		}

		#endregion
	}
}