using UnityEditor;
using UnityEngine.UIElements;

namespace UnderdorkStudios.UnderTags.Editor
{
    [CustomPropertyDrawer(typeof(UnderTag))]
    public class UnderTagPropertyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualTreeAsset underTagTemplate = UnderTagEditorResources.Instance.UnderTagPropertyUxml;
            VisualElement underTag = underTagTemplate.Instantiate();
            var rootElement = (UnderTagPropertyVisualElement)underTag.hierarchy[0];
            rootElement.SetUp(property, this);
            return underTag;
        }
    }
}