using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the battle user interface including health display, action buttons, and battle log.
/// </summary>
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

    /// <summary>
    /// Updates health display for both player and enemy including text and slider values.
    /// </summary>
    /// <param name="player">Player battle unit</param>
    /// <param name="enemy">Enemy battle unit</param>
    public void UpdateHUD(BattleUnit player, BattleUnit enemy)
    {
        playerHPText.text = $"HP: {player.currentHealth}/{player.MaxHealth}";
        enemyHPText.text = $"HP: {enemy.currentHealth}/{enemy.MaxHealth}";
        playerHPSlider.value = (float)player.currentHealth / player.MaxHealth;
        enemyHPSlider.value = (float)enemy.currentHealth / enemy.MaxHealth;
    }

    /// <summary>
    /// Displays a message in the battle log.
    /// </summary>
    /// <param name="message">Message to display</param>
    public void DisplayLog(string message) => logText.text = message;

    /// <summary>
    /// Toggles the visibility of the action panel.
    /// </summary>
    /// <param name="isActive">Whether the panel should be active</param>
    public void SetActionPanelActive(bool isActive) => actionPanel.SetActive(isActive);

    /// <summary>
    /// Sets the interactability state of all action buttons.
    /// </summary>
    /// <param name="isInteractable">Whether buttons should be interactable</param>
    public void SetButtonsInteractable(bool isInteractable)
    {
        foreach (Button btn in allActionButtons) btn.interactable = isInteractable;
    }
}
