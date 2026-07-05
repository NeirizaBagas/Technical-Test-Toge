using UnityEngine;
using System;

/// <summary>
/// Reads and broadcasts player input events.
/// Provides movement input and interaction event callbacks for other systems.
/// </summary>
public class InputReader : MonoBehaviour
{
    public InputActions action { get; private set; }

    /// <summary>
    /// Event triggered when the interaction button is pressed.
    /// </summary>
    public event Action OnInteractionPerformed;

    /// <summary>
    /// Gets the current movement input (WASD or joystick).
    /// </summary>
    public Vector2 MoveInput => action.Player.Move.ReadValue<Vector2>();

    private void Awake()
    {
        action = new InputActions();

        action.Player.Interact.performed += ctx => OnInteractionPerformed?.Invoke();
    }

    private void OnEnable() => action.Player.Enable();

    private void OnDisable() => action.Player.Disable();
}