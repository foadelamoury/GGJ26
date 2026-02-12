using UnityEngine;
using UnityEngine.Events;

public class TaxiController : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private float currentMoney;
    [SerializeField] private bool isCarryingPassenger;

    private Movement movement;
    private PedestrianController currentPassenger;
    private Waypoint currentDestination;

    public Waypoint CurrentDestination => currentDestination;
    public bool IsCarryingPassenger => isCarryingPassenger;

    // Events for UI
    public UnityEvent<float> OnMoneyChanged;

    private void Awake()
    {
        movement = GetComponent<Movement>();
        currentMoney = 0f;
        
    }

    private void Update()
    {
        if (isCarryingPassenger && currentDestination != null)
        {
            CheckDropoff();
        }
    }

    public void ReceivePassenger(PedestrianController passenger, Waypoint destination)
    {
        if (isCarryingPassenger) return;

        isCarryingPassenger = true;
        currentPassenger = passenger;
        currentDestination = destination;

        // Visuals

        Debug.Log($"Picked up passenger! Destination: {destination.name}");
    }

    private void CheckDropoff()
    {
        float dropoffDistance = 3.0f; // Could be configurable
        if (Vector2.Distance(transform.position, currentDestination.transform.position) < dropoffDistance)
        {
            if (movement != null && movement.IsStopped)
            {
                CompleteRide();
            }
        }
    }

    private void CompleteRide()
    {
        float rideFare = 50f; 
        AddMoney(rideFare);

        if (currentPassenger != null)
        {
            currentPassenger.OnDropoff();
            currentPassenger = null;
        }

        isCarryingPassenger = false;
        currentDestination = null;
        
        Debug.Log("Ride Complete! +$" + rideFare);
    }

    public void AddMoney(float amount)
    {
        currentMoney += amount;
        OnMoneyChanged?.Invoke(currentMoney);
    }
}
