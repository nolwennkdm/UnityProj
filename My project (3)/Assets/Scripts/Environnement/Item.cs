using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string itemName;      // Nom de l'objet
    public Sprite itemIcon;      // Icône de l'objet, assignée dans l'Inspector

    void Start()
    {
        // Exemple de configuration pour un objet "patate"
        if (itemName == "patate")
        {
            InitializePatate();
        }
    }

    private void InitializePatate()
    {
        //A faire pour le tetris si le temps le permet 
    }
}
