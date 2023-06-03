using Unidork.Events;
using UnityEngine;

namespace Unidork.Utility
{
    public class GameEventRaiser : MonoBehaviour
    {
        #region Fields

        [SerializeField] private GameEvent eventToRaise;
        [SerializeField] private bool raiseOnlyOnce;
        [SerializeField] private bool raiseOnStart;

        #endregion

        #region Init

        private void Start()
        {
            if (!raiseOnStart)
            {
                return;
            }
            
            RaiseEvent();
        }

        #endregion

        private bool wasRaised;

        public void RaiseEvent()
        {
            if (raiseOnlyOnce)
            {
                if (wasRaised)
                {
                    Destroy(gameObject);
                    return;
                }

                wasRaised = true;
            }
            
            eventToRaise.Raise();
        }
    }
}