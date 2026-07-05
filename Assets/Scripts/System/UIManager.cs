using UnityEngine;

/// <summary>
/// Manages UI state transitions between exploration and battle scenes.
/// Handles visibility of game containers and game over panel.
/// </summary>
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject endingPanel;
    [SerializeField] private GameObject explorationContainer;
    [SerializeField] private GameObject battleContainer;

    private void Start()
    {
        BattleAudioManager.Instance.PlayExplorationBGM();
        gameOverPanel.SetActive(false);
        endingPanel.SetActive(false);
    }

    /// <summary>
    /// Switches the UI to exploration mode.
    /// Hides battle UI and shows exploration UI.
    /// </summary>
    public void ShowExplorationContainer()
    {
        BattleAudioManager.Instance.PlayExplorationBGM();
        explorationContainer.SetActive(true);
        battleContainer.SetActive(false);
    }

    /// <summary>
    /// Switches the UI to battle mode.
    /// Hides exploration UI and shows battle UI.
    /// </summary>
    public void ShowBattleContainer()
    {
        BattleAudioManager.Instance.PlayBattleBGM();
        explorationContainer.SetActive(false);
        battleContainer.SetActive(true);
    }

    /// <summary>
    /// Displays the game over panel when the player loses.
    /// </summary>
    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }

    /// <summary>
    ///  Displays the ending panel when the player wins or completes the game.
    /// </summary>
    public void ShowEndingPanel()
    {
        endingPanel.SetActive(true);
    }
}
