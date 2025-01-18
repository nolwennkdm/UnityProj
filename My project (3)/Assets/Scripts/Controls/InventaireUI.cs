using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;  // Instance unique pour le singleton

    public GameObject inventoryPanel;    // Panneau d'inventaire
    public GameObject slotPrefab;        // Préfabriqué pour un slot (bouton)
    public Inventory inventory;          // Référence à l'inventaire
    public GameObject player;            // Référence au joueur

    private bool isInventoryOpen = false;

    private void Awake()
    {
        // Singleton : S'assurer qu'une seule instance existe
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Rend l'objet persistant entre les scènes
        }
        else
        {
            Destroy(gameObject); // Détruit les doublons
        }
    }

    private void Start()
    {
        if (inventoryPanel != null)
        {
            inventoryPanel.SetActive(false); // Cache l'inventaire au début
        }
        else
        {
            Debug.LogError("Inventory Panel is not assigned!");
        }
    }

    private void Update()
    {
        if (player == null)
        {
            Debug.LogError("Player GameObject is not assigned!");
            return;
        }

        // Ouvrir/fermer l'inventaire avec la touche "Espace"
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isInventoryOpen = !isInventoryOpen;
            inventoryPanel.SetActive(isInventoryOpen);
            UpdateInventoryUI();
        }
    }

    public void UpdateInventoryUI()
    {
        // Supprimer les anciens slots dans l'inventaire UI
        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Ajouter de nouveaux slots dans l'UI pour chaque objet
        for (int i = 0; i < 8; i++)
        {
            GameObject slot = Instantiate(slotPrefab, inventoryPanel.transform);
            if (i < inventory.GetItems().Count)
            {
                Item item = inventory.GetItems()[i];
                TMP_Text slotText = slot.GetComponentInChildren<TMP_Text>();
                Image slotImage = slot.GetComponentInChildren<Image>();

                if (slotText != null && slotImage != null)
                {
                    slotText.text = item.itemName;
                    slotImage.sprite = item.itemIcon;

                    Button button = slot.GetComponent<Button>();
                    button.onClick.AddListener(() => OnItemClick(item)); // Appelle OnItemClick avec l'item
                }
            }
            else
            {
                TMP_Text slotText = slot.GetComponentInChildren<TMP_Text>();
                Image slotImage = slot.GetComponentInChildren<Image>();

                if (slotText != null && slotImage != null)
                {
                    slotText.text = "";
                    slotImage.sprite = null;

                    Button button = slot.GetComponent<Button>();
                    button.interactable = false;
                }
            }
        }
    }

    // Méthode appelée lorsque l'utilisateur clique sur un item
    public void OnItemClick(Item item)
    {
        Debug.Log($"Item clicked: {item.itemName}");
        if (item.itemName.ToLower() == "patate")
        {
            Debug.Log("Healing with patate!");
            if (HealthBar.Instance != null)
            {
                HealthBar.Instance.Heal(2);
            }

            inventory.GetItems().Remove(item); // Retire la patate de l'inventaire
            UpdateInventoryUI();              // Rafraîchit l'inventaire dans l'UI
        }
        else
        {
            Debug.Log("This item is not a patate !");
        }
    }

}
