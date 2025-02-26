using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Image screenDamage;

    public Image hpBar;
    public Sprite[] hpSprites = new Sprite[4];

    public int totalScore = 0;
    public int survivalPoints = 10;
    public float survivalTime = 2f;
    public float survivalTimer = 0f;
    public TextMeshProUGUI scoreText;

    public Image startingImage;
    public Image gameoverImage;
    public Image restartImage;

    public int gameoverPhase = 0;

    private static HUD instance;
    public static HUD Instance {  get { return instance; } }

    private void Awake()
    {
        instance = this;

        survivalTimer = survivalTime;
        SetHp(4);

        startingImage = transform.Find("Start").GetComponent<Image>() ;
        gameoverImage = transform.Find("Gameover").GetComponent<Image>();
        restartImage = transform.Find("Restart").GetComponent<Image>();
    }
    private void Update()
    {
        if (GameManager.Instance.gameState == GameState.Play && startingImage.transform.position.x > -1600)
        {
            var movementVec = new Vector3(10, 0, 0);
            startingImage.transform.localPosition -= movementVec;
            restartImage.transform.localPosition -= movementVec;
        }
        if (screenDamage != null)
        {
            if (screenDamage.color.a > 0 && screenDamage.color.a * 255 <= 132)
                SetTransparency(screenDamage.color.a * 255 - 3f);
        }

        if (GameManager.Instance.gameState == GameState.Dead) 
        {
            if ( gameoverPhase == 1) 
            {
                gameoverImage.transform.localPosition = new Vector3(0, gameoverImage.transform.localPosition.y * 0.92f);      
                if (gameoverImage.transform.localPosition.y < 1) 
                { 
                    gameoverPhase = 2;  
                    restartImage.transform.position = new Vector3(0, -650);
                }
            }
            else if (gameoverPhase == 2)
            {
                restartImage.transform.localPosition = new Vector3(0, restartImage.transform.localPosition.y * 0.96f) ;
                if (restartImage.transform.localPosition.y > -200) { gameoverPhase = 0;}
            }
        }

        if (GameManager.Instance.gameState == GameState.Play)
        {
            if (survivalTimer < 0)
            {
                survivalTimer = survivalTime;
                AddScore(survivalPoints);
            }
            else survivalTimer -= Time.deltaTime;
        }
    }

    public void SetTransparency(float transp)
    {
        screenDamage.color = new Color(screenDamage.color.r, screenDamage.color.g, screenDamage.color.b, transp/255);
    }

    public void SetHp(int hp)
    {
        if (hp > 0)
        {
            hpBar.gameObject.SetActive(true);
            hpBar.sprite = hpSprites[hp - 1];
        }
        else
        { hpBar.gameObject.SetActive(false); }
    }

    public void AddScore(int score)
    {
        if (GameManager.Instance.gameState != GameState.Dead)
        {
            totalScore += score;
            scoreText.text = (totalScore).ToString();
        }
    }

    public void LowerGameover()
    {

    }
}
