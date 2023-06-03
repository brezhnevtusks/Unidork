using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEditor;

namespace Unidork.Events
{
	public class GameEventListenerWindow : OdinMenuEditorWindow
	{
		#region Properties

		public static bool IsShowing { get; private set; } = false;

		#endregion

		#region Fields

		private static bool needsToRefreshList = false;
		private static OdinMenuTree tree;
		private List<GameEventListenerReference> listenerReferences;

		#endregion

		#region Show

		public void RefreshListenerList() => needsToRefreshList = true;

		[MenuItem("Utility/Show Game Event Listeners")]
		private static void Open()
		{
			GetWindow<GameEventListenerWindow>().Show();
			IsShowing = true;
		}

		protected override OdinMenuTree BuildMenuTree()
		{
			listenerReferences = GameEventLocator.LocateGameEventListeners();

			tree = new OdinMenuTree
			{
				{ "Listener Data", new GameEventListenerReferences() { ListenerReferences = listenerReferences } }
			};

			return tree;
		}

		protected override void OnGUI()
		{
			if (needsToRefreshList)
			{
				foreach (GameEventListenerReference reference in listenerReferences)
				{
					if (reference.Listener == null)
					{
						ForceMenuTreeRebuild();
						needsToRefreshList = false;
						break;
					}
				}				
			}

			base.OnGUI();
		}

		#endregion

		#region Hide

		protected override void OnDestroy()
		{
			IsShowing = false;
			base.OnDestroy();
		}

		#endregion
	}
}