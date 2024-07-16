using Sirenix.OdinInspector;
using TMPro;
using Unidork.Attributes;
using Unidork.Constants;
using UnityEngine;

namespace Unidork.UI
{
	/// <summary>
	/// Allows to set material properties on a TextMeshProUGUI component.
	/// </summary>
	/// <remarks>
	/// Since Canvas Renderers can't work with Material Property Blocks,
	/// we will have to create material instances instead.
	/// </remarks>
	public class TMProMaterialPropertySetter : MonoBehaviour
    {
		#region Fields

		[Space, ComponentsHeader, Space]
		[SerializeField]
		private TextMeshProUGUI text = null;

	    /// <summary>
	    /// Should underlay color be set?
	    /// </summary>
	    [Space, Title("UNDERLAY", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false), Space]
	    [SerializeField]
	    private bool setUnderlayColor = false;
	    
	    /// <summary>
	    /// Underlay color value to set.
	    /// </summary>
	    [ShowIf("@this.setUnderlayColor")]
	    [OnValueChanged("SetUnderlaySettings")]
	    [Tooltip("Underlay color value to set.")]
	    [ColorUsage(true, true)]
	    [SerializeField]
	    private Color underlayColor = Color.black;

	    /// <summary>
	    /// Should underlay offset x value be set?
	    /// </summary>
	    [Tooltip("Should underlay offset x value be set?")]
	    [SerializeField] 
	    private bool setUnderlayOffsetX = false;

	    /// <summary>
	    /// Underlay offset x value to set.
	    /// </summary>
	    [ShowIf("@this.setUnderlayOffsetX")]
	    [OnValueChanged("SetUnderlaySettings")]
	    [Tooltip("Underlay offset x value to set.")]
	    [Range(-1f, 1f)]
	    [SerializeField]
	    private float underlayOffsetX = 0f;
	    
	    /// <summary>
	    /// Should underlay offset y value be set?
	    /// </summary>
	    [Tooltip("Should underlay offset y value be set?")]
	    [SerializeField] 
	    private bool setUnderlayOffsetY = false;
	    
	    /// <summary>
	    /// Underlay offset y value to set.
	    /// </summary>
	    [ShowIf("@this.setUnderlayOffsetY")]
	    [OnValueChanged("SetUnderlaySettings")]
	    [Tooltip("Underlay offset y value to set.")]
	    [Range(-1f, 1f)]
	    [SerializeField]
	    private float underlayOffsetY = 0f;
	    
	    /// <summary>
	    /// Should underlay dilate value be set?
	    /// </summary>
	    [Tooltip("Should underlay dilate value be set?")]
	    [SerializeField] 
	    private bool setUnderlayDilate = false;

	    /// <summary>
	    /// Underlay dilate value to set.
	    /// </summary>
	    [ShowIf("@this.setUnderlayDilate")]
	    [OnValueChanged("SetUnderlaySettings")]
	    [Tooltip("Underlay dilate value to set.")]
	    [Range(-1f, 1f)]
	    [SerializeField]
	    private float underlayDilate = 0f;
	    
	    /// <summary>
	    /// Should underlay softness be set?
	    /// </summary>
	    [Tooltip("Should underlay softness be set?")]
	    [SerializeField] 
	    private bool setUnderlaySoftness = false;
	    
	    /// <summary>
	    /// Underlay softness value to set.
	    /// </summary>
	    [ShowIf("@this.setUnderlaySoftness")]
	    [OnValueChanged("SetUnderlaySettings")]
	    [Tooltip("Underlay softness value to set.")]
	    [Range(0f, 1f)]
	    [SerializeField]
	    private float underlaySoftness = 0f;

	    #endregion

		#region Init

		private void Start() => SetUnderlaySettings();

		#endregion

		#region Underlay

	    /// <summary>
		/// Sets the Underlay settings.
		/// </summary>		
		private void SetUnderlaySettings()
		{
			Material textMaterial = new Material(text.fontSharedMaterial);

			SetUnderlayColor(textMaterial);
			SetUnderlayOffsetX(textMaterial);
			SetUnderlayOffsetY(textMaterial);
			SetUnderlayDilate(textMaterial);
			SetUnderlaySoftness(textMaterial);
			
			text.fontMaterial = textMaterial;
		}

	    /// <summary>
	    /// Sets the underlay color on the passed text material.
	    /// </summary>
	    /// <param name="textMaterial">Text material.</param>
	    private void SetUnderlayColor(Material textMaterial)
	    {
		    if (!setUnderlayColor)
		    {
			    return;
		    }
		    
		    textMaterial.SetColor(ShaderConstants.UnderlayColor, underlayColor);	
	    }

	    /// <summary>
	    /// Sets the underlay offset x value on the passed text material.
	    /// </summary>
	    /// <param name="textMaterial">Text material.</param>
	    private void SetUnderlayOffsetX(Material textMaterial)
	    {
		    if (!setUnderlayOffsetX)
		    {
			    return;
		    }
		    
		    textMaterial.SetFloat(ShaderConstants.UnderlayOffsetX, underlayOffsetX);
	    }
	    
	    /// <summary>
	    /// Sets the underlay offset y value on the passed text material.
	    /// </summary>
	    /// <param name="textMaterial">Text material.</param>
	    private void SetUnderlayOffsetY(Material textMaterial)
	    {
		    if (!setUnderlayOffsetY)
		    {
			    return;
		    }
		    
		    textMaterial.SetFloat(ShaderConstants.UnderlayOffsetY, underlayOffsetY);
	    }

	    /// <summary>
	    /// Sets the underlay dilate value on the passed text material.
	    /// </summary>
	    /// <param name="textMaterial">Text material.</param>
	    private void SetUnderlayDilate(Material textMaterial)
	    {
		    if (!setUnderlayDilate)
		    {
			    return;
		    }
		    
		    textMaterial.SetFloat(ShaderConstants.UnderlayDilate, underlayDilate);   
	    }
	    
	    /// <summary>
	    /// Sets the underlay softness value on the passed text material.
	    /// </summary>
	    /// <param name="textMaterial">Text material.</param>
	    private void SetUnderlaySoftness(Material textMaterial)
	    {
		    if (!setUnderlaySoftness)
		    {
			    return;
		    }
		    
		    textMaterial.SetFloat(ShaderConstants.UnderlaySoftness, underlaySoftness);   
	    }

		#endregion
	}
}