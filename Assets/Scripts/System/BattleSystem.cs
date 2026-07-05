using Fungus;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, ENDING }

public enum PlayerActionType { BasicAttack, HeavyAttack, Heal, Run }

/// <summary>
/// Manages the turn-based battle system between player and enemy.
/// Handles action execution, damage calculation, and battle state transitions.
/// </summary>
public class BattleSystem : MonoBehaviour
{
    public BattleState state;

    [Header("Battle Unit References")]
    [SerializeField] private GameObject playerPrefab; // Ubah nama variabel agar memperjelas status Prefab
    [SerializeField] private GameObject[] enemyLists;
    private GameObject enemyPrefab;
    private BattleUnit playerUnit;
    private BattleUnit enemyUnit;

    [Header("Spawn Points (Anchor)")]
    [SerializeField] private Transform playerSpawnPoint; // Transform kosong di Hierarchy untuk posisi spawn
    [SerializeField] private Transform enemySpawnPoint;
    private GameObject spawnedPlayerObj;
    private GameObject spawnedEnemyObj;

    [Header("Battle UI References")]
    [SerializeField] private BattleUI battleUi;

    [Header("Battle Log References")]
    [SerializeField] private string startBattleLog = "You Challenge The Enemy";

    private GameObject currentOverworldEnemy;
    private UIManager uiManager;
    public static event Action<bool> OnBattleCompleted;

    private void Start()
    {
        uiManager = GetComponent<UIManager>();
    }

    // Start the battle system and called from fungus
    public void StartBattleSystem(GameObject overworldEnemy)
    {
        currentOverworldEnemy = overworldEnemy;
        uiManager.ShowBattleContainer();
        BattleAudioManager.Instance.PlayEncounterSFX();

        InteractableObject npcInteract = currentOverworldEnemy.GetComponent<InteractableObject>();
        if (npcInteract.npcType == NPCType.Spearman) enemyPrefab = enemyLists[0];
        else if (npcInteract.npcType == NPCType.Knight) enemyPrefab = enemyLists[1];

        spawnedPlayerObj = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity, playerSpawnPoint);
        spawnedEnemyObj = Instantiate(enemyPrefab, enemySpawnPoint.position, Quaternion.identity, enemySpawnPoint);

        playerUnit = spawnedPlayerObj.GetComponent<BattleUnit>();
        enemyUnit = spawnedEnemyObj.GetComponent<BattleUnit>();

        state = BattleState.START;
        StartCoroutine(SetupBattle());

    }

    // Setup the battle by initializing the player and enemy units, updating the UI, and starting the player's turn
    private IEnumerator SetupBattle()
    {
        playerUnit.SetupUnit();
        enemyUnit.SetupUnit();

        battleUi.UpdateHUD(playerUnit, enemyUnit);
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
                playerUnit.PlayBasicAttack();
                enemyUnit.TakeDamage(basicDamage);
                battleUi.UpdateHUD(playerUnit, enemyUnit);
                battleUi.DisplayLog($"You using basic attack and dealt {basicDamage} damage to the enemy!");
                break;
            case PlayerActionType.HeavyAttack:
                int heavyDamage = Mathf.RoundToInt(playerUnit.AttackPower * 1.5F);
                playerUnit.PlayHeavyAttack();
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
            BattleAudioManager.Instance.PlayDeathSFX();
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
            BattleAudioManager.Instance.PlayDeathSFX();
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

            InteractableObject interactNpc = currentOverworldEnemy.GetComponent<InteractableObject>();

            if (currentOverworldEnemy != null && interactNpc != null && interactNpc.npcType == NPCType.Knight) uiManager.ShowEndingPanel();

            if (currentOverworldEnemy != null) Destroy(currentOverworldEnemy);

            if (spawnedPlayerObj != null) Destroy(spawnedPlayerObj);
            if (spawnedEnemyObj != null) Destroy(spawnedEnemyObj);

            uiManager.ShowExplorationContainer();
            OnBattleCompleted?.Invoke(true);
            
        }
        else if (state == BattleState.LOST)
        {
            battleUi.DisplayLog("You lost the battle!");
            yield return new WaitForSeconds(2f);

            if (spawnedPlayerObj != null) Destroy(spawnedPlayerObj);
            if (spawnedEnemyObj != null) Destroy(spawnedEnemyObj);

            uiManager.ShowExplorationContainer();
            OnBattleCompleted?.Invoke(true);
            uiManager.ShowGameOverPanel();
        }
    }
}
