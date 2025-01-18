using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class TransitionManager : MonoBehaviour
{
    public Image blackoutScreen;              // Référence à l'écran noir pour la transition
    public float transitionDuration = 2f;     // Durée de la transition
    public Transform forestSpawnPoint;        // Point de spawn pour la forêt
    public Transform scenesSpawnPoint;        // Point de spawn pour la scène principale

    private bool isTransitioning = false;

    private void Start()
    {
        if (blackoutScreen != null)
        {
            // Configurer l'écran noir pour qu'il couvre tout l'écran et soit transparent au départ
            RectTransform rectTransform = blackoutScreen.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero; // Bas-gauche
            rectTransform.anchorMax = Vector2.one; // Haut-droite
            rectTransform.offsetMin = Vector2.zero; // Pas de décalage
            rectTransform.offsetMax = Vector2.zero; // Pas de décalage
            blackoutScreen.color = new Color(0, 0, 0, 0); // Initialement transparent
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isTransitioning)
            return;

        // Vérifier si le joueur entre dans une zone de transition
        if (other.CompareTag("DetectionTransScenes"))
        {
            StartCoroutine(TransitionToNewScene("Scenes", forestSpawnPoint));
        }
        else if (other.CompareTag("DetectionTransForest"))
        {
            StartCoroutine(TransitionToNewScene("Forest", scenesSpawnPoint));
        }
    }

    private IEnumerator TransitionToNewScene(string sceneName, Transform spawnPoint)
    {
        isTransitioning = true;

        float elapsedTime = 0f;

        // Transition vers l'écran noir
        while (elapsedTime < transitionDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            blackoutScreen.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, elapsedTime / (transitionDuration / 2)));
            yield return null;
        }

        // Charger la nouvelle scène
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);
        yield return SceneManager.LoadSceneAsync(sceneName);

        // Positionner le joueur au point de spawn
        if (spawnPoint != null)
       
        {
            transform.position = spawnPoint.position;
            transform.rotation = spawnPoint.rotation; // Prend également la rotation
            Debug.Log($"Joueur repositionné au point de spawn : {spawnPoint.name} avec rotation {spawnPoint.rotation.eulerAngles}");
        }

        else
        {
            Debug.LogWarning("Aucun point de spawn défini pour cette transition !");
        }

        // Réinitialisation de l'écran noir
        elapsedTime = 0f;
        while (elapsedTime < transitionDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            blackoutScreen.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, elapsedTime / (transitionDuration / 2)));
            yield return null;
        }

        blackoutScreen.color = new Color(0, 0, 0, 0); // S'assurer qu'il est complètement transparent
        isTransitioning = false;
    }
}
