using Unidork.Events;
using UnityEngine;
using UnityEngine.Playables;

namespace Unidork.Timeline
{
    /// <summary>
    /// Timeline clip used to raise a <see cref="gameEvent"/>.
    /// </summary>
    public class GameEventClip : PlayableAsset
    {
        [SerializeField] private ExposedReference<GameEvent> gameEvent;
        public override double duration => 0.5f;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<GameEventPlayable> gameEventPlayable = ScriptPlayable<GameEventPlayable>.Create(graph);
            gameEventPlayable.GetBehaviour().GameEvent = gameEvent.Resolve(graph.GetResolver());
            return gameEventPlayable;
        }
    }
}