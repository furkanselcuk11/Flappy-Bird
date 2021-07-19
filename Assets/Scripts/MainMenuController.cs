using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public TextMeshProUGUI bestScoreText;   // En iyi skor texti
    public TextMeshProUGUI scoreText;   // Oyun sahnesindeki skor texti
    void Start()
    {
        int bestScore = PlayerPrefs.GetInt("bestScoreLoad");    // bestScore deðiþkenine "bestScoreLoad" anahtarýnda kayýtlý olan deðeri atar
        int score = PlayerPrefs.GetInt("scoreLoad");    // score deðiþkenine "scoreLoad" anahtarýnda kayýtlý olan deðeri atar
        bestScoreText.text = "En iyi Skor: " + bestScore; // bestScore deðerini ekranda gösterir  
        scoreText.text = "Skor: " + score;  // score deðerini ekranda gösterir 
    }
    public void GameStart()
    {
        SceneManager.LoadScene("1");   // Baþla butonuna basýldýðýnda 1. leveli açar
    }
    public void GameExit()
    {
        Application.Quit(); // Çýkýþ butonuna basýldýðýnda pyundan çýkýþ yapar
    }
}
