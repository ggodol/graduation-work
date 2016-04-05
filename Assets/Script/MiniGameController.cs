using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniGameController : MonoBehaviour {

    //한게임의 제한시간
    public float timeRemaining = 10;
    public int score;

    public Text TimeText;
    public Text scoreText;
    public Text gameOverText;
    //public Text reStartText;
    
    private bool gameOver;
    //private bool reStart;

    void Start () {
        gameOver = false;
        //reStart = false;
        gameOverText.text = "";
        //reStartText.text = "";
        score = 0;
        UpdateScore();
	}
	
	void Update () {
        timeRemaining -= Time.deltaTime;
        UpdateTime();
    }

    //점수를 더하는 함수
    public void AddScore (int newScoreValue)
    {
        score += newScoreValue;

        if (score < 0)
            score = 0;

        UpdateScore();
    }

    void UpdateScore ()
    {
        scoreText.text = "점수 : " + score;
    }

    void UpdateTime()
    {
        if(timeRemaining > 0)
        {
            TimeText.text = "남은 시간 : " + (int)timeRemaining;
        }
        else
        {
            TimeText.text = "Time Over!";
            GameOver();
        }
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over";
        gameOver = true;
    }
}
