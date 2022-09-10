using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR 
using UnityEditor;
#endif 

[Serializable]
public enum HitboxType {
	Hitbox,
	Hurtbox,
	Projectile,
	IFrame,
	
}

[Serializable]
public enum HitboxShape {
	Rectangle,
	Circle
}

public interface IShape {
	void DrawGizmo(Hitbox hitbox, Transform transform);

	Hitbox DrawSceneHandle(Hitbox hitbox, TargetComponents targetComponents);

	//Vector3 GetHandlePosition();
	//Vector3 GetHandleXScale();
	//Vector3 GetHandleYScale();
}

public class Rectangle : IShape {
	public void DrawGizmo(Hitbox hitbox, Transform transform) {
		Vector3 hitboxPos = new Vector3((hitbox.Boundaries.x + hitbox.Boundaries.width / 2) * transform.localScale.x,
														(hitbox.Boundaries.y + hitbox.Boundaries.height / 2) * transform.localScale.y,
														.01f);
		Vector3 hitboxSize = new Vector3(hitbox.Boundaries.width, hitbox.Boundaries.height, 0.1f);
		Gizmos.DrawCube(hitboxPos + transform.position, hitboxSize);
	}

	public Hitbox DrawSceneHandle(Hitbox hitbox, TargetComponents targetComponents) {
        /*
		if (Tools.current == Tool.Move) {
			return DrawMove(hitbox, targetComponents);
		} else if (Tools.current == Tool.Scale) {
			return DrawScale(hitbox, targetComponents);
		} else if (Tools.current == Tool.View) {
			return DrawBoth(hitbox, targetComponents);
		}
        */
		#if UNITY_EDITOR 
        if (Tools.current == Tool.View) {
			return DrawBoth(hitbox, targetComponents);
		}
		#endif 
		return hitbox;
	}

	private Hitbox DrawMove(Hitbox currentHitbox, TargetComponents targetComponents) {
		#if UNITY_EDITOR 
		Vector3 offset = new Vector3(currentHitbox.Boundaries.width / 2, currentHitbox.Boundaries.height / 2, 0);
		Vector3 handlePosition = currentHitbox.GetHandlePosition() + offset;
		Vector3 moveHandlePos = Handles.FreeMoveHandle(handlePosition + targetComponents.GameObject.transform.position, Quaternion.identity, 0.05f, new Vector3(.5F, .5F, .5F), Handles.RectangleHandleCap);
		moveHandlePos -= targetComponents.GameObject.transform.position;

		currentHitbox.Boundaries = new Rect(moveHandlePos.x - offset.x, moveHandlePos.y - offset.y,
												currentHitbox.Boundaries.width,
												currentHitbox.Boundaries.height);
		#endif 
		return currentHitbox;
	}
	private Hitbox DrawScale(Hitbox currentHitbox, TargetComponents targetComponents) {
		Vector3 ScaleHandleXPos;
		Vector3 ScaleHandleYPos;
		#if UNITY_EDITOR 
		ScaleHandleXPos = Handles.FreeMoveHandle(currentHitbox.GetHandleXScale() + targetComponents.GameObject.transform.position,
												 Quaternion.identity,
												 0.05f,
												 Vector3.one,
												 Handles.DotHandleCap);
		ScaleHandleXPos -= targetComponents.GameObject.transform.position;

		ScaleHandleYPos = Handles.FreeMoveHandle(currentHitbox.GetHandleYScale() + targetComponents.GameObject.transform.position,
												 Quaternion.identity,
												 0.05f,
												 Vector3.one,
												 Handles.DotHandleCap);
		ScaleHandleYPos -= targetComponents.GameObject.transform.position;

		currentHitbox.Boundaries = new Rect(currentHitbox.Boundaries.x, currentHitbox.Boundaries.y,
									  ScaleHandleXPos.x - currentHitbox.Boundaries.x,
									  ScaleHandleYPos.y - currentHitbox.Boundaries.y);
		#endif 
		return currentHitbox;
	}

	private Hitbox DrawBoth(Hitbox currentHitbox, TargetComponents targetComponents) {
		return DrawScale(DrawMove(currentHitbox, targetComponents), targetComponents);
	}
}

public class Circle : IShape {
	public void DrawGizmo(Hitbox hitbox, Transform transform) {
		Vector3 hitboxPos = new Vector3(hitbox.Position.x * transform.localScale.x,
													hitbox.Position.y * transform.localScale.y,
													0f);

		Gizmos.DrawSphere(hitboxPos + transform.position, hitbox.Boundaries.width);
	}

	public Hitbox DrawSceneHandle(Hitbox hitbox, TargetComponents targetComponents) {
		return hitbox;
	}
}

public class HitboxCollisionInfo
{
    public GameObject GameObject;

    public string CurrentAnimation;

    public override string ToString()
    {
        string info;

        info = "HitboxCollisionInfo" + Environment.NewLine;
        info += "GameObject Name: " + GameObject.name + Environment.NewLine;
        info += "CurrentAnimation: " + CurrentAnimation;

        return info;
    }
}

[Serializable]
public struct Hitbox {
	public HitboxType Type;
	public HitboxShape Shape;

	public Vector3 Position;
	public Rect Boundaries;

	public static Dictionary<HitboxShape, IShape> ShapeDictionary = new Dictionary<HitboxShape, IShape>
	{
		{ HitboxShape.Rectangle, new Rectangle() },
		{ HitboxShape.Circle, new Circle() }
	};

	public Hitbox(Rect boundaries, HitboxType hitboxType) {
		Type = hitboxType;
		Position = Vector3.zero;
		Boundaries = boundaries;

		//Always assume default as rectangle
		Shape = HitboxShape.Rectangle;
	}

	internal void DrawGizmo(Transform transform) {
		ShapeDictionary[Shape].DrawGizmo(this, transform);
	}

	public Vector3 GetHandlePosition() {
		if (Shape == HitboxShape.Rectangle) {
			return new Vector3(Boundaries.x, Boundaries.y, 0f);
		}
		//else
		//{
		//    return new Vector3(CircleX, CircleY, 0f);
		//}

		return Vector3.zero;
	}
	public Vector3 GetHandleXScale() {
		return new Vector3(Boundaries.x + Boundaries.width,
						   Boundaries.y + Boundaries.height / 2);
	}
	public Vector3 GetHandleYScale() {
		return new Vector3(Boundaries.x + Boundaries.width / 2,
						   Boundaries.y + Boundaries.height);
	}
}