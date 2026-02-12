using UnityEngine;
[RequireComponent(typeof(InputHandler), typeof(Movement), typeof(TheCollider))]
public class Vehicle : MonoBehaviour
{


    [SerializeField]
    [Tooltip("LayerMask to identify obstacles in the game environment.")]
    LayerMask m_ObstacleLayer;


    [SerializeField] Movement movement;
    [SerializeField] InputHandler playerInput;
    [SerializeField] TheCollider theCollider;
    [SerializeField] TaxiController taxiController;

    public TaxiController Taxi => taxiController;
    public TheCollider Collider => theCollider;

    void Awake()
    {
        Initialize();

    }

    private void Initialize()
    {
        if (!movement) movement = GetComponent<Movement>();
        if (!playerInput) playerInput = GetComponent<InputHandler>();
        if (!theCollider) theCollider = GetComponent<TheCollider>();
        if (!taxiController) taxiController = GetComponent<TaxiController>();


    }
    private void LateUpdate()
    {
        Vector3 inputVector = playerInput.MovementInput;
        movement.Move(inputVector);


    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        // Simple impact logic - if speed is high enough, take damage
        if (movement.CurrentSpeed > 2.0f && collision.gameObject.layer != LayerMask.NameToLayer("Pedestrian"))
        {
             theCollider.TakeDamage(5f); // Arbitrary damage for now
        }
    }




}
