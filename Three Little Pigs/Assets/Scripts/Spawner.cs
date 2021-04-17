using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    public float timeBetweenEnemySpawn;
    public float initialSpawnDelay;
    public Vector2 spawnDirection;
    public float xOffset;
    public float yOffset;

    private Dictionary<GameObject, int> maxEnemies = new Dictionary<GameObject, int>();
    private Dictionary<string, int> currEnemies = new Dictionary<string, int>();
    private int numEnemiesToSpawn;
    private float cooldownTimer;


    // Start is called before the first frame update
    void Start()
    {
        cooldownTimer = timeBetweenEnemySpawn - initialSpawnDelay;
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
        if (cooldownTimer >= timeBetweenEnemySpawn && numEnemiesToSpawn >= 0)
        {
            SpawnOneEnemy();
            cooldownTimer = 0;
        }
    }

    // Spawn a random enemy type dictated by the level description in LevelManager
    private void SpawnOneEnemy()
    {
        if (numEnemiesToSpawn <= 0)
        {
            GameManager.S.OnEnemiesFinishedSpawning();
            return;
        }
       
        // Choose a random enemy type
        GameObject enemyPrefab = maxEnemies.ElementAt(Random.Range(0, maxEnemies.Count())).Key;
        while (currEnemies[enemyPrefab.name] >= maxEnemies[enemyPrefab])
        {
            enemyPrefab = maxEnemies.ElementAt(Random.Range(0, maxEnemies.Count())).Key;
        }
        // Instantiate enemy at spawn location
        GameObject enemy = Instantiate(enemyPrefab, new Vector3(Random.Range(transform.position.x - xOffset, transform.position.x + xOffset), Random.Range(transform.position.y - yOffset, transform.position.y + yOffset), 0), Quaternion.identity);
        enemy.GetComponent<Wolf>().initialDirection = spawnDirection;
        currEnemies[enemyPrefab.name] += 1;
        numEnemiesToSpawn--;
        GameManager.S.OnEnemySpawned();
    }
}
