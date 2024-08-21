using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody rb;

    void Start()
    {
        // Récupère le composant Rigidbody attaché au GameObject
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Récupération des entrées clavier
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calcul du vecteur de mouvement
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Application du mouvement avec le Rigidbody
        rb.AddForce(movement * speed);
    }
}