using UnityEngine;

public class PlayerPickup : MonoBehaviour
{
    public Inventory inventory; // Référence à l'inventaire du joueur
    public InventoryUI inventoryUI; // Référence au script UI de l'inventaire
    private Item currentItem; // L'objet en cours de ramassage

    void Update()
    {
        // Lorsque la touche "E" est pressée et qu'il y a un objet à ramasser
        if (Input.GetKeyDown(KeyCode.E) && currentItem != null)
        {
            // Essayer d'ajouter l'objet à l'inventaire
            if (inventory.AddItem(currentItem))
            {
                // Supprimer l'objet de la scène
                Destroy(currentItem.gameObject);
                currentItem = null; // Réinitialiser l'objet ramassé

                // Mettre à jour l'UI de l'inventaire pour refléter le changement
                inventoryUI.UpdateInventoryUI(); // MAJ de l'UI de l'inventaire
            }
            else
            {
                //Debug.Log("Inventaire plein !"); // Si l'inventaire est plein
            }
        }
    }
// Lorsque le joueur entre en collision avec un objet "Pickable"
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            currentItem = other.GetComponent<Item>(); // Récupérer l'objet (si l'objet a un script Item attaché)
            if (currentItem != null)
            {
              //  Debug.Log("Objet détecté : " + currentItem.itemName); // Affiche le nom de l'objet pour le débogage
            }
        }
    }

    // Lorsque le joueur sort de la zone de collision avec un objet "Pickable"
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Pickable"))
        {
            currentItem = null; // Réinitialiser l'objet ramassé
        }
    }
}