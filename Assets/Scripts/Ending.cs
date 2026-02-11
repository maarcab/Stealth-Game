using System;
using TMPro;
using UnityEngine;

public class Ending : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI winOrLose;
    [SerializeField] private TextMeshProUGUI scoreOrTryAgain;
    [SerializeField] private TextMeshProUGUI timeDistance;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
        
    }

    void Start()
    {
        if (gameManager.GetGameState())
        {
            winOrLose.color = Color.red;
            winOrLose.text = "GAME OVER";
            scoreOrTryAgain.text = "Try again :(";
            timeDistance.text = $"Time: {Convert.ToInt32(gameManager.GetTime())}s    Distance: {Convert.ToInt32(gameManager.GetDis())}";
        }
        else
        {
            if (gameManager.IsNewrecord())
            {
                winOrLose.color = Color.green;
                winOrLose.text = "GREAT JOB";
                scoreOrTryAgain.text = $"You have broken a new record! \nYour new max score: {gameManager.GetHighScore()}";
                timeDistance.text = $"Time: {Convert.ToInt32(gameManager.GetTime())}s    Distance: {Convert.ToInt32(gameManager.GetDis())}m";
            }
            else
            {
                winOrLose.text = "NICE JOB";
                scoreOrTryAgain.text = $"Your score: {gameManager.GetScore()} || Max score: {gameManager.GetHighScore()}";
                timeDistance.text = $"Time: {Convert.ToInt32(gameManager.GetTime())}s    Distance: {Convert.ToInt32(gameManager.GetDis())}m";
            }
        }
    }
}

