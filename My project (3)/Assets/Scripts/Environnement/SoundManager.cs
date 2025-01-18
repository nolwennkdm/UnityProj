using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // Singleton pour accès global

    public AudioSource audioSource;     // Source audio pour jouer les sons
    public AudioClip attackSound;       // Son joué lors d'une attaque réussie
    public AudioClip healSound;       // Son joué lorsqu'on reçoit des dégâts
    public AudioClip sleepSound;    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Conserve entre les scènes
        }
        else
        {
            Destroy(gameObject); // Évite les doublons
        }
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip); // Joue un son sans interrompre les autres
        }
    }
}
