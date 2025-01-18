using UnityEngine;
using UnityEngine.SceneManagement;
//Pas utilisé finalement
public class SpawnManager : MonoBehaviour
{
    public Transform houseSpawnPoint; // Point de spawn devant la maison
    public Transform forestSpawnPoint; // Point de spawn près de l'allée menant à la forêt

    private void Start()
    {
        // Appeler la méthode pour placer le joueur au bon endroit selon la scène active
        SpawnPlayerAtCorrectLocation();
    }

    private void SpawnPlayerAtCorrectLocation()
{
    string currentScene = SceneManager.GetActiveScene().name;
    string previousScene = PlayerPrefs.GetString("PreviousScene", "");

    Transform spawnPoint = null;

    if (currentScene == "Scenes")
    {
        if (previousScene == "House")
        {
            spawnPoint = houseSpawnPoint;
        }
        else if (previousScene == "Forest")
        {
            spawnPoint = forestSpawnPoint;
        }
    }

    // Si un point de spawn existe, repositionnez le joueur
    if (spawnPoint != null)
    {
        transform.position = spawnPoint.position;
    }
    else
    {
        Debug.LogWarning("Aucun point de spawn défini pour cette transition.");
    }
}


    // Méthode pour enregistrer la scène précédente avant la transition
    public void SetPreviousScene(string sceneName)
    {
        PlayerPrefs.SetString("PreviousScene", sceneName);
        //a utiuliser lors des transitions 
    }
}
