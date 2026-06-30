using UnityEngine;

public class SpawnWatusisTemporal : MonoBehaviour
{
    [SerializeField] private GameObject watusiPrefab;
    [SerializeField] private Transform watusiSpawner;

    public void SpawnWatusi()
    {
        Instantiate(watusiPrefab, watusiSpawner.position, watusiSpawner.rotation);
    }
}
