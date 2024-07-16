using MoreMountains.Feedbacks;
using System.Collections.Generic;
using UnderdorkStudios.UnderTools.Extensions;
using Unidork.Utility;
using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace Unidork.Feedbacks
{
    public class FeedbackPlayer : MonoBehaviour, IPausable, ILevelDestroyListener
    {
        #region Properties

        public ReactiveProperty<bool> IsPaused { get; } = new(false);

        #endregion
        
        #region Fields

        private Dictionary<MMF_Player, UnityAction> activeFeedbacks;

        #endregion

        #region Init

        private void Start()
        {
            PauseManager.RegisterPausable(this);
            
            IsPaused
                .Subscribe(isPaused =>
                {
                    if (isPaused)
                    {
                        PauseActiveFeedbacks();
                    }
                    else
                    {
                        ResumeActiveFeedbacks();
                    }
                })
                .AddTo(this);
        }

        #endregion

        #region Feedbacks

        public void PlayFeedbacks(MMF_Player feedbacks)
        {
            if (feedbacks == null)
            {
                return;
            }
            
            activeFeedbacks ??= new Dictionary<MMF_Player, UnityAction>();

            if (activeFeedbacks.ContainsKey(feedbacks))
            {
                return;
            }
            
            feedbacks.PlayFeedbacks();
                
            UnityAction feedbackCompleteAction = () => { activeFeedbacks.Remove(feedbacks); };
            activeFeedbacks.Add(feedbacks, feedbackCompleteAction);
            feedbacks
                .Events
                .OnComplete
                .AddListener(feedbackCompleteAction);
        }

        public void StopFeedbacks(MMF_Player feedbacks)
        {
            if (feedbacks == null || activeFeedbacks == null || !activeFeedbacks.ContainsKey(feedbacks))
            {
                return;
            }
            
            feedbacks.StopFeedbacks();
            activeFeedbacks.Remove(feedbacks);
        }
        
        public void StopAllFeedbacks()
        {
            if (activeFeedbacks.IsNullOrEmpty())
            {
                return;
            }
            
            foreach (KeyValuePair<MMF_Player,UnityAction> activeFeedback in activeFeedbacks)
            { 
                activeFeedback.Key.StopFeedbacks();
            }
            
            activeFeedbacks.Clear();
        }
        
        private void PauseActiveFeedbacks()
        {
            if (activeFeedbacks.IsNullOrEmpty())
            {
                return;
            }

            foreach (KeyValuePair<MMF_Player,UnityAction> keyValuePair in activeFeedbacks)
            {
                keyValuePair
                    .Key
                    .PauseFeedbacks();
            }
        }

        private void ResumeActiveFeedbacks()
        {
            if (activeFeedbacks.IsNullOrEmpty())
            {
                return;
            }

            foreach (KeyValuePair<MMF_Player,UnityAction> keyValuePair in activeFeedbacks)
            {
                keyValuePair
                    .Key
                    .ResumeFeedbacks();
            }
        }

        #endregion

        #region Destroy

        public void OnLevelDestroyed()
        {
            StopAllFeedbacks();
            PauseManager.UnregisterPausable(this);
        }

        #endregion
    }
}