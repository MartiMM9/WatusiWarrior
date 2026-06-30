using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private Vector3 watusiZone;

    [Header("Stats")]
    [SerializeField]
    private float baseSpeed; //velocidad mientras acecha
    [SerializeField]
    private float sprintSpeed; //velocidad al ir a atacar a las vacas
    [SerializeField]
    private float attackingSpeed; //velocidad al ir a atacar a algo

    [Header("Chances")]
    [SerializeField]
    private int sprintChances;
    [SerializeField]
    private int playerAggroChances; //a la que sprintee hacia las vacas que tenga una posibilidad de ignorar al player e ir a por las vacas directamente, si esta en 1 ira a por el player 100%

    [Header("Logic")]
    [SerializeField]
    private float minWaitTime;
    [SerializeField]
    private float maxWaitTime;

    private bool isAttacking = false;

    private bool isWalking;
    private Vector3 objective;
    private NavMeshAgent agent;

    private EnemyCircle enemyCircle;

    void Awake()
    {
        enemyCircle = GameObject.Find("Player/Circle").GetComponent<EnemyCircle>();
    }

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        StartWalking();
    }

    void Update()
    {
        if (!isWalking || agent.destination == null)
        {
            return;
        }

        if (transform.position == agent.destination)
        {
            if (!isAttacking)
            {
                StartCoroutine(IdleCoroutine());
            }
        }
    }

    IEnumerator IdleCoroutine()
    {
        isWalking = false;
        //animacion de caminar = isWalking
        float randomWaitTime = Random.Range(minWaitTime, maxWaitTime);
        yield return new WaitForSeconds(randomWaitTime);
        int randOption = Random.Range(0, sprintChances);
        if (randOption == 0)
        {
            Sprint();
        }
        else
        {
            StartWalking();
        }
    }

    private void StartWalking()
    {
        agent.speed = baseSpeed;
        isWalking = true;
        objective = enemyCircle.moveTowards(transform.position);
        //animacion de caminar = isWalking
        agent.destination = objective;
    }

    private void Sprint()
    {
        agent.speed = sprintSpeed;
        isAttacking = true;
        isWalking = true;
        objective = watusiZone;
        //animacion de caminar = isWalking
        agent.destination = objective;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            int randChanceToAggroPlayer = Random.Range(0, playerAggroChances);
            if (randChanceToAggroPlayer == 0)
            {
                Debug.Log("Hijoputa te rajo");
            }
            else
            {
                agent.speed += 0.8f; //un pequeño boost en velocidad
                Debug.Log("A por las vacas");
            }
        }
    }
}
