using UnityEngine;
using UnityEngine.AI;

public class WatusiVisual : MonoBehaviour
{
    WatusiController watusiController;
    Animator animator;
    Transform player;
    NavMeshAgent agent;
    [SerializeField] private Transform watusiSprite;

    void Start()
    {
        //Asignación de varaibles
        watusiController = GetComponentInParent<WatusiController>();
        animator = GetComponentInChildren<Animator>();
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        agent = GetComponentInParent<NavMeshAgent>();
    }
    private void LateUpdate()
    {
        //El Sprite siempre mira hacia el player
        transform.LookAt(player.position);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
        //Se flipea el sprite dependiendo de si anda hacia la derecha o la izquierda respecto al player
        UpdateFacingDirection();
    }

    private void UpdateFacingDirection()
    {
        //Si el Watusi no se mueve, el Sprite se deja como esta
        if(agent.velocity.sqrMagnitude < 0.01f) { return; }

        //Coge la direccion en la que se mueve el Watusi
        Vector3 moveDirection = agent.velocity.normalized;
        //Comprueba si la direccion va hacia el mismo lado que la derecha del player o hacia el lado opuesto(la izquierda)
        float dot = Vector3.Dot(moveDirection, player.right); //Dot es una operacion entre 2 vectores que da como resultado un valor float que representa si esos vectoras miran en la misma direccion, direcciones opuestas o son perpendiculares

        Vector3 scale = watusiSprite.localScale;
        //Flipea la escala en X dependiendo de si dot es menor a 0(hacia la izquierda) o mayor(hacia la derecha)
        scale.x = dot < 0 ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x); //Mathf.Abs devuelve el valor absoluto, es decir, siempre positivo
        //Setea la escala de nuevo
        watusiSprite.localScale = scale;
    }

    public void SetWalkingAnimation(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }
}
