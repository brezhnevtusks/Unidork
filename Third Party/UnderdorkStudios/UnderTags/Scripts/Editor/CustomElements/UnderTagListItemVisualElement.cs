using System.Collections.Generic;
using System.Linq;
#if UNDERTAGS_NEW_INPUT
using UnityEngine.InputSystem;
#else
using UnityEngine;
#endif
using UnityEngine.UIElements;

namespace UnderdorkStudios.UnderTags.Editor
{
#if UNITY_2023_1_OR_NEWER
    [UxmlElement]
    public partial class UnderTagListItemVisualElement : VisualElement
#else
    public class UnderTagListItemVisualElement : VisualElement
#endif
    {
#if !UNITY_2023_1_OR_NEWER
        public new class UxmlFactory : UxmlFactory<UnderTagListItemVisualElement> {}
#endif
        #region Properties

        public UnderTag Tag => tag;
        
        public bool IsExpanded => GetClasses().Contains(UnderTagStyles.TagExpandedClass);
        public bool IsHidden => GetClasses().Contains(UnderTagStyles.TagHiddenClass);
        
        public delegate void OnExpandStateChangedEvent();
        public event OnExpandStateChangedEvent OnExpandStateChanged;

        #endregion
        
        #region Fields

        private UnderTag tag;
    
        private UnderTagContainer tagContainer;
        public UnderTagPropertyVisualElement propertyVisualElement;
        private VisualElement mainTagContainer;
        private VisualElement childTagContainer;
        private VisualElement expandArrow;
        private Toggle tagToggle;
        private Label tagLabel;

        #endregion

        #region Setup
        public void SetUp(UnderTag tag, UnderTagContainer tagContainer, UnderTagPropertyVisualElement propertyVisualElement)
        {
            UnderTagDatabase database = UnderTagDatabase.GetOrCreateUnderTagDatabase();
            
            this.tag = tag;
            this.tagContainer = tagContainer;
            this.propertyVisualElement = propertyVisualElement;

            mainTagContainer = this.Q<VisualElement>(UnderTagStyles.MainTagContainerName);
            childTagContainer = this.Q<VisualElement>(UnderTagStyles.ChildTagContainerName);

            expandArrow = this.Q<VisualElement>(UnderTagStyles.ExpandTagArrowName);
            tagLabel = this.Q<Label>(UnderTagStyles.TagLabelName);
            
            tagLabel.text = tag.GetIndividualName();
            
            this
                .Q<VisualElement>(UnderTagStyles.ExpandTagContainerName)
                .RegisterCallback<MouseDownEvent>(OnPressed);
            
            if (!database.HasChildTags(tag))
            {
                HideExpandArrow();
            }

            var addChildButton = this.Q<Button>(UnderTagStyles.AddChildTagButtonName);

            if (addChildButton != null)
            {
                addChildButton.clicked += OnAddChildTagButtonPressed;
            }

            var removeTagButton = this.Q<Button>(UnderTagStyles.RemoveTagButtonName);

            if (removeTagButton != null)
            {
                removeTagButton.clicked += OnRemoveTagButtonPressed;
            }

            tagToggle = this.Q<Toggle>(UnderTagStyles.TagToggleName);
            tagToggle?.RegisterCallback<ChangeEvent<bool>>(OnTogglePressed);

            float offset = UnderTagStyles.ChildTagOffset;

            if (tagContainer != null)
            {
                offset = UnderTagStyles.ChildTagOffsetTagContainer;
            }
            
            expandArrow.style.marginLeft = UnderTagStyles.DefaultChildTagOffset + offset * tag.GetDepth();
        }

        #endregion

        #region Hierarchy

        private bool TryGetParentElement(out UnderTagListItemVisualElement parentElement)
        {
            parentElement = null;
            VisualElement nextParent = parent;

            while (nextParent != null)
            {
                if (string.Equals(nextParent.name, UnderTagStyles.TagContainerName))
                {
                    parentElement = (UnderTagListItemVisualElement)nextParent;
                    return true;
                }

                nextParent = nextParent.parent;
            }

            return false;
        }

        #endregion

        #region Show/Hide

        public void Show()
        {
            RemoveFromClassList(UnderTagStyles.TagHiddenClass);
            childTagContainer.RemoveFromClassList(UnderTagStyles.TagHiddenClass);
        }

        public void Hide()
        {
            AddToClassList(UnderTagStyles.TagHiddenClass);
            childTagContainer.AddToClassList(UnderTagStyles.TagHiddenClass);
        }

        #endregion

        #region Expand

        public void HideExpandArrow()
        {
            expandArrow.AddToClassList(UnderTagStyles.ExpandTagArrowHiddenClass);
        }

        public void Expand(bool expandAllDescendants = false)
        {
            UnderTagDatabase tagDatabase = UnderTagDatabase.GetOrCreateUnderTagDatabase();

            if (tagDatabase.TryGetChildTags(tag, out _))
            {
                foreach (VisualElement visualElement in childTagContainer.Children())
                {
                    var childTagElement = visualElement.Q<UnderTagListItemVisualElement>();
                    childTagElement.Show();

                    if (childTagElement.IsExpanded || expandAllDescendants)
                    {
                        childTagElement.Expand(expandAllDescendants);
                    }
                    else
                    {
                        childTagElement.Collapse();
                    }
                }
                    
                ShowExpandArrow();
                RotateExpandArrow();
                AddToClassList(UnderTagStyles.TagExpandedClass);
            }
        }

        public void Collapse(bool collapseAllDescendants = false)
        {
            List<UnderTagListItemVisualElement> childTags = childTagContainer
                .Query<VisualElement>()
                .Descendents<UnderTagListItemVisualElement>()
                .ToList();

            foreach (UnderTagListItemVisualElement childTag in childTags)
            {
                if (collapseAllDescendants)
                {
                    childTag.Collapse(true);
                }
                
                childTag.Hide();
            }
            
            ResetExpandArrow();
            RemoveFromClassList(UnderTagStyles.TagExpandedClass);
        }

        private void ShowExpandArrow()
        {
            expandArrow.RemoveFromClassList(UnderTagStyles.ExpandTagArrowHiddenClass);
        }

        private void RotateExpandArrow()
        {
            expandArrow.AddToClassList(UnderTagStyles.ExpandTagArrowRotatedClass);
        }
        
        private void ResetExpandArrow()
        {
            expandArrow.RemoveFromClassList(UnderTagStyles.ExpandTagArrowRotatedClass);
        }
        
        private void OnPressed(MouseDownEvent mouseDownEvent)
        {
#if UNDERTAGS_NEW_INPUT
            Keyboard currentKeyboard = Keyboard.current;
            bool altHeld = currentKeyboard[Key.LeftAlt].isPressed || currentKeyboard[Key.RightAlt].isPressed;
#else
            bool altHeld = Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt);
#endif
            if (GetClasses().Contains(UnderTagStyles.TagExpandedClass))
            {
                Collapse(altHeld);
            }
            else
            {
                if (UnderTagDatabase.GetOrCreateUnderTagDatabase().HasChildTags(tag))
                {
                    Expand(altHeld);
                }
            }
            
            OnExpandStateChanged?.Invoke();
        }

