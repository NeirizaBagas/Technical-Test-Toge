using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages global game functionality including scene reloading and application quit.
/// Handles game state transitions and cleanup.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Restarts the current scene by reloading it.
    /// </summary>
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Quits the application.
    /// In editor, stops playing. In build, closes the application.
    /// </summary>
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
