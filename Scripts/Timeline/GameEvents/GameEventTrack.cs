using Unidork.Events;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Unidork.Timeline
{
    /// <summary>
    /// Timeline track that can be used to raise a <see cref="GameEvent"/>.
    /// </summary>
    [TrackColor(0.76f, 0.45f, 0.1f)]
    [TrackClipType(typeof(GameEventClip))]
    public class GameEventTrack : PlayableTrack
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<GameEventMixerBehaviour>.Create(graph, inputCount);
        }
    }
}