using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    public GameObject swordPrefab; // Prefab de l'épée
    public Transform swordAttachmentPoint; // Point d'attache de l'épée sur la main
    public Animator animator; // Animator du personnage

    private GameObject equippedSword; // Instance de l'épée équipée
    private bool isAttacking = false; // Flag pour vérifier si l'attaque est en cours
    private bool hasHitMonster = false; // Flag pour vérifier si le monstre a déjà pris 
    //des dégâts pendant l'attaque pour pas dupliquer es degats pour une attaque
    public float attackDuration = 0.5f; // Durée de l'attaque (en secondes)
    private float attackTimer = 0f; // Timer de l'attaque pour suivre la durée

    void Start()
    {
        // Attacher l'épée dès le début
        AttachSword();
    }

    void Update()
    {
        // Vérifier l'entrée clavier pour attaquer (avec la touche Enter)
        if (Input.GetKeyDown(KeyCode.Return) && !isAttacking) // Utilise Enter pour attaquer
        {
            Attack();
        }

        // Si l'attaque est en cours, mettre à jour le timer de l'attaque
        if (isAttacking)
        {
            attackTimer += Time.deltaTime;

            // Si l'attaque dure plus longtemps que la durée spécifiée, on arrête l'attaque
            if (attackTimer >= attackDuration)
            {
                isAttacking = false;
                attackTimer = 0f;
                hasHitMonster = false; // Réinitialise pour la prochaine attaque
            }
        }
    }

    void AttachSword()
    {
        if (swordPrefab != null && swordAttachmentPoint != null)
        {
            // Instancier l'épée et la positionner sur la main du personnage, fonctionne pas bien
            equippedSword = Instantiate(swordPrefab, swordAttachmentPoint.position, swordAttachmentPoint.rotation, swordAttachmentPoint);

            
            equippedSword.transform.localScale = Vector3.one;
            equippedSword.transform.localPosition = Vector3.zero;
            equippedSword.transform.localRotation = Quaternion.identity;

            
            Rigidbody swordRb = equippedSword.GetComponent<Rigidbody>();
            if (swordRb == null)
            {
                swordRb = equippedSword.AddComponent<Rigidbody>();
                swordRb.isKinematic = true; 
            }

            
            Collider swordCollider = equippedSword.GetComponent<Collider>();
            if (swordCollider == null)
            {
                swordCollider = equippedSword.AddComponent<BoxCollider>();
                swordCollider.isTrigger = true; 
            }
        }
        else
        {
            Debug.LogError("Prefab d'épée ou point d'attache non assigné !");
        }
    }
//Attacher directemnet pour simplifier 

    void Attack()
    {
        if (animator != null)
        {
            animator.SetTrigger("Attack");  // Déclenche l'animation d'attaque
            isAttacking = true;  // Marque que l'attaque est en cours
            hasHitMonster = false;  // Réinitialise le flag pour que l'on puisse infliger des dégâts au monstre
            SoundManager.Instance.PlaySound(SoundManager.Instance.attackSound); 
        }
        else
        {
            Debug.LogError("Animator non assigné !");
        }
    }

    // Cette fonction est appelée lorsqu'un objet avec un Collider en mode Trigger entre en collision avec l'épée
    private void OnTriggerEnter(Collider other)
    {
        // Si le collider touche un objet avec le tag "Monster", que l'attaque est en cours, et que le monstre n'a pas encore pris de dégâts
        if (other.CompareTag("Monster") && isAttacking && !hasHitMonster)
        {

            MonsterHealth monsterHealth = other.GetComponent<MonsterHealth>();

            if (monsterHealth != null)
            {
                // Infliger 2 points de dégâts au monstre pendant l'attaque
                monsterHealth.TakeDamage(2);
                hasHitMonster = true;  // Marque que le monstre a pris des dégâts pour ne pas répéter l'inflige de dégâts
            }
        }
    }
}
