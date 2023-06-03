using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unidork.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Unidork.Events
{
	/// <summary>
	/// Holds methods used to locate objects of type <see cref="GameEvent"/> and <see cref="GameEventListener"/>
	/// in build scenes and create data objects that can be used for custom inspectors showing game event references.
	/// </summary>	
	public static class GameEventLocator
	{
		#region Events

		/// <summary>
		/// Locates all instances of a game event in scenes added to build settings.
		/// </summary>
		/// <remarks>
		/// Scenes to search have to be active in the build settings.
		/// </remarks>
		/// <param name="eventToLocate">Event to locate.</param>
		/// <returns>
		/// A list of <see cref="GameEventReference"/> storing data about gameobjects and their components that contains a reference to the event we're trying to locate.
		/// </returns>
		public static List<GameEventReference> LocateEvent(GameEvent eventToLocate)
		{
			var locatedGameEventData = new List<GameEventReference>();

			// Iterate over all scenes added to the build settings

			foreach (EditorBuildSettingsScene buildScene in EditorBuildSettings.scenes)
			{
				// Skip inactive scenes

				if (!buildScene.enabled)
				{
					continue;
				}

				// Get scene build name

				string sceneName = AssetDatabase.LoadAssetAtPath<SceneAsset>(buildScene.path).name;

				// Get root game objects

				GameObject[] rootGameObjects = SceneManager.GetSceneByName(sceneName).GetRootGameObjects();

				foreach (GameObject rootGameObject in rootGameObjects)
				{
					var components = rootGameObject.GetComponentsInChildren<Component>();

					// Iterate over all components on an object and get their fields using reflection

					foreach (var component in components)
					{
						// Look for both private and public fields as references can be public or private with [SerializeField] attribute

						FieldInfo[] fieldInfo = component.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

						foreach (var info in fieldInfo)
						{
							// Filter fields of type GameEvent

							if (info.FieldType == typeof(GameEvent))
							{
								GameEvent fieldValue = (GameEvent)info.GetValue(component);

								// If the field's value is equal to the event we're locating, create new LocatedGameEventData and add it to the list.

								if (fieldValue == eventToLocate)
								{
									// Capitalize the first letter of the field name and split the string by capital letters
									// for better readability

									string fieldName = info.Name;	
									
									fieldName = fieldName.First().ToString().ToUpper() + fieldName.Substring(1);

									fieldName = fieldName.SplitByCapitalLetters();

									locatedGameEventData.Add(new GameEventReference(eventToLocate, component.gameObject, component, fieldName));
								}
							}
						}
					}
				}
			}

			return locatedGameEventData;
		}

		#endregion

		#region Game event listeners

		public static List<GameEventListenerReference> LocateGameEventListeners()
		{
			var locatedGameEventListenerData = new List<GameEventListenerReference>();

			// Iterate over all scenes added to the build settings

			foreach (EditorBuildSettingsScene buildScene in EditorBuildSettings.scenes)
			{
				// Skip inactive scenes

				if (!buildScene.enabled)
				{
					continue;
				}

				// Get scene build name

				string sceneName = AssetDatabase.LoadAssetAtPath<SceneAsset>(buildScene.path).name;

				// Get root game objects

				GameObject[] rootGameObjects = SceneManager.GetSceneByName(sceneName).GetRootGameObjects();

				foreach (GameObject rootGameObject in rootGameObjects)
				{
					// Get all game event listeners

					var gameEventListeners = rootGameObject.GetComponentsInChildren<GameEventListener>();					

					foreach (var gameEventListener in gameEventListeners)
					{
						// Skip listeners with no valid event response pairs

						EventResponsePair[] eventResponsePairs = gameEventListener.EventReponsePairs;

						if (eventResponsePairs == null || eventResponsePairs.Length == 0)
						{
							continue;
						}

						for (int i = 0; i < eventResponsePairs.Length; i++)
						{
							GameEvent gameEvent = eventResponsePairs[i].Event;
							UnityEvent response = eventResponsePairs[i].Response;							

							// Skip listeners that don't do anything or aren't fully initialized
							// TODO: Perhaps still show them so we know about useless components?

							if (gameEvent == null || response == null || response.GetPersistentEventCount() == 0)
							{
								continue;
							}

							for (int j = 0; j < response.GetPersistentEventCount(); j++)
							{
								string methodName = response.GetPersistentMethodName(i);
								Object @object = response.GetPersistentTarget(i);

								// Skip listeners that don't have the required data assigned

								if (string.IsNullOrEmpty(methodName) || @object == null)
								{
									continue;
								}								

								// Get method info that allows us to get the actual component type that calls the method
								// So far only works for parameterless methods but should be possible to cycle through types
								// that UnityEvent can accept and pass the array with that type to GetValidMethodInfo below

								MethodInfo methodInfo = UnityEventBase.GetValidMethodInfo(@object, methodName, new System.Type[0]);

								if (methodInfo != null)
								{
									locatedGameEventListenerData.Add(new GameEventListenerReference(gameEventListener, gameEvent, gameEventListener.gameObject,
																	gameEventListener.GetComponent(methodInfo.DeclaringType), methodName));
								}
							}							
						}
					}
				}
			}

			Debug.Log($"Returning {locatedGameEventListenerData.Count} listeners");

			return locatedGameEventListenerData;
		}

		#endregion
	}
}