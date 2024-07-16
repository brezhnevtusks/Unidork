using JetBrains.Annotations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnderdorkStudios.UnderTools.Extensions;
using UnityEngine;

namespace Unidork.Utility
{
	/// <summary>
	/// Makes screenshots with specified resolution and encodes it to the specified format.
	/// </summary>
	public class ScreenshotMaker : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// Screenshot resolution.
        /// </summary>
        [Space, Title("SCREENSHOT", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false), Space]
        [Tooltip("Screenshot resolution.")]
        [SerializeField]
        private Vector2Int screenshotResolution = new Vector2Int(1440, 2960);

        /// <summary>
        /// Format of the screenshot output file.
        /// </summary>
        [Tooltip("Format of the screenshot output file.")]
        [SerializeField]
        private ScreenshotFormat outputFormat = ScreenshotFormat.PNG;

        /// <summary>
        /// Game's main camera.
        /// </summary>
        [Space, Title("CAMERA", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false), Space]
        [Tooltip("Game's main camera.")]
        [SerializeField]
        private Camera mainCamera;

        /// <summary>
        /// Should UI elements be included in the screenshot?
        /// </summary>
        [Tooltip("Should UI elements be included in the screenshot?")]
        [SerializeField]
        private bool includeUIElements = false;

        /// <summary>
        /// Camera that renders UI.
        /// </summary>
        [ShowIf("@this.includeUIElements == true")]
        [Tooltip("Camera that renders UI.")]
        [SerializeField]
        private Camera uiCamera;

        /// <summary>
        /// Code of the modifier key, if any, to use for making screenshots.
        /// </summary>
        [Space, Title("INPUT", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false), Space]
        [Tooltip("Code of the modifier key, if any, to use for making screenshots.")]
        [ValueDropdown("GetModifierKeyCodes")]
        [SerializeField]
        private KeyCode takeScreenshotModifierKeyCode = KeyCode.None;

        /// <summary>
        /// Code of the key, if any, to use for grabbing screenshots.
        /// </summary>
        [Tooltip("Code of the key, if any, to use for grabbing screenshots.")]
        [SerializeField]
        private KeyCode takeScreenshotKeyCode = KeyCode.None;

        /// <summary>
        /// Code of the modifier key, if any, to toggle UI elements when making screenshots.
        /// </summary>
        [Space, Space]
        [Tooltip("Code of the modifier key, if any, to toggle UI elements when making screenshots.")]
        [ValueDropdown("GetModifierKeyCodes")]
        [SerializeField]
        private KeyCode toggleUiElementsModifierKeyCode = KeyCode.None;

        /// <summary>
        /// Code of the key, if any, to toggle UI elements when making screenshots.
        /// </summary>
        [Tooltip("Code of the key, if any, to toggle UI elements when making screenshots.")]
        [SerializeField]
        private KeyCode toggleUiElementsKeyCode = KeyCode.None;

        /// <summary>
        /// Render texture to render the screenshot from the main camera to.
        /// </summary>
        private RenderTexture mainCameraScreenshotRT;

        /// <summary>
        /// Render texture to render the screenshot from the UI camera to.
        /// </summary>
        private RenderTexture uiCameraScreenshotRT;

        /// <summary>
        /// Texture to store screenshots.
        /// </summary>
        private Texture2D screenshotTexture;       

        /// <summary>
        /// Rect to use for capturing screenshots.
        /// </summary>
        private Rect screenshotRect;

        /// <summary>
        /// Canvases that have their mode set to Screen Space - Overlay.
        /// </summary>
        /// <remarks>
        /// Unity's camera will not renderer screen space overlay canvases into a render texture (due to lack of depth buffer?),
        /// so we will temporarily force them to be Screen Space - Camera canvases, using our UI camera.
        /// </remarks>
        private List<Canvas> screenSpaceOverlayCanvases;

        #endregion

        #region Constants

        /// <summary>
        /// Depth of the screenshot render texture.
        /// </summary>
        private const int ScreenshotRTDepth = 32;

        /// <summary>
        /// Format of the screenshot texture.
        /// </summary>
        private const TextureFormat ScreenshotTextureFormat = TextureFormat.ARGB32;

        #endregion

        #region Init

        private void Awake()
        {
            if (mainCamera == null)
			{
                mainCamera = Camera.main;
            }   
        }

		private void Start()
		{
			_ = System.IO.Directory.CreateDirectory(Application.persistentDataPath + "/Screenshots");

            GetScreenSpaceOverlayCanvases();
		}

