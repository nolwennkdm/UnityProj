using UnityEngine;

[RequireComponent(typeof(InputPlayer))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 10f;
    public float maxSlopeAngle = 30f; // test pour slope mais pas efficace
    public LayerMask groundLayer; 
    public float rayLength = 1.1f; 
    
    private InputPlayer inputPlayer;
    private Rigidbody rb;

    private void Awake()
    {
        
        inputPlayer = GetComponent<InputPlayer>();
        rb = GetComponent<Rigidbody>();

        if (rb == null)
            Debug.LogError("Rigidbody manquant sur le joueur !");
    }

    public void Move(Vector3 direction)
    {
        if (direction == Vector3.zero)
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            return;
        }

        // Vérification de la pente
        if (IsOnSteepSlope(out Vector3 slopeNormal))
        {
            // Empêcher le mouvement sur la pente
            Vector3 adjustedDirection = Vector3.ProjectOnPlane(direction, slopeNormal); // Ajuste la direction du mouvement
            rb.linearVelocity = new Vector3(adjustedDirection.x, rb.linearVelocity.y, adjustedDirection.z); 
        }
        else
        {
            // Mouvement normal
            Vector3 velocity = direction * speed;
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
        }
    }

    public void RotateTowards(Vector3 direction)
    {
        if (direction.magnitude == 0) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private bool IsOnSteepSlope(out Vector3 slopeNormal)
    {
        slopeNormal = Vector3.up; // Valeur par défaut
        RaycastHit hit;

      
        if (Physics.Raycast(transform.position, Vector3.down, out hit, rayLength, groundLayer))
        {
            slopeNormal = hit.normal; 
            float slopeAngle = Vector3.Angle(slopeNormal, Vector3.up); 
            return slopeAngle > maxSlopeAngle; 
        }

        return false; 
    }
}
