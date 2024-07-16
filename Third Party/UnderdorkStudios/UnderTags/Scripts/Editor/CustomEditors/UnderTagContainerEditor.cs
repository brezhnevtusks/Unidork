using System.Collections.Generic;
using UnderdorkStudios.UnderTools.Extensions;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnderdorkStudios.UnderTags.Editor
{
    [CustomEditor(typeof(UnderTagContainer))]
    public class UnderTagContainerEditor : UnityEditor.Editor
    {
        #region Properties

        public static UnderTagContainerEditor Current { get; private set; }

        #endregion

        #region Fields

        private UnderTagContainer tagContainer;
        private VisualElement rootElement;
        private VisualElement ownedTagRootContainer;
        private VisualElement ownedTagContainer;
        private Button toggleAddTagContainerButton;
        private VisualElement noTagsAddedContainer;

        #endregion

        #region GUI

        public override VisualElement CreateInspectorGUI()
        {
            Current = this;
            tagContainer = (UnderTagContainer)serializedObject.targetObject;
            
            UnderTagEditorResources editorResources = UnderTagEditorResources.Instance;
            VisualTreeAsset containerTemplate = editorResources.UnderTagContainerUxml;
            rootElement = containerTemplate.Instantiate();

            noTagsAddedContainer = rootElement.Q<VisualElement>(UnderTagStyles.NoTagsAddedContainerName);

            HelpBox noTagsAddedHelpBox = new HelpBox("This container is empty! Click the plus icon above to add some tags!",
                                                     HelpBoxMessageType.Info);
            noTagsAddedContainer.hierarchy.Add(noTagsAddedHelpBox);

            ownedTagRootContainer = rootElement.Q<VisualElement>(UnderTagStyles.OwnedTagRootContainerName);
            
            ownedTagContainer = rootElement.Q<VisualElement>(UnderTagStyles.OwnedTagContainerName);
            ownedTagContainer.hierarchy.Clear();

            VisualTreeAsset tagTemplate = editorResources.UnderTagUxml;

            List<UnderTag> ownedTags = new(tagContainer.OwnedTags);
            ownedTags.Sort();
            
            foreach (UnderTag ownedTag in ownedTags)
            {
                VisualElement tagRoot = tagTemplate.Instantiate();
                var tagVisualElement = (UnderTagVisualElement)tagRoot.hierarchy[0];

                tagVisualElement.SetUp(tagContainer, ownedTag);
                ownedTagContainer.Add(tagRoot);
            }

            toggleAddTagContainerButton = rootElement.Q<Button>(UnderTagStyles.ToggleAddTagContainerButtonName);
            toggleAddTagContainerButton.clicked += OnToggleAddTagContainerButtonPressed;
            
            Refresh();
            
            return rootElement;
        }
       
        #endregion

        #region Setup

        private void OnToggleAddTagContainerButtonPressed()
        {
            Rect rect = toggleAddTagContainerButton.worldBound;
            rect.position += UnderTagStyles.DesiredTagSelectionPopupOffset;
            UnityEditor.PopupWindow.Show(rect, new UnderTagSelectionPopupContent(tagContainer, null));
        }

        #endregion
        
        #region Refresh

        public void Refresh(UnderTag tag, bool tagWasAdded)
        {
            var tagDatabase = UnderTagDatabase.GetOrCreateUnderTagDatabase();

            if (tagWasAdded)
            {
                List<UnderTagVisualElement> ownedTagElements = rootElement
                    .Query()
                    .Descendents<UnderTagVisualElement>()
                    .ToList();
                
                List<UnderTag> addedTagHierarchy = tagDatabase.GetAllTagsInHierarchy(tag, true);

                foreach (UnderTag tagInHierarchy in addedTagHierarchy)
                {
                    UnderTagVisualElement tagElement = ownedTagElements.Find(element => element.Tag == tagInHierarchy);

                    if (tagElement != null)
                    {
                        ownedTagContainer.hierarchy.Remove(tagElement.parent);
                    }
                }
                
                VisualTreeAsset tagTemplate = UnderTagEditorResources.Instance.UnderTagUxml;
                VisualElement tagRoot = tagTemplate.Instantiate();
                var newTagElement = (UnderTagVisualElement)tagRoot.hierarchy[0];
                newTagElement.SetUp(tagContainer, tag);
                List<UnderTag> ownedTags = new(tagContainer.OwnedTags);
                ownedTags.AddAndSort(tag);
                ownedTagContainer.hierarchy.Insert(ownedTags.IndexOf(tag), tagRoot);
            }
            else
            {
                UnderTagVisualElement tagVisualElement = ownedTagContainer
                    .Query()
                    .Descendents<UnderTagVisualElement>()
                    .ToList()
                    .Find(element => element.Tag == tag);

                if (tagVisualElement != null)
                {
                    ownedTagContainer.hierarchy.Remove(tagVisualElement.parent);
                }
            }

            List<UnderTag> tagHierarchy = tagDatabase.GetAllTagsInHierarchy(tag);
            var tagListItemVisualElements = rootElement
                .Query()
                .Descendents<UnderTagListItemVisualElement>()
                .ToList()
                .FindAll(item => tagHierarchy.Contains(item.Tag));

            foreach (UnderTagListItemVisualElement tagListItemVisualElement in tagListItemVisualElements)
            {
                tagListItemVisualElement.SetToggleValue(((IUnderTagOwner)tagContainer).Has(tagListItemVisualElement.Tag));
            }
            
            if (tagContainer.IsEmpty())
            {
                ownedTagRootContainer.AddToClassList(UnderTagStyles.ElementHidden);
                noTagsAddedContainer.RemoveFromClassList(UnderTagStyles.ElementHidden);
            }
            else
            {
                ownedTagRootContainer.RemoveFromClassList(UnderTagStyles.ElementHidden);
                noTagsAddedContainer.AddToClassList(UnderTagStyles.ElementHidden);
            }
        }

        private void Refresh()
        {
            if (tagContainer.IsEmpty())
            {
                ownedTagRootContainer.AddToClassList(UnderTagStyles.ElementHidden);
                noTagsAddedContainer.RemoveFromClassList(UnderTagStyles.ElementHidden);
            }
            else
            {
                ownedTagRootContainer.RemoveFromClassList(UnderTagStyles.ElementHidden);
                noTagsAddedContainer.AddToClassList(UnderTagStyles.ElementHidden);
            }

            var tagListItemVisualElements = rootElement
                .Query()
                .Descendents<UnderTagListItemVisualElement>()
                .ToList();

            foreach (UnderTagListItemVisualElement tagListItemVisualElement in tagListItemVisualElements)
            {
                tagListItemVisualElement.SetToggleValue(((IUnderTagOwner)tagContainer).Has(tagListItemVisualElement.Tag));
            }
        }

        #endregion
    }
}