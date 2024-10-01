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
    public float hauteur = 0.5f ; // Hauteur du personnage 
    private Vector3 destination; // Destination vers laquelle la sphère se déplace
    public bool seDeplacer;
    public List<Vector3> Deplacements;
    private bool Deuxiemeclik = false; // Indique si le deuxième clic a été effectué
    public float raycastDistance = 100f; // Distance du raycast pour la visualisation
    NavMeshAgent agent;
    public LineRenderer lineRenderer;


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
        if (Deplacements.Count > 0 && seDeplacer)
        {
            // Mettre à jour la destination si le personage est trops loin
            if (Vector3.Distance(Deplacements[0], transform.position) > 0.5f)
            {
                agent.SetDestination(Deplacements[0]); // Définir la destination pour le NavMeshAgent
            }
            else
            {
                Deplacements.RemoveAt(0); // suprimer la destination atteinte
                if (Deplacements.Count == 0) {seDeplacer = false;} // plus de déplacements à faire ? si oui on s'arrête
            }
        }



        // Debug
        if (Input.GetKeyDown(KeyCode.E)) {
            Debug.Log(Deplacements.Count);
        }
    }





    void ActionClik() {
        // Premier déplacement
        if (Input.GetMouseButtonDown(0) && !Deuxiemeclik && Deplacements.Count == 0)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Dessiner le rayon pour la visualisation
            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red, 2f);

            // Effectuer un raycast pour déterminer où le clic a eu lieu
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                // Définir la nouvelle destination en fonction de la hauteur du sol détectée
                if (hit.collider.CompareTag("Ground"))
                {
                    // Positionner la destination en utilisant la hauteur du sol + 0.5 pour la sphère
                    float hauteurSol = hit.point.y + hauteur;
                    destination = new Vector3(hit.point.x, hauteurSol, hit.point.z);

                    // Calculer le chemin vers la destination sans déplacer l'agent
                    NavMeshPath path = new NavMeshPath();
                    if (agent.CalculatePath(destination, path))
                    {
                        lineRenderer.enabled = true; // Activer le LineRenderer pour montrer le chemin
                        lineRenderer.positionCount = path.corners.Length;
                        lineRenderer.SetPositions(path.corners);
                    }

                    Deplacements.Add(destination); // Ajouter la destination à la liste
                    Deuxiemeclik = true; // Marquer le premier clic comme terminé
                }
            }
        }

        // Ajouter des déplacements grâce au bouton Shift
        else if (Input.GetMouseButtonDown(0) && Deuxiemeclik && Input.GetKey(KeyCode.LeftShift)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Dessiner le rayon pour la visualisation
            Debug.DrawRay(ray.origin, ray.direction * raycastDistance, Color.red, 2f);

            // Effectuer un raycast pour déterminer où le clic a eu lieu
            if (Physics.Raycast(ray, out hit, raycastDistance))
            {
                // Définir la nouvelle destination en fonction de la hauteur du sol détectée
                if (hit.collider.CompareTag("Ground"))
                {
                    // Positionner la destination en utilisant la hauteur du sol + 0.5 pour la sphère
                    float hauteurSol = hit.point.y + hauteur;
                    destination = new Vector3(hit.point.x, hauteurSol, hit.point.z);

                    Deplacements.Add(destination); // Ajouter la destination à la liste
                    lineRenderer.enabled = true; // Activer le LineRenderer pour montrer le chemin

                    Vector3[]positions = new Vector3[Deplacements.Count + 1];

                    positions[0] = agent.transform.position;

                    for (int i = 0; i <Deplacements.Count; i++) {
                        positions[i + 1] = Deplacements[i];
                    }

                    lineRenderer.positionCount = positions.Length;
                    lineRenderer.SetPositions(positions);

                    
                    Deuxiemeclik = true; // Marquer le premier clic comme terminé
                }
            }
        }



        // Vérifier si le deuxième clic gauche de la souris a été effectué
        else if (Input.GetMouseButtonDown(0) && Deuxiemeclik && !Input.GetKey(KeyCode.LeftShift))
        {
            seDeplacer = true; // on commence à se déplacer
            Deuxiemeclik = false; // Réinitialiser l'état pour les prochains clics
            lineRenderer.enabled = false; // Désactiver le LineRenderer pour cacher le chemin
        }

        // Vérifier si on annule tous les ordres
        else if (Input.GetMouseButtonDown(1) && Deuxiemeclik)
        {
            Deuxiemeclik = false;
            Deplacements.Clear(); // on vide les positions obtenue
            seDeplacer = false; // on arréter le déplacement
            lineRenderer.enabled = false; // Désactiver le LineRenderer
        }

        // On annule les déplacements
        if (Input.GetMouseButtonDown(1) && Deplacements.Count > 0) {
            Deplacements.Clear(); // on supprime les déstinations
            seDeplacer = false; // on arrête de se déplacer
            agent.ResetPath(); // on initialise la nouvelle position
        }
    }
}
