using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class Spawner : MonoBehaviour
{
    // Struct to organize enemy spawning
    [System.Serializable]
    public struct EnemyCountPair
    {
        public GameObject enemyPrefab;
        public int enemyCount;
    }

    public EnemyCountPair[] totalEnemies;
    public GameObject[] pigPrefabs;

    [Header("Spawner Controls (Note: Spawn Rate is in terms of # seconds per enemy spawned)")]
    public float startSpawnRate;
    public float spawnRateAcceleration;
    public float maxSpawnRate;
    public float initialSpawnDelay;
    public Vector2 spawnDirection;
    public float xOffset;
    public float yOffset;

    private Dictionary<GameObject, int> maxEnemies = new Dictionary<GameObject, int>();
    private Dictionary<string, int> currEnemies = new Dictionary<string, int>();
    private int numEnemiesToSpawn;
    private float cooldownTimer;
    private float spawnRate;
    private bool isFlooding = false;
    private bool isPigDoneSpawning = false;
    private bool isDoneSpawning = false;


    // Start is called before the first frame update
    void Start()
    {
        cooldownTimer = startSpawnRate - initialSpawnDelay;
        spawnRate = startSpawnRate;
        maxEnemies.Clear();
        currEnemies.Clear();
        foreach (EnemyCountPair p in totalEnemies)
        {
            maxEnemies[p.enemyPrefab] = p.enemyCount;
            numEnemiesToSpawn += p.enemyCount;
            currEnemies[p.enemyPrefab.name] = 0;
        }
        spawnDirection.Normalize();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.S.gameState != GameManager.GameState.playing) return;
        cooldownTimer += Time.deltaTime;
        if (cooldownTimer >= spawnRate && (numEnemiesToSpawn >= 0 || isFlooding))
        {
            SpawnOneEnemy();
            cooldownTimer = 0;
            if (!isFlooding) spawnRate = Mathf.Clamp(spawnRate - spawnRateAcceleration, maxSpawnRate, startSpawnRate);
        }
    }

    public void Flood()
    {
        isFlooding = true;
        spawnRate = 0.5f;
    }

    public IEnumerator SpawnPigs()
    {
        foreach (GameObject pigObject in pigPrefabs)
        {
            GameObject pig = Instantiate(pigObject, new Vector3(Random.Range(transform.position.x - xOffset, transform.position.x + xOffset), Random.Range(transform.position.y - yOffset, transform.position.y + yOffset), 0), Quaternion.identity);
            pig.GetComponent<Pig>().SetCurrentDirection(spawnDirection);
            yield return new WaitForSeconds(spawnRate);
        }
        isPigDoneSpawning = true;
    }

    public bool IsPigDoneSpawning()
    {
        return isPigDoneSpawning;
    }

    // Spawn a random enemy type dictated by the level description in LevelManager
    private void SpawnOneEnemy()
    {
        if (numEnemiesToSpawn <= 0 && !isFlooding)
        {
            if (!isDoneSpawning)
            {
                isDoneSpawning = true;
                GameManager.S.OnEnemiesFinishedSpawning();
                return;
            }
            return;
        }
        // Choose a random enemy type
        GameObject enemyPrefab = maxEnemies.ElementAt(Random.Range(0, maxEnemies.Count())).Key;
        while (!isFlooding && currEnemies[enemyPrefab.name] >= maxEnemies[enemyPrefab])
        {
            enemyPrefab = maxEnemies.ElementAt(Random.Range(0, maxEnemies.Count())).Key;
        }
        // Instantiate enemy at spawn location
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(Random.Range(transform.position.x - xOffset, transform.position.x + xOffset), Random.Range(transform.position.y - yOffset, transform.position.y + yOffset), 0), Quaternion.identity);
        if (enemy.GetComponent<Enemy>().enemyType == EnemyType.WOLF && SceneManager.GetActiveScene().name == "Level1")
        {
            enemy.GetComponent<Enemy>().speed = 2;
        }
        enemy.GetComponent<Enemy>().initialDirection = spawnDirection;
        if (!isFlooding)
        {
            currEnemies[enemyPrefab.name] += 1;
            numEnemiesToSpawn--;
            GameManager.S.OnEnemySpawned();
        }   
    }
}
