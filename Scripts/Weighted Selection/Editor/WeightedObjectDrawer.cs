using UnityEditor;
using UnityEngine;

namespace Unidork.WeightedSelection.Editor
{
    [CustomPropertyDrawer(typeof(WeightedObjectBase), useForChildren: true)]
    public class WeightedObjectDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.serializedObject.Update();

            EditorGUILayout.PropertyField(property.FindPropertyRelative("object"));

            EditorGUILayout.BeginHorizontal();
            
            EditorGUILayout.PropertyField(property.FindPropertyRelative("weight"));
            GUI.enabled = false;
            EditorGUILayout.PropertyField(property.FindPropertyRelative("selectionChance"));
            GUI.enabled = true;
            
            EditorGUILayout.EndHorizontal();
            
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}