using UnityEngine;
using TMPro;

public class HealthBar : MonoBehaviour
{
    public static HealthBar Instance; // Singleton pour rendre la santé accessible globalement
    public TextMeshProUGUI healthText;  // Référence au TextMeshPro pour afficher la santé
    public int maxHealth = 10;
    private int currentHealth;
    public static event GameOverManager.GameOverAction OnGameOver;

    private void Awake()
    {
        // Implémentation Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Conserve cet objet entre les scènes
        }
        else
        {
            Destroy(gameObject); // Supprime les doublons
            return;
        }
    }

    private void Start()
    {
        if (healthText == null)
        {
            Debug.LogError("Le TextMeshPro n'est pas assigné !");
            return;
        }

        // Initialise la santé si nécessaire
        if (currentHealth == 0) 
            currentHealth = maxHealth;

        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        if (currentHealth <= 0)
        {
            currentHealth = 0; // Empêche les valeurs négatives
            UpdateHealthUI();

            // Déclenche l'événement de Game Over
            Debug.Log("Player HP is 0. Triggering Game Over...");
            OnGameOver?.Invoke();
        }
        else
        {
            UpdateHealthUI();
        }
    }

    public void Heal(int amount)
    {
        SoundManager.Instance.PlaySound(SoundManager.Instance.healSound); 
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UpdateHealthUI();
    }

    public void SetHealthToMax()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = "Health: " + currentHealth + "/" + maxHealth;
        }
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }
}
