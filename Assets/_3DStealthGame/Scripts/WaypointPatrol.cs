using UnityEngine;

public class WaypointPatrol : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 1f;
    public float rotationSpeed = 5f;
    public float reachDistance = 0.1f;

    private int currentWaypointIndex = 0;
    private float waitCounter = 0f;

    void Update()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        // pausecounter 
        if (waitCounter > 0f)
        {
            waitCounter -= Time.deltaTime;
            return;
        }

        Transform target = waypoints[currentWaypointIndex];

        // rotation
        Vector3 direction = (target.position - transform.position).normalized;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );
        }

        // Moving
        transform.position = Vector3.MoveTowards(
            transform.position,
            target.position,
            moveSpeed * Time.deltaTime
        );

        // Check if distance is close enough before switching
        if ((transform.position - target.position).sqrMagnitude < reachDistance * reachDistance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            waitCounter = Random.Range(1f, 3f);  // Adds random wait at each waypoint between 1 to 3 seconds
        }
    }
}
