using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnderdorkStudios.UnderTags.Editor
{
#if UNITY_2023_1_OR_NEWER
    [UxmlElement]
    public partial class UnderTagPropertyVisualElement : VisualElement
#else
    public class UnderTagPropertyVisualElement : VisualElement
#endif
    {
#if !UNITY_2023_1_OR_NEWER
        public new class UxmlFactory : UxmlFactory<UnderTagPropertyVisualElement> {}
#endif
        #region Properties

        public bool HasValidTag => ((UnderTag)property.boxedValue).IsValid();
        public UnderTag Tag => (UnderTag)property.boxedValue;
        
        #endregion
        
        #region Fields

        private SerializedProperty property;
        private UnderTagPropertyDrawer propertyDrawer;
        private Label tagNameLabel;
        private Button clearTagButton;
        
        #endregion

        #region Setup

        public void SetUp(SerializedProperty property, UnderTagPropertyDrawer propertyDrawer)
        {
            this.property = property;
            this.propertyDrawer = propertyDrawer;

            this.Q<Label>(UnderTagStyles.TagPropertyNameName).text = property.displayName;
            
            tagNameLabel = this.Q<Label>(UnderTagStyles.TagValueName);

            RefreshTagValueLabel();
            
            clearTagButton = this.Q<Button>(UnderTagStyles.ClearTagButtonName);
            clearTagButton.clicked += OnClearTagButtonPressed;
            
            this
                .Q<VisualElement>(UnderTagStyles.TagBackgroundName)
                .RegisterCallback<MouseDownEvent>(_ => OnSelectTagButtonPressed());

            ToggleClearButton();
        }

        #endregion

        #region Tags

        public void SetSelectedTag(UnderTag tag)
        {
            property.boxedValue = tag;
            RefreshTagValueLabel();
            ToggleClearButton();
            property.serializedObject.ApplyModifiedProperties();
        }
        
        public void ClearSelectedTag()
        {
            OnClearTagButtonPressed();
        }

        #endregion
        
        #region Labels

        private void RefreshTagValueLabel()
        {
            var tag = (UnderTag)property.boxedValue;
            var tagDatabase = UnderTagDatabase.GetOrCreateUnderTagDatabase();
            tagNameLabel.text = tagDatabase.TagIsValid(tag) ? tag.Value : "---";
        }

        #endregion

        #region Buttons

        private void OnSelectTagButtonPressed()
        {
            Rect rect = tagNameLabel.worldBound;
            rect.position += UnderTagStyles.DesiredTagSelectionPopupOffset;
            UnityEditor.PopupWindow.Show(rect, new UnderTagSelectionPopupContent(null, this));
        }

        private void OnClearTagButtonPressed()
        {
            property.boxedValue = new UnderTag();
            RefreshTagValueLabel();
            ToggleClearButton();
            property.serializedObject.ApplyModifiedProperties();
        }
  
        private void ToggleClearButton()
        {
            var tag = (UnderTag)property.boxedValue;

            if (tag.IsValid())
            {
                clearTagButton.RemoveFromClassList(UnderTagStyles.ElementHidden);
            }
            else
            {
                clearTagButton.AddToClassList(UnderTagStyles.ElementHidden);
            }
        }

        #endregion
    }
}