using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Unidork.Timeline
{
    /// <summary>
    /// Timeline clip that is used to pause and resume a timeline that contains it.
    /// </summary>
    [System.Serializable]
    public class TimelinePauseClip : PlayableAsset
    {
        [SerializeField] private ExposedReference<SignalAsset> unpauseSignal;
        public override double duration => 0.6f;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            ScriptPlayable<TimelinePausePlayable> timelinePausePlayable = ScriptPlayable<TimelinePausePlayable>.Create(graph);
            timelinePausePlayable.GetBehaviour().UnpauseSignal = unpauseSignal.Resolve(graph.GetResolver());
            return timelinePausePlayable;
        }
    }
}