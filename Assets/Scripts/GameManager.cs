using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameState gameState;

    private static GameManager instance;
    public static GameManager Instance {  get { return instance; } }

    private void Awake()
    {
        instance = this;
        gameState = GameState.Menu;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (gameState == GameState.Menu)
            { gameState = GameState.Play; }
            if (gameState == GameState.Paused)
            { 
                gameState = GameState.Play;
                HUD.Instance.restartImage.transform.position = new Vector3(0, -650, 0);
            }
            if (gameState == GameState.Dead)
            {SceneManager.LoadScene(SceneManager.GetActiveScene().name);}
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log(gameState);
            if (gameState == GameState.Play)
            { 
                gameState = GameState.Paused;
                HUD.Instance.screenDamage.color = new Color(0.5f, 0.5f, 0.5f, 0.6f);
            }
            else if (gameState == GameState.Paused)
            { 
                gameState = GameState.Play;
                HUD.Instance.screenDamage.color = new Color(0.5f, 0, 0, 0);
            }
        }
    }
}
