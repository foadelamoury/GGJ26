using UnityEngine;
using UnityEngine.Events;

public class PedestrianController : MonoBehaviour
{
    [Header("Settings")]
    public float hailDistance = 3.0f;
    public float enterBoardingSpeed = 2.0f; // Time to fill the circle
    public Color normalOuterColor = Color.white;
    public Color fillingColor = Color.green;

    [Header("Visuals")]
    [SerializeField] private CircleOutline circleOutline; 

    private bool isWaiting = true;
    private float boardingTimer = 0f;
    private Vehicle vehicleInRange; // Using Vehicle facade
    private Waypoint destination;

    private void Start()
    {
        if (circleOutline != null)
        {
            circleOutline.SetColor(normalOuterColor);
            circleOutline.SetProgress(0f);
        }
    }

    public void Initialize(Waypoint dest)
    {
        destination = dest;
    }

    private void Update()
    {
        if (isWaiting && vehicleInRange != null)
        {
            ProcessBoarding();
        }
        else
        {
            if (boardingTimer > 0)
            {
                boardingTimer -= Time.deltaTime;
                if (boardingTimer < 0) boardingTimer = 0;
                UpdateFillVisual(boardingTimer / enterBoardingSpeed);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Pedestrian Hit Trigger: {other.gameObject.name} (Layer: {LayerMask.LayerToName(other.gameObject.layer)})");

        // Check for Vehicle facade
        Vehicle vehicle = other.GetComponent<Vehicle>();
        if (vehicle != null)
        {
            Debug.Log("Vehicle Component Found!");
            vehicleInRange = vehicle;
        }
        // Fallback checks
        else if (other.CompareTag("Player"))
        {
             Debug.Log("Player Tag Found (Fallback)");
             vehicleInRange = other.GetComponent<Vehicle>();
        }
        
        if (vehicleInRange == null)
        {
             // Try getting from parent in case collider is on a child
             vehicleInRange = other.GetComponentInParent<Vehicle>();
             if (vehicleInRange != null) Debug.Log("Vehicle Found in Parent!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Vehicle vehicle = other.GetComponent<Vehicle>();
        if (vehicle != null && vehicle == vehicleInRange)
        {
            vehicleInRange = null;
        }
    }

    private void ProcessBoarding()
    {
        if (vehicleInRange == null) return;
        if (vehicleInRange.Taxi == null) return; // Not a taxi capable vehicle

        // Check if stopped
        // Access Movement via Vehicle Facade? Vehicle has private movement field but exposed 'Taxi'?
        // We might need to expose Movement or Speed on Vehicle, or Taxi.
        // Assuming TaxiController doesn't have speed. Vehicle has it?
        // Let's check Vehicle.cs again. It doesn't expose Movement publicly. 
        // But Movement.cs has public IsStopped?
        // We will assume GetComponent<Movement>() works on the vehicle object for now, or add property to Vehicle.
        
        Movement movement = vehicleInRange.GetComponent<Movement>(); // Should be there
        
        if (movement != null && movement.IsStopped)
        {
             boardingTimer += Time.deltaTime;
             UpdateFillVisual(boardingTimer / enterBoardingSpeed);

             if (boardingTimer >= enterBoardingSpeed)
             {
                 EnterTaxi();
             }
        }
        else
        {
            boardingTimer -= Time.deltaTime;
            if (boardingTimer < 0) boardingTimer = 0;
            UpdateFillVisual(boardingTimer / enterBoardingSpeed);
        }
    }

    private void UpdateFillVisual(float percentage)
    {
        if (circleOutline != null)
        {
             circleOutline.SetProgress(percentage);
             circleOutline.SetColor(Color.Lerp(normalOuterColor, fillingColor, percentage));
        }
    }

    private void EnterTaxi()
    {
        isWaiting = false;
        if (vehicleInRange != null && vehicleInRange.Taxi != null)
        {
            vehicleInRange.Taxi.ReceivePassenger(this, destination);
            
            // Hide visual
            // Depending on architecture, maybe just disable renderer or collider?
            // Disabling gameobject stops scripts.
            // Move to Taxi
            transform.SetParent(vehicleInRange.transform); 
            transform.localPosition = Vector3.zero;
            gameObject.SetActive(false); // Disable self
            
            Debug.Log("Pedestrian entered taxi.");
        }
    }

    public void OnDropoff()
    {
        transform.SetParent(null); // Detach
        gameObject.SetActive(true);
        // Maybe walk away or fade out
        // Just destroying for now or disabling to simulating leaving
        Debug.Log("Pedestrian dropped off. Goodbye!");
        Destroy(gameObject, 1f); // Disappear after a bit
    }
}