        /// <summary>
        /// Gets all root canvases with render mode set to Screen Space - Overlay.
        /// </summary>
        private void GetScreenSpaceOverlayCanvases()
		{
            Canvas[] canvasArray = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
			screenSpaceOverlayCanvases = canvasArray.ToList(canvasArray.Length);

            for (int i = screenSpaceOverlayCanvases.Count - 1; i >= 0; i--)
			{
                Canvas canvas = screenSpaceOverlayCanvases[i];

                if (!canvas.isRootCanvas || canvas.renderMode != RenderMode.ScreenSpaceOverlay)
				{
                    screenSpaceOverlayCanvases.RemoveAt(i);
                    continue;
				}                
			}
		}

        #endregion

        #region Misc

        /// <summary>
        /// Gets the list of key codes used by Odin inspector's custom dropdown.
        /// </summary>
        [UsedImplicitly]
        private static IEnumerable GetModifierKeyCodes = new ValueDropdownList<KeyCode>()
        {
            KeyCode.None,
            KeyCode.LeftShift,
            KeyCode.LeftAlt,
            KeyCode.LeftControl,
            KeyCode.RightShift,
            KeyCode.RightAlt,
            KeyCode.RightControl
        };

		#endregion

		#region Screenshot

		private void Update()
		{
            if (toggleUiElementsKeyCode != KeyCode.None)
			{
                if (toggleUiElementsModifierKeyCode == KeyCode.None || Input.GetKey(toggleUiElementsModifierKeyCode))
				{
                    if (Input.GetKeyDown(toggleUiElementsKeyCode))
					{
                        includeUIElements = !includeUIElements;
					}
				}
			}

			if (takeScreenshotKeyCode == KeyCode.None)
			{
                return;
			}

            if (takeScreenshotModifierKeyCode != KeyCode.None && !Input.GetKey(takeScreenshotModifierKeyCode))
			{
                return;
			}

            if (Input.GetKeyDown(takeScreenshotKeyCode))
			{
                MakeScreenshot();
			}
		}

		/// <summary>
		/// Makes a screenshot and writes it to disk.
		/// </summary>
		public void MakeScreenshot()
		{
            ValidateTextures();
            ValidateCanvases();

			mainCamera.targetTexture = mainCameraScreenshotRT;
			mainCamera.Render();

			if (includeUIElements && uiCamera != null)
			{
                // NOTE: When using UI camera, make sure that game view aspect is set to the same resolution
                // you're making the screenshots in, otherwise the UI in the screenshot will be scaled improperly.
                // This is most probably due to use changing the render mode on overlay canvases and needs further investigation.

				foreach (Canvas canvas in screenSpaceOverlayCanvases)
				{
					canvas.renderMode = RenderMode.ScreenSpaceCamera;
					canvas.worldCamera = uiCamera;
				}

				uiCamera.targetTexture = uiCameraScreenshotRT;
				uiCamera.Render();

				Shader shader = Shader.Find("Unidork/BlitAlphaBlended");
				Material blitMaterial = new Material(shader);

				Graphics.Blit(uiCameraScreenshotRT, mainCameraScreenshotRT, blitMaterial);

				Destroy(blitMaterial);

				uiCamera.targetTexture = null;

				foreach (Canvas canvas in screenSpaceOverlayCanvases)
				{
					canvas.renderMode = RenderMode.ScreenSpaceOverlay;
					canvas.worldCamera = null;
				}
			}

            RenderTexture.active = mainCameraScreenshotRT;
            screenshotTexture.ReadPixels(screenshotRect, 0, 0);			

			string screenshotFileName = GetScreenshotFilePath();

			byte[] screenshotBytes = GetFormattedScreeshotBytes(outputFormat);

			WriteScreenshotToDisk(screenshotFileName, screenshotBytes);

			mainCamera.targetTexture = null;
			RenderTexture.active = null;
		}		

