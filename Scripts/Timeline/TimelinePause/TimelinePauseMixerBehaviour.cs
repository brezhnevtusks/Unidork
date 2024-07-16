using System.Collections.Generic;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Unidork.Timeline
{
    /// <summary>
    /// Mixer for the <see cref="TimelinePauseTrack"/>.
    /// </summary>
    public class TimelinePauseMixerBehaviour : PlayableBehaviour
    {
        #region Fields

        /// <summary>
        /// Indices of inputs that have already been paused.
        /// </summary>
        private readonly HashSet<int> pausedInputs = new();

        /// <summary>
        /// Indices of inputs that have already been paused.
        /// </summary>
        private readonly HashSet<int> unpausedInputs = new();

        /// <summary>
        /// Last signal received by the track this mixer belongs to.
        /// </summary>
        private SignalAsset receivedSignal;
        
        #endregion

        #region Process

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            int inputCount = playable.GetInputCount();

            for (var i = 0; i < inputCount; i++)
            {
                float inputWeight = playable.GetInputWeight(i);

                if (inputWeight <= 0f)
                {
                    continue;
                }
                
                if (!pausedInputs.Contains(i))
                {
                    var timelinePlaybackPlayable = (ScriptPlayable<TimelinePausePlayable>)playable.GetInput(i);
                    TimelinePausePlayable timelinePause = timelinePlaybackPlayable.GetBehaviour();

                    playable
                        .GetGraph()
                        .GetRootPlayable(0)
                        .SetSpeed(0f);

                    pausedInputs.Add(i);

                    TimelinePauseUtility.IsPaused = true;
                    TimelinePauseUtility.OnUnpauseSignalReceived += OnUnpauseSignalReceived;
                }

                if (receivedSignal == null)
                {
                    continue;
                }
                
                if (!unpausedInputs.Contains(i))
                {
                    var timelinePlaybackPlayable = (ScriptPlayable<TimelinePausePlayable>)playable.GetInput(i);
                    TimelinePausePlayable timelinePause = timelinePlaybackPlayable.GetBehaviour();

                    if (receivedSignal == timelinePause.UnpauseSignal)
                    {
                        playable
                            .GetGraph()
                            .GetRootPlayable(0)
                            .SetSpeed(1f);

                        TimelinePauseUtility.IsPaused = false;
                        TimelinePauseUtility.OnUnpauseSignalReceived -= OnUnpauseSignalReceived;
                        unpausedInputs.Add(i);

                        receivedSignal = null;
                    }
                }
            }
        }

        private void OnUnpauseSignalReceived(SignalAsset signal)
        {
            receivedSignal = signal;
        }
            
        #endregion
    }
}