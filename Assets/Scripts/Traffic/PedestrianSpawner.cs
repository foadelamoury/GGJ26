using UnityEngine;
using System.Collections.Generic;

public class PedestrianSpawner : MonoBehaviour
{
    [Header("Spawning")]
    public GameObject pedestrianPrefab;
    public int pedestrianCount = 5;
    public List<Waypoint> spawnWaypoints; // Assign in inspector

    private void Start()
    {
        SpawnPedestrians();
    }

    [Header("Spawning Settings")]
    public float minSpawnDistance = 5.0f; // Minimum distance between pedestrians

    private void SpawnPedestrians()
    {
        if (spawnWaypoints == null || spawnWaypoints.Count == 0) return;

        List<Vector3> spawnedPositions = new List<Vector3>();

        for (int i = 0; i < pedestrianCount; i++)
        {
            Waypoint spawnWp = null;
            Vector3 candidatePos = Vector3.zero;
            bool validPosition = false;
            int distinctRetries = 10;

            // Try to find a valid position away from others
            while (distinctRetries > 0 && !validPosition)
            {
                spawnWp = spawnWaypoints[Random.Range(0, spawnWaypoints.Count)];
                // Add random offset
                candidatePos = spawnWp.transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * 2f;
                
                validPosition = true;
                foreach (Vector3 pos in spawnedPositions)
                {
                    if (Vector3.Distance(candidatePos, pos) < minSpawnDistance)
                    {
                        validPosition = false;
                        break;
                    }
                }
                distinctRetries--;
            }

            if (!validPosition) continue; // Skip this spawn if we couldn't find a spot

            spawnedPositions.Add(candidatePos);

            // Pick random destination (different from spawn)
            Waypoint destWp = spawnWaypoints[Random.Range(0, spawnWaypoints.Count)];
            int retries = 5;
            while (destWp == spawnWp && retries > 0)
            {
                destWp = spawnWaypoints[Random.Range(0, spawnWaypoints.Count)];
                retries--;
            }

            GameObject ped = Instantiate(pedestrianPrefab, candidatePos, Quaternion.identity);
            
            PedestrianController controller = ped.GetComponent<PedestrianController>();
            if (controller != null)
            {
                controller.Initialize(destWp);
            }
        }
    }
}
