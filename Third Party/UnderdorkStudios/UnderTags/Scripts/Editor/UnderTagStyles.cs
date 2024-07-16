using UnityEngine;

namespace UnderdorkStudios.UnderTags.Editor
{
    public static class UnderTagStyles
    {
        public const string ElementHidden = "element-hidden";
        
        public const string ToggleAddTagBoxButtonName = "toggle-add-tag-box-button";
        public const string ToggleAddTagBoxButtonActiveClass = "toggle-add-tag-box-button-active";
        public const string ToggleAddTagBoxButtonInactiveClass = "toggle-add-tag-box-button-inactive";
        
        public const string AddTagButtonName = "add-tag-button";
        public const string AddTagButtonHiddenClass = "add-tag-button-hidden";
        public const string AddTagButtonVisibleClass = "add-tag-button-visible";

        public const string AddTagTextFieldName = "add-tag-text-field";
        public const string AddTagTextFieldVisibleClass = "add-tag-text-field-visible";
        public const string AddTagTextFieldHiddenClass = "add-tag-text-field-hidden";

        public const string TagListContainerName = "tag-list-container";
        public const string TagListContainerScrollViewName = "tag-list-container-scroll-view";
        public const string OwnedTagRootContainerName = "owned-tag-root-container";
        public const string OwnedTagContainerName = "owned-tag-container";
        public const string HelpBoxContainerName = "help-box-container";
        public const string DatabaseErrorContainerName = "database-error-container";

        public const string TagContainerName = "tag-container";
        public const string ExpandTagContainerName = "expand-tag-container";
        public const string ExpandTagArrowName = "expand-tag-arrow";
        public const string ExpandTagArrowRotatedClass = "expand-tag-arrow-rotated";
        public const string ExpandTagArrowHiddenClass = "expand-tag-arrow-hidden";
        public const string TagLabelName = "tag-label";
        public const string TagExpandedClass = "tag-expanded";
        public const string TagHiddenClass = "tag-hidden";
        public const string MainTagContainerName = "main-tag-container";
        public const string ChildTagContainerName = "child-tag-container";
        public const string AddChildTagButtonName = "add-child-tag-button";

        public const string ToggleAddTagContainerButtonName = "toggle-add-tag-container-button";
        public const string SelectedTagsContainerName = "selected-tags-container";
        public const string FilterTagsTextFieldName = "filter-text-field";
        public const string TagToggleName = "tag-toggle";
        public const string RemoveTagButtonName = "remove-tag-button";
        public const string NoTagsAddedContainerName = "no-tags-added-container";

        public const string TagPropertyNameName = "tag-property-name";
        public const string TagValueName = "tag-value";
        public const string TagBackgroundName = "tag-background";
        public const string ClearTagButtonName = "clear-tag-button";

        public const string AlternativeTagBackgroundClass = "tag-alternative-background";

        public const float DefaultChildTagOffset = 5f;
        public const float ChildTagOffset = 18f;
        public const float ChildTagOffsetTagContainer = 21f;

        public static readonly Vector2 TagSelectionPopupSize = new(600f, 300f);
        public static readonly Vector2 DesiredTagSelectionPopupOffset = new(0f, -330f);
    }
}