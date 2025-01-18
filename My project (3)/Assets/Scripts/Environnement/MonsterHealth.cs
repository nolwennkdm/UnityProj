using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    public int maxHealth = 4;
    private int currentHealth;
    
    private Animator animator;

    [Header("Particle Effects")]
    public GameObject damageParticlesPrefab; // Prefab des particules pour les dégâts
    

    void Start()
    {
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Méthode pour réduire la vie du monstre lorsqu'il prend des dégâts
    public void TakeDamage(int damage)
    {
        if (IsDead()) return;

        currentHealth -= damage;
        animator.SetTrigger("GetHit");

        // Jouer les particules de dégâts
        SpawnParticles(damageParticlesPrefab);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    // Méthode pour gérer la mort du monstre
    private void Die()
    {
        animator.SetTrigger("Die");

        

        Destroy(gameObject, 2f); // Délai pour laisser l'animation de mort jouer avant de détruire le monstre
    }

    // Vérifie si le monstre est mort
    public bool IsDead()
    {
        return currentHealth <= 0;
    }

    // Méthode pour instancier et détruire des particules
    private void SpawnParticles(GameObject particlesPrefab)
    {
        if (particlesPrefab != null)
        {
            // Instancier les particules à la position du monstre
            GameObject particles = Instantiate(particlesPrefab, transform.position, Quaternion.identity);

            // Détruire les particules après 2 secondes
            Destroy(particles, 2f);
        }
    }
}
