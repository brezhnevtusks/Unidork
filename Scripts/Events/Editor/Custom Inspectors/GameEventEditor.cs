#if DOOZY_UIMANAGER && DOOZY_SIGNALS
using Doozy.Editor.Signals.ScriptableObjects;
#endif
using Sirenix.OdinInspector.Editor;

namespace Unidork.Events
{
	using UnityEditor;
	using UnityEngine;

	[CanEditMultipleObjects]
	[CustomEditor(typeof(GameEvent))]
	public class GameEventEditor : OdinEditor
	{
		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			var @event = (GameEvent)target;
			string eventName = @event.name;
			
#if DOOZY_UIMANAGER && DOOZY_SIGNALS
			
			bool raiseDoozySignal = serializedObject.FindProperty("raiseDoozySignal").boolValue;

			StreamIdDataGroup signalDatabase = StreamIdDatabase.instance.database;

			string gameEventsCategoryName = GameEvent.GameEventSignalCategoryName;
			
			if (raiseDoozySignal)
			{
				if (!signalDatabase.ContainsName(gameEventsCategoryName, eventName))
				{
					signalDatabase.AddName(gameEventsCategoryName, eventName);
					EditorUtility.SetDirty(StreamIdDatabase.instance);
				}
			}
			else
			{
				if (signalDatabase.ContainsName(gameEventsCategoryName, eventName))
				{
					signalDatabase.RemoveName(gameEventsCategoryName, eventName);
					EditorUtility.SetDirty(StreamIdDatabase.instance);
				}
			}
			
#endif
			GUI.enabled = Application.isPlaying;

			if (GUILayout.Button("RAISE"))
			{
				@event.Raise();
			}

			GUI.enabled = true;
		}
	}
}