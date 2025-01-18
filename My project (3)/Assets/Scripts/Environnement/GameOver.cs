using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    // Événement public pour déclencher le Game Over
    public delegate void GameOverAction();
    public static event GameOverAction OnGameOver;

    private void OnEnable()
    {
        // S'abonner à l'événement
        HealthBar.OnGameOver += HandleGameOver;
    }

    private void OnDisable()
    {
        // Se désabonner de l'événement
        HealthBar.OnGameOver -= HandleGameOver;
    }

    private void HandleGameOver()
    {
        LoadGameOverScene(); // Appelle la scène Game Over
    }

    public void LoadGameOverScene()
    {
        Debug.Log("Chargement de la scène Game Over...");
        SceneManager.LoadScene("GameOver");
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log($"Chargement de la scène {sceneName}...");
        SceneManager.LoadScene(sceneName);
    }
}
