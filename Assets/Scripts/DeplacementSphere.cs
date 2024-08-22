using UnityEngine;

public class DeplacementSphere : MonoBehaviour
{
    public float vitesse = 5f; // Vitesse de déplacement de la sphère
    private Vector3 destination; // Destination vers laquelle la sphère se déplace
    private bool seDeplacer = false; // Indique si la sphère doit se déplacer
    private Rigidbody rb;

    void Start()
    {
        // Récupère le composant Rigidbody attaché au GameObject
        rb = GetComponent<Rigidbody>();

        TeleportPlayer(0, 5, 0);
    }

    void Update()
    {
        // Vérifier si le clic gauche de la souris a été effectué
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Effectuer un raycast pour déterminer où le clic a eu lieu
            if (Physics.Raycast(ray, out hit))
            {
                // Définir la nouvelle destination en fonction de la hauteur du sol détectée
                if (hit.collider.CompareTag("Ground"))
                {
                    // Positionner la destination en utilisant la hauteur du sol + 1 pour la sphère
                    float hauteurSol = hit.point.y;
                    destination = new Vector3(hit.point.x, hauteurSol+ 0.5f, hit.point.z);
                    seDeplacer = true;
                }
            }
        }

        // Déplacer la sphère vers la destination si nécessaire
        if (seDeplacer)
        {
            // Calculer la direction vers la destination
            Vector3 direction = (destination - rb.position).normalized;

            // Déplacer la sphère en utilisant MovePosition
            rb.MovePosition(rb.position + direction * vitesse * Time.deltaTime);

            // Arrêter le mouvement si la sphère est proche de la destination
            if (Vector3.Distance(rb.position, destination) < 0.1f)
            {
                rb.MovePosition(destination); // S'assurer que la sphère est exactement à la destination
                seDeplacer = false;
            }
        }
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
