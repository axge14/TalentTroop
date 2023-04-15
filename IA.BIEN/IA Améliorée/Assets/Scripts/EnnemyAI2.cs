using UnityEngine;
using UnityEngine.AI;

public class EnnemyAI2 : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.stoppingDistance = 0;
        PickNewRandomDestination();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            PickNewRandomDestination();
        }
    }

    void PickNewRandomDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * 10;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, 10, NavMesh.AllAreas);
        Vector3 finalPosition = hit.position;
        agent.SetDestination(finalPosition);
    }
}