using UnityEngine;

public class SpawnManager : MonoBehaviour

{
    [Header("Cars and Pedestrians")]
    [SerializeField] private GameObject taxiPrefab, pedestrianPrefab;

    public Transform[] spawnPoints;

   



    private void Awake()
    {
        Instantiate(taxiPrefab, spawnPoints[0].position, Quaternion.identity);
    }








}

