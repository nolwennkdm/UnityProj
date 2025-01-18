using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Méthode appelée lorsque le bouton "Start" est cliqué
    public void StartGame()
    {
        SceneManager.LoadScene("House"); // Charge la scène House
        Debug.Log("Chargement de la scène 'House'");
    }

    // Méthode appelée lorsque le bouton "Quit" est cliqué
    public void QuitGame()
    {
        Debug.Log("Quitter le jeu");
        Application.Quit(); // Quitte l'application
    }

    public void GoToMainMenu()
    {
        Debug.Log("Retour au menu principal...");
        SceneManager.LoadScene("MainMenu");
    }
}

