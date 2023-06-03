using MoreMountains.Feedbacks;
using System.Collections.Generic;

namespace Unidork.Feedbacks
{
	public static class MMFeedbacksExtensions
    {
        /// <summary>
        /// Plays all <see cref="MMF_Feedback"/> components assigned in a <see cref="MMF_Player"/>
        /// that match the passed label.
        /// </summary>
        /// <param name="feedbacks">MM feedbacks.</param>
        /// <param name="label">Feedback label.</param>
        /// <param name="playInReverse">Should feedbacks be plated in reverse?</param>
        public static void PlayFeedbacksWithLabel(this MMF_Player feedbacks, string label,
                                                  bool playInReverse = false)
		{
            List<MMF_Feedback> oldList = new List<MMF_Feedback>(feedbacks.FeedbacksList);

            List<MMF_Feedback> feedbacksWithLabel = oldList.FindAll(feedback => feedback.Label.Equals(label));
            feedbacks.FeedbacksList = feedbacksWithLabel;

            if (playInReverse)
			{
                feedbacks.PlayFeedbacksInReverse();
			}
			else
			{
                feedbacks.PlayFeedbacks();
            }

            feedbacks.FeedbacksList = oldList;
		}

        /// <summary>
        /// Activates all feedbacks assigned in a <see cref="MMF_Player"/> component
        /// that match the passed label.
        /// </summary>
        /// <param name="feedbacks">Feedbacks.</param>
        /// <param name="label">Label.</param>
        /// <param name="deactivateOtherFeedbacks">Should feedbacks that don't match the 
        /// label be deactivated?</param>
        public static void ActivateFeedbacksWithLabel(this MMF_Player feedbacks, string label,
                                                      bool deactivateOtherFeedbacks = false)
		{
            ToggleFeedbacksWithLabel(feedbacks, label, true, deactivateOtherFeedbacks);
        }

        /// <summary>
        /// Deactivates all feedbacks assigned in a <see cref="MMF_Player"/> component
        /// that match the passed label.
        /// </summary>
        /// <param name="feedbacks">Feedbacks.</param>
        /// <param name="label">Label.</param>
        public static void DeactivateFeedbacksWithLabel(this MMF_Player feedbacks, string label)
        {
            ToggleFeedbacksWithLabel(feedbacks, label, false, false);
        }

        /// <summary>
        /// Sets the <see cref="MMF_Feedback.Active"/> property of all feedbacks
        /// assigned in a <see cref="MMF_Player"/> component
        /// that match the passed label to the passed toggle value.
        /// </summary>
        /// <param name="feedbacks">Feedbacks.</param>
        /// <param name="label">Label.</param>
        /// <param name="deactivateOtherFeedbacks">Should feedbacks that don't match the 
        /// label be deactivated?</param>
        private static void ToggleFeedbacksWithLabel(MMF_Player feedbacks, string label, bool toggleValue,
                                                     bool deactivateOtherFeedbacks)
		{
            foreach (MMFeedback feedback in feedbacks.Feedbacks)
            {
                if (!feedback.Label.Equals(label))
                {
                    if (deactivateOtherFeedbacks)
					{
                        feedback.Active = false;
					}

                    continue;
                }

                feedback.Active = toggleValue;
            }
        }
    }
}