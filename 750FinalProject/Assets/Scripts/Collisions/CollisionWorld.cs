using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public struct Collision
{
    public GameObject obj_1; // Usually the enemy
    public GameObject obj_2; // Usually the projectile
    public bool resolved;

    public Collision(GameObject obj_1, GameObject obj_2, bool resolved = false)
    {
        this.obj_1 = obj_1; this.obj_2 = obj_2; this.resolved = resolved;
    }
}

public class CollisionWorld : MonoBehaviour
{
    public List<CircleCollider> colliders;
    public List<Collision> collisions;

    private Quadrant quadTree;

    // Start is called before the first frame update
    void Start()
    {
        colliders = new List<CircleCollider>(FindObjectsOfType<CircleCollider>());
        collisions = new List<Collision>();
    }

    // Update is called once per frame
    void Update()
    {
        colliders = new List<CircleCollider>(FindObjectsOfType<CircleCollider>());
        collisions.Clear();

        BruteForce();
        ResolveCollisions();

        //quadTree = new Quadrant(CalculateRootBounds());
        //InitializeQuadTree();
        //QuadTreeCollision();
        //ResolveCollisions();
    }

    private Rect CalculateRootBounds()
    {
        Vector2 min = Vector2.zero;
        Vector2 max = Vector2.zero;

        foreach (var collider in colliders)
        {
            if(min.x > collider.MinPosition().x) min.x = collider.MinPosition().x;
            if(max.x < collider.MaxPosition().x) max.x = collider.MaxPosition().x;
            if(min.y > collider.MinPosition().y) min.y = collider.MinPosition().y;
            if(max.y < collider.MaxPosition().y) max.y = collider.MaxPosition().y;
        }

        Vector2 center = (min + max) / 2;
        Vector2 extent = max - min;

        float longestSide = Mathf.Max(extent.x, extent.y);
        Vector2 size = new Vector2(longestSide, longestSide);

        return new Rect(center - (size/2), size);
    }

    private void InitializeQuadTree()
    {
        foreach(CircleCollider element in colliders)
        {
            quadTree.Insert(element);
        }
    }

    private void QuadTreeCollision()
    {
        foreach (CircleCollider element in colliders)
        {
            quadTree.Remove(element);

            List<CircleCollider> collidingObjects = quadTree.FindCollisions(element);

            foreach(CircleCollider collider in collidingObjects)
            {
                collisions.Add(new Collision(element.gameObject, collider.gameObject));
            }

            quadTree.Insert(element);
        }
    }

    private void ResolveCollisions()
    {
        for(int i = 0; i < collisions.Count; i++)
        {
            Collision col = collisions[i];
            col.resolved = true;

            try
            {
                col.obj_1.GetComponent<CollisionResponse>().OnCollision(col.obj_2);
                col.obj_2.GetComponent<CollisionResponse>().OnCollision(col.obj_1);
            } catch(System.Exception e)
            {
                // Debug.Log(e.ToString());
            }
        }
    }

    private void BruteForce()
    {
        for(int i = 0; i < colliders.Count; i++)
        {
            for(int j = i + 1; j < colliders.Count; j++)
            {
                CircleCollider obj_1 = colliders[i];
                CircleCollider obj_2 = colliders[j];

                if (obj_1.CollidesWith(obj_2))
                {
                   collisions.Add(new Collision(obj_1.gameObject, obj_2.gameObject));
                }
            }
        }
    }
}
