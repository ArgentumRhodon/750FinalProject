using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

// Code adapted from https://badecho.com/index.php/2023/01/14/fast-simple-quadtree/
public class Quadrant
{
    private readonly List<CircleCollider> elements = new List<CircleCollider>();
    private readonly int bucketCap;
    private readonly int maxDepth;

    private Quadrant topRight;
    private Quadrant topLeft;
    private Quadrant bottomRight;
    private Quadrant bottomLeft;

    public Rect Bounds { get; private set; }
    public bool IsLeaf { get; private set; }
    public int Level { get; private set; }

    public Quadrant(Rect bounds) :
        this(bounds, 0, 2, 5)
    {}

    public Quadrant(Rect bounds, int level, int bucketCap, int maxDepth)
    {
        this.bucketCap = bucketCap;
        this.maxDepth = maxDepth;
        Level = level;
        Bounds = bounds;
    }

    public void Insert(CircleCollider element)
    {
        Assert.IsNotNull(element, nameof(element));
        if (!Bounds.Overlaps(element.SquareBounds()))
        {
            throw new ArgumentException("Element Outside QuadTree Bounds", nameof(element));
        }

        if(elements.Count > bucketCap)
        {
            Split();
        }

        // If element would fit into child node, insert there instead
        Quadrant containingChild = GetContainingChild(element.SquareBounds());
        if(containingChild != null)
        {
            containingChild.Insert(element);
        }
        else
        {
            elements.Add(element);
        }
    }

    private void Split()
    {
        if (!IsLeaf || Level + 1 > maxDepth) return;

        topRight = CreateChild(Bounds.position);
        topLeft = CreateChild(new Vector2(Bounds.position.x - Bounds.width / 2, Bounds.position.y));
        bottomRight = CreateChild(new Vector2(Bounds.position.x, Bounds.position.y - Bounds.height / 2));
        bottomLeft = CreateChild(new Vector2(Bounds.position.x - Bounds.width / 2, Bounds.position.y - Bounds.height / 2));

        List<CircleCollider> elements = this.elements.ToList();

        foreach(CircleCollider element in elements)
        {
            Quadrant containingChild = GetContainingChild(element.SquareBounds());
            if(containingChild != null)
            {
                this.elements.Remove(element);
                containingChild.Insert(element);
            }
        }
    }

    public bool Remove(CircleCollider element)
    {
        Assert.IsNotNull(element, nameof(element));
        Quadrant containingChild = GetContainingChild(element.SquareBounds());

        bool removed = containingChild?.Remove(element) ?? elements.Remove(element);

        if(removed && CountElements() < bucketCap)
        {
            Merge();
        }

        return removed;
    }

    public int CountElements()
    {
        int count = elements.Count;

        if (!IsLeaf)
        {
            count += topLeft.CountElements();
            count += bottomLeft.CountElements();
            count += topRight.CountElements();
            count += bottomRight.CountElements();
        }

        return count;
    }

    private void Merge()
    {
        if (IsLeaf) return;

        elements.AddRange(topLeft.elements);
        elements.AddRange(topRight.elements);
        elements.AddRange(bottomLeft.elements);
        elements.AddRange(bottomRight.elements);

        topLeft = topRight = bottomRight = bottomLeft = null;
    }

    public List<CircleCollider> FindCollisions(CircleCollider element)
    {
        Assert.IsNotNull(element, nameof(element));
        Queue<Quadrant> nodes = new Queue<Quadrant>();
        List<CircleCollider> collisions = new List<CircleCollider>();

        nodes.Enqueue(this);

        while(nodes.Count > 0)
        {
            Quadrant node = nodes.Dequeue();

            if (!element.SquareBounds().Overlaps(node.Bounds)) continue;

            collisions.AddRange(node.elements.Where(e => e.GetComponent<CircleCollider>().SquareBounds().Overlaps(element.SquareBounds())));

            if (!node.IsLeaf)
            {
                if (element.SquareBounds().Overlaps(node.topRight.Bounds))
                {
                    nodes.Enqueue(node.topRight);
                }
                if (element.SquareBounds().Overlaps(node.topLeft.Bounds))
                {
                    nodes.Enqueue(node.topLeft);
                }
                if (element.SquareBounds().Overlaps(node.bottomRight.Bounds))
                {
                    nodes.Enqueue(node.bottomRight);
                }
                if (element.SquareBounds().Overlaps(node.bottomLeft.Bounds))
                {
                    nodes.Enqueue(node.bottomLeft);
                }
            }
        }

        return collisions;
    }

    private Quadrant CreateChild(Vector2 position)
    {
        Rect childBounds = new Rect(position, Bounds.size / 2);
        return new Quadrant(childBounds, Level + 1, bucketCap, maxDepth);
    }

    public List<CircleCollider> GetElements()
    {
        List<CircleCollider> children = new List<CircleCollider>();
        Queue<Quadrant> nodes = new Queue<Quadrant>();

        nodes.Enqueue(this);

        while(nodes.Count > 0)
        {
            var node = nodes.Dequeue();

            if (!node.IsLeaf)
            {
                nodes.Enqueue(node.topLeft);
                nodes.Enqueue(node.topRight);
                nodes.Enqueue(node.bottomLeft);
                nodes.Enqueue(node.bottomRight);
            }

            children.AddRange(node.elements);
        }

        return children;
    }

    private Quadrant GetContainingChild(Rect bounds)
    {
        if (IsLeaf) return null;

        if (topLeft.Bounds.Overlaps(bounds)) return topLeft;
        if (topRight.Bounds.Overlaps(bounds)) return topLeft;
        if (bottomLeft.Bounds.Overlaps(bounds)) return topLeft;

        return bottomRight.Bounds.Overlaps(bounds) ? bottomRight : null;
    }
}
