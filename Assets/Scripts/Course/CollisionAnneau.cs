using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAnneau : MonoBehaviour
{
    // Marque si l’anneau est une cible, ce qui le fait réagir aux collisions
    public bool target = false;
    private bool hit = false;

    // L’avion qui doit entrer en collision avec l’anneau
    public GameObject avion;

    // Marque que l’anneau a été franchi si les conditions sont respectées : bon objet (avion) et s’il est une cible
    private void OnTriggerEnter(Collider collider)
    {
        if (target && collider.name == avion.transform.GetChild(0).name)
        {
            hit = true;
            Debug.Log("OUI OUI OUI C'EST GAGNÉ");
        }
    }

    public bool IsHit() { return hit; }
}
