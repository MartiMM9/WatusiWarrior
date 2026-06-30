using UnityEngine;

public class SpawnEnemyTemporal : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    private EnemyCircle enemyCircle;

    void Awake()
    {
        enemyCircle = GameObject.Find("Player/Circle").GetComponent<EnemyCircle>();
    }

    public void SpawnEnemy()
    {
        Vector3 spawnPos = enemyCircle.getRandomSpawnPos();
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
    }
}
