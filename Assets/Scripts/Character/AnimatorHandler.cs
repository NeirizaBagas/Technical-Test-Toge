using System.Collections;
using UnityEngine;

/// <summary>
/// Handles animator state transitions and visual effects for character animations.
/// Manages attack animations, movement, and damage visual feedback.
/// </summary>
public class AnimatorHandler : MonoBehaviour
{
    private Animator baseAnimator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        baseAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Controls character movement animation state.
    /// </summary>
    /// <param name="isMoving">Whether the character is moving</param>
    public void HandlingMovement(bool isMoving)
    {
        baseAnimator.SetBool("isMoving", isMoving);
    }

    /// <summary>
    /// Triggers the basic attack animation.
    /// </summary>
    public void HandlingBasicAttack()
    {
        baseAnimator.SetTrigger("basicAttack");
    }

    /// <summary>
    /// Triggers the heavy attack animation (only for player character).
    /// </summary>
    /// <param name="player">Whether this is the player character</param>
    public void HandlingHeavyAttack(bool player)
    {
        if (!player) return;

        baseAnimator.SetTrigger("heavyAttack");
    }

    /// <summary>
    /// Initiates the visual feedback for taking damage.
    /// </summary>
    public void HandlingTakingDamage()
    {
        StartCoroutine(TakingDamageVisual());
    }

    /// <summary>
    /// Visual effect for damage: briefly flashes the character sprite red.
    /// </summary>
    /// <returns>IEnumerator for damage visual feedback</returns>
    public IEnumerator TakingDamageVisual()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        spriteRenderer.color = Color.white;
    }

    public void PlayStepSfx()
    {
        BattleAudioManager.Instance.PlayStepSFX();
    }
}
