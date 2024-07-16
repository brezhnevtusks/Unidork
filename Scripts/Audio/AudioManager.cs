using Cysharp.Threading.Tasks;
using Unidork.Attributes;
using Unidork.Constants;
using Unidork.Settings;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;

namespace Unidork.Audio
{
    // Base class for a simple AudioManager that handles a mixer with sound effect and music channels.
    public class AudioManager : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// Game's main audio mixer;
        /// </summary>
        [Space, SettingsHeader, Space] 
        [Tooltip("Game's main audio mixer")]
        [SerializeField] private AudioMixer mixer;

        #endregion

        #region Music

        /// <summary>
        /// Initializes the audio manager.
        /// </summary>
        protected virtual void Init()
        {
            if (mixer == null)
            {
                Debug.LogError("No audio mixer assigned to AudioManager!", this);
                return;
            }
            
            SettingsManager.MusicVolume.Subscribe(volume =>
            {
                SetVolume(AudioConstants.MusicVolume, volume);
            });
            
            SettingsManager.SoundEffectsVolume.Subscribe(volume =>
            {
                SetVolume(AudioConstants.SoundEffectsVolume, volume);
            });
        }
        
        private void Start()
        {
            InitAsync().Forget();
        }

        private async UniTaskVoid InitAsync()
        {
            await UniTask.WaitUntil(() => SettingsManager.IsInitialized);
            Init();
        }

        #endregion

        #region Volume

        private void SetVolume(string volumeParameterName, float normalizedVolume)
        {
            mixer.SetFloat(volumeParameterName, normalizedVolume);
        }

        #endregion
    }
}