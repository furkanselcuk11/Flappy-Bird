using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject skybox1;  // Skybox1
    public GameObject skybox2;  // Skybox2

    Rigidbody2D skybox1Rb;  
    Rigidbody2D skybox2Rb;  

    public float skyboxSpeed = -1.5f;   // Skybox hareket hýzý
    float length=0; // Skyboxlarýn Size(x ekseni) deðerleri
    float changeTime=0; // Block objelerinin eklenme süresini tutar
    int counter = 0;    // Pozisyonu deðiþen block sayýsý
    bool gameOverControl = true;

    public GameObject block;    // Block objesi
    public int totalBlock;  // Toplam block sayýsý
    GameObject[] blocks;    // Blocklarýn tutulduðu dizi    
    void Start()
    {
        skybox1Rb = skybox1.GetComponent<Rigidbody2D>();    // Skybox1'in rigidbody componentine eriþir - Skybox hareketi için
        skybox2Rb = skybox2.GetComponent<Rigidbody2D>();    // Skybox2'nin rigidbody componenti eriþir - Skybox hareketi için

        skybox1Rb.velocity = new Vector2(-skyboxSpeed, 0);    // Skybox1 geriye doðru hareket eder
        skybox2Rb.velocity = new Vector2(-skyboxSpeed, 0);    // Skybox2 geriye doðru hareket eder

        length = skybox1.GetComponent<BoxCollider2D>().size.x;  // Skybox'ýn BoxCollider componentinin "x" size uzunluðunu

        CreatBlock();   // Block oluþturma
    }
    void Update()
    {
        if (gameOverControl)   
        {
            SkyboxCreate();     // Skybox konumu deðiþtirir
            CreatBlockTime();   // Oluþturulan Block objelerinin pozisyonu belirlenir
        }        
    }
    void SkyboxCreate()
    {   // Skybox konumu deðiþtirir
        if (skybox1.transform.position.x <= -length)
        {   // Skybox1'in "x" ekseninde pozisyonu lenght deðerinden küçük eþit ise konumunu deðiþ
            // Skybox1 boyutu kadar mesafe gittiði zman konumunu deðþirtir  (-lenght olmasý skyboxlarýn geriye doðru hareket etmesi)
            skybox1.transform.position += new Vector3(length * 2, 0);   // Skybox1 diðer Skybox2'ýn bitiþiðine ekler
        }
        if (skybox2.transform.position.x <= -length)
        {   // Skybox2'in "x" ekseninde pozisyonu lenght deðerinden küçük eþit ise konumunu deðiþ
            // Skybox2 boyutu kadar mesafe gittiði zman konumunu deðþirtir  (-lenght olmasý skyboxlarýn geriye doðru hareket etmesi)
            skybox2.transform.position += new Vector3(length * 2, 0);   // Skybox2 diðer Skybox1'ýn bitiþiðine ekler
        }
    }
    public void CreatBlock()
    {   // Block oluþturma
        blocks = new GameObject[totalBlock];    // Kaç adet block olacaðý seçilir
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i] = Instantiate(block, new Vector2(-20, -20), Quaternion.identity); // Toplam Block sayýsý kadar block objesi ekler ve konumlarýný -20, -20 eþitler
            Rigidbody2D blockRb = blocks[i].AddComponent<Rigidbody2D>();   // Block objelerine Rigidbody compenenti ekler
            blockRb.gravityScale = 0;  // Yerçekimini 0 yapar
            blockRb.velocity = new Vector2(-skyboxSpeed, 0);    // Block objeisne hareket hýzý verir - Skboy hareket hýzý ile ayný olur
        }
    }
    void CreatBlockTime()
    {   // Blocklarýn oluþturulma süreleri
        changeTime += Time.deltaTime;   // Time.deltaTime deðerine göre "changeTime" deðeri artar 
        if (changeTime > 2f)    // 2 saniyede bir block konumunu belirler
        {
            changeTime = 0; //"changeTime" deðri 2f olursa deðeri sýfýrla
            float blockAxisY = Random.Range(-0.30f, 1.10f);  // Blocklarýn Y eksenindeki random pozisyonunu belirler
            blocks[counter].transform.position = new Vector3(15, blockAxisY);    // Blocklarýn X ekseninde  pozisyonunu belirler
            counter++;
            if (counter >= blocks.Length)
            {   // Pozisyonu deðiþen block sayýsý toplam block sayýsna eþit olduðunda "counter" deðerini sýfýrla
                counter = 0;
            }
        }
    }
    public void GameOver()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            //blocks[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0); // Blocklarýn hýzýný 0 yapar - 1.Yöntem
            blocks[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Blocklarýn hýzýný 0 yapar - 2.Yöntem
            skybox1Rb.velocity = Vector2.zero;  // Skybox'larýn hýzýný sýfýrlar
            skybox2Rb.velocity = Vector2.zero;  // Skybox'larýn hýzýný sýfýrlar
        }
        gameOverControl = false;
    }
}
