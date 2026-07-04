using UnityEngine;
using System; // Wajib panggil ini untuk memakai Action

public class InputReader : MonoBehaviour
{
    public InputActions action { get; private set; }

    // 1. Bikin "Stasiun Radio" C# Action untuk tombol Shoot
    public event Action OnInteractionPerformed;

    public Vector2 MoveInput => action.Player.Move.ReadValue<Vector2>();

    private void Awake()
    {
        action = new InputActions();

        action.Player.Interact.performed += ctx => OnInteractionPerformed?.Invoke();
    }

    private void OnEnable() => action.Player.Enable();

    private void OnDisable() => action.Player.Disable();
}