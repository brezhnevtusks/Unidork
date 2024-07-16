using System;
using UnityEngine;

namespace UnderdorkStudios.UnderTools.Extensions
{
	public static class GameObjectExtensions
	{	
		/// <summary>
		/// Checks if a game object has a component of a specific type.
		/// </summary>
		/// <typeparam name="T">Type of component to check for.</typeparam>
		/// <param name="gameObject">Game object.</param>
		/// <returns>
		/// True if <paramref name="gameObject"/> has a component of type T attached, False otherwise.
		/// </returns>
		public static bool HasComponent<T>(this GameObject gameObject) where T : Component
		{
			return gameObject.TryGetComponent<T>(out _);
		}

		/// <summary>
		/// Checks if a game object has a component of a specific type.
		/// </summary>
		/// <param name="gameObject">Game object.</param>
		/// <param name="componentType">Type of component to check for.</param>
		/// <returns>
		/// True if <paramref name="gameObject"/> has a component of type <paramref name="componentType"/> attached, False otherwise.
		/// </returns>
		public static bool HasComponent(this GameObject gameObject, Type componentType)
		{
			return gameObject.TryGetComponent(componentType, out _);
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
			T componentToReturn = gameObject.GetComponent<T>();

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
			Component componentToReturn = gameObject.GetComponent(componentType);

			if (componentToReturn == null)
			{
				componentToReturn = gameObject.AddComponent(componentType);
			}

			return componentToReturn;
		}
	}
}