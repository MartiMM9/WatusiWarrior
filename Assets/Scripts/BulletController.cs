using UnityEngine;

public class BulletController : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Ahi le dite");
        }
        Destroy(gameObject);
    }
}
