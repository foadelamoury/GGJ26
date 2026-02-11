using UnityEngine;

[RequireComponent(typeof(Movement))]
public class TrafficAIController : MonoBehaviour
{
    [Header("Waypoint Following")]
    public Waypoint currentWaypoint;
    public float stoppingDistance = 1.0f;

    [Header("Obstacle Avoidance")]
    [Tooltip("LayerMask for objects the car should avoid (e.g., other cars, player, walls).")]
    public LayerMask obstacleLayer;
    public float detectionDistance = 5.0f;
    public float detectionRadius = 0.5f;

    private Movement movement;

    private void Awake()
    {
        movement = GetComponent<Movement>();
    }

    private void OnEnable()
    {
        if (movement != null)
        {
            movement.StopVehicle();
        }
    }

    private void Update()
    {
        if (currentWaypoint == null) return;

        // 1. Calculate steering to target
        Vector2 targetPosition = currentWaypoint.transform.position;
        Vector2 directionToTarget = (targetPosition - (Vector2)transform.position).normalized;
        float distanceToTarget = Vector2.Distance(transform.position, targetPosition);

        // Calculate angle to target relative to forward direction
        // SignedAngle returns angle in degrees between -180 and 180
        // Forward is transform.up
        float angle = Vector2.SignedAngle(transform.up, directionToTarget);

        float steeringInput = 0f;
        // If target is to the left (positive angle), steer left (negative input)
        if (angle > 5f) 
        {
            steeringInput = -1f; 
        }
        // If target is to the right (negative angle), steer right (positive input)
        else if (angle < -5f) 
        {
            steeringInput = 1f; 
        }

        // 2. Obstacle Detection
        float throttleInput = 1f;
        
        // Cast a circle forward to detect obstacles
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, detectionRadius, transform.up, detectionDistance, obstacleLayer);
        
        RaycastHit2D hit = new RaycastHit2D();
        bool foundObstacle = false;
        float minDistance = float.MaxValue;

        foreach (var h in hits)
        {
            if (h.collider != null && h.collider.gameObject != gameObject)
            {
                if (h.distance < minDistance)
                {
                    hit = h;
                    minDistance = h.distance;
                    foundObstacle = true;
                }
            }
        }

        if (foundObstacle)
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
            // Obstacle detected!
            if (hit.distance < 2f)
            {
                // Too close, reverse/brake hard
                throttleInput = -1f; 
            }
            else
            {
                // Slow down
                throttleInput = 0f; 
            }
        }
        else
        {
            Debug.DrawRay(transform.position, transform.up * detectionDistance, Color.green);
        }

        // 3. Send Input to Movement
        movement.Move(new Vector2(steeringInput, throttleInput));

        // 4. Check Waypoint Reached
        if (distanceToTarget < stoppingDistance)
        {
            Waypoint next = currentWaypoint.GetNextWaypoint();
            if (next == null)
            {
                // No more waypoints, disable the car to return it to the pool
                gameObject.SetActive(false);
                return;
            }
            currentWaypoint = next;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + transform.up * detectionDistance, detectionRadius);
    }
}
