using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace UnderdorkStudios.UnderTags.Editor
{
    public class UnderTagEditorResources : ScriptableObject
    {
        #region Properties

        public static UnderTagEditorResources Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = AssetDatabase.LoadAssetAtPath<UnderTagEditorResources>("Assets/Editor Default Resources/UnderTagEditorResources.asset");
                }

                return instance;
            }
        }

        public VisualTreeAsset UnderTagSettingsUxml => underTagSettingsUxml;
        public VisualTreeAsset UnderTagUxml => underTagUxml;
        public VisualTreeAsset UnderTagPropertyUxml => underTagPropertyUxml;
        public VisualTreeAsset UnderTagContainerUxml => underTagContainerUxml;
        public VisualTreeAsset TagSettingsListItemUxml => underTagSettingsListItemUxml;
        public VisualTreeAsset UnderTagPropertyListItemUxml => underTagPropertyListItemUxml;
        public VisualTreeAsset UnderTagContainerListItemUxml => underTagContainerListItemUxml;
        public VisualTreeAsset UnderTagSelectionPopupUxml => underTagSelectionPopupUxml;

        #endregion
        
        #region Fields

        private static UnderTagEditorResources instance;

        [Header("UXMLs")] 
        [SerializeField] private VisualTreeAsset underTagSettingsUxml;
        [SerializeField] private VisualTreeAsset underTagUxml;
        [SerializeField] private VisualTreeAsset underTagPropertyUxml;
        [SerializeField] private VisualTreeAsset underTagContainerUxml;
        [SerializeField] private VisualTreeAsset underTagSettingsListItemUxml;
        [SerializeField] private VisualTreeAsset underTagPropertyListItemUxml;
        [SerializeField] private VisualTreeAsset underTagContainerListItemUxml;
        [SerializeField] private VisualTreeAsset underTagSelectionPopupUxml;

        #endregion
    }
}