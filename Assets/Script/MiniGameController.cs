using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;

public class MiniGameController : MonoBehaviour {

    //한게임의 제한시간
    public float timeRemaining = 10;
    public int score;
    public int clear_score = 100;

    public Text TimeText;
    public Text scoreText;
    public Text gameOverText;
    
    private bool gameOver;

    List<Word> WordList;

    void Start () {
        gameOver = false;
        gameOverText.text = "";
        score = 0;

        WordsToList();
        UpdateScore();
        GiveQuestion();

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
            IsClear();
        }
    }

    public void GameOver()
    {
        gameOverText.text = "Game Over";
        gameOver = true;
    }

    //문제를 제출하는 함수
    void GiveQuestion()
    {
        List<Word> shuffled = WordList.OrderBy(arg => Guid.NewGuid()).Take(3).ToList();
        
        for (int i = 0; i < shuffled.Count; ++i)
        {
            Word word = shuffled[i];
            Debug.Log(string.Format("word[{0}] : ({1}, {2})",
                i, word.id, word.value));
        }
    }

    //저장되어있는 XML 파일을 읽어와 WordList에 저장
    void WordsToList()
    {
        // 저장되어있는 XML 파일을 읽어와 리스트에 저장한다.
        WordList = WordIO.Read(Application.dataPath + "/Resource/XML/Items.xml");
    }

    //게임을 클리어했는지 게임오버인지 판별하는 함수
    void IsClear()
    {
        if(score > clear_score)
        {
            gameOverText.text = "Clear!";
        }
        else
        {
            GameOver();
        }
    }
}
