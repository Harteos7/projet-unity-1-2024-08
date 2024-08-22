using UnityEngine;

public class Camera1 : MonoBehaviour
{
    public float sensitivity = 3.0f;  // Sensibilité de la souris pour la rotation
    public float speed = 10f;         // Vitesse de déplacement de la caméra
    public float zoomSpeed = 4f;      // Vitesse de zoom
    public float minZoom = 2f;        // Zoom minimum (distance la plus proche)
    public float maxZoom = 15f;       // Zoom maximum (distance la plus éloignée)
    public float fixedHeight = 10f;   // Hauteur fixe de la caméra
    public Vector3 offset = new Vector3(0, 0.5f, -13);  // Décalage initial de la caméra par rapport au joueur

    private float currentRotationX = 0f;
    private float currentRotationY = 0f;

    void Start()
    {
        // Angle de rotation souhaité
        float angle = 30.0f;

        // Modifier la rotation de la caméra sur l'axe X pour regarder vers le bas à 30 degrés
        transform.rotation = Quaternion.Euler(angle, 0, 0);
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
        

    }
}