        #endregion
        
        #region Toggle

        public void SetToggleValue(bool value)
        {
            tagToggle.SetValueWithoutNotify(value);
        }
        
        private void OnTogglePressed(ChangeEvent<bool> changeEvent)
        {
            bool newToggleValue = changeEvent.newValue;
            
            if (propertyVisualElement != null)
            {
                if (!newToggleValue)
                {
                    if (propertyVisualElement.HasValidTag && propertyVisualElement.Tag != tag)
                    {
                        if (propertyVisualElement.HasValidTag)
                        {
                            propertyVisualElement.ClearSelectedTag();
                        }

                        propertyVisualElement.SetSelectedTag(tag);
                    }
                    else
                    {
                        propertyVisualElement.ClearSelectedTag();
                    }
                }
                else
                {
                    if (propertyVisualElement.HasValidTag)
                    {
                        propertyVisualElement.ClearSelectedTag();
                    }
                    
                    propertyVisualElement.SetSelectedTag(tag);
                }
            }
            else
            {
                if (!newToggleValue)
                {
                    if (((IUnderTagOwner)tagContainer).HasExact(tag))
                    {
                        tagContainer.RemoveTag(tag);
                        UnderTagContainerEditor.Current.Refresh(tag, false);
                    }
                    else
                    {
                        tagContainer.AddTag(tag);
                        UnderTagContainerEditor.Current.Refresh(tag, true);
                    }
                }
                else
                {
                    tagContainer.AddTag(tag);
                    UnderTagContainerEditor.Current.Refresh(tag, true);
                }
            }
            
            UnderTagSelectionPopupContent.Current?.RefreshToggles();
        }

        #endregion

        #region Label

        public void UpdateLabel(string filterString = "")
        {
            if (string.IsNullOrEmpty(filterString))
            {
                tagLabel.text = tag.GetIndividualName();
            }
            else
            {
                tagLabel.text = $"<mark=#ffff00aa>{tag.GetIndividualName()}</mark>";
            }
        }

        #endregion
        
        #region Child tag

        private void OnAddChildTagButtonPressed()
        {
            UnderTagSettingsProvider.Instance.OnAddChildTagButtonPressed(tag);
        }

        private void OnRemoveTagButtonPressed()
        {
            UnderTagSettingsProvider.Instance.OnRemoveTagButtonPressed(tag);
        }

        #endregion

        #region Background

        public void RemoveAlternativeBackgroundColor()
        {
            mainTagContainer.RemoveFromClassList(UnderTagStyles.AlternativeTagBackgroundClass);
        }
        
        public void SetAlternativeBackgroundColor()
        {
            mainTagContainer.AddToClassList(UnderTagStyles.AlternativeTagBackgroundClass);
        }

        #endregion
    }
}