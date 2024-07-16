using UnityEngine;

namespace Unidork.WeightedSelection
{
    /// <summary>
    /// A scriptable object that wraps a weighted object selection table.
    /// </summary>
    /// <typeparam name="TWeightedTable">Weighted table type.</typeparam>
    /// <typeparam name="TWeightedObject">Weighted object type.</typeparam>
    /// <typeparam name="TObject">Weighted value type.</typeparam>
    public class WeightedTableSO<TWeightedTable, TWeightedObject, TObject> : ScriptableObject 
        where TWeightedTable : WeightedTable<TWeightedObject, TObject>
        where TWeightedObject : WeightedObject<TObject>
    {
        public TWeightedTable Table => table;
        [SerializeField] private TWeightedTable table;
    }
}