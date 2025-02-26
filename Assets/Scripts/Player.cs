using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour, IEntityDamageable
{
    [SerializeField] private HUD hudReference;
    [SerializeField] private EntityStatSO stats;
    private IEntityMovement movement = new PlayerMovement();

    [SerializeField] private float reloadDelay = 5f;
    private float reloadTimer = 0f;
    private bool autofireActive = false;
    private float activateAutofire = 0;

    [SerializeField] private Bullet bullet;
    private Bullet[] bulletMagazine = new Bullet[8];

    [SerializeField] private Transform shootPoint;

    private static Player instance;
    public static Player Instance { get { return instance; } }

    [SerializeField] private float invTimer;
    [SerializeField] private float invFrames;

    private float deathTimer = 0;

    private int health;
    public int Health { get { return health; } set { health = value; } }

    private void Awake()
    {
        instance = this;
        health = (int)(stats.Health);

        for (int i = 0; i < bulletMagazine.Length; i++)
        {
            Bullet b = Instantiate(bullet);
            b.gameObject.SetActive(false);
            bulletMagazine[i] = b;
            b.transform.parent = GameObject.Find("BulletHolder").transform;
        }
    }
    public void Update()
    {
        if (GameManager.Instance.gameState == GameState.Play)
        {
            movement.Move(this.transform, stats);

            HandleShooting();
            HandleTimers();
            if (transform.position.x < -9) { TakeDamage(); }
        }


        if (GameManager.Instance.gameState == GameState.Dead)
        {
            deathTimer += Time.deltaTime;
            if (deathTimer > 2)
            {
                if (deathTimer < 2 + Time.deltaTime * 20)
                {
                    transform.localScale *= 1.1f;
                    this.gameObject.GetComponentInChildren<SpriteRenderer>().color -= new Color(0, 0, 0, 0.05f);
                }
                else 
                {
                    hudReference.SetTransparency(128);
                    hudReference.gameoverPhase = 1;
                    this.gameObject.SetActive(false); 
                }
            }
        }
    }

    void HandleShooting()
    {
        //If the button has just been pressed deactivates autofire
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetMouseButtonDown(0))
        { autofireActive = false; activateAutofire = 0.5f; }

        var shootPressed = (Input.GetKey(KeyCode.Z) || Input.GetMouseButton(0));

        if (shootPressed && activateAutofire > 0)
        {
            activateAutofire -= Time.deltaTime;
            if (activateAutofire <= 0f)
            { autofireActive = true; }
        }
        else
        { activateAutofire = -1; }

        //Checks if (Shoot button is pressed or Autofire is active) and if the ship has reloaded
        if (( shootPressed || autofireActive) && reloadTimer <= 0f)
        { Shoot(); }
    }

    void HandleTimers()
    {
        if (reloadTimer > 0f){ reloadTimer -= Time.deltaTime; }
        if (invTimer > 0f) { invTimer -= Time.deltaTime; }
    }

    void Shoot()
    {
        var check = 0;
        while (check < 99)
        {
            var rnd = Random.Range(0, bulletMagazine.Length);
            if (!bulletMagazine[rnd].gameObject.activeSelf)
            {
                bulletMagazine[rnd].gameObject.SetActive(true);
                bulletMagazine[rnd].gameObject.transform.position = shootPoint.position;
                bulletMagazine[rnd].gameObject.transform.rotation = Quaternion.Euler(Vector3.zero);
                bulletMagazine[rnd].gameObject.GetComponent<Bullet>().direction = Vector3.right;
                bulletMagazine[rnd].gameObject.GetComponent<Bullet>().parent = this.gameObject;
                check = 100;
            }
            else { check++; }
        }

        reloadTimer = reloadDelay;
    }

    public void TakeDamage()
    {
        if (invTimer <= 0)
        {
            health--;
            hudReference.SetHp(health);
            invTimer = invFrames;
            if (health > 0)
            { hudReference.SetTransparency(128); }
            else
            {
                Explode();
            }
        }
    }

    public void Explode()
    {
        this.gameObject.GetComponent<CircleCollider2D>().enabled = false;
        hudReference.SetTransparency(132);
        GameManager.Instance.gameState = GameState.Dead;
        this.gameObject.GetComponentInChildren<SpriteRenderer>().sortingOrder = 1;
    }
}
