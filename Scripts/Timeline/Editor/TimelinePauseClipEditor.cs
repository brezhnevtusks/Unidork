using UnityEditor.Timeline;
using UnityEngine.Timeline;

namespace Unidork.Timeline.Editor
{
    [CustomTimelineEditor(typeof(TimelinePauseClip))]
    public class TimelinePauseClipEditor : ClipEditor
    {
        public override void OnClipChanged(TimelineClip clip)
        {
            clip.displayName = "Pause";
        }
    }
}