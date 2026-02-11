using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public List<GameObject> carPrefabs;
    public Waypoint startWaypoint;
    public float spawnInterval = 3f;
    public float spawnRadius = 2f; // Check radius to ensure spawn point is clear

    [Header("Safety")]
    public LayerMask obstacleLayer;

    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            TrySpawnCar();
            timer = 0f;
        }
    }

    private List<GameObject> carPool = new List<GameObject>();
    public int poolSize = 10;

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject prefab = carPrefabs[Random.Range(0, carPrefabs.Count)];
            GameObject car = Instantiate(prefab, transform.position, transform.rotation);
            car.SetActive(false);
            car.transform.SetParent(transform);
            carPool.Add(car);
        }
    }

    private GameObject GetPooledCar()
    {
        foreach (GameObject car in carPool)
        {
            if (!car.activeInHierarchy)
            {
                return car;
            }
        }
        
        // Optional: Expand pool if needed
        GameObject prefab = carPrefabs[Random.Range(0, carPrefabs.Count)];
        GameObject newCar = Instantiate(prefab, transform.position, transform.rotation);
        newCar.SetActive(false);
        newCar.transform.SetParent(transform);
        carPool.Add(newCar);
        return newCar;
    }

    private void TrySpawnCar()
    {
        if (carPrefabs == null || carPrefabs.Count == 0) return;
        if (startWaypoint == null) return;

        // Check if spawn area is clear
        Collider2D hit = Physics2D.OverlapCircle(transform.position, spawnRadius, obstacleLayer);
        if (hit != null)
        {
            return;
        }

        // Get car from pool
        GameObject car = GetPooledCar();
        if (car == null) return;

        car.transform.position = transform.position;
        car.transform.rotation = transform.rotation;
        car.SetActive(true);

        // Setup AI
        TrafficAIController ai = car.GetComponent<TrafficAIController>();
        if (ai == null)
        {
            ai = car.AddComponent<TrafficAIController>();
        }

        ai.currentWaypoint = startWaypoint;
        // Optionally pass obstacle layer if not set on prefab
        if (ai.obstacleLayer == 0)
        {
            ai.obstacleLayer = obstacleLayer;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
}
