using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [Header("Waypoint Settings")]
    [Tooltip("The next possible waypoint(s) a vehicle can go to.")]
    public List<Waypoint> nextWaypoints;
    [Range(0f, 5f)]
    public float roadWidth = 1f;

    [Header("Debug")]
    public Color debugColor = Color.yellow;

    /// <summary>
    /// Gets a random next waypoint if there are branches.
    /// </summary>
    public Waypoint GetNextWaypoint()
    {
        if (nextWaypoints == null || nextWaypoints.Count == 0)
            return null;

        return nextWaypoints[Random.Range(0, nextWaypoints.Count)];
    }

    /// <summary>
    /// Returns a position on the road with some random width offset.
    /// </summary>
    public Vector3 GetPosition()
    {
        Vector3 minBound = transform.position + transform.right * roadWidth / 2f;
        Vector3 maxBound = transform.position - transform.right * roadWidth / 2f;

        return Vector3.Lerp(minBound, maxBound, Random.Range(0f, 1f));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = debugColor;
        Gizmos.DrawWireSphere(transform.position, 0.3f);

        if (nextWaypoints != null)
        {
            foreach (Waypoint next in nextWaypoints)
            {
                if (next != null)
                {
                    Gizmos.color = Color.cyan;
                    Gizmos.DrawLine(transform.position, next.transform.position);
                }
            }
        }
        
        // Draw width
        Gizmos.color = new Color(1, 0, 0, 0.3f);
        Gizmos.DrawLine(transform.position + transform.right * roadWidth / 2f, transform.position - transform.right * roadWidth / 2f);
    }
}
