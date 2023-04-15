using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class VisionConeMesh : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;
    private Rigidbody rb;

    public Transform playerTransform;
    public float speed = 5f;
    public float raycastDistance = 10f;
    public int numberOfRaycasts = 6;
    public float angleBetweenRaycasts = 20f;

    private bool detectedPlayer = false;
    private float randomMovementTimer = 0f;
    private float randomMovementDuration = 5f;
    private Vector3 randomMovementDirection;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Si l'IA a détecté le joueur, elle se dirige vers lui
        if (detectedPlayer)
        {
            // Calcule la direction du joueur
            Vector3 directionToPlayer = (playerTransform.position - transform.position).normalized;
            agent.SetDestination(playerTransform.position);
        }
        // Sinon, elle se déplace de manière aléatoire
        else
        {
            // Réduit le timer
            randomMovementTimer -= Time.deltaTime;

            // Si le timer est écoulé, change de direction aléatoirement et réinitialise le timer
            if (randomMovementTimer <= 0f)
            {
                randomMovementDirection = Random.insideUnitSphere;
                randomMovementDirection.y = 0f;
                randomMovementDirection.Normalize();
                randomMovementTimer = randomMovementDuration;
            }

            // Déplace l'IA dans la direction aléatoire
            Vector3 moveDirection = transform.forward + randomMovementDirection * 0.5f;
            rb.MovePosition(transform.position + moveDirection * speed * Time.fixedDeltaTime);
        }

        // Lance les raycasts
        for (int i = 0; i < numberOfRaycasts; i++)
        {
            // Calcule la direction du raycast
            Vector3 direction = Quaternion.Euler(0f, angleBetweenRaycasts * i, 0f) * transform.forward;

            // Dessine une ligne pour visualiser le raycast dans l'éditeur
            Debug.DrawRay(transform.position, direction * raycastDistance, Color.red);

            // Effectue le raycast
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, raycastDistance))
            {
                // Si le raycast touche le joueur, détecte le joueur
                if (hit.transform == playerTransform)
                {
                    detectedPlayer = true;
                }
            }
        }
    }
}
