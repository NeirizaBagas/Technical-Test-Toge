using Fungus;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("UI Clue Canvas")]
    [SerializeField] private CanvasGroup interactionClue;

    private InputReader inputReader;
    private PlayerMovement playerMovement;

    private InteractableObject currentTarget;
    private bool canInteract;

    private void Start()
    {
        inputReader = GetComponent<InputReader>();
        playerMovement = GetComponent<PlayerMovement>();

        if (inputReader != null ) {inputReader.OnInteractionPerformed += HandleInteraction; }

        SetupCanvasGroup();
    }

    private void OnDestroy()
    {
        if (inputReader != null) { inputReader.OnInteractionPerformed -= HandleInteraction; }
    }

    void SetupCanvasGroup()
    {
        interactionClue.alpha = 0f;
    }

    private void HandleInteraction()
    {

        if (!canInteract && currentTarget == null) return;

        if (playerMovement != null )
        {
            playerMovement.SetToggleMovement(false);
            currentTarget.TriggerDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<InteractableObject>(out var interactable))
        {
            currentTarget = interactable;
            interactionClue.alpha = 1f;
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            currentTarget = null;
            interactionClue.alpha = 0f;
            canInteract = false;
        }
    }
}
