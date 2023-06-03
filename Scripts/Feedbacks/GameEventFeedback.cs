using MoreMountains.Feedbacks;
using Unidork.Events;
using UnityEngine;

namespace Unidork.Feedbacks
{
    /// <summary>
    /// Feedback that raises a <see cref="GameEvent"/>.
    /// </summary>
    [System.Serializable]
	[FeedbackPath("Game Events/Game Event")]
	[FeedbackHelp("This feedback lets you raise a GameEvent.")]
	public class GameEventFeedback : MMF_Feedback
    {
        #region Properties      

#if UNITY_EDITOR

        /// <inheritdoc/>
        public override Color FeedbackColor => FeedbacksEditorColors.GameEventFeedbackColor;

#endif

		#endregion

		#region Fields

		/// <summary>
		/// Event to raise when the feedback is triggered.
		/// </summary>
		[SerializeField]
        private GameEvent eventToRaise = null;

		/// <inheritdoc/>
		

		#endregion

		#region Feedback

		protected override void CustomPlayFeedback(Vector3 position, float feedbacksIntensity = 1)
		{
			if (eventToRaise == null)
			{
				return;
			}

			eventToRaise.Raise();
		}

	    #endregion
    }
}