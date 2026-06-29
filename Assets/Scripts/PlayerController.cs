using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    PlayerInput playerInput;
    Rigidbody rb;
    float verticalLookRotation;

    [Header("Stats")]
    public float health;
    public float maxHealth;
    public float maxBullets;
    public float bullets;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lookSensitivity;

    [Header("ExternalObjects")]
    [SerializeField] private GameObject cam;
    [SerializeField] private GameObject bullet;

    void Start()
    {
        //Asignar variables
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        //Temporal
        bullets = maxBullets;
    }

    void Update()
    {
        //Movimiento del player
        Vector2 movementInput = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 movement = (transform.forward * movementInput.y + transform.right * movementInput.x);
        rb.linearVelocity = new Vector3(movement.x * moveSpeed, rb.linearVelocity.y, movement.z * moveSpeed);
    }

    void LateUpdate()
    {
        //Movimiento de la camara
        Vector2 cameraInput = playerInput.actions["Look"].ReadValue<Vector2>();
        transform.eulerAngles += new Vector3(0f, cameraInput.x * lookSensitivity, 0f);

        verticalLookRotation -= cameraInput.y * lookSensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);
        cam.transform.localEulerAngles = new Vector3(verticalLookRotation, 0f, 0f);
    }

    //Input de disparo
    public void OnShoot(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Shoot();
        }
    }

    //Logica de disparo
    private void Shoot()
    {
        //Disparar solo si hay balas
        if (bullets > 0)
        {
            //Crea un rayo desde el centro de la pantalla
            Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f));
            RaycastHit hit;

            //Comprueba si el rayo colisiona con algo
            if(Physics.Raycast(ray, out hit))
            {
                //La bala va en direccion al punto de colision (ahora mismo la bala sale desde el centro de la camara asi que no es necesario que salga desde el punto de colision)
                Vector3 bulletDirection = (hit.point - cam.transform.position).normalized;
                GameObject bulletClone = Instantiate(bullet, cam.transform.position, cam.transform.rotation);
                bulletClone.GetComponent<Rigidbody>().linearVelocity = bulletDirection * bulletSpeed;
            }
            else
            {
                //La bala va en direccion a donde apunta la camara
                GameObject bulletClone = Instantiate(bullet, cam.transform.position, cam.transform.rotation);
                bulletClone.GetComponent<Rigidbody>().linearVelocity = cam.transform.forward * bulletSpeed;
            }

            //Resta balas al disparar y evita que las balas sean negativas
            bullets--;
            if(bullets <= 0)
            {
                bullets = 0;
            }
        }
    }
}
