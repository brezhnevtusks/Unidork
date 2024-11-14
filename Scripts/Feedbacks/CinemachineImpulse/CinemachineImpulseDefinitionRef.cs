using Unity.Cinemachine;
using UnityEngine;

namespace Unidork.Feedbacks
{
    [CreateAssetMenu(fileName = "CFG_NewCinemachineImpulseDefinitionRef", menuName = "Unidork/Feedbacks/Cinemachine Impulse Definition")]
    public class CinemachineImpulseDefinitionRef : ScriptableObject
    {
        public CinemachineImpulseDefinition Value { get => value; set => this.value = value; }
        public Vector3 Velocity { get => velocity; set => velocity = value; }

        [SerializeField] private CinemachineImpulseDefinition value;
        [SerializeField] private Vector3 velocity;
    }
}