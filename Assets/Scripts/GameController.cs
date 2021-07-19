using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject skybox1;
    public GameObject skybox2;

    Rigidbody2D skybox1Physics;
    Rigidbody2D skybox2Physics;

    public float skyboxSpeed = -1.5f;
    float length=0;
    float changeTime=0;
    int counter = 0;
    bool gameOverControl = true;

    public GameObject block;
    public int totalBlock;
    GameObject[] blocks;    
    void Start()
    {
        skybox1Physics = skybox1.GetComponent<Rigidbody2D>();
        skybox2Physics = skybox2.GetComponent<Rigidbody2D>();

        skybox1Physics.velocity = new Vector2(-skyboxSpeed, 0);    // Skybox'lar geriye doðru hareket eder
        skybox2Physics.velocity = new Vector2(-skyboxSpeed, 0);    // Skybox'lar geriye doðru hareket eder

        length = skybox1.GetComponent<BoxCollider2D>().size.x;  // Sybox 'da BoxCollider'in uzunluðunu deðiþkene atar

        creatBlock();   // Engel oluþturma
    }
    void Update()
    {
        if (gameOverControl)   
        {
            skyboxCreate();     // Skybox konumu deðiþtirir
            creatBlockTime();   // Engellerin pozisyonunu belirlenir
        }
        else
        {

        }

        
    }
    void skyboxCreate()
    {
        // Skybox konumu deðiþtirir
        if (skybox1.transform.position.x <= -length)
        {
            skybox1.transform.position += new Vector3(length * 2, 0);   // Skybox1 diðer Skybox2'ýn bitiþiðine ekler
        }
        if (skybox2.transform.position.x <= -length)
        {
            skybox2.transform.position += new Vector3(length * 2, 0);   // Skybox2 diðer Skybox1'ýn bitiþiðine ekler
        }
    }
    public void creatBlock()
    {        
        // Engel oluþturma
        blocks = new GameObject[totalBlock];    // Kaç adet engel olacaðý seçilir
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i] = Instantiate(block, new Vector2(-20, -20), Quaternion.identity);
            Rigidbody2D blockPhysics = blocks[i].AddComponent<Rigidbody2D>();   // Engel objelerine Rigidbody compenenti ekler
            blockPhysics.gravityScale = 0;  // Yerçekimini 0 yapar
            blockPhysics.velocity = new Vector2(-skyboxSpeed, 0);    // Engellere hareket hýzý verir
        }
    }
    void creatBlockTime()
    {
        changeTime += Time.deltaTime;
        if (changeTime > 2f)    // 2 saniyede bir engel konumunu belirler
        {
            changeTime = 0;
            float blockAxisY = Random.Range(-0.30f, 1.10f);  // Engellerin Y eksenindeki random konumlarýný belirler
            blocks[counter].transform.position = new Vector3(15, blockAxisY);    // Engellerin X ekseninde konumunu pozisyonunu belirler
            counter++;
            if (counter >= blocks.Length)
            {
                counter = 0;
            }
        }
    }
    public void gameOver()
    {
        for (int i = 0; i < blocks.Length; i++)
        {
            //blocks[i].GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0); // Engellerin hýzýný 0 yapar - 1.Yöntem
            blocks[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero; // Engellerin hýzýný 0 yapar - 2.Yöntem
            skybox1Physics.velocity = Vector2.zero; // Skybox'larýn hýzýný yapar
            skybox2Physics.velocity = Vector2.zero;
        }
        gameOverControl = false;
    }
}
