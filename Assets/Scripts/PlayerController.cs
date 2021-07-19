using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Sprite[] birdSprites;    // Sprite'lar i�in dizi olu�turur - animasyon haline getirmek i�in

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    AudioSource []sounds;   // Seslerin turuldu�u dizi

    bool backForwardControl = true; // Player animasyon ileri gidiyor 
    int birdCounter=0;  // Player hareket say�s�
    float birdAnimationSpeed=0; // Player animasyon h�z�
    public  float jumpForce;    // Player z�plama g�c�

    public TextMeshProUGUI scoreText;
    int score = 0;  // Oyun sahnesindeki skor
    int bestScore = 0;  // Oyun sonundaki en iyi skor
    
    bool gameOverControl = true;    // Oyun bitti mi
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    // Playerin SpriteRenderer componentine eri�ir - animasyonlar kareleri aras� ge�i� i�in
        rb = GetComponent<Rigidbody2D>();
        sounds = GetComponents<AudioSource>();
        bestScore = PlayerPrefs.GetInt("bestScoreLoad");    // Oyun ba�lad���nda en y�ksek skoru al�r
    }
    void Update()
    {
        BirdMove(); // Player hareket kontrolleri
        BirdAnimation();    // Player animasyon hareketleri
    }
    void BirdAnimation()
    {   // Player animasyon kontrol�
        birdAnimationSpeed += Time.deltaTime;   // Player animasyon h�z� Time.deltaTime de�eri kadar artar
        if (birdAnimationSpeed > 0.1f) // Her 0.1 saniyede bir kanat ��rpar
        {
            birdAnimationSpeed = 0; // E�er "birdAnimationSpeed" 0.1f olursa de�eri s�f�rla ve kanat ��rp

            if (backForwardControl)
            {   // E�er Player ileri gidiyor aktif ise - Yukar� kanat ��rpar
                spriteRenderer.sprite = birdSprites[birdCounter];   // "birdSprites" dizisindeki "birdCounter" de�erine sahip sprite g�r�nt�le
                birdCounter++;  // birdCounter de�erini artt�r
                if (birdCounter == birdSprites.Length)
                {   // E�er "birdCounter" de�eri "birdSprites" de�erine e�it ise 
                    birdCounter--;  // birdCounter de�eri azalt
                    backForwardControl = false; // Player ileri gidiyor pasif yap
                }
            }
            else
            {   // E�er Player ileri gidiyor pasif ise - A�a�� kanat ��rparr
                birdCounter--;  // birdCounter de�eri azalt
                spriteRenderer.sprite = birdSprites[birdCounter];   // "birdSprites" dizisindeki "birdCounter" de�erine sahip sprite g�r�nt�le
                if (birdCounter == 0)
                {   //E�er "birdCounter" de�eri 0 ise
                    birdCounter++;  // birdCounter de�erini artt�r
                    backForwardControl = true;  // Player ileri gidiyor aktif yap
                }
            }
        }
    }
    void BirdMove()
    {   // Player objesinin Mouse t�klams�na g�re a�a�� ve yukar� bakmas�
        // Player sabit kal�r sdece a�a�� ve yukar� z�plar skyboxlar geri hareket etti�i i�in Player ileri y�nde hareketi verir
        if (Input.GetMouseButtonDown(0) && gameOverControl)
        {   // Mouse sol tu�una bas�ld�ysa ve oyun bitmediyse
            rb.velocity = new Vector3(0, 0);   // Vector� s�f�rlar
            rb.AddForce(new Vector2(0, jumpForce));  // Z�plama kuvvet uygular
            sounds[0].Play();   // Z�plama sesi �alar
        }
        if (rb.velocity.y > 0)
        {   // E�er Player objesinin "y" eksenindeki pozisyonu 0 dan b�y�kse 
            transform.eulerAngles = new Vector3(0, 0, 45);  // Player 45 derece a�� ile yukar� bakar
        }
        else
        {   // E�er Player objesinin "y" eksenindeki pozisyonu 0 dan k���kse 
            transform.eulerAngles = new Vector3(0, 0, -45); // Player 45 derece a�� ile a�a�� bakar
        }
    }
     void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "score")
        {   // E�er Player score tag�na temas etmi� ise
            score += 10;    // Score 10 artar
            scoreText.text = "Skor: " + score;  // G�ncel skoru ekranda g�sterir
            sounds[1].Play();   // Score ge�i� sesi �alar
        }
        if (collision.gameObject.tag == "block")
        {   // E�er Player block tag�na temas etmi� ise
            gameOverControl = false;    // Oyun bitti durumu false olur
            sounds[2].Play();   // Gameover sesi �alar
            GameController.instance.GameOver(); // GameController scripti �zerinden GameOver methodu �al���r
            GetComponent<CircleCollider2D>().enabled = false;   // Player objesinin CircleCollider2D componenti false olur

            if (score > bestScore)
            {   // E�er score de�eri bestScore de�erinden b�y�kse en iyi skor de�i�ir
                bestScore = score;  // score de�rini bestScore de�erine atar
                PlayerPrefs.SetInt("bestScoreLoad", bestScore);  // En y�ksek skoru haf�zaya kaydeder
            }
            Invoke("MainMenuReturn", 2);    // 2 saniye sonra 'mainMenuReturn' methodu �al���r
        }
    }
    void MainMenuReturn()
    {   // E�er oyun bitmi� ise men� a�ar  ve skore kaydeder
        PlayerPrefs.SetInt("scoreLoad", score); // Oyundaki skoru kaydeder
        SceneManager.LoadScene("MainMenu"); // Anamen�y� a�
    }
}
