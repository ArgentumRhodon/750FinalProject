using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CollisionResponse : MonoBehaviour
{
    public abstract void OnCollision(GameObject other);
}
