using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Sprite[] birdSprites;    // Sprite'lar için dizi oluþturur - animasyon haline getirmek için

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    AudioSource []sounds;   // Seslerin turulduðu dizi

    bool backForwardControl = true; // Player animasyon ileri gidiyor 
    int birdCounter=0;  // Player hareket sayýsý
    float birdAnimationSpeed=0; // Player animasyon hýzý
    public  float jumpForce;    // Player zýplama gücü

    public TextMeshProUGUI scoreText;
    int score = 0;  // Oyun sahnesindeki skor
    int bestScore = 0;  // Oyun sonundaki en iyi skor
    
    bool gameOverControl = true;    // Oyun bitti mi
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    // Playerin SpriteRenderer componentine eriþir - animasyonlar kareleri arasý geçiþ için
        rb = GetComponent<Rigidbody2D>();
        sounds = GetComponents<AudioSource>();
        bestScore = PlayerPrefs.GetInt("bestScoreLoad");    // Oyun baþladýðýnda en yüksek skoru alýr
    }
    void Update()
    {
        BirdMove(); // Player hareket kontrolleri
        BirdAnimation();    // Player animasyon hareketleri
    }
    void BirdAnimation()
    {   // Player animasyon kontrolü
        birdAnimationSpeed += Time.deltaTime;   // Player animasyon hýzý Time.deltaTime deðeri kadar artar
        if (birdAnimationSpeed > 0.1f) // Her 0.1 saniyede bir kanat çýrpar
        {
            birdAnimationSpeed = 0; // Eðer "birdAnimationSpeed" 0.1f olursa deðeri sýfýrla ve kanat çýrp

            if (backForwardControl)
            {   // Eðer Player ileri gidiyor aktif ise - Yukarý kanat çýrpar
                spriteRenderer.sprite = birdSprites[birdCounter];   // "birdSprites" dizisindeki "birdCounter" deðerine sahip sprite görüntüle
                birdCounter++;  // birdCounter deðerini arttýr
                if (birdCounter == birdSprites.Length)
                {   // Eðer "birdCounter" deðeri "birdSprites" deðerine eþit ise 
                    birdCounter--;  // birdCounter deðeri azalt
                    backForwardControl = false; // Player ileri gidiyor pasif yap
                }
            }
            else
            {   // Eðer Player ileri gidiyor pasif ise - Aþaðý kanat çýrparr
                birdCounter--;  // birdCounter deðeri azalt
                spriteRenderer.sprite = birdSprites[birdCounter];   // "birdSprites" dizisindeki "birdCounter" deðerine sahip sprite görüntüle
                if (birdCounter == 0)
                {   //Eðer "birdCounter" deðeri 0 ise
                    birdCounter++;  // birdCounter deðerini arttýr
                    backForwardControl = true;  // Player ileri gidiyor aktif yap
                }
            }
        }
    }
    void BirdMove()
    {   // Player objesinin Mouse týklamsýna göre aþaðý ve yukarý bakmasý
        // Player sabit kalýr sdece aþaðý ve yukarý zýplar skyboxlar geri hareket ettiði için Player ileri yönde hareketi verir
        if (Input.GetMouseButtonDown(0) && gameOverControl)
        {   // Mouse sol tuþuna basýldýysa ve oyun bitmediyse
            rb.velocity = new Vector3(0, 0);   // Vectorü sýfýrlar
            rb.AddForce(new Vector2(0, jumpForce));  // Zýplama kuvvet uygular
            sounds[0].Play();   // Zýplama sesi çalar
        }
        if (rb.velocity.y > 0)
        {   // Eðer Player objesinin "y" eksenindeki pozisyonu 0 dan büyükse 
            transform.eulerAngles = new Vector3(0, 0, 45);  // Player 45 derece açý ile yukarý bakar
        }
        else
        {   // Eðer Player objesinin "y" eksenindeki pozisyonu 0 dan küçükse 
            transform.eulerAngles = new Vector3(0, 0, -45); // Player 45 derece açý ile aþaðý bakar
        }
    }
     void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "score")
        {   // Eðer Player score tagýna temas etmiþ ise
            score += 10;    // Score 10 artar
            scoreText.text = "Skor: " + score;  // Güncel skoru ekranda gösterir
            sounds[1].Play();   // Score geçiþ sesi çalar
        }
        if (collision.gameObject.tag == "block")
        {   // Eðer Player block tagýna temas etmiþ ise
            gameOverControl = false;    // Oyun bitti durumu false olur
            sounds[2].Play();   // Gameover sesi çalar
            GameController.instance.GameOver(); // GameController scripti üzerinden GameOver methodu çalýþýr
            GetComponent<CircleCollider2D>().enabled = false;   // Player objesinin CircleCollider2D componenti false olur

            if (score > bestScore)
            {   // Eðer score deðeri bestScore deðerinden büyükse en iyi skor deðiþir
                bestScore = score;  // score deðrini bestScore deðerine atar
                PlayerPrefs.SetInt("bestScoreLoad", bestScore);  // En yüksek skoru hafýzaya kaydeder
            }
            Invoke("MainMenuReturn", 2);    // 2 saniye sonra 'mainMenuReturn' methodu çalýþýr
        }
    }
    void MainMenuReturn()
    {   // Eðer oyun bitmiþ ise menü açar  ve skore kaydeder
        PlayerPrefs.SetInt("scoreLoad", score); // Oyundaki skoru kaydeder
        SceneManager.LoadScene("MainMenu"); // Anamenüyü aç
    }
}
