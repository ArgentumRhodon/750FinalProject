using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileCollisionResponse : CollisionResponse
{
    public override void OnCollision(GameObject other)
    {
        if(other.tag == "Enemy" || other.tag == "ProjDel")
        {
            Destroy(gameObject);
        }
    }
}
