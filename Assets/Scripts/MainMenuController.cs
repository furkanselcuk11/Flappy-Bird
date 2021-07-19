using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI scoreText;
    void Start()
    {
        int bestScore = PlayerPrefs.GetInt("bestScoreLoad");
        int score = PlayerPrefs.GetInt("scoreLoad");
        bestScoreText.text = "En iyi Skor: " + bestScore;
        scoreText.text = "Skor: " + score;
    }
    void Update()
    {
        
    }
    public void gameStart()
    {
        SceneManager.LoadScene("1");
    }
    public void gameExit()
    {
        Application.Quit();
    }
}
