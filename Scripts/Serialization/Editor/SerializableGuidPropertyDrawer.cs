using System.Text;
using UnityEditor;
using UnityEngine;

namespace Unidork.Serialization
{
    [CustomPropertyDrawer(typeof(SerializableGuid))]
    public class SerializableGuidPropertyDrawer : PropertyDrawer 
    {
        private float ySep = 20f;
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            
            EditorGUI.BeginProperty(position, label, property);

            GUI.enabled = false;
            
            var serializedGuid = (SerializableGuid)fieldInfo.GetValue(property.serializedObject.targetObject);
            
            byte[] serializedGuidAsBytes = serializedGuid;

            var guidString = new StringBuilder(70);

            for (var i = 0; i < serializedGuidAsBytes.Length; i++)
            {
                guidString.Append(serializedGuidAsBytes[i].ToString());

                if (i < serializedGuidAsBytes.Length - 1)
                {
                    guidString.Append("-");
                }
            }

            position = EditorGUI.PrefixLabel(new Rect(position.x, position.y + ySep / 2f, position.width, position.height), GUIUtility.GetControlID(FocusType.Passive), label);
            position.y -= ySep;

            Rect pos = new Rect(position.xMin, position.yMin + ySep, position.width, ySep - 2f);

            EditorGUI.TextField(pos, guidString.ToString());
            
            EditorGUI.EndProperty();
            
            GUI.enabled = true;
        }
     
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) => ySep * 2f;
    }
}