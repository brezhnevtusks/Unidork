using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Unidork.Events
{
	[CustomPropertyDrawer(typeof(EventResponsePair))]
    public class EventResponsePairPropertyDrawer : PropertyDrawer
    {
		private EventResponsePair eventResponsePair;

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{		
			const string typePropertyName = "type";
			const string eventPropertyName = "event";
			const string eventsPropertyName = "events";
			const string responsePropertyName = "response";
			const string showContentPropertyName = "showContent";

			EditorGUI.LabelField(position, label, EditorStyles.boldLabel);

			EditorGUI.BeginProperty(position, label, property);

			SerializedProperty typeProperty = property.FindPropertyRelative(typePropertyName);

			EditorGUILayout.PropertyField(typeProperty);

			var eventResponsePairType = (EventResponsePairType)typeProperty.enumValueIndex;

			if (eventResponsePairType == EventResponsePairType.SingleEvent)
			{
				EditorGUILayout.PropertyField(property.FindPropertyRelative(eventPropertyName));
			}
			else
			{
				EditorGUILayout.PropertyField(property.FindPropertyRelative(eventsPropertyName));
			}			

			SerializedProperty foldContentProperty = property.FindPropertyRelative(showContentPropertyName);

			bool showContent = foldContentProperty.boolValue;

			showContent = EditorGUILayout.Foldout(showContent, "Response");

			foldContentProperty.boolValue = showContent;

			SerializedProperty responseProperty = property.FindPropertyRelative(responsePropertyName);

			if (showContent)
			{
				EditorGUILayout.PropertyField(responseProperty);

				if (GUILayout.Button("INVOKE"))
				{
					if (eventResponsePair == null)
					{
						var targetObject = fieldInfo.GetValue(property.serializedObject.targetObject);

						var index = Convert.ToInt32(new string(property.propertyPath.Where(c => char.IsDigit(c)).ToArray()));
						eventResponsePair = ((EventResponsePair[])targetObject)[index];
					}

					eventResponsePair.Response.Invoke();
				}
			}			

			EditorGUI.EndProperty();
		}
	}
}