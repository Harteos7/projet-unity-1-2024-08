using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f;
    public float speedSaut = 30f;
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        // Récupère le composant Rigidbody attaché au GameObject
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Initialisation des variables de mouvement
        float moveHorizontal = 0f;
        float moveVertical = 0f;
        float sauter = 0f;

        // Vérification des touches enfoncées pour un clavier AZERTY
        if (Input.GetKey(KeyCode.W))  // Z pour avancer
        {
            moveVertical = 1f;
        }
        if (Input.GetKey(KeyCode.S))  // S pour reculer
        {
            moveVertical = -1f;
        }
        if (Input.GetKey(KeyCode.A))  // Q pour aller à gauche
        {
            moveHorizontal = -1f;
        }
        if (Input.GetKey(KeyCode.D))  // D pour aller à droite
        {
            moveHorizontal = 1f;
        }
        if (Input.GetKey(KeyCode.Space)&&isGrounded)  // espace pour sauter
        {
            sauter = 1f;
        }

        // Calcul du vecteur de mouvement
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        // Calcul du vecteur de mouvement
        Vector3 movementSaut = new Vector3(0.0f, sauter, 0.0f);

        // Application du mouvement avec le Rigidbody
        rb.AddForce(movement * speed);
        rb.AddForce(movementSaut * speedSaut);



        if  (transform.position.y < 0) {
            Debug.Log("t'es tomber");
            TeleportPlayer(0, 1, 0);
        }



        // Méthode pour téléporter le joueur
        void TeleportPlayer(float x, float y, float z)
        {
            // Change directement la position du joueur
            transform.position = new Vector3(x, y, z);

            // Si tu veux aussi arrêter tout mouvement en cours
            rb.velocity = Vector3.zero;
        }
    }
    void OnCollisionEnter(Collision collision){
        if (collision.gameObject.CompareTag("Ground")){
            isGrounded=true;
        }
        
    }
    void OnCollisionExit(Collision collision){
        isGrounded=false;
    }
}
