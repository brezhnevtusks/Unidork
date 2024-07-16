using Unidork.Events;
using UnityEngine.Playables;

namespace Unidork.Timeline
{
    /// <summary>
    /// Playable behaviour used to raise a <see cref="GameEvent"/>.
    /// </summary>
    [System.Serializable]
    public class GameEventPlayable : PlayableBehaviour
    {
        public GameEvent GameEvent;
    }
}