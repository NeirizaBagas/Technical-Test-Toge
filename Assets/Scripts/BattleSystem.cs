using Fungus;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

public enum  PlayerActionType { BasicAttack, HeavyAttack, Heal, Run }
public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    [Header("Battle Unit References")]
    [SerializeField] private BattleUnit playerUnit;
    [SerializeField] private BattleUnit enemyUnit;

    [Header("Battle UI References")]
    [SerializeField] private BattleUI battleUi;

    [Header("Battle Log References")]
    [SerializeField] private string startBattleLog = "You Challenge The Enemy";

    private GameObject currentOverworldEnemy;
    private UIManager uiManager;
    public static event Action<bool> OnBattleCompleted;
    public static event Action OnEnemyAttack;

    private void Start()
    {
        uiManager = GetComponent<UIManager>();
    }

    // Start the battle system and called from fungus
    public void StartBattleSystem(GameObject overworldEnemy)
    {
        currentOverworldEnemy = overworldEnemy;
        uiManager.ShowBattleContainer();
        StartCoroutine(SetupBattle());
        battleUi.UpdateHUD(playerUnit, enemyUnit);

        state = BattleState.START;
    }

    // Setup the battle by initializing the player and enemy units, updating the UI, and starting the player's turn
    private IEnumerator SetupBattle()
    {
        playerUnit.SetupUnit();
        enemyUnit.SetupUnit();

        
        battleUi.SetActionPanelActive(false);
        battleUi.DisplayLog(startBattleLog);

        yield return new WaitForSeconds(1.5f);
        PlayerTurn();
    }

    private void PlayerTurn()
    {
        state = BattleState.PLAYERTURN;
        battleUi.SetActionPanelActive(true);
        battleUi.SetButtonsInteractable(true);
        battleUi.DisplayLog("Choose an action");
    }

    #region Player Action Buttons

    public void BasicAttackButton()
    {
        if (state != BattleState.PLAYERTURN) return;
        StartCoroutine(ExecutePlayerSequence(PlayerActionType.BasicAttack));
    }

    public void HeavyAttackButton()
    {
        if (state != BattleState.PLAYERTURN) return;
        StartCoroutine(ExecutePlayerSequence(PlayerActionType.HeavyAttack));
    }

    public void HealButton()
    {
        if (state != BattleState.PLAYERTURN) return;
        StartCoroutine(ExecutePlayerSequence(PlayerActionType.Heal));
    }

    public void Run()
    {
        if (state != BattleState.PLAYERTURN) return;
        StartCoroutine(ExecutePlayerSequence(PlayerActionType.Run));
    }

    #endregion

    IEnumerator ExecutePlayerSequence(PlayerActionType actionType)
    {
        battleUi.SetButtonsInteractable(false);
        switch (actionType)
        {
            case PlayerActionType.BasicAttack:
                int basicDamage = playerUnit.AttackPower;
                enemyUnit.TakeDamage(basicDamage);
                battleUi.UpdateHUD(playerUnit, enemyUnit);
                battleUi.DisplayLog($"You using basic attack and dealt {basicDamage} damage to the enemy!");
                break;
            case PlayerActionType.HeavyAttack:
                int heavyDamage = Mathf.RoundToInt(playerUnit.AttackPower * 1.5F);
                enemyUnit.TakeDamage(heavyDamage);
                battleUi.UpdateHUD(playerUnit, enemyUnit);
                battleUi.DisplayLog($"You using heavy attack and dealt {heavyDamage} damage to the enemy!");
                break;
            case PlayerActionType.Heal:
                int healAmount = Mathf.RoundToInt(playerUnit.MaxHealth / 4f);
                playerUnit.Heal(healAmount);
                battleUi.UpdateHUD(playerUnit, enemyUnit);
                battleUi.DisplayLog($"You healed yourself for {healAmount} HP!");
                break;
            case PlayerActionType.Run:
                battleUi.DisplayLog("Knight never run away from the battle");
                // Handle running away (e.g., return to exploration)
                break;
        }

        yield return new WaitForSeconds(1.5f);

        if (enemyUnit.currentHealth <= 0)
        {
            state = BattleState.WON;
            StartCoroutine(EndBattleSequence());
        }
        else
        {
            StartCoroutine(EnemyTurnSequence());
        }
    }

    private IEnumerator EnemyTurnSequence()
    {
        state = BattleState.ENEMYTURN;
        battleUi.SetActionPanelActive(false);
        
        yield return new WaitForSeconds(1.2f);

        int basicEnemyDamage = enemyUnit.AttackPower;
        int heavyEnemyDamage = Mathf.RoundToInt(enemyUnit.AttackPower * 1.5F);

        int enemyDamage = UnityEngine.Random.Range(basicEnemyDamage, heavyEnemyDamage);
        enemyUnit.PlayBasicAttack();

        playerUnit.TakeDamage(enemyDamage);
        battleUi.UpdateHUD(playerUnit, enemyUnit);
        battleUi.DisplayLog($"Enemy attacking and dealt {enemyDamage} damage to you!");

        yield return new WaitForSeconds(1.5f);

        if (playerUnit.currentHealth <= 0)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattleSequence());
        }
        else
        {
            PlayerTurn();
        }
    }

    private IEnumerator EndBattleSequence()
    {
        battleUi.SetActionPanelActive(false);

        if (state == BattleState.WON)
        {
            battleUi.DisplayLog("You won the battle!");
            yield return new WaitForSeconds(2f);

            if (currentOverworldEnemy != null) Destroy(currentOverworldEnemy);

            uiManager.ShowExplorationContainer();
            OnBattleCompleted?.Invoke(true);

        }
        else if (state == BattleState.LOST)
        {
            battleUi.DisplayLog("You lost the battle!");
            yield return new WaitForSeconds(2f);

            uiManager.ShowExplorationContainer();
            OnBattleCompleted?.Invoke(true);
            uiManager.ShowGameOverPanel();
        }
    }
}
