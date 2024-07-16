using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Unidork.Timeline
{
    /// <summary>
    /// Timeline track that can be used to add clips that pauses and resumes a timeline.
    /// </summary>
    [TrackColor(0.8f, 0.1f, 0.1f)]
    [TrackClipType(typeof(TimelinePauseClip))]
    public class TimelinePauseTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<TimelinePauseMixerBehaviour>.Create(graph, inputCount);
        }
    }
}