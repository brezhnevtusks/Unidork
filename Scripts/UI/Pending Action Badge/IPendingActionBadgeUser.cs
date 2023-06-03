using UniRx;

namespace Unidork.UI
{
	/// <summary>
	/// Interface for objects that use a <see cref="PendingActionBadge"/> to inform
	/// player of actions they need to take/rewards they can receive/etc.
	/// </summary>
	public interface IPendingActionBadgeUser
	{
		ReactiveProperty<bool> HasPendingActions { get; }
	}
}