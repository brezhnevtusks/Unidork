using UnityEditor;
using UnityEngine;

namespace UnderdorkStudios.UnderTags.Editor
{
    [CustomEditor(typeof(UnderTagDatabase))]
    public class UnderTagDatabaseEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Generate"))
            {
                ((UnderTagDatabase)serializedObject.targetObject).GenerateConstantsFile();
            }
        }
    }
}