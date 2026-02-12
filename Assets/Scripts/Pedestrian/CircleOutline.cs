using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleOutline : MonoBehaviour
{
    public int steps = 100; // Number of segments to form the circle
    public float radius = 1.0f; // Radius of the circle
    private LineRenderer lineRenderer;

    [Range(0f, 1f)] public float progress = 1.0f; // 0 to 1
    

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer != null)
        {
            lineRenderer.useWorldSpace = false;
        }
    }

    void OnValidate()
    {
        if(lineRenderer == null) lineRenderer = GetComponent<LineRenderer>();
        DrawCircle();
    }

    void Start()
    {
        DrawCircle();
    }

    public void SetProgress(float p)
    {
        progress = Mathf.Clamp01(p);
        DrawCircle();
    }
    
    public void SetColor(Color color)
    {
        if (lineRenderer != null)
        {
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
    }

    void DrawCircle()
    {
        if (lineRenderer == null) return;

        int currentSteps = Mathf.CeilToInt(steps * progress);
        lineRenderer.positionCount = currentSteps + 1; // +1 if we want to connect to start, but for progress we might just stop

        // If progress is 1, we want a closed loop. If less, open arc.
        // Actually, logic: draw 'progress' portion of 2PI.
        
        // However, LineRenderer loops if 'loop' is true. We should control this.
        lineRenderer.loop = (progress >= 0.99f); 

        for (int i = 0; i <= currentSteps; i++)
        {
            float rate = (float)i / steps;
            if (rate > progress) break; // Should be handled by loop count mostly

            float currentRadian = rate * 2 * Mathf.PI;

            float x = Mathf.Cos(currentRadian) * radius;
            float y = Mathf.Sin(currentRadian) * radius;
            float z = 0; // For 2D

            lineRenderer.SetPosition(i, new Vector3(x, y, z));
        }
    }
}
