using UnityEditor;
using UnityEditor.EditorTools;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

public class CircleCollider : MonoBehaviour
{
    public float radius = 1f;
    public Vector2 center = Vector2.zero;

    public Vector2 MaxPosition()
    {
        return (Vector2)transform.position + center + new Vector2(radius, radius);
    }

    public Vector2 MinPosition()
    {
        return (Vector2)transform.position + center - new Vector2(radius, radius);
    }

    public Vector2 WorldPosition()
    {
        return (Vector2)transform.position + center;
    }

    public Rect SquareBounds()
    {
        return new Rect(MinPosition(), new Vector2(radius * 2, radius * 2));
    }

    /// <summary>
    /// Checks if this collides with another circle collider
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool CollidesWith(CircleCollider other)
    {
        float sqrDistance = Vector2.SqrMagnitude(WorldPosition() - other.WorldPosition());
        float sqrRadSum = Mathf.Pow(radius + other.radius, 2);

        return sqrDistance < sqrRadSum;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(gameObject.transform.position + (Vector3)center, radius);
    }
}

#if UNITY_EDITOR
[EditorTool("Circle Collider Edit", typeof(CircleCollider)), CanEditMultipleObjects]
public class CircleColliderEditor : EditorTool
{
    public override GUIContent toolbarIcon => EditorGUIUtility.IconContent("d_EditCollider");

    private SphereBoundsHandle boundsHandle = new SphereBoundsHandle();
    public override void OnToolGUI(EditorWindow window)
    {
        if (!(window is SceneView)) return;

        CircleCollider collider = (CircleCollider)target;
        boundsHandle.radius = collider.radius;
        boundsHandle.center = collider.center;

        EditorGUI.BeginChangeCheck();
        Handles.color = Color.green;

        Matrix4x4 matrix = Matrix4x4.TRS(collider.transform.position, collider.transform.rotation, Vector3.one);
        using (new Handles.DrawingScope(matrix))
        {
            boundsHandle.DrawHandle();
        }

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(collider, "Change Circle Collider Radius");
            collider.radius = boundsHandle.radius;
            collider.center = boundsHandle.center;
        }
    }
}
#endif