using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject skybox1;  // Skybox1
    public GameObject skybox2;  // Skybox2

    Rigidbody2D skybox1Rb;  
    Rigidbody2D skybox2Rb;  

    public float skyboxSpeed = -1.5f;   // Skybox hareket h�z�
    float length=0; // Skyboxlar�n Size(x ekseni) de�erleri
    float changeTime=0; // Block objelerinin eklenme s�resini tutar
    int counter = 0;    // Pozisyonu de�i�en block say�s�
    bool gameOverControl = true;

    public GameObject block;    // Block objesi
    public int totalBlock;  // Toplam block say�s�
    GameObject[] blocks;    // Blocklar�n tutuldu�u dizi    
    void Start()
    {
        skybox1Rb = skybox1.GetComponent<Rigidbody2D>();    // Skybox1'in rigidbody componentine eri�ir - Skybox hareketi i�in
        skybox2Rb = skybox2.GetComponent<Rigidbody2D>();    // Skybox2'nin rigidbody componenti eri�ir - Skybox hareketi i�in

        skybox1Rb.velocity = new Vector2(-skyboxSpeed, 0);    // Skybox1 geriye do�ru hareket eder
        skybox2Rb.velocity = new Vector2(-skyboxSpeed, 0);    // Skybox2 geriye do�ru hareket eder

        length = skybox1.GetComponent<BoxCollider2D>().size.x;  // Skybox'�n BoxCollider componentinin "x" size uzunlu�unu

        CreatBlock();   // Block olu�turma
    }
    void Update()
    {
        if (gameOverControl)   
        {
            SkyboxCreate();     // Skybox konumu de�i�tirir
            CreatBlockTime();   // Olu�turulan Block objelerinin pozisyonu belirlenir
        }        
    }
    void SkyboxCreate()
    {   // Skybox konumu de�i�tirir
        if (skybox1.transform.position.x <= -length)
        {   // Skybox1'in "x" ekseninde pozisyonu lenght de�erinden k���k e�it ise konumunu de�i�
            // Skybox1 boyutu kadar mesafe gitti�i zman konumunu de��irtir  (-lenght olmas� skyboxlar�n geriye do�ru hareket etmesi)
            skybox1.transform.position += new Vector3(length * 2, 0);   // Skybox1 di�er Skybox2'�n biti�i�ine ekler
        }
        if (skybox2.transform.position.x <= -length)
        {   // Skybox2'in "x" ekseninde pozisyonu lenght de�erinden k���k e�it ise konumunu de�i�
            // Skybox2 boyutu kadar mesafe gitti�i zman konumunu de��irtir  (-lenght olmas� skyboxlar�n geriye do�ru hareket etmesi)
            skybox2.transform.position += new Vector3(length * 2, 0);   // Skybox2 di�er Skybox1'�n biti�i�ine ekler
        }
    }
    public void CreatBlock()
    {   // Block olu�turma
        blocks = new GameObject[totalBlock];    // Ka� adet block olaca�� se�ilir
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i] = Instantiate(block, new Vector2(-20, -20), Quaternion.identity); // Toplam Block say�s� kadar block objesi ekler ve konumlar�n� -20, -20 e�itler
            Rigidbody2D blockRb = blocks[i].AddComponent<Rigidbody2D>();   // Block objelerine Rigidbody compenenti ekler
            blockRb.gravityScale = 0;  // Yer�ekimini 0 yapar
            blockRb.velocity = new Vector2(-skyboxSpeed, 0);    // Block objeisne hareket h�z� verir - Skboy hareket h�z� ile ayn� olur
        }
    }
    void CreatBlockTime()
    {   // Blocklar�n olu�turulma s�releri
        changeTime += Time.deltaTime;   // Time.deltaTime de�erine g�re "changeTime" de�eri artar 
        if (changeTime > 2f)    // 2 saniyede bir block konumunu belirler
        {
            changeTime = 0; //"changeTime" de�ri 2f olursa de�eri s�f�rla
            float blockAxisY = Random.Range(-0.30f, 1.10f);  // Blocklar�n Y eksenindeki random pozisyonunu belirler
            blocks[counter].transform.position = new Vector3(15, blockAxisY);    // Blocklar�n X ekseninde  pozisyonunu belirler
            counter++;
            if (counter >= blocks.Length)
            {   // Pozisyonu de�i�en block say�s� toplam block say�sna e�it oldu�unda "counter" de�erini s�f�rla
                counter = 0;
            }
        }
    }
    public void GameOver()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            //blocks[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0); // Blocklar�n h�z�n� 0 yapar - 1.Y�ntem
            blocks[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Blocklar�n h�z�n� 0 yapar - 2.Y�ntem
            skybox1Rb.velocity = Vector2.zero;  // Skybox'lar�n h�z�n� s�f�rlar
            skybox2Rb.velocity = Vector2.zero;  // Skybox'lar�n h�z�n� s�f�rlar
        }
        gameOverControl = false;
    }
}
