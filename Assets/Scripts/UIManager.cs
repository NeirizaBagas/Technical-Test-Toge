using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject explorationContainer;
    [SerializeField] private GameObject battleContainer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverPanel.SetActive(false);
    }

    public void ShowExplorationContainer()
    {
        explorationContainer.SetActive(true);
        battleContainer.SetActive(false);
    }

    public void ShowBattleContainer()
    {
        explorationContainer.SetActive(false);
        battleContainer.SetActive(true);
    }

    public void ShowGameOverPanel()
    {
        gameOverPanel.SetActive(true);
    }
}
