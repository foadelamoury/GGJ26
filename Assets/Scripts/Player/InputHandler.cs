using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputHandler : MonoBehaviour
{
    [Header("Input Values")]
    public Vector2 MovementInput { get; private set; }
    public bool IsBraking { get; private set; }

    void Awake()
    {
        var playerInput = GetComponent<PlayerInput>();
        if (playerInput == null) Debug.LogError("PlayerInput component missing on " + gameObject.name);
    }

    public void OnMove(InputValue inputValue)
    {
        MovementInput = inputValue.Get<Vector2>();
        Debug.Log("OnMove: " + MovementInput);
    }

    public void OnBrake(InputValue inputValue)
    {
        IsBraking = inputValue.isPressed;
    }





}
