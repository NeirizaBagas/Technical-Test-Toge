using System.Collections;
using UnityEngine;

public class AnimatorHandler : MonoBehaviour
{
    private Animator baseAnimator;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        baseAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void HandlingMovement(bool isMoving)
    {
        baseAnimator.SetBool("isMoving", isMoving);
    }

    public void HandlingBasicAttack()
    {
        baseAnimator.SetTrigger("basicAttack");
    }

    public void HandlingHeavyAttack(bool player)
    {
        if (!player) return;

        baseAnimator.SetTrigger("heavyAttack");
    }

    public void HandlingTakingDamage()
    {
        StartCoroutine(TakingDamageVisual());
    }

    public IEnumerator TakingDamageVisual()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(0.2f);

        spriteRenderer.color = Color.white;
    }
}
