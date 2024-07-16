using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnderdorkStudios.UnderTools.Extensions;
using UnityEngine;

namespace Unidork.WeightedSelection
{
    [CreateAssetMenu(fileName = "WeightedObjectTable_", menuName = "Unidork/Weighted Selection/Table")]
    public class WeightedTableBase : ScriptableObject
    {
        #region Fields

        /// <summary>
        /// Objects in this group.
        /// </summary>
        [SerializeReference]  
        private List<WeightedObjectBase> objects = new();
        
        /// <summary>
        /// Have the weights been computed for this group?
        /// </summary>
        private bool weightsComputed;
		
        /// <summary>
        /// Total weight of all objects in this group.
        /// </summary>
        private float totalObjectWeight;
        
        #endregion

        #region Get

        /// <summary>
        /// Gets a random weight selected object from this table.
        /// </summary>
        /// <returns>
        /// A <see cref="WeightedObjectBase"/> or null if group has no valid data.
        /// </returns>
        public WeightedObjectBase GetRandomWeightedObject(System.Random random = null)
        {
            if (objects.IsNullOrEmpty())
            {
                Debug.LogError("{Object group has no data!}");
                return default;
            }
			
            if (!weightsComputed)
            {
                ComputeWeights();
            }

            random ??= new System.Random();
            
            float weight = (float)(random.NextDouble() * totalObjectWeight);
 
            foreach (var obj in objects)
            {
                if (!(weight > obj.RangeFrom) || !(weight < obj.RangeTo))
                {
                    continue;
                }
                
                if (obj is WeightedObject_WeightedObjectTable table)
                {
                    return table.Object.GetRandomWeightedObject();
                }

                return obj;
            }

            Debug.LogError(("Failed to get weighted object!"));
            return (WeightedObjectBase)objects.First().Object;
        }

        #endregion
        
        #region Weights

        /// <summary>
        /// Computes the weights of the objects in this group.
        /// </summary>
        [Button("COMPUTE WEIGHTS", ButtonSizes.Large), GUIColor(0f, 1f, 0f)]
        public void ComputeWeights()
        {
            ComputeWeights(objects, out float totalComputedWeight);
            totalObjectWeight = totalComputedWeight;

            weightsComputed = true;
        }

        /// <summary>
        /// Computes the weights of the objects in the passed list.
        /// </summary>
        /// <param name="objectList">List of objects.</param>
        /// <param name="totalWeight">Total weight of all objects in the list.</param>
        private void ComputeWeights(List<WeightedObjectBase> objectList, out float totalWeight)
        {
            totalWeight = 0f;
			
            foreach(var weightedObject in objectList)
            {
                if(weightedObject.Weight >= 0f)
                {
                    weightedObject.RangeFrom = totalWeight;
                    totalWeight += weightedObject.Weight;	
                    weightedObject.RangeTo = totalWeight;
                } 
                else 
                {
                    weightedObject.Weight =  0f;						
                }
            }
			
            foreach(var group in objectList)
            {
                group.SelectionChance = group.Weight / totalWeight * 100f;
            }
        }

        #endregion

#if UNITY_EDITOR
        #region Editor

        public List<WeightedObjectBase> GetObjects() => objects;

        #endregion

#endif
    }
}