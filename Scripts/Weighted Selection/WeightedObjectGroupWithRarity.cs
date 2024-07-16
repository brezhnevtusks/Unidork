using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnderdorkStudios.UnderTools.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unidork.WeightedSelection
{
	/// <summary>
	/// Collection of weighted objects. Groups can separate objects by type and have weights of their own
	/// so we are able to use random weighted selection on group level first (for example, to select between armor, weapon, or gold)
	/// and then on objects in the group itself (to pick a specific armor/weapon/gold item based on its weight/rarity).
	/// </summary>
	/// <typeparam name="TObjectGroupType">Type of object group.</typeparam>
	/// <typeparam name="TObject">Object that stores data.</typeparam>
	/// <typeparam name="TWeightedObject">Actual object.</typeparam>
	/// <typeparam name="TRarity">Object rarity.</typeparam>
	[System.Serializable]
	public class WeightedObjectGroupWithRarity<TObjectGroupType, TObject, TWeightedObject, TRarity> : WeightedObjectWithRarity<List<TObject>, TRarity> where TObjectGroupType : System.Enum
																																   where TObject: WeightedObjectWithRarity<TWeightedObject, TRarity>
																																   where TRarity : System.Enum
	{
		#region Properties

		/// <summary>
		/// Type of this object group.
		/// </summary>
		/// <value>
		/// Gets the value of the field type.
		/// </value>
		public TObjectGroupType Type => type;

		#endregion
		
		#region Fields

		/// <summary>
		/// Type of this object group.
		/// </summary>
		[Tooltip("Type of this object group.")]
		[PropertyOrder(0)]
		[SerializeField]
		private TObjectGroupType type = default;

		/// <summary>
		/// Have the weights been computed for this group?
		/// </summary>
		private bool weightsComputed;
		
		/// <summary>
		/// Total weight of all objects in this group.
		/// </summary>
		private float totalObjectWeight;

		/// <summary>
		/// Array storing all rarity values.
		/// </summary>
		private static TRarity[] AllRarities;

		#endregion

		#region Init

		/// <summary>
		/// Initializes the all rarities array.
		/// </summary>
		/// <param name="allRarities">Array to initialize with.</param>
		public static void InitRaritiesArray(TRarity[] allRarities)
		{
			AllRarities = allRarities;
		}

		#endregion
		
		#region Get

		/// <summary>
		/// Gets a random object.
		/// </summary>
		/// <returns>
		/// An instance of <see cref="TWeightedObject"/> or default value for its type if no data exists.
		/// </returns>
		public TWeightedObject GetRandomObject()
		{
			if (!@object.IsNullOrEmpty())
			{
				return GetObjectByRarity(new List<TRarity>(AllRarities));
			}
			
			Debug.LogError("{Object group has no data!}");
			return default;
		}

		/// <summary>
		/// Gets an object with specified rarity.
		/// </summary>
		/// <param name="raritiesToInclude">Rarities to include.</param>
		/// <param name="raritiesToExclude">Rarities to exclude..</param>
		/// <returns>
		/// An instance of <see cref="TWeightedObject"/> or default value for its type if no object with specified rarity exists.
		/// </returns>
		public TWeightedObject GetObjectByRarity(List<TRarity> raritiesToInclude = default, List<TRarity> raritiesToExclude = default)
		{
			if (@object.IsNullOrEmpty())
			{
				Debug.LogError("{Object group has no data!}");
				return default;
			}
			
			if (!weightsComputed)
			{
				ComputeWeights();
			}

			List<TRarity> allowedRarities = CreateAllowedRarityList(raritiesToInclude, raritiesToExclude);
			
			List<TObject> rarityList = GetObjectListByRarity(allowedRarities);

			if (rarityList.IsEmpty())
			{
				Debug.LogError($"Object group has no object with specified rarities!");
				return default;
			}
			
			if (rarityList.Count == @object.Count)
			{
				return GetWeightedRandomObject(@object, totalObjectWeight);
			}
			
			ComputeWeights(rarityList, out float objectWeight);

			return GetWeightedRandomObject(rarityList, objectWeight);
		}
		
		/// <summary>
		/// Gets entries in this group that match any of the passed object rarities.
		/// </summary>
		/// <param name="rarities">Allowed object rarities.</param>
		/// <returns>
		/// A list of <see cref="TObject"/> objects.v
		/// </returns>
		private List<TObject> GetObjectListByRarity(List<TRarity> rarities)
		{
			if (rarities.Count == AllRarities.Length)
			{
				return @object;
			}
			
			var objectWithRarity = new List<TObject>();

			foreach (TObject weightedObject in @object)
			{
				if (!rarities.Contains(weightedObject.Rarity))
				{
					continue;
				}
				
				objectWithRarity.Add(weightedObject);
			}
			
			return objectWithRarity;
		}

		/// <summary>
		/// Gets a random weighted object from the passed list.
		/// </summary>
		/// <param name="weightedObjectList">Object list.</param>
		/// <param name="totalObjectWeight">Total weight of items in the list.</param>
		/// <returns>
		/// </returns>
		private TWeightedObject GetWeightedRandomObject(List<TObject> weightedObjectList, float totalObjectWeight)
		{
			float index = Random.Range(0, totalObjectWeight);
 
			foreach (var weightedObject in weightedObjectList)
			{
				if (index > weightedObject.RangeFrom && index < weightedObject.RangeTo)
				{
					return weightedObject.Object;
				}
			}

			Debug.LogError(("Failed to get weighted object!"));
			return weightedObjectList.First().Object;
		}

		/// <summary>
		/// Creates a list of allowed rarities.
		/// </summary>
		/// <param name="raritiesToInclude">Rarities to include in the resulting list.</param>
		/// <param name="raritiesToExclude">Rarities to exclude from the resulting list.</param>
		/// <returns>A list of rarities based on passed arguments.</returns>
		private List<TRarity> CreateAllowedRarityList(List<TRarity> raritiesToInclude = default,
		                                              List<TRarity> raritiesToExclude = default)
		{
			List<TRarity> allowedRarities = new List<TRarity>((raritiesToInclude.IsNullOrEmpty() ? AllRarities : raritiesToInclude)!);

			if (raritiesToExclude != null)
			{
				allowedRarities = allowedRarities.FindAll(rarity => !raritiesToExclude.Contains(rarity));
			}

			return allowedRarities;
		}
		
		#endregion

		#region Weights

		/// <summary>
		/// Computes the weights of the objects in this group.
		/// </summary>
		public void ComputeWeights()
		{
			ComputeWeights(@object, out float totalComputedWeight);
			totalObjectWeight = totalComputedWeight;

			weightsComputed = true;
		}

		/// <summary>
		/// Computes the weights of the objects in the passed list.
		/// </summary>
		/// <param name="objectList">List of objects.</param>
		/// <param name="totalWeight">Total weight of all objects in the list.</param>
		private void ComputeWeights(List<TObject> objectList, out float totalWeight)
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

		#region Constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="value">Values stored in this weighted object group.</param>
		public WeightedObjectGroupWithRarity(List<TObject> value) : base(value)
		{
		}

		#endregion
	}
}