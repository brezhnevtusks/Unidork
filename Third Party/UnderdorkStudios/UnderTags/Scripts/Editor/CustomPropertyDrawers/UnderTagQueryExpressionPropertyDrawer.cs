using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace UnderdorkStudios.UnderTags.Editor
{
    [CustomPropertyDrawer(typeof(UnderTagQueryExpression))]
    public class UnderTagQueryExpressionPropertyDrawer : PropertyDrawer
    {
        #region Fields

        private SerializedProperty property;
        private VisualElement rootElement;

        #endregion
        
        #region Constants

        private const string TypePropertyName = "type";
        private const string TagsPropertyName = "tags";
        private const string SubExpressionsPropertyName = "subExpressions";
        
        #endregion

        #region UI
        
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            this.property = property;
            
            rootElement = new VisualElement();

            SerializedProperty typeProperty = property.FindPropertyRelative("type");
            
            PropertyField propertyField = new PropertyField();
            propertyField.BindProperty(typeProperty);
            rootElement.Add(propertyField);

            var expressionType = (UnderTagQueryExpressionType)typeProperty.enumValueIndex;
            UpdateProperties(expressionType);
            
            propertyField.RegisterValueChangeCallback(OnExpressionTypeChanged);
            
            return rootElement;
        }

        private void OnExpressionTypeChanged(SerializedPropertyChangeEvent changeEvent)
        {
            var expressionType = (UnderTagQueryExpressionType)changeEvent.changedProperty.enumValueIndex;
            UpdateProperties(expressionType);
        }

        private void UpdateProperties(UnderTagQueryExpressionType expressionType)
        {
            var expression = (UnderTagQueryExpression)property.boxedValue;
            
            PropertyField tagsPropertyField = rootElement.Q<PropertyField>(TagsPropertyName);
            PropertyField subExpressionsPropertyField = rootElement.Q<PropertyField>(SubExpressionsPropertyName);
            
            if (expressionType is UnderTagQueryExpressionType.None)
            {
                expression.ClearTags();
                expression.ClearSubExpressions();
                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.Update();
                
                if (tagsPropertyField != null)
                {
                    rootElement.Remove(tagsPropertyField);
                }

                if (subExpressionsPropertyField != null)
                {
                    rootElement.Remove(subExpressionsPropertyField);
                }
            }
            else if (expressionType is UnderTagQueryExpressionType.AnyTagsMatch or UnderTagQueryExpressionType.AllTagsMatch
                     or UnderTagQueryExpressionType.NoTagsMatch)
            {
                expression.ClearSubExpressions();
                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.Update();
                
                if (tagsPropertyField == null)
                {
                    SerializedProperty tagsProperty = property.FindPropertyRelative(TagsPropertyName);
                
                    PropertyField propertyField = new PropertyField
                    {
                        name = TagsPropertyName
                    };
                    propertyField.BindProperty(tagsProperty);
                    rootElement.Add(propertyField);
                }
                
                if (subExpressionsPropertyField != null)
                {
                    rootElement.Remove(subExpressionsPropertyField);
                }
            }
            else
            {
                expression.ClearTags();
                property.serializedObject.ApplyModifiedProperties();
                property.serializedObject.Update();
                
                if (tagsPropertyField != null)
                {
                    rootElement.Remove(tagsPropertyField);
                }

                if (subExpressionsPropertyField == null)
                {
                    SerializedProperty subExpressionsProperty = property.FindPropertyRelative(SubExpressionsPropertyName);
                
                    PropertyField propertyField = new PropertyField
                    {
                        name = SubExpressionsPropertyName
                    };
                    propertyField.BindProperty(subExpressionsProperty);
                    rootElement.Add(propertyField);
                }
            }
        }
        
        #endregion
    }
}