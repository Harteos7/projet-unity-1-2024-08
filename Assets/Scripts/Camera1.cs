using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera1 : MonoBehaviour
{
    public Transform target;  // Référence au Transform du joueur (Cubro)
    public Vector3 offset;    // Décalage de la caméra par rapport au joueur
    public float sensitivity = 3.0f;  // Sensibilité de la souris pour la rotation

    public float zoomSpeed = 4f;  // Vitesse de zoom
    public float minZoom = 2f;    // Zoom minimum (distance la plus proche)
    public float maxZoom = 15f;   // Zoom maximum (distance la plus éloignée)

    private float currentRotationX = 0f;
    private float currentRotationY = 0f;

    void Start()
    {
        if (offset == Vector3.zero)
        {
            offset = new Vector3(0, 5, -13);  // Décalage par défaut
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

            // Gestion du zoom avec la molette de la souris
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            float zoomAmount = scroll * zoomSpeed;

            // Modifier la distance de l'offset
            offset = offset.normalized * Mathf.Clamp(offset.magnitude - zoomAmount, minZoom, maxZoom);

            // Calcul de la nouvelle position de la caméra
            Quaternion rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
            transform.position = target.position + rotation * offset;
            
            // Faire en sorte que la caméra regarde toujours le joueur
            transform.LookAt(target);
        }
    }
}
