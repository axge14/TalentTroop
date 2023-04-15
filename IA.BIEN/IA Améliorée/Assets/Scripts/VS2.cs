using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

public class VS2 : MonoBehaviour
{
    
    [Header("Paramètres")]
    

    
    private float speed;

    [SerializeField]
    private float speedmin;

    [SerializeField]
    private float speedmax;

    [SerializeField]
    private int numberOfRaycasts;

    [SerializeField]
    private int raycastDistance;

    [SerializeField]
    private int champsdevision;

    [SerializeField]
    private float wanderingWaitTimeMin;

    [SerializeField]
    private float wanderingWaitTimeMax;

    [SerializeField]
    private float wanderingDistanceMin;

    [SerializeField]
    private float wanderingDistanceMax;



    [Header("Agent")]
    [SerializeField]
    private NavMeshAgent agent;


    private static Transform _transform;
    private static Transform _player;

    private Vector3 _movementDirection;


    private int f;

    private Vector3 previousRotation;
    private Vector3 _currentPosition;

    private bool hasDestination = false;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        _transform = transform;
        f = raycastDistance;
        previousRotation = transform.position;
        _player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {


        // Si le temps écoulé depuis le dernier changement de direction est supérieur ou égal à l'intervalle de temps souhaité
        if ( agent.remainingDistance < 0.75f && !hasDestination)
        {
            StartCoroutine(GetNewDestination());
        }

        
        _currentPosition = transform.position;
        _movementDirection = (_currentPosition - previousRotation).normalized;


        for (int i = 0; i < numberOfRaycasts / 2; i++)
        {
            raycastDistance = f;
            float angle = champsdevision / numberOfRaycasts * i;
            Vector3 moveDirection = _transform.forward;
            Quaternion rotation = Quaternion.AngleAxis(angle, transform.up);
            Vector3 rayDirection = rotation * moveDirection;

            RaycastHit hit;
            if (Physics.Raycast(_transform.position, rayDirection, out hit, raycastDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    _transform.LookAt(hit.transform);
                    
                        // Définit la destination de l'IA
                    Physics.Raycast(_transform.position, _player.position, out hit, Vector3.Distance(_transform.position, _player.position));
                    Debug.DrawRay(_transform.position, rayDirection * raycastDistance, Color.red);


                    hasDestination = true;
                    return;
                }
                else
                {
                    // Trouver un point sur le côté de l'obstacle
                    Vector3 g = hit.point - _transform.position;
                    float q = g.magnitude;
                    raycastDistance = (int)q;
                    hasDestination = false;
                    //if (raycastDistance < 0.5f)
                    //{
                       // StartCoroutine(GetNewDestination());
                    //}
                }
            }

            previousRotation = _currentPosition;
            Debug.DrawRay(_transform.position, rayDirection * raycastDistance, Color.red);
        }

        for (int i = 0; i < numberOfRaycasts / 2; i++)
        {
            raycastDistance = f;
            float angle = -champsdevision / numberOfRaycasts * i;
            Vector3 moveDirection = _transform.forward;
            Quaternion rotation = Quaternion.AngleAxis(angle, transform.up);
            Vector3 rayDirection = rotation * moveDirection;

            RaycastHit hit;
            if (Physics.Raycast(_transform.position, rayDirection, out hit, raycastDistance))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    _transform.LookAt(hit.transform);

                    // Définit la destination de l'IA
                    Physics.Raycast(_transform.position, _player.position, out hit, Vector3.Distance(_transform.position, _player.position));

                    Debug.DrawRay(_transform.position, rayDirection * raycastDistance, Color.red);

                    hasDestination = true;
                    return;
                }
                else
                {
                    // Trouver un point sur le côté de l'obstacle
                    
                    Vector3 g = hit.point - _transform.position;
                    float q = g.magnitude;
                    raycastDistance = (int)q;
                    hasDestination = false;
                    //if (raycastDistance < 0.5f)
                    //{
                        //StartCoroutine(GetNewDestination());
                    //}
                }
            }

            previousRotation = _currentPosition;
            Debug.DrawRay(_transform.position, rayDirection * raycastDistance, Color.red);
        }
        // L'IA se déplace en ligne droite vers sa destination actuelle

        speed = Random.Range(speedmin, speedmax);
        agent.speed = speed;
    }


    IEnumerator GetNewDestination()
    {
        hasDestination = true;
        yield return new WaitForSeconds(Random.Range(wanderingWaitTimeMin, wanderingWaitTimeMax));

        Vector3 nextDestination = transform.position;
        nextDestination += Random.Range(wanderingDistanceMin, wanderingDistanceMax) * new Vector3(Random.Range(.1f, 1), 0f, Random.Range(-1f, 1f)).normalized;

        NavMeshHit d;
        if (NavMesh.SamplePosition(nextDestination, out d, wanderingDistanceMax, NavMesh.AllAreas))
        {
            agent.SetDestination(d.position);
        }

        hasDestination = false;
    }
}