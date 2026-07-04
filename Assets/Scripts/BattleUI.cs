using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [Header("Action Panel References")]
    [SerializeField] private GameObject actionPanel;
    [SerializeField] private TextMeshProUGUI logText;
    [SerializeField] private Button[] allActionButtons;

    [Header("Health UI References")]
    [SerializeField] private TextMeshProUGUI playerHPText;
    [SerializeField] private TextMeshProUGUI enemyHPText;
    [SerializeField] private Slider playerHPSlider;
    [SerializeField] private Slider enemyHPSlider;


    // Update the HUD with the current health of the player and enemy
    public void UpdateHUD(BattleUnit player, BattleUnit enemy)
    {
        playerHPText.text = $"HP: {player.currentHealth}/{player.MaxHealth}";
        enemyHPText.text = $"HP: {enemy.currentHealth}/{enemy.MaxHealth}";
        playerHPSlider.value = (float)player.currentHealth / player.MaxHealth;
        enemyHPSlider.value = (float)enemy.currentHealth / enemy.MaxHealth;
    }

    // Display message about the state or action taken in the battle
    public void DisplayLog(string message) => logText.text = message;

    // Enable or disable the action panel for player actions
    public void SetActionPanelActive(bool isActive) => actionPanel.SetActive(isActive);

    public void SetButtonsInteractable(bool isInteractable)
    {
        foreach (Button btn in allActionButtons) btn.interactable = isInteractable;
    }
}
