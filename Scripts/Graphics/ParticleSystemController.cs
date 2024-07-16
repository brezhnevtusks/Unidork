#if UNIDORK_ADDRESSABLES

using Unidork.Attributes;
using Unidork.Extensions;
using Unidork.ObjectPooling;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Unidork.Graphics
{
    /// <summary>
    /// Controls the behavior of a particle system that plays a certain animation.
    /// </summary>
    public class ParticleSystemController : MonoBehaviour, IPooledObject
    {
        #region Properties

        /// <summary>
        /// Object spawner for particle systems.
        /// </summary>
        public static ObjectSpawner ParticleSystemSpawner { get; set; }

        /// <summary>
        /// Are the attached particle systems currently playing?
        /// </summary>
        /// <value>
        /// Gets the value of the <see cref="ParticleSystem.isPlaying"/> property 
        /// of <see cref="particleSystems"/> first member.
        /// </value>
        public bool IsPlaying => particleSystems.First().isPlaying;

        /// <summary>
        /// Is this particle system controller currently active?
        /// </summary>
        public bool IsActive => gameObject.activeInHierarchy;

        #endregion

        #region Fields       

        /// <summary>
        /// Particle systems that belongs to this controller.
        /// </summary>
        [Space, ComponentsHeader, Space]
        [Tooltip("Particle systems that belongs to this controller.")]
        [SerializeField]
        private ParticleSystem[] particleSystems = null;

        /// <summary>
        /// Behavior type when the particle systems are stopped.
        /// </summary>
        [Space, SettingsHeader, Space]
        [Tooltip("Behavior type when the particle systems are stopped.")]
        [SerializeField]
        private ParticleSystemStopBehavior particleSystemStopBehavior = ParticleSystemStopBehavior.StopEmitting;

        /// <summary>
        /// Particle system controller's default scale.
        /// </summary>
        private Vector3 defaultScale = Vector3.one;

        #endregion

        #region Init

        private void Awake()
		{
			defaultScale = transform.localScale;

			if (ParticleSystemSpawner != null)
			{
				return;
			}

			ParticleSystemSpawner = ObjectSpawner.GetSpawnerWithName("ParticleSpawner");
		}

		#endregion

		#region Play

		/// <summary>
		/// Tells the attached particle system to start playing.
		/// </summary>
		public void Play()
		{
            foreach (ParticleSystem particleSystem in particleSystems)
			{
                particleSystem.Play();
            }			
		}

        #endregion

        #region Pooling

        /// <inheritdoc/>  
        public void SetUpTransform(Transform parent, Vector3 position, Quaternion rotation = default,
                                   bool overrideScale = false, Vector3 scale = default)
        {
            SetParent(parent);
            transform.position = position;
            transform.rotation = rotation;

            if (!overrideScale)
            {
                return;
            }

            transform.localScale = scale;
        }

        /// <inheritdoc/>  
        public void SetParent(Transform parent) => transform.SetParent(parent, worldPositionStays: true);

        /// <inheritdoc/>  
        public void Activate()
        {
            transform.localScale = defaultScale;
            transform.gameObject.SetActive(true);
        }

        /// <inheritdoc/>  
        public void Deactivate(bool deactivateOnStart = false)
        {
            foreach (ParticleSystem particleSystem in particleSystems)
			{
                particleSystem.Stop(true, particleSystemStopBehavior);
            }
            
            transform.gameObject.SetActive(false);
            transform.localScale = defaultScale;
        }

        /// <inheritdoc/>  
        public void Destroy() => Addressables.ReleaseInstance(transform.parent.gameObject);		
        
        /// <inheritdoc/>        
        public void Despawn()
        {
            ParticleSystemSpawner.Despawn(this);
        }

        private void OnParticleSystemStopped()
        {
            ParticleSystemSpawner.Despawn(this);
        }

        #endregion
    }
}

#endif