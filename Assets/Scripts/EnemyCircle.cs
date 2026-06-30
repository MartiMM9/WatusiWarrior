using UnityEngine;

public class EnemyCircle : MonoBehaviour
{
    [SerializeField]
    private Transform[] possibleSpawn; //lugares donde el enemigo podria spawnear
    [SerializeField]
    private Transform[] placesToMove; //lugares a donde el enemigo puede moverse

    public Vector3 getRandomSpawnPos() //devuelve un lugar donde puede spawnear el enemigo
    {
        int num = Random.Range(0, possibleSpawn.Length);
        float randAddX = Random.Range(-4.7f, 4.7f);
        float randAddZ = Random.Range(-4.7f, 4.7f);

        return new Vector3(possibleSpawn[num].position.x + randAddX, possibleSpawn[num].position.y, possibleSpawn[num].position.z + randAddZ);
    }

    public Vector3 moveTowards(Vector3 pos) //dandole la ubicacion del enemigo encuentra un punto donde el enemigo se pueda mover
    {
        float randScale = Random.Range(1.3f, 1.6f); //para que el movimiento sea mas aleatorio y se alejen/acerquen mas
        transform.localScale = new Vector3(randScale, randScale, randScale);

        float closest = 9999999;
        int closestPointIndex = 0;
        int i = 0;
        foreach (Transform t in placesToMove)
        {
            float distance = Vector3.Distance(t.position, pos);
            if (distance < closest)
            {
                closest = distance;
                closestPointIndex = i;
            }
            i++;
        }

        //aqui closestPointIndex tendra el punto aprox donde se encuentra el enemigo

        int toGo = 0; //en toGo guardamos a donde ira
        int sumOrSubs = Random.Range(0, 2); //para ver si hacemos al enemigo ir al punto de su izq. o der.

        switch (sumOrSubs)
        {
            case 0: //sumar
                if (closestPointIndex + 1 >= placesToMove.Length) //loopea al primero si se pasa
                {
                    toGo = 0;
                }
                else
                {
                    toGo = closestPointIndex + 1;
                }
                break;

            case 1: //restar
                if (closestPointIndex - 1 == -1)
                {
                    toGo = placesToMove.Length - 1;
                }
                else
                {
                    toGo = closestPointIndex - 1;
                }
                break;
        }
        //toGo es el index de a donde a decidido moverse el enemigo
        return placesToMove[toGo].position; //devolvemos la posicion final de a donde ira el enemigo
    }
}
