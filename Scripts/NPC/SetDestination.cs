using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SetDestination : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent agent;
    [SerializeField]
    private Transform destination;
    [SerializeField]
    private float moveDirectionThreshold;

    private Vector3 positionLastFrame = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        agent.SetDestination(destination.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(positionLastFrame, destination.position) > moveDirectionThreshold)
        {
            agent.SetDestination(destination.position);
        }
    }
}
