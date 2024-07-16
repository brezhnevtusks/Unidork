using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnderdorkStudios.UnderTools.Extensions;
using UnityEngine;

namespace Unidork.WeightedSelection
{
    [System.Serializable]
    public class WeightedTable<TWeightedObject, TObject> where TWeightedObject: WeightedObject<TObject>
    {
        #region Fields

        /// <summary>
        /// Objects in this group.
        /// </summary>
        [SerializeField]
        private List<TWeightedObject> objects;
        
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
        /// <param name="objectsToExclude">Objects to exclude from the original list when computing weights.</param>
        /// <param name="random">Optional System.Random to use for weight computation.</param>
        /// <returns>
        /// A <see cref="TObject"/> or null if group has no valid data.
        /// </returns>
        public TObject GetRandomWeightedObject(List<TWeightedObject> objectsToExclude = null, System.Random random = null)
        {
            if (objects.IsNullOrEmpty())
            {
                Debug.LogError("{Object group has no data!}");
                return default;
            }
            
            if (!objectsToExclude.IsNullOrEmpty())
            {
                var weightedObjects = new List<TWeightedObject>();
                weightedObjects.AddRange(objects);

                foreach (TWeightedObject objectToExclude in objectsToExclude)
                {
                    weightedObjects.Remove(objectToExclude);
                }
                
                ComputeWeights(weightedObjects, out float totalWeight);
                
                return GetRandomWeightedObject(weightedObjects, totalWeight, random);
            }
            
            if (!weightsComputed) 
            { 
                ComputeWeights();
            }

            return GetRandomWeightedObject(objects, totalObjectWeight, random);
        }

        /// <summary>
        /// Gets a random weight selected object from passed selection.
        /// </summary>
        /// <param name="weightedObjects">Objects to select from.</param>
        /// <param name="totalWeight">Total weight of objects in selection.</param>
        /// <param name="random">Optional System.Random to use for random weight generation.</param>
        /// <returns>
        /// An <see cref="TWeightedObject"/> or default value if passed list is null or empty.
        /// </returns>
        private TObject GetRandomWeightedObject(List<TWeightedObject> weightedObjects, float totalWeight, System.Random random = null)
        {
            if (weightedObjects.IsNullOrEmpty())
            {
                Debug.LogError("Weighted object list is null or empty!");
                return default;
            }
            
            random ??= new System.Random();
            
            float weight = (float)(random.NextDouble() * totalWeight);
 
            foreach (var weightedObject in weightedObjects)
            {
                if (weight > weightedObject.RangeFrom && weight < weightedObject.RangeTo)
                {
                    return weightedObject.Object;
                }
            }

            Debug.LogError(("Failed to get random weighted object!"));
            return weightedObjects.First().Object;
        }

        #endregion
        
        #region Weights

        /// <summary>
        /// Computes and gets weighted selection data from this table.
        /// </summary>
        /// <param name="objectsToExclude">Objects to exclude from the original list when computing weights.</param>
        /// <returns>
        /// A tuple containing a list of <see cref="TWeightedObject"/> and a float representing total weight.
        /// </returns>
        public (List<TWeightedObject>, float) GetComputedWeights(List<TWeightedObject> objectsToExclude = null)
        {
            if (objects.IsNullOrEmpty())
            {
                Debug.LogError("Weighted object list is null or empty!");
                return default;
            }

            var weightedObjects = new List<TWeightedObject>();
            weightedObjects.AddRange(objects);

            if (!objectsToExclude.IsNullOrEmpty())
            {
                foreach (TWeightedObject objectToExclude in objectsToExclude)
                {
                    weightedObjects.Remove(objectToExclude);
                }
            }
            
            float totalWeight = 0f;
			
            foreach(var weightedObject in weightedObjects)
            {
                if (weightedObject.Weight >= 0f)
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
			
            foreach(var weightedObject in weightedObjects)
            {
                weightedObject.SelectionChance = weightedObject.Weight / totalWeight * 100f;
            }

            return (weightedObjects, totalWeight);
        }
        
        /// <summary>
        /// Computes the weights of the objects in this group.
        /// </summary>
        [Button("COMPUTE WEIGHTS", ButtonSizes.Large), GUIColor(0f, 1f, 0f)]
        public void ComputeWeights()
        {
            ComputeWeights(objects, out totalObjectWeight);
            weightsComputed = true;
        }

        /// <summary>
        /// Computes the weights of the objects in the passed list.
        /// </summary>
        /// <param name="objectList">List of objects.</param>
        /// <param name="totalWeight">Total weight of all objects in the list.</param>
        private void ComputeWeights(List<TWeightedObject> objectList, out float totalWeight)
        {
            totalWeight = 0f;
			
            foreach(var weightedObject in objectList)
            {
                if (weightedObject.Weight >= 0f)
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
    }
}