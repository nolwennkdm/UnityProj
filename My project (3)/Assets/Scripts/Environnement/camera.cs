using UnityEngine;

public class TPSCamera : MonoBehaviour
{
    public Transform player; // Référence au joueur
    public float distance = 5f; // Distance de la caméra par rapport au joueur
    public float height = 2f; // Hauteur de la caméra par rapport au joueur
    public float rotationSpeed = 5f; // Vitesse de rotation de la caméra
    public float smoothSpeed = 0.125f; // Vitesse de lissage pour un mouvement fluide

    private Vector3 currentVelocity;

    void Start()
    {
        // Vérifie si le joueur est assigné au démarrage
        if (player == null)
        {
            UpdatePlayerReference();
        }
    }

    void Update()
    {
        // Si le joueur n'est pas assigné, essaye de le réassigner
        if (player == null)
        {
            UpdatePlayerReference();
            return; // Ne pas exécuter le reste du code tant qu'aucun joueur n'est assigné
        }

        // Rotation de la caméra en fonction de la souris
        float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed;
        float verticalInput = Input.GetAxis("Mouse Y") * rotationSpeed;

        // Appliquer la rotation
        transform.RotateAround(player.position, Vector3.up, horizontalInput);

        // Limiter l'angle vertical de la caméra pour éviter que la caméra ne passe sous le joueur
        float currentXRotation = transform.eulerAngles.x;
        float newXRotation = Mathf.Clamp(currentXRotation - verticalInput, -30f, 60f);
        transform.eulerAngles = new Vector3(newXRotation, transform.eulerAngles.y, transform.eulerAngles.z);

        // Calculer la position de la caméra en suivant le joueur à la bonne distance et hauteur
        Vector3 desiredPosition = player.position - transform.forward * distance + Vector3.up * height;

        // Lissage de la position de la caméra pour un mouvement plus fluide
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothSpeed);
    }

    /// <summary>
    /// Met à jour la référence du joueur en cherchant un GameObject avec le tag "Player".
    /// </summary>
    public void UpdatePlayerReference()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
            Debug.Log("Player trouvé et assigné à la caméra TPS.");
        }
        else
        {
            Debug.LogWarning("Aucun joueur trouvé avec le tag 'Player'.");
        }
    }
}
