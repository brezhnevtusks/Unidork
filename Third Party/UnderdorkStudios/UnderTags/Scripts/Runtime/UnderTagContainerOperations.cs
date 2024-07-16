using UnderdorkStudios.UnderTools.Extensions;
using UnityEngine;

namespace UnderdorkStudios.UnderTags
{
    /// <summary>
    /// Stores a set of operations of adding/removing tags from one or multiple <see cref="UnderTagContainer"/>.
    /// Call <see cref="Perform"/> to execute the tag operations.
    /// </summary>
    public class UnderTagContainerOperations : MonoBehaviour
    {
        #region Fields

        /// <summary>
        /// Containers to perform the operations on.
        /// </summary>
        [SerializeField] private UnderTagContainer[] targetContainers;
        
        /// <summary>
        /// Tags to add to <see cref="targetContainers"/>.
        /// </summary>
        [SerializeField] private UnderTag[] tagsToAdd;
        
        /// <summary>
        /// Tags to remove from <see cref="targetContainers"/>.
        /// </summary>
        [SerializeField] private UnderTag[] tagsToRemove;
        
        #endregion

        #region Perform

        /// <summary>
        /// Performs all tag operations set in the inspector.
        /// Adds <see cref="tagsToAdd"/> and removes <see cref="tagsToRemove"/> from every item in <see cref="targetContainers"/>.
        /// </summary>
        public void Perform()
        {
            if (targetContainers.IsNullOrEmpty())
            {
                Debug.LogError($"UnderTags: UnderTagContainerOperations on {gameObject.name} don't have any target containers!", this);
                return;
            }

            foreach (UnderTagContainer targetContainer in targetContainers)
            {
                if (targetContainer == null)
                {
                    Debug.LogError($"UnderTags: UnderTagContainerOperations on {gameObject.name} has a null target container!", this);
                    continue;
                }
                
                targetContainer.AddTags(tagsToAdd);
                targetContainer.RemoveTags(tagsToRemove);
            }
        }

        #endregion
    }
}