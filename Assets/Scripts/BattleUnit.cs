using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] private CharacterStats statsTemplate;

    public bool isPlayerCharacter { get; private set; }
    public int MaxHealth { get; private set; }
    public int AttackPower { get; private set; }
    public int currentHealth { get; private set; }

    private AnimatorHandler animatorHandler;

    private void Awake()
    {
        animatorHandler = GetComponent<AnimatorHandler>();
    }

    public void SetupUnit()
    {
        isPlayerCharacter = statsTemplate.isPlayerCharacter;
        MaxHealth = statsTemplate.maxHealth;
        AttackPower = statsTemplate.attackDamage;
        currentHealth = MaxHealth;

    }

    public void TakeDamage(int damage)
    {
        animatorHandler.HandlingTakingDamage();
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;
    }

    public void PlayBasicAttack() => animatorHandler.HandlingBasicAttack();

    public void PlayHeavyAttack() => animatorHandler.HandlingHeavyAttack(isPlayerCharacter);

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > MaxHealth) currentHealth = MaxHealth;
    }
}
