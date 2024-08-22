using UnityEngine;

public class DeplacementSphere : MonoBehaviour
{
    public float vitesse = 5f; // Vitesse de déplacement de la sphère
    private Vector3 destination; // Destination vers laquelle la sphère se déplace
    private bool seDeplacer = false; // Indique si la sphère doit se déplacer
    private Rigidbody rb;
    private bool isGrounded;

    void Start()
    {
        // Récupère le composant Rigidbody attaché au GameObject
        rb = GetComponent<Rigidbody>();
    }
    // Update is called once per frame
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
                // Définir la nouvelle destination avec y toujours égal à 1
                destination = new Vector3(hit.point.x, 1f, hit.point.z);
                seDeplacer = true;
            }
        }

        // Déplacer la sphère vers la destination si nécessaire
        if (seDeplacer)
        {
            // Calculer la direction vers la destination
            Vector3 direction = (destination - transform.position).normalized;
            // Déplacer la sphère
            transform.position += direction * vitesse * Time.deltaTime;

            // Arrêter le mouvement si la sphère est proche de la destination
            if (Vector3.Distance(transform.position, destination) < 0.1f)
            {
                transform.position = destination; // S'assurer que la sphère est exactement à la destination
                seDeplacer = false;
            }
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
}
