using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DeplacementSphere : MonoBehaviour
{
    public float vitesse ; // Vitesse de déplacement de la sphère
    private Vector3 destination; // Destination vers laquelle la sphère se déplace
    private bool seDeplacer = false; // Indique si la sphère doit se déplacer
    private bool Deuxiemeclik = false; // Indique si la sphère doit se déplacer
    public float raycastDistance = 100f; // Distance du raycast pour la visualisation
    NavMeshAgent agent;
    public LineRenderer lineRenderer;
    public float pathUpdateInterval = 0.1f;
    public GameObject previsuInstance ;
    public GameObject previsu ;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        InvokeRepeating("UpdatePath", 0f, pathUpdateInterval);
        agent = GetComponent<NavMeshAgent>();
        agent.speed = vitesse;
        agent.acceleration = vitesse;
        destination = agent.destination;
    }

    void Update()
    {   
        // Vérifier si le clic gauche de la souris a été effectué
        if (Input.GetMouseButtonDown(0) && !Deuxiemeclik)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Dessiner le rayon pour la visualisation
            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red, 2f);

            // Détruire l'ancienne prévisualisation si elle existe
            if (previsuInstance != null) { Destroy(previsuInstance); }

            // Effectuer un raycast pour déterminer où le clic a eu lieu
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                // Définir la nouvelle destination en fonction de la hauteur du sol détectée
                if (hit.collider.CompareTag("Ground"))
                {
                    // Positionner la destination en utilisant la hauteur du sol + 1 pour la sphère
                    float hauteurSol = hit.point.y;
                    destination = new Vector3(hit.point.x, hauteurSol + 0.5f, hit.point.z);
                    previsuInstance  = Instantiate(previsu, destination, Quaternion.identity);
                    Deuxiemeclik = true;
                }
            }
        }
        // Vérifier si le dexieme clic gauche de la souris a été effectué
        else if (Input.GetMouseButtonDown(0) && Deuxiemeclik)
        {
            seDeplacer = true;
            Deuxiemeclik = false;
        }
        // Vérifier si on annule tout les ordres
        else if (Input.GetMouseButtonDown(1) && Deuxiemeclik)
        {
            seDeplacer = false;
            Deuxiemeclik = false;
            if (previsuInstance != null) { Destroy(previsuInstance); }
        }

        // Déplacer la sphère vers la destination si nécessaire
        if (seDeplacer)
        {
            // Update destination if the target moves one unit
            if (Vector3.Distance(destination, transform.position) > 0.5f)
            {
                agent.SetDestination(destination);
            }
            else
            {
                seDeplacer = false;
                Destroy(previsuInstance );
            }
        }
    }
        private void UpdatePath()
    {
        if (agent == null || lineRenderer == null)
            return;

        // Obtenir le chemin actuel du NavMeshAgent
        NavMeshPath path = agent.path;

        // Vérifier si le chemin est valide
        if (path == null || path.corners.Length < 2)
            return;

        // Mettre à jour le LineRenderer pour correspondre aux coins du chemin
        lineRenderer.positionCount = path.corners.Length;
        lineRenderer.SetPositions(path.corners);
    }
}
