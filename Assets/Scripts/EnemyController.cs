using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    EnemyVisual enemyVisual;

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
    [SerializeField]
    private bool toPlayer = false;

    private bool isAttacking = false;

    private GameObject player;
    private bool isWalking;
    private Vector3 objective;
    private NavMeshAgent agent;
    private EnemyCircle enemyCircle;

    private AudioManager audioManager;
    [Header("SFX")]
    [SerializeField]
    private AudioClip roar;
    [SerializeField]
    private AudioClip alert;

    void Awake()
    {
        enemyCircle = GameObject.Find("Player/Circle").GetComponent<EnemyCircle>();
        audioManager = Object.FindFirstObjectByType<AudioManager>();
        player = GameObject.Find("Player");
    }

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        enemyVisual = GetComponentInChildren<EnemyVisual>();
        StartWalking();
    }

    void Update()
    {
        if (!isWalking || agent.destination == null)
        {
            return;
        }

        if (toPlayer)
        {
            objective = player.transform.position;
            agent.destination = objective;
        }

        if (isAttacking)
        {
            agent.stoppingDistance = 6.5f;
        }

        if (transform.position == agent.destination)
        {
            if (!isAttacking)
            {
                StartCoroutine(IdleCoroutine());
                agent.stoppingDistance = 0;
            }
        }
    }

    IEnumerator IdleCoroutine()
    {
        isWalking = false;
        enemyVisual.SetWalkingAnimation(isWalking);
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
        enemyVisual.SetWalkingAnimation(isWalking);
        agent.destination = objective;
    }

    private void Sprint()
    {
        audioManager.PlaySFX(roar, transform.position, Random.Range(0.8f, 1f));
        agent.speed = sprintSpeed;
        isAttacking = true;
        isWalking = true;
        objective = watusiZone;
        enemyVisual.SetWalkingAnimation(isWalking);
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
                audioManager.PlaySFX(alert, transform.position, Random.Range(0.55f, 0.8f));
                toPlayer = true;
                agent.speed = attackingSpeed;
                agent.stoppingDistance = 6.5f;
            }
            else
            {
                agent.speed += 1.1f; //un pequeño boost en velocidad
                Debug.Log("A por las vacas");
                Sprint();
            }
        }
    }
}
