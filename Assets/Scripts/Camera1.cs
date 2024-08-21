using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera1 : MonoBehaviour
{
    public Transform target;  // Référence au Transform du joueur (Cubro)
    public Vector3 offset;    // Décalage de la caméra par rapport au joueur
    public float sensitivity = 3.0f;  // Sensibilité de la souris pour la rotation

    private float currentRotationX = 0f;
    private float currentRotationY = 0f;

    void Start()
    {
        if (offset == Vector3.zero)
        {
            offset = new Vector3(0, 5, -13);  // Par exemple, un décalage par défaut
        }

        // Initialisation des rotations actuelles en fonction de la position initiale de la caméra
        currentRotationX = transform.eulerAngles.y;
        currentRotationY = transform.eulerAngles.x;
    }

    void LateUpdate()
    {
        if (target != null)
        {          
            if (Input.GetKey(KeyCode.LeftAlt)) // On doit appuyer sur une touche pour faire tourner la caméra
            {
                // Rotation basée sur les mouvements de la souris
                currentRotationX += Input.GetAxis("Mouse X") * sensitivity;
                currentRotationY -= Input.GetAxis("Mouse Y") * sensitivity;
                currentRotationY = Mathf.Clamp(currentRotationY, -20f, 60f);  // Limite la rotation verticale
            }

            // Calcul de la nouvelle position de la caméra
            Quaternion rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
            transform.position = target.position + rotation * offset;
            
            // Faire en sorte que la caméra regarde toujours le joueur
            transform.LookAt(target);
        }
    }
}
