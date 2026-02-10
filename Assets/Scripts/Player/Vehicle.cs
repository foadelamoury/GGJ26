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





    void Awake()
    {
        Initialize();

    }

    private void Initialize()
    {
        if (!movement) movement = GetComponent<Movement>();
        if (!playerInput) playerInput = GetComponent<InputHandler>();
        if (!theCollider) theCollider = GetComponent<TheCollider>();


    }
    private void LateUpdate()
    {
        Vector3 inputVector = playerInput.MovementInput;
        movement.Move(inputVector);


    }


    //void OnCollisionEnter2D(Collision2D collision)
    //{



    //}




}
