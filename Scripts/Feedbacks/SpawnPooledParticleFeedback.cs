using MoreMountains.Feedbacks;
using Sirenix.OdinInspector;
using Unidork.Attributes;
using Unidork.Extensions;
using Unidork.Graphics;
using Unidork.ObjectPooling;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Unidork.Feedbacks
{
	/// <summary>
	/// Feedbacks that spawns a pooled particle system.
	/// </summary>
	[FeedbackPath("Particles/Spawn Pooled Particle")]
	[FeedbackHelp("This feedback lets you spawn a pooled particle system controller.")]
	public class SpawnPooledParticleFeedback : MMF_Feedback
    {
		#region Enums
		
		/// <summary>
		/// Type of pooled particle spawn.
		/// <para>Manual - specific particle is spawned.</para>
		/// <para>Random - random particle from an array is spawned.</para>
		/// </summary>
		private enum PooledParticleSpawnType
		{
			/// <summary>
			/// Specific particle is spawned.
			/// </summary>
			Manual,

			/// <summary>
			/// Random particle from an array is spawned.
			/// </summary>
			Random
		}

		/// <summary>
		/// Defines how the particles are positioned when spawned.
		/// <para>FeedbackPosition - particles match feedback's position.</para>
		/// <para>Transform - particle match selected transform's position.</para>
		/// <para>WorldPosition - particles are positioned at specific world coordinates.</para>
		/// </summary>
		private enum PositionSelectionType 
		{
			/// <summary>
			/// Particles match feedback's position.
			/// </summary>
			FeedbackPosition,

			/// <summary>
			/// Particle match selected transform's position.
			/// </summary>
			Transform,

			/// <summary>
			/// Particles are positioned at specific world coordinates.
			/// </summary>
			WorldPosition
		}

		/// <summary>
		/// Defines how the particles are rotated when spawned.
		/// <para>FeedbackRotation - particles match feedback's rotation.</para>
		/// <para>Transform - particle's match selected transform's rotation.</para>
		/// <para>LocalRotation - particles have explicit local rotation.</para>
		/// <para>GlobalRotation - particles have explicit global rotation.</para>
		/// </summary>
		private enum RotationSelectionType
		{
			/// <summary>
			/// Particles match feedback's rotation.
			/// </summary>
			FeedbackRotation,

			/// <summary>
			/// Particle's match selected transform's rotation.
			/// </summary>
			Transform,

			/// <summary>
			/// Particles have explicit local rotation.
			/// </summary>
			LocalRotation,

			/// <summary>
			/// Particles have explicit global rotation.
			/// </summary>
			GlobalRotation
		}

		#endregion

		#region Properties

#if UNITY_EDITOR

		/// <inheritdoc/>
		public override Color FeedbackColor => FeedbacksEditorColors.PooledParticleSpawnColor;

#endif

		#endregion

		#region Fields

		/// <summary>
		/// Name of the particle system spawner that spawns particles accociated with this feedback.
		/// </summary>
		[Space, GeneralHeader, Space]
		[Tooltip("Name of the particle system spawner that spawns particles accociated with this feedback.")]
		[SerializeField]
		private string particleSpawnerName;

		/// <summary>
		/// Transform to use as a parent override for the spawned particle system.
		/// </summary>
		[Tooltip("Transform to use as a parent override for the spawned particle system.")]
		[SerializeField]
		private Transform particleParent = null;

		/// <summary>
		/// Pooled particle spawn type.
		/// </summary>
		[Space, AssetsHeader, Space]
		[Tooltip("Pooled particle spawn type.")]
		[SerializeField]
		private PooledParticleSpawnType spawnType = PooledParticleSpawnType.Manual;

		/// <summary>
		/// Addressable asset reference for the particle system used when spawnType is set to Manual.
		/// </summary>
		[ShowIf("@this.spawnType", PooledParticleSpawnType.Manual)]
		[Tooltip("Addressable asset reference for the particle system used when Spawn Type is set to Manual.")]
		[SerializeField]
		private AssetReference particleAssetReference = null;

		/// <summary>
		/// Addressable asset references for the particle systems used when spawnType is set to Random.
		/// </summary>
		[ShowIf("@this.spawnType", PooledParticleSpawnType.Random)]
		[Tooltip("Addressable asset references for the particle systems used when Spawn Type is set to Random.")]
		[SerializeField]
		private AssetReference[] particleAssetReferences = null;

		/// <summary>
		/// Spawned particle position selection mode.
		/// </summary>
		[Space, Title("POSITION", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false), Space]
		[Tooltip("Spawned particle position selection mode.")]
		[SerializeField]
		private PositionSelectionType positionSelectionType = PositionSelectionType.FeedbackPosition;

		/// <summary>
		/// Transform to use as position origin for the spawned particles.
		/// </summary>
		[ShowIf("@this.positionSelectionType", PositionSelectionType.Transform)]
		[Tooltip("Transform to use as position origin for the spawned particles.")]
		[SerializeField]
		private Transform spawnedParticlePositionOrigin = null;

		/// <summary>
		/// World position to use for spawned particles.
		/// </summary>
		[ShowIf("@this.positionSelectionType", PositionSelectionType.WorldPosition)]
		[Tooltip("World position to use for spawned particles.")]
		[SerializeField]
		private Vector3 spawnWorldPosition = Vector3.zero;

		/// <summary>
		/// Offset to apply to spawned particles' position.
		/// </summary>
		[Tooltip("Offset to apply to spawned particles' position.")]
		[SerializeField]
		public Vector3 positionOffset = Vector3.zero;

		/// <summary>
		/// Spawned particle rotation selection mode.
		/// </summary>
		[Space, Title("ROTATION", TitleAlignment = TitleAlignments.Centered, HorizontalLine = false), Space]
		[Tooltip("Spawned particle rotation selection mode.")]
		[SerializeField]
		private RotationSelectionType rotationSelectionType = RotationSelectionType.GlobalRotation;

		/// <summary>
		/// Transform to use as rotation origin for the spawned particles.
		/// </summary>
		[ShowIf("@this.rotationSelectionType", RotationSelectionType.Transform)]
		[Tooltip("Transform to use as rotation origin for the spawned particles.")]
		[SerializeField]
		private Transform spawnedParticleRotationOrigin = null;

		/// <summary>
		/// Local rotation to apply to spawned particles.
		/// </summary>
		[ShowIf("@this.rotationSelectionType", RotationSelectionType.LocalRotation)]
		[Tooltip("Local rotation to apply to spawned particles.")]
		[SerializeField]
		private Vector3 spawnLocalRotation = Vector3.zero;

		/// <summary>
		/// Global rotation to apply to spawned particles.
		/// </summary>		
		[ShowIf("@this.rotationSelectionType", RotationSelectionType.GlobalRotation)]
		[Tooltip("Global rotation to apply to spawned particles.")]
		[SerializeField]
		private Vector3 spawnGlobalRotation = Vector3.zero;

		/// <summary>
		/// Component that spawns/despawns particle systems.
		/// </summary>
		private ObjectSpawner particleSpawner;

		#endregion

		#region Init

		private void Awake() => particleSpawner = ObjectSpawner.GetSpawnerWithName(particleSpawnerName);

		#endregion

		#region Play

		/// <inheritdoc/>		
		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1)
		{
			AssetReference particleAssetReference = this.particleAssetReference;

			if (spawnType == PooledParticleSpawnType.Random)
			{
				particleAssetReference = particleAssetReferences.GetRandomOrDefault();
			}

			Vector3 spawnPosition = GetSpawnPosition();
			Quaternion spawnRotation = GetSpawnRotation();

			var particleSystemController = (ParticleSystemController)particleSpawner.Spawn(particleAssetReference,
				spawnPosition, spawnRotation, parent: particleParent);

			particleSystemController.Play();
		}

		/// <summary>
		/// Gets the spawn position for the particles.
		/// </summary>
		/// <returns>
		/// A <see cref="Vector3"/>.
		/// </returns>
		private Vector3 GetSpawnPosition()
		{
			var spawnPosition = Vector3.zero;

			switch (positionSelectionType)
			{
				case PositionSelectionType.FeedbackPosition:
					spawnPosition = Owner.transform.position;
					break;
				case PositionSelectionType.Transform:
					spawnPosition = spawnedParticlePositionOrigin.position;
					break;
				case PositionSelectionType.WorldPosition:
					spawnPosition = spawnWorldPosition;
					break;
			}

			spawnPosition += positionOffset;

			return spawnPosition;
		}

		/// <summary>
		/// Gets the spawn rotation for the particles.
		/// </summary>
		/// <returns>
		/// A <see cref="Quaternion"/>.
		/// </returns>
		private Quaternion GetSpawnRotation()
		{
			Quaternion spawnRotation = Quaternion.identity;

			switch (rotationSelectionType)
			{
				case RotationSelectionType.FeedbackRotation:
					spawnRotation = Owner.transform.rotation;
					break;
				case RotationSelectionType.Transform:
					spawnRotation = spawnedParticleRotationOrigin.rotation;
					break;
				case RotationSelectionType.LocalRotation:
					spawnRotation = Quaternion.Euler(spawnLocalRotation);
					break;
				case RotationSelectionType.GlobalRotation:
					spawnRotation = Quaternion.Euler(spawnGlobalRotation);
					break;
			}

			return spawnRotation;
		}

		#endregion		
	}
}