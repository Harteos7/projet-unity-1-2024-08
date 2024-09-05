using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Camera1 : MonoBehaviour
{
    public Transform target;  // Référence au Transform du joueur
    public LayerMask obstacleLayers;  // Couches à considérer comme obstacles
    public float sensitivity = 3.0f;  // Sensibilité de la souris pour la rotation
    public float speed = 10.0f; // Vitesse de déplacement
    public float xAdjustment = 0.2f; // Facteur pour réduire le déplacement sur l'axe X
    public float height;  // Hauteur comprie entre 2 et 20 de la caméra
    private float currentRotationX;
    private float currentRotationY;
    private bool camerafix = false; // la variable global de suivie d'un déplacement
    public float distanceCamera = 10f;

    void Start()
    {
        // Assigner le LayerMask en utilisant le nom "Ground"
        obstacleLayers = LayerMask.GetMask("Ground");
        // Réajuster la rotation pour regarder vers le bas à 30 degrés
        transform.rotation = Quaternion.Euler(30, transform.rotation.eulerAngles.y, 0);
        // Mettre à jour les variables de rotation pour refléter la nouvelle rotation de la caméra
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        currentRotationX = eulerAngles.y;  // Rotation autour de l'axe Y
        currentRotationY = eulerAngles.x;  // Rotation autour de l'axe X
    }

    void LateUpdate()
    {
        float moveHorizontal = 0f;
        float moveVertical = 0f;

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
        if (Input.GetKey(KeyCode.Q) || camerafix) // A pour centrer la camera
        {
            TeleportCamera(target); // on donne un cible est si on apelle cette fonction on la regarde !
        }

        // Mettre à jour les variables de rotation pour refléter la nouvelle rotation de la caméra
        Vector3 eulerAngles = transform.rotation.eulerAngles;
        currentRotationX = eulerAngles.y;  // Rotation autour de l'axe Y
        currentRotationY = eulerAngles.x;  // Rotation autour de l'axe X
        height = transform.position.y;  // position en y
        height = Mathf.Clamp(height, 2, 20);


        // déplacement sur x et z
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement = transform.TransformDirection(movement);

        // Appliquer le mouvement en fixant la hauteur Y
        transform.position += movement * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, height, transform.position.z);


        // ROTATION de la camera
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            currentRotationX += Input.GetAxis("Mouse X") * sensitivity;
            currentRotationY -= Input.GetAxis("Mouse Y") * sensitivity;
            currentRotationY = Mathf.Clamp(currentRotationY, 10f, 50f);
            // Appliquer la rotation de la caméra
            transform.rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
        }

        Zoom(50f);
    }





    void TeleportCamera(Transform cible)
    {
        Vector3 backward = cible.position -transform.forward * distanceCamera;

        camerafix = true;

        Vector3 newCameraPosition = backward;

        transform.position = Vector3.MoveTowards(transform.position, newCameraPosition, 0.1f);

        if (Vector3.Distance(transform.position, newCameraPosition) < 0.01f)
        {
            camerafix = false;
            distanceCamera = 10f;
        }

        float currentAngle = 0f;
        bool avance = false;

        while (!VisonDegager(backward,cible.position)) {
            
            if (avance) {
                distanceCamera --;
                backward = cible.position -transform.forward * distanceCamera;
                currentAngle ++;
            } else {
                transform.Rotate(Vector3.up, 1f, Space.World);
                currentAngle ++;
                backward = cible.position -transform.forward * distanceCamera;
            }            
            if (currentAngle >= 360f) {
                Debug.Log("Imposible de trouver un chemin dégagé");
                avance = true;
            }
            if (currentAngle >= 1000f) {
                Debug.Log("Ptit probleme");
                camerafix = false;
                break;
            }
        }
    }


    void Zoom(float speed)
    {
        // Détection du défilement de la molette de la souris pour zoomer
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {

            if (scroll > 0 && transform.position.y > 2)
            {
                transform.position += transform.forward * speed * Time.deltaTime;
            }
            else if (scroll < 0 && transform.position.y < 20)
            {
                transform.position -= transform.forward * speed * Time.deltaTime;
            }

        }
    }

    bool VisonDegager(Vector3 fromPosition, Vector3 toPosition) {
        
        // l'anglle/Direction du raycast
        Vector3 direction = toPosition - fromPosition;

        // Distance
        float rayDistance = direction.magnitude;

        direction.Normalize();
        // KAMEAMEA !!!!
        if (Physics.Raycast(fromPosition, direction, out RaycastHit hit, rayDistance, obstacleLayers)) {
            // Debug.Log("OUTCH ! ca touche un truc " + hit.collider.name);
            return false;
        }   else {
            // Debug.Log("Ca touche pas");
            return true;
        }
    }
}
