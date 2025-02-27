using System;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI scoreText;
    public GameObject resultText;
    public GameObject player;
    private int coinCount;
    private int score;
    public Camera cam;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        coinCount = 0;
        score = 0;
        SetCoinCount();
        SetScoreText();
        resultText.SetActive(false);
        CharacterControllerMar.OnBlockInteractions += OnOnBlockInteractions;
    }

    private void OnOnBlockInteractions(int points, int coins)
    {
        score = score + points;
        coinCount = coinCount + coins;
        SetScoreText();
        SetCoinCount();
    }
    
    //Update coin count
    void SetCoinCount()
    {
        coinText.text = "x" + coinCount.ToString();
    }
    
    //Update Score
    void SetScoreText()
    {
        scoreText.text = score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        int timeLeft = 100 - (int)(Time.time);
        timerText.text = $"Time: {timeLeft}";
        brickRaycast();
        qBlockRaycast();

        if (timeLeft == 0)
        {
            timeLeft = 0;
            Destroy(GameObject.FindWithTag("Player"));
            resultText.SetActive(true);
            resultText.GetComponent<TextMeshProUGUI>().text = "Time Up!";
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            coinCount = coinCount + 1;
            score = score + 100;
            SetCoinCount();
            SetScoreText();
        }
    }

    void brickRaycast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Input.mousePosition;

            //create a ray that goes through the screenPosition using a camera
            Ray cursorRay = cam.ScreenPointToRay(screenPos);
            bool rayHitSomething = Physics.Raycast(cursorRay, out RaycastHit screenHitInfo);

            if (rayHitSomething && screenHitInfo.transform.gameObject.CompareTag("Brick"))
            {
                Destroy(screenHitInfo.transform.gameObject);
            }
        }
    }
    
    void qBlockRaycast()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 screenPos = Input.mousePosition;

            //create a ray that goes through the screenPosition using a camera
            Ray cursorRay = cam.ScreenPointToRay(screenPos);
            bool rayHitSomething = Physics.Raycast(cursorRay, out RaycastHit screenHitInfo);

            if (rayHitSomething && screenHitInfo.transform.gameObject.CompareTag("QBlock"))
            {
                coinCount = coinCount + 1;
                
                SetCoinCount();
            }
        }
    }

}
