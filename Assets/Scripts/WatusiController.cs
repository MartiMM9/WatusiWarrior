using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class WatusiController : MonoBehaviour
{
    WatusiVisual watusiVisual;
    NavMeshAgent agent;
    [SerializeField] private Vector2 maxPatrolPoint, minPatrolPoint;
    private bool isWalking;

    void Start()
    {
        watusiVisual = GetComponentInChildren<WatusiVisual>();
        agent = GetComponent<NavMeshAgent>();
        StartWalking();
    }

    void Update()
    {
        if (!isWalking || agent.destination == null) { return; }

        if(transform.position == agent.destination)
        {
            StartCoroutine(IdleCoroutine());
        }
    }

    IEnumerator IdleCoroutine()
    {
        isWalking = false;
        Debug.Log("Alooo");
        watusiVisual.SetWalkingAnimation(isWalking);
        float randomWaitTime = Random.Range(1, 5);
        yield return new WaitForSeconds(randomWaitTime);
        StartWalking();
    }

    private void StartWalking()
    {
        isWalking = true;
        watusiVisual.SetWalkingAnimation(isWalking);
        float randomX = Random.Range(minPatrolPoint.x, maxPatrolPoint.x);
        float randomZ = Random.Range(minPatrolPoint.y, maxPatrolPoint.y);
        agent.destination = new Vector3(randomX, transform.position.y, randomZ);
    }
}
