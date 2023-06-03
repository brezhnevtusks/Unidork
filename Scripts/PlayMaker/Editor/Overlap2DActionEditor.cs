#if PLAYMAKER

using System;
using HutongGames.PlayMakerEditor;
using UnityEngine;
	
namespace Unidork.PlayMaker.Editor
{
	[CustomActionEditor(typeof(Overlap2D))]
	public class Overlap2DActionEditor : CustomActionEditor
	{
		private Overlap2D overlapAction;

		private const float Space = 10f;
		
		public override void OnEnable()
		{
			base.OnEnable();
			overlapAction = (Overlap2D)target;
		}

		public override bool OnGUI()
		{
			EditField("LayerMask");
			AddSpace();
			
			EditField("Shape");
			AddSpace();
			
			switch (overlapAction.Shape)
			{
				case Overlap2D.Shape2D.Box:
					DrawBoxSettings();
					break;
				case Overlap2D.Shape2D.Circle:
					DrawCircleSettings();
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}

			AddSpace();
			
			EditField("OverlapCount");
			EditField("OverlappingColliders");

			AddSpace();
			EditField("everyFrame");

			return GUI.changed;
		}

		private void DrawBoxSettings()
		{
			DrawOrigin();
			AddSpace();
			DrawSizeSource();
		}

		private void DrawCircleSettings()
		{
			DrawOrigin();
			AddSpace();
			DrawSizeSource();
		}

		private void DrawOrigin()
		{
			EditField("OriginType");
			
			switch (overlapAction.OriginType)
			{
				case Overlap2D.CheckOriginType.GameObject:
					EditField("OriginGameObject");
					break;
				case Overlap2D.CheckOriginType.Collider:
					EditField("Collider");
					break;
				case Overlap2D.CheckOriginType.Vector2:
					EditField("OriginPosition");
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void DrawSizeSource()
		{
			EditField("SizeSource");
			
			switch (overlapAction.Shape)
			{
				case Overlap2D.Shape2D.Box:
					if (overlapAction.OriginType == Overlap2D.CheckOriginType.Collider && overlapAction.SizeSource == Overlap2D.SizeSourceType.Collider)
					{
						break;
					}
					
					EditField(overlapAction.SizeSource == Overlap2D.SizeSourceType.Collider ? "Collider" : "BoxExtents");
					break;
				case Overlap2D.Shape2D.Circle:
					if (overlapAction.OriginType == Overlap2D.CheckOriginType.Collider && overlapAction.SizeSource == Overlap2D.SizeSourceType.Collider)
					{
						break;
					}
			
					EditField(overlapAction.SizeSource == Overlap2D.SizeSourceType.Collider ? "Collider" : "Radius");
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private void AddSpace() => GUILayout.Space(Space);
	}
}
	
#endif