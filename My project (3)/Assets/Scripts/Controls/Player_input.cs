using UnityEngine;

public class InputPlayer : MonoBehaviour
{
    public Player player;             // Référence au script Player
    public Camera camera_;            // Caméra utilisée pour calculer la direction
    public Animator animator;         // Référence à l'animator pour les animations

    private Vector3 movementDirection; // Direction calculée à chaque frame

    void Update()
    {
        // Récupérer les axes de déplacement
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Calculer les vecteurs de direction à partir de la caméra
        Vector3 forward = camera_.transform.forward;
        Vector3 right = camera_.transform.right;

        forward.y = 0; // Ignorer la composante verticale
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        // Combiner les directions pour obtenir une direction de mouvement
        movementDirection = (forward * vertical + right * horizontal).normalized;
        //eventuellement à modifier, je ne sais pas ce qui est le plus fluide, a direction de la caméra ou celle du joueuer ?
        // Gérer les animations, en appelant le parametres de transition 
        float speed = movementDirection.magnitude;
        if (animator != null)
        {
            animator.SetFloat("Speed", speed);
        }
    }

    private void FixedUpdate()
    {
        if (movementDirection.magnitude > 0.1f) // Seuil pour éviter les petites fluctuations
        {
            player?.RotateTowards(movementDirection); // Tourner le joueur
            player?.Move(movementDirection);          // Déplacer le joueur
        }
        else
        {
            player?.Move(Vector3.zero); // Arrêter le mouvement si aucune direction
        }
    }
}
