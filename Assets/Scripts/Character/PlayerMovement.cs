using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles player character movement and animation based on input.
/// Integrates with the battle system to disable movement during battles.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private int facingDirection = 1; // 1 for right, -1 for left

    private AnimatorHandler animatorHandler;
    private InputReader inputReader;
    private Rigidbody2D rb;
    private Vector2 input;

    private bool isMoving;
    public bool canMove = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animatorHandler = GetComponent<AnimatorHandler>();
        inputReader = GetComponent<InputReader>();
    }

    private void OnEnable()
    {
        BattleSystem.OnBattleCompleted += SetToggleMovement;
    }

    private void OnDisable()
    {
        BattleSystem.OnBattleCompleted -= SetToggleMovement;
    }

    /// <summary>
    /// Processes input and updates movement state each frame.
    /// </summary>
    void Update()
    {
        if (canMove)
        {
            input = inputReader.MoveInput;
            input = input.normalized;
            isMoving = input != Vector2.zero;
            animatorHandler.HandlingMovement(isMoving);
        }
        else
        {
            input = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        Movement();
    }

    /// <summary>
    /// Applies movement velocity and handles direction flipping.
    /// </summary>
    private void Movement()
    {
        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (input.x > 0 && facingDirection < 0 || input.x < 0 && facingDirection > 0)
        {
            Flip();
        }

        rb.linearVelocity = input * moveSpeed;
    }

    /// <summary>
    /// Flips the character sprite to face the direction of movement.
    /// </summary>
    private void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(facingDirection, 1, 1);
    }

    /// <summary>
    /// Toggles player movement on/off (called when battle starts/ends).
    /// </summary>
    /// <param name="state">Whether the player should be able to move</param>
    public void SetToggleMovement(bool state)
    {
        canMove = state;
    }
}
