using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class CollisionHandler : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject healthBar;
    public Canvas interactionCanvas;
    public TextMeshProUGUI interactionText;
    public Image blackoutScreen;

    [Header("Durations")]
    public float sleepDuration = 2f;        // Durée du sommeil
    public float transitionDuration = 2f;  // Durée de la transition de scène

    [Header("Spawn Points")]
    [SerializeField] private Transform enterHouseSpawnPoint;  // Point de spawn pour entrer dans la maison
    [SerializeField] private Transform exitHouseSpawnPoint;   // Point de spawn pour sortir de la maison

    private bool isSleeping = false;
    private bool isTransitioning = false;
    private GameObject currentInteractionObject;

    private void Start()
    {
        // Configurer l'écran noir pour couvrir tout l'écran
        if (blackoutScreen != null)
        {
            RectTransform rectTransform = blackoutScreen.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            blackoutScreen.color = new Color(0, 0, 0, 0);
        }

        if (interactionCanvas != null)
            interactionCanvas.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bed"))
        {
            currentInteractionObject = other.gameObject;
            ShowInteractionText("Appuyez sur E pour dormir");
        }
        else if (other.CompareTag("door"))
        {
            currentInteractionObject = other.gameObject;
            ShowInteractionText("Appuyez sur E pour sortir");
        }
        else if (other.CompareTag("enter_door"))
        {
            currentInteractionObject = other.gameObject;
            ShowInteractionText("Appuyez sur E pour entrer");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == currentInteractionObject)
        {
            currentInteractionObject = null;
            if (interactionCanvas != null)
                interactionCanvas.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentInteractionObject != null)
        {
            if (currentInteractionObject.CompareTag("Bed") && !isSleeping)
            {
                StartCoroutine(Sleep());
            }
            else if (currentInteractionObject.CompareTag("door") && !isTransitioning)
            {
                StartCoroutine(TransitionToNewScene("Scenes", exitHouseSpawnPoint));
            }
            else if (currentInteractionObject.CompareTag("enter_door") && !isTransitioning)
            {
                StartCoroutine(TransitionToNewScene("House", enterHouseSpawnPoint));
            }
        }
    }

    private void ShowInteractionText(string message)
    {
        if (interactionCanvas != null)
        {
            interactionCanvas.gameObject.SetActive(true);
            interactionText.text = message;
        }
    }

    private IEnumerator Sleep()
    {
        isSleeping = true;

        // Transition vers l'écran noir
        float elapsedTime = 0f;
        SoundManager.Instance.PlaySound(SoundManager.Instance.sleepSound);
        while (elapsedTime < sleepDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            blackoutScreen.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, elapsedTime / (sleepDuration / 2)));
            yield return null;
        }

        yield return new WaitForSeconds(sleepDuration / 2);

        if (HealthBar.Instance != null)
        {
            HealthBar.Instance.SetHealthToMax();
        }

        // Transition vers l'écran transparent
        elapsedTime = 0f;
        while (elapsedTime < sleepDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            blackoutScreen.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, elapsedTime / (sleepDuration / 2)));
            yield return null;
        }

        isSleeping = false;
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

        // Enregistrer la scène précédente
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);

        // Charger la nouvelle scène
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

        // Transition vers l'écran transparent
        elapsedTime = 0f;
        while (elapsedTime < transitionDuration / 2)
        {
            elapsedTime += Time.deltaTime;
            blackoutScreen.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, elapsedTime / (transitionDuration / 2)));
            yield return null;
        }

        blackoutScreen.color = new Color(0, 0, 0, 0); 
        isTransitioning = false;
    }
}
