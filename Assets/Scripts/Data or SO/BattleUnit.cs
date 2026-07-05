using UnityEngine;

/// <summary>
/// Represents a battle participant (player or enemy) with health and attack stats.
/// Handles damage, healing, and animation triggers during battle.
/// </summary>
public class BattleUnit : MonoBehaviour
{
    [SerializeField] private CharacterStats statsTemplate;

    public bool isPlayerCharacter { get; private set; }
    public int MaxHealth { get; private set; }
    public int AttackPower { get; private set; }
    public int currentHealth { get; set; }

    private AnimatorHandler animatorHandler;

    private void Awake()
    {
        animatorHandler = GetComponent<AnimatorHandler>();
    }

    /// <summary>
    /// Initializes unit stats from the assigned CharacterStats template.
    /// Must be called before the unit can take damage or attack.
    /// </summary>
    public void SetupUnit()
    {
        if (statsTemplate == null)
        {
            Debug.LogError($"statsTemplate is null for {gameObject.name}!");
            return;
        }

        isPlayerCharacter = statsTemplate.isPlayerCharacter;
        MaxHealth = statsTemplate.maxHealth;
        AttackPower = statsTemplate.attackDamage;
        currentHealth = MaxHealth;
    }

    /// <summary>
    /// Applies damage to the unit and triggers damage animation.
    /// </summary>
    /// <param name="damage">Amount of damage to apply</param>
    public void TakeDamage(int damage)
    {
        animatorHandler.HandlingTakingDamage();
        BattleAudioManager.Instance.PlayTakeDamageSFX();
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
    }

    /// <summary>
    /// Triggers the basic attack animation.
    /// </summary>
    public void PlayBasicAttack()
    {
        animatorHandler.HandlingBasicAttack();
        BattleAudioManager.Instance.PlayActionSFX(PlayerActionType.BasicAttack);
    }

    /// <summary>
    /// Triggers the heavy attack animation (only for player character).
    /// </summary>
    public void PlayHeavyAttack()
    {
        animatorHandler.HandlingHeavyAttack(isPlayerCharacter);
        BattleAudioManager.Instance.PlayActionSFX(PlayerActionType.HeavyAttack);
    }

    /// <summary>
    /// Restores health to the unit, capped at MaxHealth.
    /// </summary>
    /// <param name="amount">Amount of health to restore</param>
    public void Heal(int amount)
    {
        currentHealth += amount;
        BattleAudioManager.Instance.PlayActionSFX(PlayerActionType.Heal);
        if (currentHealth > MaxHealth) currentHealth = MaxHealth;
    }
}
