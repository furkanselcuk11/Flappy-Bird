using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Sprite[] birdSprites;    // Sprite'lar i�in dizi olu�turur - animasyon haline getirmek i�in

    SpriteRenderer spriteRenderer;
    Rigidbody2D physics;
    GameController gameControllerObj;
    AudioSource []sounds;

    bool backForwardControl = true; // Skybox ileri gidiyor mu gitmiyor mu kontrol
    int birdCounter=0;
    float birdAnimationSpeed=0;
    public  float jumpForce;

    public TextMeshProUGUI scoreText;
    int score = 0;
    int bestScore = 0;
    
    bool gameOverControl = true;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        physics = GetComponent<Rigidbody2D>();
        sounds = GetComponents<AudioSource>();
        gameControllerObj = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        // GameController objesindeki gameController Koduna(Component)'ne eri�ir

        bestScore = PlayerPrefs.GetInt("bestScoreLoad");    // Oyun ba�lad���nda en y�ksek skoru al�r
    }
    void Update()
    {
        birdMove();
        birdAnimation();
    }
    void birdAnimation()
    {
        birdAnimationSpeed += Time.deltaTime;   // Ku� animsyon h�z� saniye gibi �al���r
        if (birdAnimationSpeed > 0.1f) // Her 0.1 saniyede bir buraya girer ve kanat ��rpar
        {
            birdAnimationSpeed = 0;

            if (backForwardControl)
            {
                spriteRenderer.sprite = birdSprites[birdCounter];
                birdCounter++;  // 0,1,2,3 - Hareket ileri
                if (birdCounter == birdSprites.Length)
                {
                    birdCounter--;
                    backForwardControl = false;
                }
            }
            else
            {
                birdCounter--;  // 3,2,1,0  - Hareket geri
                spriteRenderer.sprite = birdSprites[birdCounter];
                if (birdCounter == 0)
                {
                    birdCounter++;
                    backForwardControl = true;
                }
            }
        }
    }
    void birdMove()
    {
        if (Input.GetMouseButtonDown(0) && gameOverControl)
        {
            physics.velocity = new Vector3(0, 0);   // H�z� s�f�r yapar
            physics.AddForce(new Vector2(0, jumpForce));  // Kuvvet uygular
            sounds[0].Play();
        }
        if (physics.velocity.y > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 45);  // E�er ku�a kuvvet uygulan�rsa yukar� bak
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, -45); // E�er ku�a kuvvet uygulanmam��sa ��karsa a�a�� bak
        }
    }
     void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "score")
        {
            score += 10;
            scoreText.text = "Skor: " + score;
            sounds[1].Play();
        }
        if (collision.gameObject.tag == "block")
        {
            gameOverControl = false;
            sounds[2].Play();
            gameControllerObj.gameOver();
            GetComponent<CircleCollider2D>().enabled = false;

            if (score > bestScore)
            {
                bestScore = score;
                PlayerPrefs.SetInt("bestScoreLoad", bestScore);  // En y�ksek skoru haf�zaya kaydeder
            }
            Invoke("mainMenuReturn", 2);    // 2 saniye sonra 'mainMenuReturn' methodu �al���r
        }
    }
    void mainMenuReturn()
    {
        PlayerPrefs.SetInt("scoreLoad", score); // yundaki skoru kaydeder
        SceneManager.LoadScene("MainMenu"); // Anamen�y� a�
    }
}
