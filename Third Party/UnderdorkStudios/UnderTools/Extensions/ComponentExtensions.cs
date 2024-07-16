using System;
using UnityEngine;

namespace UnderdorkStudios.UnderTools.Extensions
{
	public static class ComponentExtensions
	{
		/// <summary>
		/// Checks if component's game object has a component of a specific type.
		/// </summary>
		/// <typeparam name="T">Type of component to check for.</typeparam>
		/// <param name="component">Component.</param>
		/// <returns>
		/// True if <paramref name="component"/>'s game object has a component of type T attached, False otherwise.
		/// </returns>
		public static bool HasComponent<T>(this Component component) where T : Component
		{
			return component.gameObject.TryGetComponent<T>(out _);
		}

		/// <summary>
		/// Checks if component's game object has a component of a specific type.
		/// </summary>
		/// <param name="component">Component.</param>
		/// <param name="componentType">Type of component to check for.</param>
		/// <returns>
		/// True if <paramref name="component"/>'s game object has a component of type <paramref name="componentType"/> attached, False otherwise.
		/// </returns>
		public static bool HasComponent(this Component component, Type componentType)
		{
			return component.TryGetComponent(componentType, out _);
		}
		
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
			T componentToReturn = component.GetComponent<T>();

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
			Component componentToReturn = component.GetComponent(componentType);

			if (componentToReturn == null)
			{
				componentToReturn = component.gameObject.AddComponent(componentType);
			}

			return componentToReturn;
		}
	}
}