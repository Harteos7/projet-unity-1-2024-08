using UnityEngine;

public class DeplacementSphere : MonoBehaviour
{
    public float vitesse = 5f; // Vitesse de déplacement de la sphère
    private Vector3 destination; // Destination vers laquelle la sphère se déplace
    private bool seDeplacer = false; // Indique si la sphère doit se déplacer
    private Rigidbody rb;
    public float raycastDistance = 100f; // Distance du raycast pour la visualisation

    void Start()
    {
        // Récupère le composant Rigidbody attaché au GameObject
        rb = GetComponent<Rigidbody>();

        TeleportPlayer(0,5,0);
    }

    void Update()
    {
        // Vérifier si le clic gauche de la souris a été effectué
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Dessiner le rayon pour la visualisation
            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red, 2f);

            // Effectuer un raycast pour déterminer où le clic a eu lieu
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                // Dessiner une ligne pour visualiser le hit
                Debug.DrawLine(ray.origin, hit.point, Color.green, 2f);

                // Définir la nouvelle destination en fonction de la hauteur du sol détectée
                if (hit.collider.CompareTag("Ground"))
                {
                    // Positionner la destination en utilisant la hauteur du sol + 1 pour la sphère
                    float hauteurSol = hit.point.y;
                    destination = new Vector3(hit.point.x, hauteurSol + 0.5f, hit.point.z);
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
                StopMovement();
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

    void StopMovement()
    {
        rb.velocity = Vector3.zero; // Arrête le mouvement en mettant la vitesse à zéro
        rb.angularVelocity = Vector3.zero; // Arrête la rotation en mettant la vitesse angulaire à zéro
    }
}
