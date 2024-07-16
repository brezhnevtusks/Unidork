using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnderdorkStudios.UnderTags.Editor
{
    public class UnderTagSettingsProvider : SettingsProvider
    {
        #region Constants

        private const string UnderTagsLabel = "UnderTags";
        private const string SettingsPath = "Project/UnderTags";

        #endregion

        #region Properties

        public static UnderTagSettingsProvider Instance => instance;

        #endregion

        #region Fields

        private static UnderTagSettingsProvider instance;
        private static VisualElement settingsRootElement;
        private Button toggleAddTagBoxButton;
        private Button addTagButton;
        private TextField addTagTextField;
        private ListView tagListView;
        private VisualElement tagListContainer;
        private VisualElement helpBoxContainer;
        private VisualElement databaseErrorContainer;
        private bool isShowingDatabaseEmptyHelpBox;
        private bool isShowingDatabaseErrorHelpBox;
        
        #endregion

        #region Constructor

        [SettingsProvider]
        public static SettingsProvider CreateUnderTagSettingsProvider()
        {
            instance = new UnderTagSettingsProvider(SettingsPath, SettingsScope.Project)
            {
                label = UnderTagsLabel,
                activateHandler = SetUpUnderTagSettings
            };
            
            return instance;
        }
        
        private UnderTagSettingsProvider(string path, SettingsScope scopes, IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
            label = UnderTagsLabel;
        }

        #endregion

        #region Setup

        private static void SetUpUnderTagSettings(string searchContext, VisualElement windowRoot)
        {
            UnderTagEditorResources editorResources = UnderTagEditorResources.Instance;

            VisualTreeAsset settingsTemplate = editorResources.UnderTagSettingsUxml;
            VisualElement settingsRoot = settingsTemplate.Instantiate();
            settingsRootElement = settingsRoot;

            instance.toggleAddTagBoxButton = settingsRootElement.Q<Button>(UnderTagStyles.ToggleAddTagBoxButtonName);
            instance.toggleAddTagBoxButton.clicked += instance.OnToggleAddTagBoxButtonClicked;

            instance.addTagTextField = settingsRootElement.Q<TextField>(UnderTagStyles.AddTagTextFieldName);
            instance.addTagButton = settingsRootElement.Q<Button>(UnderTagStyles.AddTagButtonName);
            
            UnderTagDatabase tagDatabase = UnderTagDatabase.GetOrCreateUnderTagDatabase();
            instance.tagListContainer = settingsRootElement.Q<VisualElement>(UnderTagStyles.TagListContainerName);
            
            instance.helpBoxContainer = settingsRootElement.Q<VisualElement>(UnderTagStyles.HelpBoxContainerName);
            instance.databaseErrorContainer = settingsRootElement.Q<VisualElement>(UnderTagStyles.DatabaseErrorContainerName);
            
            if (tagDatabase.IsEmpty)
            {
                HelpBox databaseEmptyBox = new HelpBox("Tag database is empty! Click the plus icon above to add a tag!", HelpBoxMessageType.Info);
                instance.helpBoxContainer.hierarchy.Add(databaseEmptyBox);
                instance.ShowDatabaseEmptyHelpBox();
            }
            else
            {
                instance.HideDatabaseEmptyHelpBox();
                instance.SetUpTagList();
            }

            HelpBox errorBox = new HelpBox("Database error", HelpBoxMessageType.Error);
            instance.databaseErrorContainer.AddToClassList(UnderTagStyles.ElementHidden);
            instance.databaseErrorContainer.hierarchy.Add(errorBox);
            
            windowRoot.Add(settingsRoot);
        }
   
        private void SetUpTagList()
        {
            UnderTagDatabase tagDatabase = UnderTagDatabase.GetOrCreateUnderTagDatabase();

            List<UnderTag> rootTags = tagDatabase.RootTags;

            if (rootTags.Count == 0)
            {
                return;
            }
            
            rootTags.Sort();
            tagListContainer.hierarchy.Clear();
            
            VisualTreeAsset tagTemplate = UnderTagEditorResources.Instance.TagSettingsListItemUxml;

            foreach (UnderTag rootTag in rootTags)
            { 
                VisualElement tagElement = tagTemplate.Instantiate();
                var rootTagVisualElement = (UnderTagListItemVisualElement)tagElement.hierarchy[0];
                rootTagVisualElement.SetUp(rootTag, null, null);
                rootTagVisualElement.OnExpandStateChanged += RefreshTagBackgroundColors;
                tagListContainer.hierarchy.Add(tagElement);
                
                SetUpChildTags(rootTagVisualElement, tagDatabase);
            }
            
            RefreshTagBackgroundColors();
        }

        private void SetUpChildTags(UnderTagListItemVisualElement parentVisualElement, UnderTagDatabase tagDatabase, 
                                    bool autoShowMatching = false, UnderTag tagToMatch = default)
        {
            UnderTag parentTag = parentVisualElement.Tag;
            
            if (tagDatabase.TryGetChildTags(parentTag, out List<UnderTag> childTags))
            {
                VisualTreeAsset tagTemplate = UnderTagEditorResources.Instance.TagSettingsListItemUxml;
                var childTagContainer = parentVisualElement.Q<VisualElement>(UnderTagStyles.ChildTagContainerName);
                    
                foreach (UnderTag childTag in childTags)
                {
                    UnderTagListItemVisualElement childTagVisualElement = childTagContainer
                        .Query<UnderTagListItemVisualElement>()
                        .ToList()
                        .Find(element => element.Tag == childTag);

                    if (childTagVisualElement == null)
                    {
                        VisualElement childTagElement = tagTemplate.Instantiate();

                        childTagVisualElement = (UnderTagListItemVisualElement)childTagElement.hierarchy[0];
                        childTagVisualElement.SetUp(childTag, null, null);
                        childTagVisualElement.OnExpandStateChanged += RefreshTagBackgroundColors;

                        if (!autoShowMatching)
                        {
                            childTagVisualElement.Hide();
                        }
                    }

                    if (autoShowMatching)
                    {
                        if (childTag == tagToMatch || tagDatabase.GetParentTags(tagToMatch).Contains(childTag))
                        {
                            childTagVisualElement.Show();

                            if (tagDatabase.HasChildTags(childTag))
                            {
                                childTagVisualElement.Expand();
                            }
                        }
                    }
                    
                    childTagContainer.Add(childTagVisualElement);
                    SetUpChildTags(childTagVisualElement, tagDatabase, autoShowMatching);
                }
            }
        }

        #endregion

        #region Activate/Deactivate

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);
            
            UnderTagDatabase tagDatabase = UnderTagDatabase.GetOrCreateUnderTagDatabase();
            tagDatabase.OnTagAdded.AddListener(RefreshOnTagAddition);
            tagDatabase.OnRootTagRemoved.AddListener(RefreshOnRootTagRemoval);
            tagDatabase.OnTagRemoved.AddListener(RefreshOnTagRemoval);
        }

        public override void OnDeactivate()
        {
            base.OnDeactivate();

            isShowingDatabaseEmptyHelpBox = false;
            isShowingDatabaseErrorHelpBox = false;
            
            UnderTagDatabase tagDatabase = UnderTagDatabase.GetOrCreateUnderTagDatabase();
            tagDatabase.OnTagAdded.RemoveListener(RefreshOnTagAddition);
            tagDatabase.OnRootTagRemoved.RemoveListener(RefreshOnRootTagRemoval);
            tagDatabase.OnTagRemoved.RemoveListener(RefreshOnTagRemoval);
            AssetDatabase.SaveAssetIfDirty(tagDatabase);
        }

        #endregion

        #region UI

        public void OnAddChildTagButtonPressed(UnderTag tag)
        {
            if (!toggleAddTagBoxButton.GetClasses().Contains(UnderTagStyles.ToggleAddTagBoxButtonActiveClass))
            {
                toggleAddTagBoxButton.RemoveFromClassList(UnderTagStyles.ToggleAddTagBoxButtonInactiveClass);
                toggleAddTagBoxButton.AddToClassList(UnderTagStyles.ToggleAddTagBoxButtonActiveClass);
                
                addTagTextField.RemoveFromClassList(UnderTagStyles.AddTagTextFieldHiddenClass);
                addTagTextField.AddToClassList(UnderTagStyles.AddTagTextFieldVisibleClass);
                addTagTextField.RegisterCallback<KeyDownEvent>(OnInputReceivedFromAddTagTextField);
                
                // We wait a frame for out element to become displayed, otherwise it won't be focusable
                EditorCoroutineUtility.StartCoroutine(FocusInputFieldAfterDelay(), this);
                
                addTagButton.RemoveFromClassList(UnderTagStyles.AddTagButtonHiddenClass);
                addTagButton.AddToClassList(UnderTagStyles.AddTagButtonVisibleClass);
                addTagButton.clicked += OnAddTagButtonPressed;
            }

            addTagTextField.value = $"{tag.Value}.";
        }

        public void OnRemoveTagButtonPressed(UnderTag tag)
        {
            HideDatabaseErrorHelpBox();
            
            if (EditorUtility.DisplayDialog("Delete Tag", $"Are you sure you want to delete tag {tag.Value} and all of its children? " +
                                                          $"This will also make any reference to this tag on existing tag containers and script properties invalid!", "Confirm", "Cancel"))
            {
                UnderTagDatabase tagDatabase = UnderTagDatabase.GetOrCreateUnderTagDatabase();
                tagDatabase.RemoveTag(tag);
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

        private void OnToggleAddTagBoxButtonClicked()
        {
            bool isActive = toggleAddTagBoxButton
                .GetClasses()
                .Contains(UnderTagStyles.ToggleAddTagBoxButtonActiveClass);

            if (isActive)
            {
                HideDatabaseErrorHelpBox();
                
                toggleAddTagBoxButton.RemoveFromClassList(UnderTagStyles.ToggleAddTagBoxButtonActiveClass);
                toggleAddTagBoxButton.AddToClassList(UnderTagStyles.ToggleAddTagBoxButtonInactiveClass);

                addTagTextField.RemoveFromClassList(UnderTagStyles.AddTagTextFieldVisibleClass);
                addTagTextField.AddToClassList(UnderTagStyles.AddTagTextFieldHiddenClass);
                addTagTextField.UnregisterCallback<KeyDownEvent>(OnInputReceivedFromAddTagTextField);
                
                addTagButton.RemoveFromClassList(UnderTagStyles.AddTagButtonVisibleClass);
                addTagButton.AddToClassList(UnderTagStyles.AddTagButtonHiddenClass);
                addTagButton.clicked -= OnAddTagButtonPressed;
            }
            else
            {
                toggleAddTagBoxButton.RemoveFromClassList(UnderTagStyles.ToggleAddTagBoxButtonInactiveClass);
                toggleAddTagBoxButton.AddToClassList(UnderTagStyles.ToggleAddTagBoxButtonActiveClass);
                
                addTagTextField.RemoveFromClassList(UnderTagStyles.AddTagTextFieldHiddenClass);
                addTagTextField.AddToClassList(UnderTagStyles.AddTagTextFieldVisibleClass);
                addTagTextField.value = "Enter new tag name...";
                addTagTextField.RegisterCallback<KeyDownEvent>(OnInputReceivedFromAddTagTextField);
                
                // We wait a frame for out element to become displayed, otherwise it won't be focusable
                EditorCoroutineUtility.StartCoroutine(FocusInputFieldAfterDelay(), this);

                addTagButton.RemoveFromClassList(UnderTagStyles.AddTagButtonHiddenClass);
                addTagButton.AddToClassList(UnderTagStyles.AddTagButtonVisibleClass);
                addTagButton.clicked += OnAddTagButtonPressed;
            }
        }

        private IEnumerator FocusInputFieldAfterDelay()
        {
            yield return null;
            addTagTextField.Focus();
        }

        private void OnInputReceivedFromAddTagTextField(KeyDownEvent keyDownEvent)
        {
            if (keyDownEvent.keyCode is KeyCode.Return or KeyCode.KeypadEnter)
            {
                OnTagEntered(addTagTextField.value);
            }
        }

        private void OnAddTagButtonPressed()
        {
            OnTagEntered(addTagTextField.value);
        }
        
        private void OnTagEntered(string tag)
        {
            UnderTagDatabase uniTagDatabase = UnderTagDatabase.GetOrCreateUnderTagDatabase();

            UnderTagDatabaseResult result = uniTagDatabase.TryRegisterTag(tag);

            if (result == UnderTagDatabaseResult.Success)
            {
                HideDatabaseErrorHelpBox();
                addTagTextField.value = "Enter new tag name...";
            }
            else
            {
                //TODO: Add case "Tags MUST start with a letter"
                // Otherwise when generating constants that start with digits, we will get an error
                // Or prefix tags starting with number
                string errorMessage = result switch
                {
                    UnderTagDatabaseResult.TagStartsWithDot => "Tags can't start with a dot!",
                    UnderTagDatabaseResult.TagEndsWithDot => "Tags can't end with a dot!",
                    UnderTagDatabaseResult.TwoOrMoreDotsInARow => "Tags can't have two dots in a row!",
                    UnderTagDatabaseResult.InvalidCharacters => "Tag contains an invalid character!",
                    UnderTagDatabaseResult.TagAlreadyExists => "Tag already exists!",
                    _ => throw new ArgumentOutOfRangeException()
                };

                ShowDatabaseErrorHelpBox(errorMessage);
            }
            
            addTagTextField.Focus();
        }

        #endregion

        #region Refresh

        private void RefreshOnTagAddition(UnderTag tag)
        {
            HideDatabaseEmptyHelpBox();
            
            UnderTagDatabase tagDatabase = UnderTagDatabase.GetOrCreateUnderTagDatabase();
            List<UnderTagListItemVisualElement> listItemElements = GetUnderTagListItemElements();
            
            VisualTreeAsset tagTemplate = UnderTagEditorResources.Instance.TagSettingsListItemUxml;
            
            if (tagDatabase.IsRootTag(tag))
            {
                VisualElement tagElement = tagTemplate.Instantiate();
                var rootTagVisualElement = (UnderTagListItemVisualElement)tagElement.hierarchy[0];
                rootTagVisualElement.SetUp(tag, null, null);
                rootTagVisualElement.OnExpandStateChanged += RefreshTagBackgroundColors;
                tagListContainer.hierarchy.Insert(tagDatabase.GetRootTagIndex(tag), tagElement);
            }
            else
            {
                UnderTag rootTag = tagDatabase.GetParentTags(tag)[0];
                UnderTagListItemVisualElement rootElement = listItemElements.Find(element => element.Tag == rootTag);

                if (rootElement == null)
                {
                    VisualElement tagElement = tagTemplate.Instantiate();
                    rootElement = (UnderTagListItemVisualElement)tagElement.hierarchy[0];
                    rootElement.SetUp(rootTag, null, null);
                    rootElement.OnExpandStateChanged += RefreshTagBackgroundColors;
                    tagListContainer.hierarchy.Insert(tagDatabase.GetRootTagIndex(rootTag), tagElement);
                }
                
                SetUpChildTags(rootElement, tagDatabase, true, tag);
            }
            
            RefreshTagBackgroundColors();
        }

        private void RefreshOnRootTagRemoval(int rootTagIndex)
        {
            if (rootTagIndex != -1)
            {
                tagListContainer.hierarchy.RemoveAt(rootTagIndex);
            }

            if (UnderTagDatabase.GetOrCreateUnderTagDatabase().IsEmpty)
            {
                ShowDatabaseEmptyHelpBox();
            }
            else
            {
                RefreshTagBackgroundColors();
            }
        }
        
        private void RefreshOnTagRemoval(UnderTag tag)
        {
            List<UnderTagListItemVisualElement> underTagListItemElements = GetUnderTagListItemElements();
            UnderTagListItemVisualElement tagElement = underTagListItemElements.Find(element => element.Tag == tag);

            if (tagElement != null)
            {
                UnderTag[] tagParts = tag.GetAllTagPartsInHierarchy();
                int tagDepth = tag.GetDepth();
                int currentDepth = 0;

                UnderTag parentTag = default;
                
                while (currentDepth < tagDepth)
                {
                    parentTag.Value += $"{tagParts[currentDepth].Value}";
                    currentDepth++;

                    if (currentDepth < tagDepth)
                    {
                        parentTag.Value += ".";
                    }
                }
                
                UnderTagListItemVisualElement tagParentElement = underTagListItemElements.Find(element => string.Equals(element.Tag.Value, parentTag.Value));
                VisualElement childTagContainer = tagParentElement.Q<VisualElement>(UnderTagStyles.ChildTagContainerName);
                childTagContainer.Remove(tagElement);
                tagParentElement.Collapse();
                tagParentElement.HideExpandArrow();
            }
            
            RefreshTagBackgroundColors();
        }
        
        private List<UnderTagListItemVisualElement> GetUnderTagListItemElements()
        {
            return tagListContainer
                .Query<UnderTagListItemVisualElement>()
                .ToList();
        }

        #endregion

        #region Help Boxes

        private void ShowDatabaseEmptyHelpBox()
        {
            if (isShowingDatabaseEmptyHelpBox)
            {
                Debug.Log("ALREADY");
                return;
            }

            isShowingDatabaseEmptyHelpBox = true;
            
            settingsRootElement
                .Q<VisualElement>(UnderTagStyles.HelpBoxContainerName)
                .RemoveFromClassList(UnderTagStyles.ElementHidden);
            
            settingsRootElement
                .Q<VisualElement>(UnderTagStyles.TagListContainerScrollViewName)
                .AddToClassList(UnderTagStyles.ElementHidden);
        }

        private void HideDatabaseEmptyHelpBox()
        {
            if (!isShowingDatabaseEmptyHelpBox)
            {
                return;
            }

            isShowingDatabaseEmptyHelpBox = false;
            
            settingsRootElement
                .Q<VisualElement>(UnderTagStyles.HelpBoxContainerName)
                .AddToClassList(UnderTagStyles.ElementHidden);
            
            settingsRootElement
                .Q<VisualElement>(UnderTagStyles.TagListContainerScrollViewName)
                .RemoveFromClassList(UnderTagStyles.ElementHidden);
        }

        private void ShowDatabaseErrorHelpBox(string errorMessage)
        {
            isShowingDatabaseErrorHelpBox = true;
            databaseErrorContainer.Q<HelpBox>().text = errorMessage;
            databaseErrorContainer.RemoveFromClassList(UnderTagStyles.ElementHidden);
        }

        private void HideDatabaseErrorHelpBox()
        {
            if (!isShowingDatabaseErrorHelpBox)
            {
                return;
            }

            isShowingDatabaseErrorHelpBox = false;
            databaseErrorContainer.AddToClassList(UnderTagStyles.ElementHidden);
        }

        #endregion
    }
}