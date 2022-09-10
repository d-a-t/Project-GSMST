using UnityEngine;
using System.Collections;

public static class CollisionDetection
{
    public static bool CollidesWithlist(Hitbox hitbox, GameObject gameObject1, Hitbox[] hitboxes, GameObject gameObject2)
    {
        bool collisionHappened = false;

        for (int i = 0; i < hitboxes.Length; i++)
        {
            collisionHappened = CollidesWithHitbox(hitbox, gameObject1, hitboxes[i], gameObject2);

			if (collisionHappened)
                return true;
        }

        return collisionHappened;
    }

    private static bool CollidesWithHitbox(Hitbox hitbox1, GameObject gameObject1, Hitbox hitbox2, GameObject gameObject2)
    {
        if (hitbox2.Type != HitboxType.Hurtbox)
            return false;

        if (hitbox1.Shape == HitboxShape.Rectangle && hitbox2.Shape == HitboxShape.Rectangle)
        {
            Rect rect1 = new Rect(gameObject1.transform.position.x + hitbox1.Boundaries.x * gameObject1.transform.localScale.x,
                                  gameObject1.transform.position.y + hitbox1.Boundaries.y * gameObject1.transform.localScale.y,
                                  hitbox1.Boundaries.width * gameObject1.transform.localScale.x, hitbox1.Boundaries.height * gameObject1.transform.localScale.y);

//Debug.DrawLine(new Vector3(rect1.x, rect1.y), new Vector3(rect1.x + rect1.width, rect1.y ),Color.green);
         Debug.DrawLine(new Vector3(rect1.x, rect1.y), new Vector3(rect1.x , rect1.y + rect1.height), Color.red);
         Debug.DrawLine(new Vector3(rect1.x + rect1.width, rect1.y + rect1.height), new Vector3(rect1.x + rect1.width, rect1.y), Color.green);
         Debug.DrawLine(new Vector3(rect1.x + rect1.width, rect1.y + rect1.height), new Vector3(rect1.x, rect1.y + rect1.height), Color.red);

            Rect rect2 = new Rect(gameObject2.transform.position.x + hitbox2.Boundaries.x * gameObject2.transform.localScale.x,
                                  gameObject2.transform.position.y + hitbox2.Boundaries.y * gameObject2.transform.localScale.y,
                                  hitbox2.Boundaries.width * gameObject2.transform.localScale.x, hitbox2.Boundaries.height * gameObject2.transform.localScale.y);

            return rect1.Overlaps(rect2, true);
        }
        else
            return false;

        //else if (hitbox1.Shape == HitboxShape.Circle && hitbox2.Shape == HitboxShape.Circle)
        //{
        //    return CircleCollision(hitbox1, hitbox2);
        //}
        //else
        //{
        //    if (hitbox1.Shape == HitboxShape.Rectangle)
        //        return MixCollision(hitbox1, hitbox2);
        //    else
        //        return MixCollision(hitbox2, hitbox1);
        //}
    }

    private static bool MixCollision(Hitbox rectangle, Hitbox circle)
    {
        //TODO implement this
        return false;
    }
}
