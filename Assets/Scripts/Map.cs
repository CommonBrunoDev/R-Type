using UnityEngine;

public class Map : MonoBehaviour
{
    public float scrollSpeed = 1f;
    private float currentScrollSpeed;
    public bool reachedBoss = false;

    private static Map instance;
    public static Map Instance {  get { return instance; } }

    private void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.Paused)
        {
            if (GameManager.Instance.gameState == GameState.Dead || reachedBoss)
            {
                if (currentScrollSpeed >= 0)
                { currentScrollSpeed -= scrollSpeed * 0.004f; }
            }
            else
            {
                switch (GameManager.Instance.gameState)
                {
                    case GameState.Menu: currentScrollSpeed = 0; break;
                    case GameState.Play: currentScrollSpeed = scrollSpeed; break;
                }
            }
            transform.position = transform.position - new Vector3(currentScrollSpeed, 0, 0);
        }
    }
}
