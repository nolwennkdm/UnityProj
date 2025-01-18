using UnityEngine;
using System.Collections.Generic;

public class PersistentObjectManager : MonoBehaviour
{
    private static List<GameObject> persistentObjects = new List<GameObject>(); // Liste statique pour stocker les objets
//statique pour être indépendante d'un gameobject d'attache
// A attacher a tout ce qui doit rester
    private void Awake()
    {
        // Vérifie si un objet similaire existe déjà dans la liste
        GameObject existingObject = persistentObjects.Find(obj => obj.name == gameObject.name);

        if (existingObject != null && existingObject != gameObject)
        {
            // Si un doublon est détecté, détruit cet objet
            Destroy(gameObject);
        }
        else
        {
            // Sinon, ajoute cet objet à la liste et le rend persistant
            persistentObjects.Add(gameObject);
            DontDestroyOnLoad(gameObject);
        }
    }

    private void OnDestroy()
    {
        // Retire cet objet de la liste lorsqu'il est détruit
        persistentObjects.Remove(gameObject);
    }
}
