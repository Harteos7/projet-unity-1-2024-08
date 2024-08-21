using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody rb;

    void Start()
    {
        // Récupère le composant Rigidbody attaché au GameObject
        rb = GetComponent<Rigidbody>();
        
    }

    void FixedUpdate()
    {
        Debug.Log(transform.position.z);

        // Initialisation des variables de mouvement
        float moveHorizontal = 0f;
        float moveVertical = 0f;

        // Vérification des touches enfoncées
        if (Input.GetKey(KeyCode.Z))
        {
            moveVertical = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVertical = -1f;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            moveHorizontal = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal = 1f;
        }

        // Calcul du vecteur de mouvement
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Application du mouvement avec le Rigidbody
        rb.AddForce(movement * speed);
    }
}