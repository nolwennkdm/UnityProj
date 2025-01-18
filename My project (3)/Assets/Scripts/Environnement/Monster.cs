using UnityEngine;
using System.Collections;

public class MonsterAI : MonoBehaviour
{
    public float moveSpeed = 20f;
    public float waitTime = 2f;
    public float attackRange = 10f;
    public float attackCooldown = 1f;
    public float rotationSpeed = 5f; // Vitesse de rotation

    // Zone de déplacement
    public GameObject movementArea; // GameObject définissant la zone de mouvement

    // Référence au joueur
    private Transform player;
    
    // Gestion de la santé
    private MonsterHealth monsterHealth;

    // Composants
    private Animator animator;
    private Vector3 targetPosition;
    private bool isMoving = false;
    private bool isAttacking = false;
    
    private Bounds areaBounds;

    // Attaque du monstre
    public Collider attackCollider;  // Collider de la zone d'attaque du monstre

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player")?.transform;  // Vérifie si le joueur existe
        monsterHealth = GetComponent<MonsterHealth>();

        if (player == null)
        {
            Debug.LogError("Le joueur (Player) n'a pas été trouvé dans la scène !");
            return;
        }

        if (monsterHealth == null)
        {
            Debug.LogError("Le script MonsterHealth n'a pas été trouvé sur ce monstre !");
            return;
        }

        if (movementArea != null)
        {
            Collider areaCollider = movementArea.GetComponent<Collider>();
            if (areaCollider != null)
            {
                areaBounds = areaCollider.bounds;
            }
            else
            {
                Debug.LogError("Le GameObject 'movementArea' doit avoir un Collider !");
            }
        }
        else
        {
            Debug.LogError("Aucun GameObject défini pour 'movementArea' !");
        }

        ChooseNewTargetPosition();
    }

    void Update()
    {
        if (monsterHealth.IsDead()) return; // Ne faire rien si le monstre est mort

        if (isMoving)
        {
            MoveTowardsTarget();
            animator.SetBool("IsRunning", true);  // Utilisation de SetBool pour gérer les animations de course
        }
        else
        {
            animator.SetBool("IsRunning", false);  // Arrêter l'animation de course quand le monstre est statique
        }

        // Vérifier la distance avec le joueur pour attaquer
        if (player != null && Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            if (!isAttacking)
            {
                StartCoroutine(Attack());
            }
        }
    }
//Deplacement aleatoire dans la zone, alternance avec arret 
    void ChooseNewTargetPosition()
    {
        if (movementArea != null)
        {
            targetPosition = new Vector3(
                Random.Range(areaBounds.min.x, areaBounds.max.x),
                transform.position.y, 
                Random.Range(areaBounds.min.z, areaBounds.max.z)
            );
            isMoving = true;
        }
    }

    void MoveTowardsTarget()
    {
        // Calculer la direction vers la nouvelle cible
        Vector3 direction = targetPosition - transform.position;

        // Appliquer la rotation pour que le monstre se tourne vers la direction du mouvement
        if (direction.magnitude > 0.1f)  // Si le monstre se déplace
        {
            // Calculer la rotation désirée
            Quaternion toRotation = Quaternion.LookRotation(direction.normalized);
            // Appliquer la rotation avec interpolation fluide
            transform.rotation = Quaternion.Slerp(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

        // Déplacer le monstre vers la position cible
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Vérifier si le monstre est arrivé à destination
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            isMoving = false;
            animator.SetTrigger("Idle");  // Déclencher l'animation de pause après avoir atteint la destination
            Invoke(nameof(ChooseNewTargetPosition), waitTime); // Pause avant de choisir une nouvelle position
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");  // Déclencher l'animation d'attaque

        // Attente avant d'infliger des dégâts
        yield return new WaitForSeconds(0.5f);  // Attendre un moment pour s'assurer que l'attaque a lieu

        // Si le collider d'attaque est en contact avec le joueur, il lui inflige des dégâts
        if (attackCollider != null && attackCollider.bounds.Intersects(player.GetComponent<Collider>().bounds))
        {
            if (HealthBar.Instance != null)
            {
                HealthBar.Instance.TakeDamage(2);
            }
        }

        yield return new WaitForSeconds(attackCooldown);
        isAttacking = false;
    }
}
