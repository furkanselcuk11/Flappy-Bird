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
        int bestScore = PlayerPrefs.GetInt("bestScoreLoad");    // bestScore de�i�kenine "bestScoreLoad" anahtar�nda kay�tl� olan de�eri atar
        int score = PlayerPrefs.GetInt("scoreLoad");    // score de�i�kenine "scoreLoad" anahtar�nda kay�tl� olan de�eri atar
        bestScoreText.text = "En iyi Skor: " + bestScore; // bestScore de�erini ekranda g�sterir  
        scoreText.text = "Skor: " + score;  // score de�erini ekranda g�sterir 
    }
    public void GameStart()
    {
        SceneManager.LoadScene("1");   // Ba�la butonuna bas�ld���nda 1. leveli a�ar
    }
    public void GameExit()
    {
        Application.Quit(); // ��k�� butonuna bas�ld���nda pyundan ��k�� yapar
    }
}
