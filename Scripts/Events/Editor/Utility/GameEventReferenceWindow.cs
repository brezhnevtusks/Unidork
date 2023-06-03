using Sirenix.OdinInspector.Editor;

namespace Unidork.Events
{
	public class GameEventReferenceWindow : OdinMenuEditorWindow
	{
		#region Properties

		/// <summary>
		/// Is the window currently showing?
		/// </summary>
		public static bool IsShowing { get; set; } = false;

		#endregion
		
		#region Fields

		/// <summary>
		/// Reference to the event that needs to be located.
		/// </summary>
		private static GameEvent eventToLocate;

		#endregion

		#region Show		

		public static void ShowWindow(GameEvent eventToLocate)
		{
			GameEventReferenceWindow.eventToLocate = eventToLocate;
			IsShowing = true;
			GetWindow<GameEventReferenceWindow>().Show();
		}

		protected override OdinMenuTree BuildMenuTree()
		{
			var tree = new OdinMenuTree();

			var locatedGameEvents = new GameEventReferences
			{
				References = GameEventLocator.LocateEvent(eventToLocate)
			};

			tree.Add(eventToLocate.Name, locatedGameEvents);

			return tree;
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