using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager S;
    public enum GameState { getReady, playing, paused, oops, gameOver };
    public GameState gameState;

    public int money;
    public TextMeshProUGUI moneyText;

    private int numEnemies;
    private bool isSpawning;

    private void Awake()
    {
        // Singleton Definition
        if (GameManager.S)
        {
            // singleton exists, delete this object
            Destroy(this.gameObject);
        }
        else
        {
            S = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        isSpawning = true;
        StartNewGame(); //TODO: DELETE WHEN LEVELMANAGER IS IMPLEMENTED
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNewGame()
    {
        gameState = GameState.getReady;
        ResetLevel();
    }

    private void ResetLevel()
    {
        isSpawning = true;
        gameState = GameState.playing;
    }

    public void OnEnemiesFinishedSpawning()
    {
        isSpawning = false;
    }

    public void OnEnemySpawned()
    {
        numEnemies++;
    }

    public void OnEnemyDeath()
    {
        numEnemies--;
        if (!isSpawning && numEnemies <= 0) OnEnemiesCleared();
    }

    public void OnHutDestroyed()
    {
        gameState = GameState.oops;
        Debug.Log("hut gone");
        // TODO: if it's the brick hut, call OnLevelLost()
        OnLevelCleared();
    }

    private void OnEnemiesCleared()
    {
        Debug.Log("level complete!");
        // TODO: if it's the brick hut, call OnLevelWon()
        FloodEnemies();
    }

    private void OnLevelCleared()
    {
        // TODO: Go to next level stuff
    }

    private void OnLevelWon()
    {
        // TODO: On final level when enemies are dead
    }

    private void OnLevelLost()
    {
        // TODO: On final level when the brick hut is destroyed
    }

    public void SubtractMoney(int amount)
    {
        money -= amount;
        moneyText.text = money.ToString();
    }

    private void FloodEnemies()
    {
        // TODO: Enemy Flooding Mechanism. Tell the spawner to flood enemies
        gameState = GameState.oops;
    }
}
