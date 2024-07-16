using UniRx;

namespace Unidork.Utility
{
	/// <summary>
	/// Interface that can be implemented in components so that they can be handled by the <see cref="PauseManager"/>
	/// </summary>
	public interface IPausable
	{
		/// <summary>
		/// Is the component currently paused?
		/// </summary>
		ReactiveProperty<bool> IsPaused { get; }

		/// <summary>
		/// Pauses the component.
		/// </summary>
		void Pause() => IsPaused.Value = true;

		/// <summary>
		/// Unpauses the component.
		/// </summary>
		void Unpause() => IsPaused.Value = false;
	}    
}