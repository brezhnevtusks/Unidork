using JetBrains.Annotations;
using MoreMountains.Feedbacks;
using Unidork.Events;
using UnityEngine;

namespace Unidork.Feedbacks
{
    /// <summary>
    /// Feedback that raises a <see cref="GameEvent"/>.
    /// </summary>
    [System.Serializable]
    [UsedImplicitly]
	[FeedbackPath("Game Events/Game Event")]
	[FeedbackHelp("This feedback lets you raise a GameEvent.")]
	public class GameEventFeedback : MMF_Feedback
    {
        #region Properties      

#if UNITY_EDITOR
        /// <inheritdoc/>
        public override Color FeedbackColor => FeedbacksEditorColors.GameEventFeedbackColor;
        public override bool EvaluateRequiresSetup() => eventToRaise == null;
        public override string RequiresSetupText => "This feedback requires that you specify a Game Event below!";
#endif

		#endregion

		#region Fields

		/// <summary>
		/// Event to raise when the feedback is triggered.
		/// </summary>
		[MMFInspectorGroup("Raise Game Event", true)] 
		[SerializeField]
		private GameEvent eventToRaise;

		#endregion

		#region Play

		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1)
		{
			if (!Active || eventToRaise == null)
			{
				return;
			}

			eventToRaise.Raise();
		}

	    #endregion
    }
}