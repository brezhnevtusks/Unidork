namespace Unidork.Unlockables
{
	/// <summary>
	/// Way in which next progress-unlockable item to be unlocked is selected.
	/// <para>Item is assigned manually in the <see cref="ProgressUnlockableItemProgression"/>.</para>
	/// <para>Item is selected randomly from entries in <see cref="ProgressUnlockableItemDatabase"/>.</para>
	/// </summary>    
	public enum ProgressUnlockableItemSelectionType
    {
        /// <summary>
        /// Item is assigned manually in the <see cref="ProgressUnlockableItemProgression"/>
        /// </summary>
        Manual,

        /// <summary>
        /// Item is selected randomly from entries in <see cref="ProgressUnlockableItemDatabase"/>.
        /// </summary>
        Random
    }
}