using System.Collections.Generic;

namespace Unidork.Events
{
	/// <summary>
	/// Class that stores a list of <see cref="GameEventListenerReference"/> objects to view in <see cref="GameEventListenerWindow"/>.
	/// </summary>
	[System.Serializable]
	public class GameEventListenerReferences
	{
		public List<GameEventListenerReference> ListenerReferences = new List<GameEventListenerReference>();
	}
}