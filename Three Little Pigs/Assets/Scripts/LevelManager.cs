using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager S;
    public int currLevel;
    public string currLevelName;
    public bool isFinalLevel = false;

    private void Awake()
    {
        S = this;
    }

    private void Start()
    {
        if (GameManager.S)
        {
            GameManager.S.StartNewGame();
        }
    }

    public void RestartLevel()
    {
        //Destroy(GameObject.Find("ButtonManager"));
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToNextLevel()
    {
        //Destroy(GameObject.Find("ButtonManager"));
        SceneManager.LoadScene("Level" + (currLevel + 1));
        currLevel += 1;
    }
}
