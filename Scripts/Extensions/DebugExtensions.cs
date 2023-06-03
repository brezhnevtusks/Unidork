using UnityEngine;

/// <summary>
/// Courtesy of https://answers.unity.com/questions/1156087/how-can-you-visualize-a-boxcast-boxcheck-etc.html
/// </summary>
public static class DebugExtensions
{
     public static void DrawBoxCastOnHit(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float hitInfoDistance, Color color)
     {
         origin = CastCenterOnCollision(origin, direction, hitInfoDistance);
         DrawBox(origin, halfExtents, orientation, color);
     }

     public static void DrawBoxCastBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float distance, Color color)
     {
         direction.Normalize();
         var bottomBox = new Box(origin, halfExtents, orientation);
         var topBox = new Box(origin + (direction * distance), halfExtents, orientation);
             
         Debug.DrawLine(bottomBox.BackBottomLeft, topBox.BackBottomLeft, color);
         Debug.DrawLine(bottomBox.BackBottomRight, topBox.BackBottomRight, color);
         Debug.DrawLine(bottomBox.BackTopLeft,topBox.BackTopLeft, color);
         Debug.DrawLine(bottomBox.BackTopRight, topBox.BackTopRight, color);
         Debug.DrawLine(bottomBox.FrontTopLeft, topBox.FrontTopLeft, color);
         Debug.DrawLine(bottomBox.FrontTopRight, topBox.FrontTopRight, color);
         Debug.DrawLine(bottomBox.FrontBottomLeft, topBox.FrontBottomLeft, color);
         Debug.DrawLine(bottomBox.FrontBottomRight, topBox.FrontBottomRight, color);
     
         DrawBox(bottomBox, color);
         DrawBox(topBox, color);
     }

     private static void DrawBox(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Color color)
     {
         DrawBox(new Box(origin, halfExtents, orientation), color);
     }

     private static void DrawBox(Box box, Color color)
     {
         Debug.DrawLine(box.FrontTopLeft,     box.FrontTopRight,    color);
         Debug.DrawLine(box.FrontTopRight,     box.FrontBottomRight, color);
         Debug.DrawLine(box.FrontBottomRight, box.FrontBottomLeft, color);
         Debug.DrawLine(box.FrontBottomLeft,     box.FrontTopLeft, color);
                                                  
         Debug.DrawLine(box.BackTopLeft,         box.BackTopRight, color);
         Debug.DrawLine(box.BackTopRight,     box.BackBottomRight, color);
         Debug.DrawLine(box.BackBottomRight,     box.BackBottomLeft, color);
         Debug.DrawLine(box.BackBottomLeft,     box.BackTopLeft, color);
                                                  
         Debug.DrawLine(box.FrontTopLeft,     box.BackTopLeft, color);
         Debug.DrawLine(box.FrontTopRight,     box.BackTopRight, color);
         Debug.DrawLine(box.FrontBottomRight, box.BackBottomRight, color);
         Debug.DrawLine(box.FrontBottomLeft,     box.BackBottomLeft, color);
     }
     
     public struct Box
     {
         public Vector3 LocalFrontTopLeft {get; private set;}
         public Vector3 LocalFrontTopRight {get; private set;}
         public Vector3 LocalFrontBottomLeft {get; private set;}
         public Vector3 LocalFrontBottomRight {get; private set;}
         public Vector3 LocalBackTopLeft => -LocalFrontBottomRight;
         public Vector3 LocalBackTopRight => -LocalFrontBottomLeft;
         public Vector3 LocalBackBottomLeft => -LocalFrontTopRight;
         public Vector3 LocalBackBottomRight => -LocalFrontTopLeft;

         public Vector3 FrontTopLeft => LocalFrontTopLeft + Origin;
         public Vector3 FrontTopRight => LocalFrontTopRight + Origin;
         public Vector3 FrontBottomLeft => LocalFrontBottomLeft + Origin;
         public Vector3 FrontBottomRight => LocalFrontBottomRight + Origin;
         public Vector3 BackTopLeft => LocalBackTopLeft + Origin;
         public Vector3 BackTopRight => LocalBackTopRight + Origin;
         public Vector3 BackBottomLeft => LocalBackBottomLeft + Origin;
         public Vector3 BackBottomRight => LocalBackBottomRight + Origin;

         public Vector3 Origin {get; }
 
         public Box(Vector3 origin, Vector3 halfExtents, Quaternion orientation) : this(origin, halfExtents)
         {
             Rotate(orientation);
         }

         private Box(Vector3 origin, Vector3 halfExtents)
         {
             LocalFrontTopLeft = new Vector3(-halfExtents.x, halfExtents.y, -halfExtents.z);
             LocalFrontTopRight = new Vector3(halfExtents.x, halfExtents.y, -halfExtents.z);
             LocalFrontBottomLeft = new Vector3(-halfExtents.x, -halfExtents.y, -halfExtents.z);
             LocalFrontBottomRight = new Vector3(halfExtents.x, -halfExtents.y, -halfExtents.z);
 
             Origin = origin;
         }

         private void Rotate(Quaternion orientation)
         {
             LocalFrontTopLeft = RotatePointAroundPivot(LocalFrontTopLeft, Vector3.zero, orientation);
             LocalFrontTopRight = RotatePointAroundPivot(LocalFrontTopRight, Vector3.zero, orientation);
             LocalFrontBottomLeft = RotatePointAroundPivot(LocalFrontBottomLeft, Vector3.zero, orientation);
             LocalFrontBottomRight = RotatePointAroundPivot(LocalFrontBottomRight, Vector3.zero, orientation);
         }
     }
     private static Vector3 CastCenterOnCollision(Vector3 origin, Vector3 direction, float hitInfoDistance)
     {
         return origin + direction.normalized * hitInfoDistance;
     }
     
     private static Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Quaternion rotation)
     {
         Vector3 direction = point - pivot;
         return pivot + rotation * direction;
     }
}