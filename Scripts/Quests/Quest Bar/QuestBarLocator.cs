using System.Collections.Generic;
using Unidork.Constants;
using UnityEngine;

namespace Unidork.QuestSystem
{
	/// <summary>
	/// Utility object that helps find <see cref="IQuestBar"/> objects.
	/// </summary>
	public static  class QuestBarLocator
	{
		/// <summary>
		/// All quest bars in the game.
		/// </summary>
		private static readonly List<IQuestBar> questBars;

		/// <summary>
		/// Constructor.
		/// </summary>
		static QuestBarLocator()
		{
			questBars = new List<IQuestBar>();
			questBars.AddRange(GameObject.FindWithTag(CommonTags.QuestBarHolder).transform.GetComponentsInChildren<IQuestBar>());
		}
		
		/// <summary>
		/// Gets a quest bar with a specific name.
		/// </summary>
		/// <param name="questBarName">Quest bar name.</param>
		/// <returns>
		/// An <see cref="IQuestBar"/> that matches the passed name or null if such bar doesn't exist.
		/// </returns>
		public static IQuestBar GetQuestBar(string questBarName)
		{
			foreach (IQuestBar questBar in questBars)
			{
				if (!string.Equals(questBar.Name, questBarName))
				{
					continue;
				}
				
				return questBar;
			}

			return null;
		}
	}    
}