using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Camera1 : MonoBehaviour
{
    public Transform target;  // Référence au Transform du joueur
    public float sensitivity = 3.0f;  // Sensibilité de la souris pour la rotation
    public float speed = 10.0f; // Vitesse de déplacement
    public float xAdjustment = 0.2f; // Facteur pour réduire le déplacement sur l'axe X
    public float zoomSpeed = 50f;      // Vitesse de zoom
    public float height = 12f;    // Hauteur fixe de la caméra
    public Vector3 offset = new Vector3(0, 2, 0); // Décalage initial de la caméra par rapport à la cible
    private float currentRotationX = 0f;
    private float currentRotationY = 0f;
    public LayerMask obstacleLayers;  // Couches à considérer comme obstacles
    public float fixedXRotation = 30f; // Inclinaison fixe de la caméra sur l'axe X

    void Start()
    {
        // Assigner le LayerMask en utilisant le nom "Ground"
        obstacleLayers = LayerMask.GetMask("Ground");

        // Réajuster la rotation pour regarder vers le bas à 30 degrés
        transform.rotation = Quaternion.Euler(fixedXRotation, transform.rotation.eulerAngles.y, 0);
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
        if (Input.GetKey(KeyCode.Q)) // A pour centrer la camera
        {
            transform.position = TeleportCamera();
            transform.LookAt(target);

            // Mettre à jour les variables de rotation pour refléter la nouvelle rotation de la caméra
            Vector3 eulerAngles = transform.rotation.eulerAngles;
            currentRotationX = eulerAngles.y;  // Rotation autour de l'axe Y
            currentRotationY = eulerAngles.x;  // Rotation autour de l'axe X
        }

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement = transform.TransformDirection(movement);

        // Appliquer le mouvement en fixant la hauteur Y
        transform.position += movement * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, height, transform.position.z);

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            currentRotationX += Input.GetAxis("Mouse X") * sensitivity;
            currentRotationY -= Input.GetAxis("Mouse Y") * sensitivity;
            currentRotationY = Mathf.Clamp(currentRotationY, -60f, 60f);
            // Appliquer la rotation de la caméra
            transform.rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
        }

        Zoom();
    }





    Vector3 TeleportCamera()
    {
        Vector3 directionToTarget = target.position - transform.position;
        float distanceToTarget = directionToTarget.magnitude;

        // Lancer un Raycast de la caméra vers la cible
        if (Physics.Raycast(transform.position, directionToTarget.normalized, out RaycastHit hit, distanceToTarget, obstacleLayers))
        {
            // Si un obstacle est détecté, se téléporter devant l'obstacle
            return new Vector3(hit.point.x, hit.point.y, 0);; // Reculer légèrement pour ne pas coller l'obstacle
        }
        else
        {
            return new Vector3(target.position.x+5, target.position.y+7, target.position.z);
        }
    }


    void Zoom()
    {
        // Détection du défilement de la molette de la souris pour zoomer
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            // Récupère la direction dans laquelle la caméra regarde dans le système de coordonnées locales
            Vector3 forwardDirection = transform.forward;

            Debug.Log("Direction : " + forwardDirection);

            // Applique l'ajustement sur l'axe X
            forwardDirection.x *= xAdjustment;

            // Détermine le vecteur de mouvement en fonction du défilement
            Vector3 movement = Vector3.zero;

            if (scroll > 0 && transform.position.y > 2)
            {
                movement = new Vector3(forwardDirection.x, forwardDirection.y, forwardDirection.z);
            }
            else if (scroll < 0 && transform.position.y < 20)
            {
                movement = new Vector3(-forwardDirection.x, -forwardDirection.y, -forwardDirection.z);
            }

            // Normalise le vecteur de mouvement pour éviter une vitesse disproportionnée
            movement.Normalize();

            // Déplace la caméra
            transform.position += movement * zoomSpeed * Time.deltaTime;    

            
            height = transform.position.y;
            height = Mathf.Clamp(height, 2, 20);       
        }
    }
}
