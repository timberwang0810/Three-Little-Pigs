using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager S;
    public enum GameState { getReady, playing, paused, oops, gameOver };
    public GameState gameState;

    public int money;

    private int numEnemies;
    private bool isSpawning;

    private bool isEnemiesCleared = false;
    private bool isFlooding = false;

    private int finishedSpawners = 0;

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
        DontDestroyOnLoad(this);
        isSpawning = true;
        //StartNewGame();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameState == GameState.playing)
            {
                gameState = GameState.paused;
                UIManager.S.ShowPausePanel();
                Time.timeScale = 0;
            }
            else if (gameState == GameState.paused)
            {
                gameState = GameState.playing;
                UIManager.S.HidePausePanel();
                Time.timeScale = 1;
            }
        }
    }

    public void StartNewGame()
    {
        Time.timeScale = 1;
        gameState = GameState.getReady;
        UIManager.S.HideAll();
        UIManager.S.AdjustHealthBar(1);
        foreach (GameObject spawner in LevelManager.S.spawners)
        {
            Debug.Log("reached");
            StartCoroutine(spawner.GetComponent<Spawner>().SpawnPigs());
        }
    }

    public IEnumerator ResetLevel()
    {
        isSpawning = true;
        isEnemiesCleared = false;
        isFlooding = false;
        finishedSpawners = 0;
        numEnemies = 0;
        ResetMoney();
        yield return StartCoroutine(UIManager.S.FlashMiddleText("Build Phase", 0.25f, 2.0f));
        gameState = GameState.playing;
    }

    public void OnEnemiesFinishedSpawning()
    {
        finishedSpawners++;
        Debug.Log("done spawning " + finishedSpawners);
        if (finishedSpawners == LevelManager.S.spawners.Length) isSpawning = false;
        if (!isSpawning && numEnemies <= 0) OnEnemiesCleared();
    }

    public void OnEnemySpawned()
    {
        numEnemies++;
        //Debug.Log("spawned " + numEnemies);
    }

    public void OnEnemyDeath(int amount)
    {
        numEnemies--;
       // Debug.Log("died " + numEnemies);
        money += amount;
        UIManager.S.UpdateMoney(money);
        //Debug.Log("isSpawning: " + isSpawning + ", numEnmies: " + numEnemies);
        if (!isSpawning && numEnemies <= 0) OnEnemiesCleared();
    }

    public void OnHutDestroyed()
    {
        gameState = GameState.oops;
        Debug.Log("hut gone");
        if (LevelManager.S.isFinalLevel || isSpawning || numEnemies > 0) OnLevelLost();
        else OnLevelCleared();
    }

    private void OnEnemiesCleared()
    {
        Debug.Log("level complete!");
        if (isEnemiesCleared) return;
        isEnemiesCleared = true;
        if (isFlooding) OnLevelWon();
        else StartCoroutine(FloodEnemies());
        //if (LevelManager.S.isFinalLevel) OnLevelWon();
        //else StartCoroutine(FloodEnemies());
    }

    private void OnLevelCleared()
    {
        // TODO: Go to next level stuff
        StartCoroutine(LevelCompleteCoroutine());
    }

    private void OnLevelWon()
    {
        // TODO: On final level when enemies are dead
        gameState = GameState.gameOver;

        SoundManager.S.StopBGM();
        SoundManager.S.OnWinGame();

        LevelManager.S.hut.GetComponent<Hut>().OnPigsVictory();
    }

    private void OnLevelLost()
    {
        // TODO: On final level when the brick hut is destroyed
        gameState = GameState.gameOver;
        SoundManager.S.StopBGM();
        SoundManager.S.OnLoseGame();
        StartCoroutine(LevelManager.S.hut.GetComponent<Hut>().ReleasePigs(true));
    }

    public void AddMoney(int amount)
    {
        money += amount;
        UIManager.S.UpdateMoney(money);
    }

    public void SubtractMoney(int amount)
    {
        money -= amount;
        UIManager.S.UpdateMoney(money);
    }

    private IEnumerator FloodEnemies()
    {
        SoundManager.S.OnFinalWave();
        if (LevelManager.S.isFinalLevel)
        {
            isFlooding = true;
            isEnemiesCleared = false;
            isSpawning = true;
            finishedSpawners = 0;
        }
        yield return StartCoroutine(UIManager.S.FlashMiddleText("Final Wave Incoming!!!", 0.25f, 2.5f));
        // TODO: Enemy Flooding Mechanism. Tell the spawner to flood enemies
        for (int i = 0; i < LevelManager.S.spawners.Length; i++)
        {
            Debug.Log("flod");
            if (LevelManager.S.isFinalLevel) LevelManager.S.spawners[i].GetComponent<Spawner>().Flood(LevelManager.S.floodTime);
            else LevelManager.S.spawners[i].GetComponent<Spawner>().Flood();
        }
    }

    private IEnumerator LevelCompleteCoroutine()
    {
        yield return StartCoroutine(LevelManager.S.hut.GetComponent<Hut>().ReleasePigs(false));
        yield return new WaitForSeconds(3.0f);
        LevelManager.S.GoToNextLevel();
        yield return new WaitForSeconds(2.0f);
        ResetMoney();
        ResetLevel();
    }


    private void ResetMoney()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            money = 100;
        }
        else if (SceneManager.GetActiveScene().name == "Level2")
        {
            money = 200;
        }
        else if (SceneManager.GetActiveScene().name == "Level3")
        {
            money = 400;
        }
    }
}
