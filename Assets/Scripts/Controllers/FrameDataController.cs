using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameDataController : Singleton {
	public static FrameDataController Singleton;

    [SerializeField]
    private List<FrameData> hitboxList;

    public const string COLLISION_EVENT_ENTER = "OnHitboxCollisionEnter";
    public const string COLLISION_EVENT_STAY = "OnHitboxCollisionStay";
    public const string COLLISION_EVENT_EXIT = "OnHitboxCollisionExit";

    private CollisionSolver solver;

    void Awake()
    {
        if (hitboxList == null)
        {
            hitboxList = new List<FrameData>();
        }

        solver = new CollisionSolver(new BruteForceStrategy());

		if (Singleton == null) {
			Singleton = this;
		}
    }

    public void AddAlphaHitbox(FrameData alphaHitbox)
    {
        //TODO check this projectile thing
        //ProjectileController pController = alphaHitbox.GetComponent<ProjectileController>();
        //if (pController != null)
        //{
        //    alphaHitbox.IsProjectile = true;
        //    alphaHitbox.ProjectileController = pController;
        //}

        hitboxList.Add(alphaHitbox);
    }

    void Update()
    {
        solver.SolveCollisions(hitboxList);
    }

    public void SendCollisionMessage(FrameData receiver, FrameData collider, string message)
    {
        HitboxCollisionInfo collisionInfo = new HitboxCollisionInfo();
        collisionInfo.GameObject = collider.gameObject;
        collisionInfo.CurrentAnimation = receiver.AnimationClips[receiver.AnimationClipsIndex].Name;

		HitboxCollisionInfo otherInfo = new HitboxCollisionInfo();
		otherInfo.GameObject = receiver.gameObject;
		otherInfo.CurrentAnimation = collider.AnimationClips[collider.AnimationClipsIndex].Name;

		receiver.gameObject.SendMessage(message, collisionInfo, SendMessageOptions.DontRequireReceiver);
		collider.gameObject.SendMessage(message, otherInfo, SendMessageOptions.DontRequireReceiver);

		if (message == COLLISION_EVENT_ENTER) {
			Debug.Log(receiver.AnimationClips[receiver.AnimationClipsIndex].Name);
			receiver.AnimationClips[receiver.AnimationClipsIndex].OnHitboxCollisionEnter.Invoke(collisionInfo);
            collider.AnimationClips[collider.AnimationClipsIndex].OnHitboxCollisionEnter.Invoke(otherInfo);
		} else if (message == COLLISION_EVENT_STAY) {
            receiver.AnimationClips[receiver.AnimationClipsIndex].OnHitboxCollisionStay.Invoke(collisionInfo);
            collider.AnimationClips[collider.AnimationClipsIndex].OnHitboxCollisionStay.Invoke(otherInfo);
        } else {
            receiver.AnimationClips[receiver.AnimationClipsIndex].OnHitboxCollisionExit.Invoke(collisionInfo);
            collider.AnimationClips[collider.AnimationClipsIndex].OnHitboxCollisionExit.Invoke(otherInfo);
        }
	}
}