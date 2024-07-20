using System.Collections.Generic;
using System.Reflection;
using UnderdorkStudios.UnderTools.Extensions;
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
        private VisualElement tagValueContainer;
        private Label tagNameLabel;
        private DropdownField tagDropdown;
        private Button clearTagButton;

        private Dictionary<string, UnderTag> dropdownTagDict;
        
        #endregion

        #region Setup

        public void SetUp(SerializedProperty property, UnderTagPropertyDrawer propertyDrawer)
        {
            this.property = property;
            this.propertyDrawer = propertyDrawer;

            this.Q<Label>(UnderTagStyles.TagPropertyNameName).text = property.displayName;
            
            tagNameLabel = this.Q<Label>(UnderTagStyles.TagValueName);
            tagValueContainer = this.Q<VisualElement>(UnderTagStyles.TagValueContainerName);
            tagDropdown = this.Q<DropdownField>(UnderTagStyles.TagDropdownName);
            clearTagButton = this.Q<Button>(UnderTagStyles.ClearTagButtonName);

            UnderTagDropdownAttribute dropdownAttribute = propertyDrawer.fieldInfo.GetCustomAttribute<UnderTagDropdownAttribute>(false);

            if (dropdownAttribute != null)
            {
                UnderTagDatabase tagDatabase = UnderTagDatabase.GetOrCreateUnderTagDatabase();
                UnderTag parentTag = new UnderTag(dropdownAttribute.ParentTag);
                dropdownTagDict = new Dictionary<string, UnderTag>();
                
                if (tagDatabase.TagIsValid(parentTag))
                {
                    List<UnderTag> dropdownTags = new();

                    if (dropdownAttribute.IncludeParent)
                    {
                        dropdownTags.Add(parentTag);
                    }
                    
                    if (dropdownAttribute.DirectChildrenOnly)
                    {
                        _ = tagDatabase.TryGetChildTags(parentTag, out dropdownTags);
                    }
                    else
                    {
                        tagDatabase.GetDescendentTags(parentTag, dropdownTags);
                    }

                    foreach (UnderTag dropdownTag in dropdownTags)
                    {
                        _ = dropdownTagDict.TryAdd(dropdownTag.GetIndividualName(), dropdownTag);
                    }

                    tagDropdown.RemoveFromClassList(UnderTagStyles.ElementHidden);
                    
                    tagValueContainer.AddToClassList(UnderTagStyles.ElementHidden);
                    clearTagButton.AddToClassList(UnderTagStyles.ElementHidden);
                    
                    tagDropdown.choices = dropdownTagDict.Keys.ToList(dropdownTagDict.Count);

                    if (tagDropdown.choices.Count > 0)
                    {
                        UnderTag currentValue = (UnderTag)property.boxedValue;
                        tagDropdown.value = currentValue.IsValid() ? currentValue.GetIndividualName() : string.Empty;
                    }
                    
                    tagDropdown.RegisterValueChangedCallback(evt =>
                    {
                        SetSelectedTag(dropdownTagDict[evt.newValue], false);
                    });
                    
                    return;
                }

                Debug.LogError($"UnderTags: UnderTagDropdownAttribute has an invalid parent tag: {parentTag.Value}!");
            }
            
            tagValueContainer.RemoveFromClassList(UnderTagStyles.ElementHidden);
            clearTagButton.RemoveFromClassList(UnderTagStyles.ElementHidden);
            tagDropdown.AddToClassList(UnderTagStyles.ElementHidden);
            
            RefreshTagValueLabel();
            
            clearTagButton.clicked += OnClearTagButtonPressed;
            
            this
                .Q<VisualElement>(UnderTagStyles.TagBackgroundName)
                .RegisterCallback<MouseDownEvent>(_ => OnSelectTagButtonPressed());

            ToggleClearButton();
        }

        #endregion

        #region Tags

        public void SetSelectedTag(UnderTag tag, bool toggleClearButton = true)
        {
            property.boxedValue = tag;
            RefreshTagValueLabel();

            if (toggleClearButton)
            {
                ToggleClearButton();
            }
            
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