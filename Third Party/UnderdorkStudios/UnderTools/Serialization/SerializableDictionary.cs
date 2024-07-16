using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnderdorkStudios.UnderTools.Serialization
{
    /// <summary>
    /// Base class for serializable dictionaries. Inherit from this to have a dictionary that can be used in a serialized field.
    /// For example: class UnderDictionary : SerializableDictionary{string, int} { }
    /// </summary>
    /// <typeparam name="TKey">Key type. Must be serializable by Unity.</typeparam>
    /// <typeparam name="TValue">Value type. Must be serializable by Unity.</typeparam>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        #region Fields

        /// <summary>
        /// Dictionary keys.
        /// </summary>
        [SerializeField] private List<TKey> keys = new();
        
        /// <summary>
        /// Dictionary values.
        /// </summary>
        [SerializeField] private List<TValue> values = new();

        #endregion

        #region Serialization

        public void OnBeforeSerialize()
        {
            // Save out dictionary key-value pairs into respective serializable lists.
            keys.Clear();
            values.Clear();
            
            foreach (KeyValuePair<TKey, TValue> keyValuePair in this)
            {
                keys.Add(keyValuePair.Key);
                values.Add(keyValuePair.Value);
            }
        }

        public void OnAfterDeserialize()
        {
            // Grab keys and values from serializable lists and re-create dictionary's key value pairs.
            Clear();

            if (keys.Count != values.Count)
                throw new Exception($"Key and value count doesn't match after deserialization! There are {keys.Count} keys and {values.Count} values! Make sure that key and value types are serializable!");

            for (int i = 0, count = keys.Count; i < count; i++)
            {
                Add(keys[i], values[i]);
            }
        }

        #endregion
    }
}