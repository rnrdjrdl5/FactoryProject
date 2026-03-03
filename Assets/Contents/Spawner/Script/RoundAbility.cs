using UnityEngine;

public class RoundAbility : Ability
{
    [SerializeField] float circleRadius = 1f;
    [SerializeField] bool useLocalSpace = false;

    public Vector3 GetRandomPoint()
    {
        Vector2 localPoint2D = Random.insideUnitCircle * Mathf.Max(0f, circleRadius);
        Vector3 localPoint = new Vector3(localPoint2D.x, localPoint2D.y, 0f);

        if (useLocalSpace)
        {
            return transform.TransformPoint(localPoint);
        }

        return localPoint;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Vector3 center = useLocalSpace ? transform.TransformPoint(new Vector3(0f, 0f, 0f)) : new Vector3(0f, 0f, 0f);
        DrawCircleGizmo(center, circleRadius, 32);
    }

    void DrawCircleGizmo(Vector3 center, float radius, int segments)
    {
        if (segments < 3 || radius <= 0f)
        {
            return;
        }

        float angleStep = Mathf.PI * 2f / segments;
        Vector3 previous = center + new Vector3(radius, 0f, 0f);

        for (int i = 1; i <= segments; i++)
        {
            float angle = angleStep * i;
            Vector3 current = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0f);
            Gizmos.DrawLine(previous, current);
            previous = current;
        }
    }
}
