using UnityEngine.UIElements;

namespace UnderdorkStudios.UnderTags.Editor
{
#if UNITY_2023_1_OR_NEWER
    [UxmlElement]
    public partial class UnderTagContainerVisualElement : VisualElement
#else
    public class UnderTagContainerVisualElement : VisualElement
#endif
    {
#if !UNITY_2023_1_OR_NEWER
        public new class UxmlFactory : UxmlFactory<UnderTagContainerVisualElement> {}
#endif
    }
}