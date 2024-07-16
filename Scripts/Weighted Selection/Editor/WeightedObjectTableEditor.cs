using UnityEditor;
using UnityEngine;

namespace Unidork.WeightedSelection.Editor
{
    [CustomEditor(typeof(WeightedTableBase))]
    public class WeightedObjectTableEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var table = (WeightedTableBase)serializedObject.targetObject;

            SerializedProperty objects = serializedObject.FindProperty("objects");
        
            for (int i = 0; i < objects.arraySize; i++)
            {
                SerializedProperty weightedObjectProperty = objects.GetArrayElementAtIndex(i);
            
                EditorGUILayout.PropertyField(weightedObjectProperty, new GUIContent(weightedObjectProperty.FindPropertyRelative("editorLabel").stringValue));
            }

            if (GUILayout.Button("COMPUTE WEIGHTS"))
            {
                table.ComputeWeights();
            }
            
            DrawBuiltInTypeButtons(table);
            DrawCustomTypeButtons(table);

            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawCustomTypeButtons(WeightedTableBase table)
        {
        }

        private void DrawBuiltInTypeButtons(WeightedTableBase table)
        {
            if (GUILayout.Button("ADD FLOAT"))
            {
                table.GetObjects().Add(new WeightedObject_Float());
                EditorUtility.SetDirty(table);
            }
        
            if (GUILayout.Button("ADD INT"))
            {
                table.GetObjects().Add(new WeightedObject_Int());
                EditorUtility.SetDirty(table);
            }
#if UNIDORK_ADDRESSABLES 
            if (GUILayout.Button("ADD ASSET REF"))
            {
                table.GetObjects().Add(new WeightedObject_AssetReference());
                EditorUtility.SetDirty(table);
            }
#endif
            if (GUILayout.Button("ADD TABLE REF"))
            {
                table.GetObjects().Add(new WeightedObject_WeightedObjectTable());
                EditorUtility.SetDirty(table);
            }
        }
    }
}