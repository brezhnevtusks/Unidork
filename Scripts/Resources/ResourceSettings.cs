using UnityEngine;

namespace Unidork.Resources
{
    /// <summary>
    /// Base class for scriptable objects that store configuration data for in-game objects like currencies, resources, etc.
    /// </summary>
    public class ResourceSettings<T> : ScriptableObject where T : System.Enum
    {
        public ResourceData<T>[] Resources => (ResourceData<T>[])resources.Clone();
        [SerializeField] private ResourceData<T>[] resources;
    }
}