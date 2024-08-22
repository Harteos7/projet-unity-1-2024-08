using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    void Start()
    {
        // Récupère le composant Rigidbody attaché au GameObject
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {

    }

    // Méthode pour téléporter le joueur
    public void TeleportPlayer(float x, float y, float z)
    {
        // Change directement la position du joueur
        rb.position = new Vector3(x, y, z);

        // Si tu veux aussi arrêter tout mouvement en cours
        rb.velocity = Vector3.zero;
    }

}
