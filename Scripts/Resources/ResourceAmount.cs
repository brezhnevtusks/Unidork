using System;
using UnityEngine;

namespace Unidork.Scripts.Resources
{
    /// <summary>
    /// Helper class to store an amount of resource.
    /// </summary>
    /// <typeparam name="T">Resource type</typeparam>
    [Serializable]
    public class ResourceAmount<T> where T : Enum
    {
        public T ResourceType => resourceType;
        public double Amount => amount;

        [SerializeField] private T resourceType;
        [SerializeField] private double amount;
    }
}