namespace Unidork.QuestSystem
{
    /// <summary>
    /// Type of value that is used to track the progress of the quest.
    /// Value of "None" is reserved for quests that require the player to perform
    /// a unique action/interaction that can't be measured in integers or floats.
    /// </summary>
    public enum QuestProgressValueType
    {
        Int,
        Float,
        None
    }
}