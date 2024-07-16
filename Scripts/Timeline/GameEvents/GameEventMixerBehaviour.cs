using System.Collections.Generic;
using UnityEngine.Playables;

namespace Unidork.Timeline
{
    /// <summary>
    /// Mixer for the <see cref="GameEventTrack"/>.
    /// </summary>
    public class GameEventMixerBehaviour : PlayableBehaviour
    {
        /// <summary>
        /// Indices of inputs that have already been played.
        /// </summary>
        private readonly HashSet<int> playedInputs = new();
        
        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            int inputCount = playable.GetInputCount();

            for (var i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);

                if (inputWeight <= 0f || !playedInputs.Add(i))
                {
                    continue;
                }
                
                var timelinePlaybackPlayable = (ScriptPlayable<GameEventPlayable>)playable.GetInput(i);
                GameEventPlayable gameEvent = timelinePlaybackPlayable.GetBehaviour();
                
                gameEvent.GameEvent.Raise();
            } 
        }
    }
}