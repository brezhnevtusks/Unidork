using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnderdorkStudios.UnderTags.Editor
{
#if UNITY_2023_1_OR_NEWER
    [UxmlElement]
    public partial class UnderTagSelectionPopupVisualElement : VisualElement
#else
    public class UnderTagSelectionPopupVisualElement : VisualElement
#endif
    {
#if !UNITY_2023_1_OR_NEWER
        public new class UxmlFactory : UxmlFactory<UnderTagSelectionPopupVisualElement> {}
#endif
        #region Fields

        private UnderTagContainer tagContainer;
        private UnderTagPropertyVisualElement propertyVisualElement;
        private VisualElement selectTagsContainer;
        private TextField filterTextField;
        
        #endregion

        #region Setup

        public void SetUp(UnderTagContainer tagContainer, UnderTagPropertyVisualElement propertyVisualElement)
        {
            this.tagContainer = tagContainer;
            this.propertyVisualElement = propertyVisualElement;

            selectTagsContainer = this.Q<VisualElement>(UnderTagStyles.SelectedTagsContainerName);
            
            filterTextField = this.Q<TextField>(UnderTagStyles.FilterTagsTextFieldName);
            filterTextField.RegisterCallback<ChangeEvent<string>>(OnFilterTextFieldUpdated);
            
            SetUpAddTagList();
            RefreshToggles();
            RefreshTagBackgroundColors();
        }
        
        private void SetUpAddTagList()
        {
            UnderTagDatabase tagDatabase = UnderTagDatabase.GetOrCreateUnderTagDatabase();

            List<UnderTag> rootTags = new(tagDatabase.RootTags);

            if (rootTags.Count == 0)
            {
                return;
            }
            
            rootTags.Sort();
            
            selectTagsContainer.hierarchy.Clear();

            VisualTreeAsset tagTemplate = UnderTagEditorResources.Instance.UnderTagContainerListItemUxml;

            foreach (UnderTag rootTag in rootTags)
            { 
                VisualElement tagElement = tagTemplate.Instantiate();
                var rootTagVisualElement = (UnderTagListItemVisualElement)tagElement.hierarchy[0];
                rootTagVisualElement.SetUp(rootTag, tagContainer, propertyVisualElement);
                rootTagVisualElement.OnExpandStateChanged += RefreshTagBackgroundColors;
                selectTagsContainer.hierarchy.Add(tagElement);
                
                SetUpChildTags(rootTagVisualElement, tagDatabase);
            }
        }

        private void SetUpChildTags(UnderTagListItemVisualElement parentVisualElement, UnderTagDatabase tagDatabase)
        {
            UnderTag parentTag = parentVisualElement.Tag;
            
            if (tagDatabase.TryGetChildTags(parentTag, out List<UnderTag> childTags))
            {
                VisualTreeAsset tagTemplate = UnderTagEditorResources.Instance.UnderTagContainerListItemUxml;
                var childTagContainer = parentVisualElement.Q<VisualElement>(UnderTagStyles.ChildTagContainerName);
                    
                foreach (UnderTag childTag in childTags)
                {
                    VisualElement childTagElement = tagTemplate.Instantiate();

                    var childTagVisualElement = (UnderTagListItemVisualElement)childTagElement.hierarchy[0];
                    childTagVisualElement.SetUp(childTag, tagContainer, propertyVisualElement);
                    childTagVisualElement.OnExpandStateChanged += RefreshTagBackgroundColors;
                    childTagVisualElement.Hide();
                    childTagContainer.Add(childTagElement);
                    
                    SetUpChildTags(childTagVisualElement, tagDatabase);
                }
            }
        }

        #endregion

        #region Filter

        private void OnFilterTextFieldUpdated(ChangeEvent<string> changeEvent)
        {
            string filterString = changeEvent.newValue;

            List<UnderTagListItemVisualElement> tagVisualElements = GetUnderTagListItemElements();
            
            if (string.IsNullOrEmpty(filterString))
            {
                Debug.Log("FILTER IS EMPTY");
                foreach (UnderTagListItemVisualElement tagVisualElement in tagVisualElements)
                {
                    tagVisualElement.UpdateLabel();
                }
            }
            else
            {
                UnderTagDatabase tagDatabase = UnderTagDatabase.GetOrCreateUnderTagDatabase();
                List<UnderTag> bottomTags = tagDatabase.GetBottomTags();

                foreach (UnderTagListItemVisualElement tagVisualElement in tagVisualElements)
                {
                    tagVisualElement.UpdateLabel();
                }
                
                GetUnderTagListItemElements();
                
                foreach (UnderTag bottomTag in bottomTags)
                {
                    if (bottomTag.Value.Contains(filterString))
                    {
                        tagVisualElements.Find(element => element.Tag.MatchesExactly(bottomTag)).UpdateLabel(filterString);
                        
                        Debug.Log(bottomTag.Value);
                    }
                }
            }
        }

        #endregion
        
        #region Refresh

        public void RefreshToggles()
        {
            var tagListItemElements = this
                .Query()
                .Descendents<UnderTagListItemVisualElement>()
                .ToList();

            if (propertyVisualElement != null)
            {
                if (!propertyVisualElement.HasValidTag)
                {
                    foreach (UnderTagListItemVisualElement tagListItemVisualElement in tagListItemElements)
                    {
                        tagListItemVisualElement.SetToggleValue(false);
                    }
                }
                else
                {
                    foreach (UnderTagListItemVisualElement tagListItemElement in tagListItemElements)
                    {
                        UnderTag tag = propertyVisualElement.Tag;
                        List<UnderTag> tagsToEnable = UnderTagDatabase
                            .GetOrCreateUnderTagDatabase()
                            .GetParentTags(tag);
                        tagsToEnable.Add(tag);
                        tagListItemElement.SetToggleValue(tagsToEnable.Contains(tagListItemElement.Tag));
                    }
                }
            }
            else if (tagContainer != null)
            {
                foreach (UnderTagListItemVisualElement tagListItemElement in tagListItemElements)
                {
                    tagListItemElement.SetToggleValue(((IUnderTagOwner)tagContainer).Has(tagListItemElement.Tag));
                }
            }
        }
        
        private void RefreshTagBackgroundColors()
        {
            UnderTagDatabase tagDatabase = UnderTagDatabase.GetOrCreateUnderTagDatabase();

            List<UnderTagListItemVisualElement> listElements = GetUnderTagListItemElements();
            listElements = listElements.FindAll(element => tagDatabase.IsRootTag(element.Tag) || !element.IsHidden);

            for (int i = 0, count = listElements.Count; i < count; i++)
            {
                if (i % 2 == 0)
                {
                    listElements[i].RemoveAlternativeBackgroundColor();
                }
                else
                {
                    listElements[i].SetAlternativeBackgroundColor();
                }
            }
        }
        
        private List<UnderTagListItemVisualElement> GetUnderTagListItemElements()
        {
            return selectTagsContainer
                .Query<UnderTagListItemVisualElement>()
                .ToList();
        }

        #endregion
    }
}