using UnityEngine;

public class Camera1 : MonoBehaviour
{
    public Transform target;  // Référence au Transform du joueur
    public float sensitivity = 3.0f;  // Sensibilité de la souris pour la rotation
    public float speed = 10f;         // Vitesse de déplacement de la caméra
    public float zoomSpeed = 4f;      // Vitesse de zoom
    public float minZoom = 2f;        // Zoom minimum (distance la plus proche)
    public float maxZoom = 15f;       // Zoom maximum (distance la plus éloignée)
    public float fixedHeight = 7f;   // Hauteur fixe de la caméra

    private float currentRotationX = 0f;
    private float currentRotationY = 0f;

    void Start()
    {   
        // Réajuster la position de la caméra
        transform.position = new Vector3(0, fixedHeight, -7);

        // Réajuster la rotation pour regarder vers le bas à 30 degrés
        transform.rotation = Quaternion.Euler(30, transform.rotation.eulerAngles.y, 0);
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

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        movement = transform.TransformDirection(movement);

        // Appliquer le mouvement en fixant la hauteur Y
        transform.position += movement * speed * Time.deltaTime;
        transform.position = new Vector3(transform.position.x, fixedHeight, transform.position.z);

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            currentRotationX += Input.GetAxis("Mouse X") * sensitivity;
            currentRotationY -= Input.GetAxis("Mouse Y") * sensitivity;
            currentRotationY = Mathf.Clamp(currentRotationY, -90f, 90f);
        }

        // Appliquer la rotation de la caméra
        transform.rotation = Quaternion.Euler(30, currentRotationX, 0);
        
        // Détection du défilement de la molette de la souris pour zoomer
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            // Ajuster fixedHeight en fonction du défilement de la molette
            fixedHeight -= scroll * zoomSpeed;
            // Clamping pour rester entre minZoom et maxZoom
            fixedHeight = Mathf.Clamp(fixedHeight, minZoom, maxZoom);
        }
        
    }
}
