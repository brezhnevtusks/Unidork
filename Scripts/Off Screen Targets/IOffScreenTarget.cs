using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Unidork.OffScreenTargets
{
	public interface IOffScreenTarget
	{
		bool OffScreenTargetIsActive { get; }
		Vector3 GetPosition();
		UniTask<AssetReference> GetIconAssetReference(CancellationToken cancellationToken);
	}    
}