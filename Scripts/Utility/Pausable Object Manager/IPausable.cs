namespace Unidork.Utility
{
	/// <summary>
	/// Interface that can be implemented in components so that they can be handled by the <see cref="PausableObjectManager"/>
	/// </summary>
	public interface IPausable
	{
		/// <summary>
		/// Is the component currently paused?
		/// </summary>
		bool IsPaused { get; }
		
		/// <summary>
		/// Pauses the component.
		/// </summary>
		void Pause();
		
		/// <summary>
		/// Unpauses the component.
		/// </summary>
		void Unpause();
	}    
}