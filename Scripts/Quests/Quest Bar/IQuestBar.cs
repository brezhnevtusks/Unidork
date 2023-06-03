using UniRx;

namespace Unidork.QuestSystem
{
    /// <summary>
    /// Interface for objects that have a bar that is filled when quests are completed.
    /// Examples: daily quests with rewards, seasons, etc.
    /// </summary>
    public interface IQuestBar
    {
        /// <summary>
        /// Name of this quest bar. Should be unique so that quest bars can be located with the <see cref="QuestBarLocator"/>.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Has this quest bar been initialized?
        /// </summary>
        bool IsInitialized { get; }
        
        /// <summary>
        /// 
        /// </summary>
        ReactiveProperty<int> CurrentPoints { get; }

        /// <summary>
        /// List of divisions that have been completed and their rewards has been collected.
        /// </summary>
        ReactiveCollection<QuestBarDivision> DivisionsWithCollectedRewards { get; }
        
        /// <summary>
        /// List of divisions that 
        /// </summary>
        ReactiveCollection<QuestBarDivision> CompletedDivisions { get; }

        /// <summary>
        /// Has the bar been fully filled?
        /// </summary>
        ReactiveProperty<bool> HasReachedMaxFill { get; }

        /// <summary>
        /// Gets divisions on the quest bar.
        /// </summary>
        QuestBarDivision[] GetDivisions();

        /// <summary>
        /// Called when a completed division is shown in the quest bar. 
        /// </summary>
        /// <param name="division">Quest bar division.</param>
        void OnCompletedDivisionShown(QuestBarDivision division);
    }
}