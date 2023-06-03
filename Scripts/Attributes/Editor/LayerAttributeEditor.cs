using Unidork.Attributes;
using UnityEditor;
using UnityEngine;
	
namespace Unidork.Attributes.Editor
{
	[CustomPropertyDrawer(typeof(SingleLayerAttribute))]
	public class LayerAttributeEditor : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			property.intValue = EditorGUI.LayerField(position, label, property.intValue);
			EditorGUI.EndProperty();
		}
	}
}