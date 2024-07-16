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

        private bool wasRaised;
        
        #endregion

        private void Start()
        {
            if (raiseOnStart)
            {
                RaiseEvent();
            }
        }

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