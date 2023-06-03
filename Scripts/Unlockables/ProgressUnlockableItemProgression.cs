using Unidork.Attributes;
using UnityEngine;

namespace Unidork.Unlockables
{
	/// <summary>
	/// Scriptable object that stores data about items that are unlocked by progressing through
	/// stages/levels or other increments of the games progression. This is mostly intended
	/// to be used for hypercasual-like progressions like skins/vanity items unlocked by beating levels.
	/// </summary>
	[CreateAssetMenu(fileName = "ProgressUnlockableItemProgression", 
                     menuName = "Progress/Progress Unlockable Item Progression")]
    
    public class ProgressUnlockableItemProgression : ScriptableObject
    {
        #region Properties

        /// <summary>
        /// Level (stage) at which we should start calculating unlock progress for items.
        /// </summary>
        /// <value>
        /// Gets the value of the integer field levelToStartUnlockingItems.
        /// </value>
        public int LevelToStartUnlockingItems => levelToStartUnlockingItems;

        #endregion

        #region Fields

        /// <summary>
        /// Level (stage) at which we should start calculating unlock progress for items.
        /// </summary>
        [Space, SettingsHeader, Space]
        [Tooltip("Level (stage) at which we should start calculating unlock progress for items.")]
        [SerializeField]
        private int levelToStartUnlockingItems = 1;

        /// <summary>
        /// Array storing data about progress unlockable items.
        /// </summary>
        [Tooltip("Array storing data about progress unlockable items.")]
        [SerializeField]
        private ProgressUnlockableItemProgressionEntry[] progressionItems = null;

        /// <summary>
		/// Scriptable object storing references to all progress-unlockable items in the game.
		/// </summary>
		[Tooltip("Scriptable object storing references to all progress-unlockable items in the game.")]
        [SerializeField]
        private ProgressUnlockableItemDatabase progressUnlockableItemDatabase = null;

        #endregion

        #region Get

        /// <summary>
        /// Gets the progression entry at the specified index.
        /// </summary>
        /// <param name="index">Entry index.</param>
        /// <returns>
        /// A <see cref="ProgressUnlockableItemProgressionEntry"/> at the 
        /// passed index in the <see cref="progressionItems"/> array.
        /// </returns>
        public ProgressUnlockableItemProgressionEntry GetProgressionEntryAtIndex(int index)
		{
            return progressionItems[index];
        }        

		#endregion
	}
}