using System.Collections.Generic;

namespace Unidork.Events
{
	/// <summary>
	/// Class that stores a list of <see cref="GameEventReference"/> objects to view in <see cref="GameEventReferenceWindow"/>.
	/// </summary>
	[System.Serializable]
	public class GameEventReferences
	{
		public List<GameEventReference> References = new List<GameEventReference>();
	}
}