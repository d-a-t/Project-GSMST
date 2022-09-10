using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class BruteForceStrategy : CollisionStrategy
{
    List<FrameData> collisionList;

    public BruteForceStrategy()
    {
        collisionList = new List<FrameData>();
    }

    public override void CheckForCollisions(IEnumerable<FrameData> hitboxList)
    {
        ResetCollisionList();

        foreach (FrameData FrameData in hitboxList)
        {
            if (!FrameData.IsActive || !FrameData.enabled)
                continue;

            Hitbox[] currentHitboxes = FrameData.CurrentHitboxes;

            for (int i = 0; i < currentHitboxes.Length; i++)
            {
                if (currentHitboxes[i].Type == HitboxType.Projectile
                 || currentHitboxes[i].Type == HitboxType.Hitbox)
                {
                    foreach (FrameData alphaTemp in hitboxList)
                    {
                        if (!alphaTemp.enabled)
                            continue;

                        if (alphaTemp.gameObject != FrameData.gameObject)
                        {
                            if (CollisionDetection.CollidesWithlist(currentHitboxes[i], FrameData.gameObject,
                                alphaTemp.AnimationClips[alphaTemp.AnimationClipsIndex].keyframes[alphaTemp.KeyframesIndex].hitboxes, alphaTemp.gameObject))
                            {
                                if (!collisionList.Contains(alphaTemp))
                                {
                                    Debug.Log("Hi");

                                    collisionList.Add(alphaTemp);
                                    alphaTemp.MarkedForCollision = true;
                                    FrameDataController.Singleton.SendCollisionMessage(alphaTemp, FrameData, FrameDataController.COLLISION_EVENT_ENTER);
                                }
                                else
                                {
                                    Debug.Log("Hey");

                                    alphaTemp.MarkedForCollision = true;
                                    FrameDataController.Singleton.SendCollisionMessage(alphaTemp, FrameData, FrameDataController.COLLISION_EVENT_STAY);
                                }
                            }
                        }
                    }
                }
            }
        }

        for (int i = collisionList.Count - 1; i >= 0; i--)
        {
            if (collisionList[i].MarkedForCollision == false)
            {
                collisionList.RemoveAt(i);
            }
        }
    }

    private void ResetCollisionList()
    {
        foreach (FrameData FrameData in collisionList)
        {
            FrameData.MarkedForCollision = false;
        }
    }
}