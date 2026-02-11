using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] float movement = 0;
    [SerializeField] float time = 0;
    [SerializeField] int highScore = 0;

    int score;
    bool gameLost;
    bool newHighScore = false;

    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        HandleGlobalInput();
    }

    private void HandleGlobalInput()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame || Keyboard.current.numpadEnterKey.wasPressedThisFrame)
        {
            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene == "Title")
            {
                OnStart();
            }
            else if (currentScene == "Gameplay")
            {
                OnRestart();
            }
            else if (currentScene == "Ending")
            {
                OnRestart();
            }
        }

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            string currentScene = SceneManager.GetActiveScene().name;

            if (currentScene == "Title")
            {
                OnExit();
            }
            else if (currentScene == "Gameplay" || currentScene == "Ending")
            {
                ReturnToTitle();
            }
        }
    }

    public void OnStart()
    {
        if (SceneManager.GetActiveScene().name == "Title")
        {
            SceneManager.LoadScene("Gameplay");
        }
        else if (SceneManager.GetActiveScene().name == "Ending")
        {
            SceneManager.LoadScene("Title");
        }
    }

    public void OnRestart()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void OnExit()
    {
        Debug.Log("Exited");
        Application.Quit();
    }

    public void OnEndGame(bool gameLost)
    {
        this.gameLost = gameLost;

        movement = 0;
        time = 0;
        score = 0;

        if (!gameLost)
        {
            movement = CountersLogic.distanceTraveled;
            time = CountersLogic.elapsedTime;

            score = 1000 - Convert.ToInt32(time * 0.5f + movement * 1f);
            score = Mathf.Max(0, score);

            if (score > highScore)
            {
                newHighScore = true;
                highScore = score;
                PlayerPrefs.SetInt("HighScore", highScore);
                PlayerPrefs.Save();
                Debug.Log($"¡Nuevo récord! Score: {score}");
            }
            else
            {
                newHighScore = false;
            }
        }
        else
        {
            movement = CountersLogic.distanceTraveled;
            time = CountersLogic.elapsedTime;
            score = 0;
            newHighScore = false;
            Debug.Log($"Game Over - Atrapado. Tiempo: {time}s, Distancia: {movement}m");
        }

        SceneManager.LoadScene("Ending");
    }

    public bool IsNewrecord()
    {
        return newHighScore;
    }

    public float GetDis()
    {
        return movement;
    }

    public float GetTime()
    {
        return time;
    }

    public bool GetGameState()
    {
        return gameLost;
    }

    public int GetHighScore()
    {
        return highScore;
    }

    public int GetScore()
    {
        return score;
    }
}