		/// <summary>
		/// Checks whether the necessary textures exist and are of correct size and creates them if needed.
		/// </summary>
		private void ValidateTextures()
		{
            if (mainCameraScreenshotRT == null || mainCameraScreenshotRT.width != screenshotResolution.x || mainCameraScreenshotRT.height != screenshotResolution.y)
            {
                mainCameraScreenshotRT = new RenderTexture(screenshotResolution.x, screenshotResolution.y, ScreenshotRTDepth,
                                                           RenderTextureFormat.ARGB32);
            }
            else
			{
                ClearRenderTexture(mainCameraScreenshotRT);
            }

            if (screenshotTexture == null || screenshotTexture.width != screenshotResolution.x ||
                screenshotTexture.height != screenshotResolution.y)
			{
                screenshotTexture = new Texture2D(screenshotResolution.x, screenshotResolution.y, ScreenshotTextureFormat, false);
            }

            if (screenshotRect == null || screenshotRect.width != screenshotResolution.x || screenshotRect.height != screenshotResolution.y)
			{
                screenshotRect = new Rect(0, 0, screenshotResolution.x, screenshotResolution.y);
			}            

            if (!includeUIElements)
			{
                return;
			}

            if (uiCamera == null)
			{
                Debug.LogError($"Trying to take a screenshot with UI elements included but no UI camera is assigned! " +
                               $"Make sure you have the camera asssigned in the UI Camera field!");
                return;
			}

            if (uiCameraScreenshotRT == null || uiCameraScreenshotRT.width != screenshotResolution.x ||
                uiCameraScreenshotRT.height != screenshotResolution.y)
			{
                uiCameraScreenshotRT = new RenderTexture(screenshotResolution.x, screenshotResolution.y, ScreenshotRTDepth,
                                                         RenderTextureFormat.ARGB32);
            }
            else
			{
                ClearRenderTexture(uiCameraScreenshotRT);
            }            
        }

        /// <summary>
        /// Validates the screen space overlay canvas list.
        /// </summary>
        /// <remarks>
        /// We do this because some canvases (like splash canvases) might get destroyed.
        /// </remarks>
        private void ValidateCanvases()
        {
            for (int i = screenSpaceOverlayCanvases.Count - 1; i >= 0; i--)
			{
                if (screenSpaceOverlayCanvases[i] == null)
				{
                    screenSpaceOverlayCanvases.RemoveAt(i);
				}
			}
        }

        /// <summary>
        /// Clears a render textures to black.
        /// </summary>
        /// <param name="renderTexture">Render texture.</param>
        private void ClearRenderTexture(RenderTexture renderTexture)
		{
            RenderTexture activeRT = RenderTexture.active;

            RenderTexture.active = renderTexture;
            GL.Clear(true, true, Color.clear);

            RenderTexture.active = activeRT;
		}

        /// <summary>
        /// Creates a path for the screen shot using its resolution and current date/time.
        /// </summary>
        /// <returns>
        /// A string.
        /// </returns>
        private string GetScreenshotFilePath()
		{
            return string.Format("{0}/Screenshots/screen_{1}x{2}_{3}.png",
                              Application.persistentDataPath,
                              screenshotResolution.x, screenshotResolution.y,
                              System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
        }

        /// <summary>
        /// Gets the array of bytes for the screenshot texture based on the desired screenshot format.
        /// </summary>
        /// <param name="format">Screenshot format.</param>
        /// <returns>
        /// An array of bytes.
        /// </returns>
        private byte[] GetFormattedScreeshotBytes(ScreenshotFormat format)
		{
			switch (format)
			{
				case ScreenshotFormat.PNG:
                    return screenshotTexture.EncodeToPNG();					
				case ScreenshotFormat.JPG:
                    return screenshotTexture.EncodeToJPG();					
                case ScreenshotFormat.RAW:
                    return screenshotTexture.GetRawTextureData();                    
				default:
                    throw new System.ComponentModel.InvalidEnumArgumentException($"Trying to create a screenshot of unhandled format: {format}!");
			}
		}

        /// <summary>
        /// Writes the screenshot to disk on a separate thread.
        /// </summary>
        /// <param name="screenshotFilePath">Path for the screenshot file.</param>
        /// <param name="screenshotBytes">Bytes of the screenshot formatted to the desire format.</param>
        private void WriteScreenshotToDisk(string screenshotFilePath, byte[] screenshotBytes)
		{
            var thread = new System.Threading.Thread(() =>
            {
                System.IO.FileStream fileStream = System.IO.File.Create(screenshotFilePath);
                fileStream.Write(screenshotBytes, 0, screenshotBytes.Length);
                fileStream.Close();
                Debug.Log($"Create a screenshot at {screenshotFilePath}");
            });

            thread.Start();        
        }

        #endregion

        #region Destroy

        private void OnDestroy()
        {
            if (mainCameraScreenshotRT != null)
			{
                mainCameraScreenshotRT.Release();
			}

            if (uiCameraScreenshotRT != null)
			{
                uiCameraScreenshotRT.Release();
			}
        }

        #endregion
    }
}