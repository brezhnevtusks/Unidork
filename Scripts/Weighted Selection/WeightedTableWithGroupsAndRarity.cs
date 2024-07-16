using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnderdorkStudios.UnderTools.Extensions;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Unidork.WeightedSelection
{
	/// <summary>
	/// Table storing data used for random weighted selection. Data is separated into groups, each with its own type.
	/// Groups also have have weights of their own so we are able to use random weighted selection on group level first
	/// (for example, to select between armor, weapon, or gold) and then on objects in the group itself (to pick a specific armor/weapon/gold item
	/// based on its weight/rarity).
	/// </summary>
	/// <typeparam name="ObjectGroupType">Type of object group.</typeparam>
	/// <typeparam name="TObject">Object.</typeparam>
	/// <typeparam name="TObjectType">Type of object or value.</typeparam>
	/// <typeparam name="TRarity">Object or value rarity.</typeparam>
	public class WeightedTableWithGroupsAndRarity<ObjectGroupType, TObject, TObjectType, TRarity> : ScriptableObject where ObjectGroupType : Enum
																									  where TObject:WeightedObjectWithRarity<TObjectType, TRarity>
																									  where TRarity : Enum
	{
		#region Fields

		/// <summary>
		/// Groups in this table.
		/// </summary>
		[Tooltip("Groups in this table.")]
		[SerializeField]
		private List<WeightedObjectGroupWithRarity<ObjectGroupType, TObject, TObjectType, TRarity>> groups = new();

		/// <summary>
		/// Total weight of all groups in this table.
		/// </summary>
		[SerializeField, ReadOnly]
		private float totalWeight;
		
		/// <summary>
		/// Have the weights for this table been computed?
		/// </summary>
		private bool weightsComputed;

		/// <summary>
		/// Array storing all object group type values.
		/// </summary>
		private static ObjectGroupType[] allObjectGroupTypes;
		
		/// <summary>
		/// Array storing all rarity values.
		/// </summary>
		private static TRarity[] allRarities;

		#endregion

		#region Select

		/// <summary>
		/// Gets a random weighted object. Includes/excluded object groups and rarities based on passed optional arguments.
		/// </summary>
		/// <param name="groupTypesToInclude">Type of groups to include.</param>
		/// <param name="groupTypesToExclude">Type of groups to exclude.</param>
		/// <param name="raritiesToInclude">Type of rarities to include.</param>
		/// <param name="raritiesToExclude">Types of rarities to exclude.</param>
		/// <returns>
		/// An instance of <see cref="TObjectType"/> or <see cref="TObjectType"/> default value if no data exists
		/// or no data matches the specified selection parameters.
		/// </returns>
		public TObjectType GetObject(List<ObjectGroupType> groupTypesToInclude = null, List<ObjectGroupType> groupTypesToExclude = null, 
		                               List<TRarity> raritiesToInclude = null, List<TRarity> raritiesToExclude = null)
		{
			if (groups.IsNullOrEmpty())
			{
				Debug.Log("Object groups are null or empty!");
				return default;
			}

			WeightedObjectGroupWithRarity<ObjectGroupType, TObject, TObjectType, TRarity> randomGroupWithRarity;

			List<ObjectGroupType> allowedObjectGroupTypes = CreateAllowedObjectGroupList(groupTypesToInclude, groupTypesToExclude);

			if (allowedObjectGroupTypes.IsEmpty())
			{
				Debug.LogError($"Allowed object group type list is empty!");
				return default;
			}

			if (allowedObjectGroupTypes.Count == allObjectGroupTypes.Length)
			{
				ComputeGroupWeights(groups, out float _);
				randomGroupWithRarity = GetWeightedRandomGroup(groups, totalWeight);
			}
			else
			{
				var groupsOfTypes = GetGroupsOfTypes(allowedObjectGroupTypes);
				
				if (groupsOfTypes.IsEmpty())
				{
					Debug.LogWarning("Failed to find any object groups matching the passed types!");
					return default;
				}

				ComputeGroupWeights(groupsOfTypes, out float groupTotalWeight);
				randomGroupWithRarity = GetWeightedRandomGroup(groupsOfTypes, groupTotalWeight);
			}

			return randomGroupWithRarity.GetObjectByRarity(raritiesToInclude, raritiesToExclude);
		}

		/// <summary>
		/// Creates a list of allowed object groups.
		/// </summary>
		/// <param name="groupTypesToInclude">Group types to include.</param>
		/// <param name="groupTypesToExclude">Group types to exclude.</param>
		/// <returns>
		/// A list of <see cref="ObjectGroupType"/> based on passed arguments.
		/// </returns>
		private List<ObjectGroupType> CreateAllowedObjectGroupList(List<ObjectGroupType> groupTypesToInclude = null, List<ObjectGroupType> groupTypesToExclude = null)
		{
			List<ObjectGroupType> allowedObjectGroupTypes = new List<ObjectGroupType>((groupTypesToInclude.IsNullOrEmpty() ? allObjectGroupTypes : groupTypesToInclude)!);

			if (groupTypesToExclude != null)
			{
				allowedObjectGroupTypes = allowedObjectGroupTypes.FindAll(type => !groupTypesToExclude.Contains(type));	
			}

			return allowedObjectGroupTypes;
		}

		/// <summary>
		/// Gets a weighted random group from a list of passed groups.
		/// </summary>
		/// <param name="weightedGroups">List of groups to choose from.</param>
		/// <param name="groupTotalWeight">Total weight of objects in this group.</param>
		/// <returns>
		/// A weighted object group.
		/// </returns>
		private WeightedObjectGroupWithRarity<ObjectGroupType, TObject, TObjectType, TRarity> GetWeightedRandomGroup(
			List<WeightedObjectGroupWithRarity<ObjectGroupType, TObject, TObjectType, TRarity>> weightedGroups, float groupTotalWeight)
		{
			float index = Random.Range(0, groupTotalWeight);
 
			foreach (var group in weightedGroups)
			{
				if (index > group.RangeFrom && index < group.RangeTo)
				{
					return group;
				}
			}

			Debug.LogError(("Failed to get weighted group!"));
			return groups.First();
		}
		
		/// <summary>
		/// Gets all groups that match the passed group types.
		/// </summary>
		/// <param name="groupTypes">Group types to include.</param>
		/// <returns>
		/// A list of weighted groups that match the condition.
		/// </returns>
		private List<WeightedObjectGroupWithRarity<ObjectGroupType, TObject, TObjectType, TRarity>> GetGroupsOfTypes(List<ObjectGroupType> groupTypes)
		{
			var groupsOfTypes = new List<WeightedObjectGroupWithRarity<ObjectGroupType, TObject, TObjectType, TRarity>>();

			foreach (var group in groups)
			{
				if (!groupTypes.Contains(group.Type))
				{
					continue;
				}
				
				groupsOfTypes.Add(group);
			}
			
			return groupsOfTypes;
		}

		#endregion

		#region Weights

		/// <summary>
		/// Computes the weights for all groups in this table and their content.
		/// </summary>
		[Button("COMPUTE WEIGHTS", ButtonSizes.Large), GUIColor(0f, 1f, 0f)]
		public void ComputeWeights()
		{
			if (groups.IsNullOrEmpty())
			{
				return;
			}

			ComputeGroupWeights(groups, out float totalGroupWeight);

			totalWeight = totalGroupWeight;

			weightsComputed = true;
		}

		/// <summary>
		/// Computes weights in the passed list of groups.
		/// </summary>
		/// <param name="groups">Group list.</param>
		/// <param name="totalWeight">Total calculated weight of objects.</param>
		private void ComputeGroupWeights(List<WeightedObjectGroupWithRarity<ObjectGroupType, TObject, TObjectType, TRarity>> groups, out float totalWeight)
		{
			totalWeight = 0f;
			
			if (groups.IsNullOrEmpty())
			{
				return;
			}

			foreach(var group in groups)
			{
				if(group.Weight >= 0f)
				{
					group.RangeFrom = totalWeight;
					totalWeight += group.Weight;	
					group.RangeTo = totalWeight;
				} 
				else 
				{
					group.Weight =  0f;						
				}
			}
			
			foreach(var group in groups)
			{
				group.SelectionChance = group.Weight / totalWeight * 100f;
				group.ComputeWeights();
			}
		}

		#endregion

		#region Enable

		private void OnEnable()
		{
			if (!allObjectGroupTypes.IsNullOrEmpty())
			{
				return;
			}
			
			Array groupTypeArray = Enum.GetValues(typeof(ObjectGroupType));
			allObjectGroupTypes = new ObjectGroupType[groupTypeArray.Length];
			groupTypeArray.CopyTo(allObjectGroupTypes, 0);
			
			Array rarityArray = Enum.GetValues(typeof(TRarity));
			allRarities = new TRarity[rarityArray.Length];
			rarityArray.CopyTo(allRarities, 0);
			
			WeightedObjectGroupWithRarity<ObjectGroupType, TObject, TObjectType, TRarity>.InitRaritiesArray(allRarities);
		}

		#endregion
	}
}