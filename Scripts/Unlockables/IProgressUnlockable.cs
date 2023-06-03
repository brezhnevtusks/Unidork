namespace Unidork.Unlockables
{
	public interface IProgressUnlockable
    {
        bool UnlockedWithRewardedAd { get; }
        void OnUnlocked();
    }
}