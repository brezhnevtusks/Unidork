using JetBrains.Annotations;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace Unidork.Feedbacks
{
    /// <summary>
    /// Turns an object active or inactive at the various stages of the feedback
    /// </summary>
    [System.Serializable]
    [UsedImplicitly]
    [FeedbackHelp("This feedback allows you to set local properties on , or enable/disable keywords.")]
    [FeedbackPath("Renderer/Set Local Shader Keyword")]
    public class SetLocalShaderKeywordFeedback : MMF_Feedback
    {
        #region Properties

#if UNITY_EDITOR
        public override Color FeedbackColor => MMFeedbacksInspectorColors.RendererColor;
        public override bool EvaluateRequiresSetup() => targetRenderer == null || string.IsNullOrEmpty(keyword);
        public override string RequiresSetupText => "This feedback requires that you specify a target renderer and a keyword below!";
#endif

        #endregion
        
        #region Fields

        /// <summary>
        /// Should shared material be used?
        /// </summary>
        [MMFInspectorGroup("Set Local Shader Keyword", true)]
        [Tooltip("Use shared material instead of instance?")]
        [SerializeField] 
        private bool useSharedMaterial;
        
        /// <summary>
        /// Renderer that has the material we want to change.
        /// </summary>
        [Tooltip("Renderer that has the material we want to change")]
        [SerializeField] 
        private Renderer targetRenderer;

        /// <summary>
        /// Keyword to enable/disable.
        /// </summary>
        [Tooltip("Keyword to enable/disable.")] 
        [SerializeField] 
        private string keyword = "";
        
        /// <summary>
        /// Should the keyword be enabled or disabled?
        /// </summary>
        [Tooltip("Should the keyword be enabled or disabled?")]
        [SerializeField] 
        private bool enableKeyword = true;
        
        #endregion

        #region Play

        protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1)
        {
            if (!Active)
            {
                return;
            }

            if (targetRenderer == null)
            {
                Debug.LogError("{Set Local Shader Keyword feedbacks doesn't have a renderer assigned!}", Owner);
                return;
            }

            Material targetMaterial = useSharedMaterial ? targetRenderer.sharedMaterial : targetRenderer.material;
            
            if (enableKeyword)
            {
                targetMaterial.EnableKeyword(keyword);
            }
            else
            {
                targetMaterial.DisableKeyword(keyword);
            }
        }

        #endregion
    }
}