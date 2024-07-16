using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnderdorkStudios.UnderTags.Editor
{
    public class UnderTagSelectionPopupContent : PopupWindowContent
    {
        #region Properties

        public static UnderTagSelectionPopupContent Current { get; private set; }

        #endregion

        #region Fields

        private readonly UnderTagContainer tagContainer;
        private readonly UnderTagPropertyVisualElement propertyVisualElement;
        private UnderTagSelectionPopupVisualElement underTagSelectionPopupVisualElement;

        #endregion

        #region Constructor

        public UnderTagSelectionPopupContent(UnderTagContainer tagContainer, UnderTagPropertyVisualElement propertyVisualElement)
        {
            this.tagContainer = tagContainer;
            this.propertyVisualElement = propertyVisualElement;
        }

        #endregion

        #region GUI
        
        public override void OnGUI(Rect rect)
        {
        }

        public override Vector2 GetWindowSize() => UnderTagStyles.TagSelectionPopupSize;

        public override void OnOpen()
        {
            Current = this;
            
            VisualTreeAsset template = UnderTagEditorResources.Instance.UnderTagSelectionPopupUxml;
            VisualElement popup = template.Instantiate();
            underTagSelectionPopupVisualElement = popup.Q<UnderTagSelectionPopupVisualElement>();
            underTagSelectionPopupVisualElement.SetUp(tagContainer, propertyVisualElement);
            editorWindow.rootVisualElement.hierarchy.Add(popup);
        }
        
        #endregion

        #region Popup

        public void RefreshToggles()
        {
            underTagSelectionPopupVisualElement.RefreshToggles();
        }

        #endregion
    }
}
