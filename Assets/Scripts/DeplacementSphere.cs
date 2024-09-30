using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class DeplacementSphere : MonoBehaviour
{
    public float vitesse; // Vitesse de déplacement de la sphère
    private Vector3 destination; // Destination vers laquelle la sphère se déplace
    private bool Deuxiemeclik = false; // Indique si le deuxième clic a été effectué
    public float raycastDistance = 100f; // Distance du raycast pour la visualisation
    NavMeshAgent agent;
    public LineRenderer lineRenderer;
    public float pathUpdateInterval = 0.1f;
    public GameObject previsuInstance;
    public GameObject previsu;
    public List<Vector3> seDeplacer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false; // Désactiver la ligne par défaut
        agent = GetComponent<NavMeshAgent>();
        agent.speed = vitesse;
        agent.acceleration = vitesse*100;
        destination = agent.destination;
    }

    void Update()
    {   
        ActionClik();

        // Déplacer la sphère vers la destination si nécessaire
        if (seDeplacer.Count > 0)
        {
            // Mettre à jour la destination si le personage est trops loin
            if (Vector3.Distance(destination, transform.position) > 0.5f)
            {
                agent.SetDestination(destination); // Définir la destination pour le NavMeshAgent
            }
            else
            {
                seDeplacer.RemoveAt(0); // suprimer la destination atteinte
                if (previsuInstance != null) { Destroy(previsuInstance); } // Détruire la prévisualisation lorsque la destination est atteinte
            }
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            foreach (var i in seDeplacer) {
                Debug.Log(i);
                Debug.Log(seDeplacer.Count);
            }

        }
    }





    void ActionClik() {
        // Vérifier si le clic gauche de la souris a été effectué (Premier clic)
        if (Input.GetMouseButtonDown(0) && !Deuxiemeclik && seDeplacer.Count == 0)
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
                    // Positionner la destination en utilisant la hauteur du sol + 0.5 pour la sphère
                    float hauteurSol = hit.point.y;
                    destination = new Vector3(hit.point.x, hauteurSol + 0.5f, hit.point.z);
                    previsuInstance = Instantiate(previsu, destination, Quaternion.identity);

                    // Calculer le chemin vers la destination sans déplacer l'agent
                    NavMeshPath path = new NavMeshPath();
                    if (agent.CalculatePath(destination, path))
                    {
                        lineRenderer.enabled = true; // Activer le LineRenderer pour montrer le chemin
                        lineRenderer.positionCount = path.corners.Length;
                        lineRenderer.SetPositions(path.corners);
                    }

                    Deuxiemeclik = true; // Marquer le premier clic comme terminé
                }
            }
        }
        // Vérifier si le deuxième clic gauche de la souris a été effectué
        else if (Input.GetMouseButtonDown(0) && Deuxiemeclik)
        {
            seDeplacer.Add(destination); // Commencer à déplacer la sphère
            Deuxiemeclik = false; // Réinitialiser l'état pour les prochains clics
            lineRenderer.enabled = false; // Désactiver le LineRenderer pour cacher le chemin
        }
        // Vérifier si on annule tous les ordres
        else if (Input.GetMouseButtonDown(1) && Deuxiemeclik)
        {
            Deuxiemeclik = false;
            if (previsuInstance != null) { Destroy(previsuInstance); }
            lineRenderer.enabled = false; // Désactiver le LineRenderer
        }

        // On annule le déplacement
        if (Input.GetMouseButtonDown(1) && seDeplacer.Count > 0) {
            seDeplacer.RemoveAt(0); // arrête de se déplacer
            agent.isStopped = true;
            agent.ResetPath();
            if (previsuInstance != null) { Destroy(previsuInstance); }
        }
    }
}
