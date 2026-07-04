using UnityEngine;
using UnityEngine.InputSystem;

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

    // Update is called once per frame
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

    private void Flip()
    {
        facingDirection *= -1;
        transform.localScale = new Vector3(facingDirection, 1, 1);
    }

    public void SetToggleMovement(bool state)
    {
        Debug.Log("SetToggleMovement called with state: " + state);
        canMove = state;
    }

}
