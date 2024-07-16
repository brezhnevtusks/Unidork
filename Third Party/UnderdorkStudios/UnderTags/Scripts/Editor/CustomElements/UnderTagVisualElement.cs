using UnityEngine.UIElements;

namespace UnderdorkStudios.UnderTags.Editor
{
#if UNITY_2023_1_OR_NEWER
    [UxmlElement]
    public partial class UnderTagVisualElement : VisualElement
#else
    public class UnderTagVisualElement : VisualElement
#endif
    {
#if !UNITY_2023_1_OR_NEWER
        public new class UxmlFactory : UxmlFactory<UnderTagVisualElement> {}
#endif
        #region Properties

        public UnderTag Tag => tag;

        #endregion
        
        #region Fields

        private Label tagLabel;
        private Button removeTagButton;
        private UnderTagContainer container;
        private UnderTag tag;

        #endregion

        #region Setup

        public void SetUp(UnderTagContainer container, UnderTag tag = default)
        {
            this.container = container;
            this.tag = tag;

            tagLabel = this.Q<Label>(UnderTagStyles.TagLabelName);
            removeTagButton = this.Q<Button>(UnderTagStyles.RemoveTagButtonName);
            
            if (tag.IsValid())
            {
                tagLabel.text = tag.Value;
                
                removeTagButton.RemoveFromClassList(UnderTagStyles.ElementHidden);
                removeTagButton.clicked += RemoveTag;
            }
            else
            {
                removeTagButton.AddToClassList(UnderTagStyles.ElementHidden);
                removeTagButton.clicked -= RemoveTag;
            }
        }

        #endregion

        #region Tag

        private void RemoveTag()
        {
            container.RemoveTag(tag);
            UnderTagContainerEditor.Current.Refresh(tag, false);
        }

        #endregion
    }
}