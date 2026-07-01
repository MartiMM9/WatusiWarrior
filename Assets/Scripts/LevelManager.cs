using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] Image healthBarPlayer;
    [SerializeField] TextMeshProUGUI textHealth;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject mapPanel;

    [Header("Enemies")]
    public int currentEnemies;
    [SerializeField] private int maxEnemies = 10;
    [SerializeField] private GameObject[] enemy;

    [Header("Spawning")]
    [SerializeField] private Transform[] enemySpawnPoints;
    public bool gameEnded;

    [Header("Timer")]
    [SerializeField] private float timeToWin = 60f;
    [SerializeField] private TextMeshProUGUI timerText;
    private float currentTime;

    [Header("Audio")]
    [SerializeField] AudioClip musicBG;
    [SerializeField] AudioClip winMusicBG;
    [SerializeField] AudioClip loseMusicBG;

    [Header("Misc")]
    [SerializeField] bool isDay;
    [SerializeField] Transform sunLight;
    [SerializeField] bool isTesting;
    [SerializeField] PlayerController player;

    [SerializeField] Volume volume;
    private Vignette vignette;

    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

        //ApplyDifficulty();
        UpdateLife();

        Time.timeScale = 1f;
        gameEnded = false;
        currentTime = timeToWin;

        AudioManager.instance.PlayMusic(musicBG);

        //SearchSpawnPoints();
        InvokeRepeating("SpawnEnemy", 7f, 14f);
    }

    private void Update()
    {
        if (isTesting)
        {
            return;
        }

        Timer();
    }

    public void ChangeTime()
    {
        StartCoroutine(RotateSun());
    }
    private IEnumerator RotateSun()
    {
        Quaternion start = sunLight.rotation;
        Quaternion end = start * Quaternion.Euler(180, 0, 0);

        float t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime * 0.25f;
            sunLight.rotation = Quaternion.Slerp(start, end, t);
            yield return null;
        }
        sunLight.rotation = end;
        isDay = !isDay;
    }

    public void UpdateLife()
    {
        //float percentage = 1 - (player.health / player.maxHealth);

        // Lo divido / 2 para que no sea de 0 a 1, si no de 0 a 0.5 (por simple estetica)
        //vignette.intensity.value = percentage / 2;

        //textHealth.text = player.health.ToString() + " / " + player.maxHealth.ToString();
        float healthPercent = player.health / player.maxHealth;
        //healthBarPlayer.fillAmount = healthPercent;
        //healthBarPlayer.color = Color.Lerp(Color.red, Color.green, healthPercent);
        //healthBarPlayer.fillAmount = Mathf.Lerp(healthBarPlayer.fillAmount, player.health / player.maxHealth, 5f * Time.deltaTime);
    }

    /*
    public void SearchSpawnPoints()
    {
        // Para encontrar todos los spawnPoints, y no tener que colocarlo manualmente
        GameObject[] spawnEnemyPoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
        enemySpawnPoints = new Transform[spawnEnemyPoints.Length];
        for (int i = 0; i < spawnEnemyPoints.Length; i++)
        {
            enemySpawnPoints[i] = spawnEnemyPoints[i].transform;
        }
    }

    public void SpawnEnemy()
    {
        if (isTesting)
        {
            return;
        }
        if (gameEnded)
        {
            return;
        }
        if (currentEnemies >= maxEnemies)
        {
            return;
        }

        int randomEnemy = Random.Range(0, enemy.Length);
        int randomSpawn = Random.Range(0, enemySpawnPoints.Length);

        GameObject enemyRandom = enemy[randomEnemy];
        Transform spawnPointRandom = enemySpawnPoints[randomSpawn];

        Vector3 spawnPosition = new Vector3(spawnPointRandom.position.x, spawnPointRandom.position.y, spawnPointRandom.position.z);

        Instantiate(enemyRandom, spawnPosition, spawnPointRandom.rotation);
        currentEnemies++;
    }*/

    public void Timer()
    {
        currentTime -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(currentTime / 60f);
        int seconds = Mathf.FloorToInt(currentTime - (minutes * 60));

        //timerText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        //textHealth.text = minutes.ToString("00") + ":" + seconds.ToString("00");

        if (currentTime <= 0)
        {
            GameOver();
        }
    }

    public void Win()
    {
        winPanel.SetActive(true);
        Time.timeScale = 0f;
        gameEnded = true;

        AudioManager.instance.StopMusic();
        AudioManager.instance.PlayMusic(winMusicBG);
    }
    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        gameEnded = true;

        AudioManager.instance.StopMusic();
        AudioManager.instance.PlayMusic(loseMusicBG);
    }

    /*void ApplyDifficulty()
    {
        switch (DifficultyManager.difficulty)
        {
            case 0: // Facil
                timeToWin = 200f;
                player.maxHealth = 175;
                break;

            case 1: // Med
                timeToWin = 150f;
                player.maxHealth = 125;
                break;

            case 2: // Dif
                timeToWin = 100f;
                player.maxHealth = 100;
                break;
        }

        player.health = player.maxHealth;
    }*/

    public void OpenMap()
    {
        if (mapPanel.activeInHierarchy)
        {
            mapPanel.SetActive(false);
        }
        else
        {
            mapPanel.SetActive(true);
        }
    }

    public void MainMenu()
    {
        AudioManager.instance.StopMusic();
        SceneManager.LoadScene("MainMenu");
    }
    public void Retry()
    {
        AudioManager.instance.StopMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}