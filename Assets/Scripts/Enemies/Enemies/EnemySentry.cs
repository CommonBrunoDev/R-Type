using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemySentry : Enemy
{
    public Bullet[] bulletMagazine = new Bullet[3];
    public Bullet bulletRef;

    public float shootDelay = 1.5f;
    private float shootTimer = 1.5f;

    private void Awake()
    {
        base.Awake();

        for (int i = 0; i < bulletMagazine.Length; i++)
        {
            Bullet b = Instantiate(bulletRef);
            b.gameObject.SetActive(false);
            bulletMagazine[i] = b;
            b.transform.parent = GameObject.Find("BulletHolder").transform;
        }
    }

    void Update()
    {
        base.Update();

        if (GameManager.Instance.gameState != GameState.Paused)
        {
            if (shootTimer < 0)
            {
                ShootBullet();
                shootTimer = shootDelay;
            }
            else shootTimer -= Time.deltaTime;

            if (transform.position.x < -11) { this.gameObject.SetActive(false); }
        }
    }

    void ShootBullet()
    {
        var check = 0;
        while (check < 99)
        {
            var rnd = Random.Range(0, bulletMagazine.Length);
            if (!bulletMagazine[rnd].gameObject.activeSelf)
            {
                bulletMagazine[rnd].gameObject.SetActive(true);
                bulletMagazine[rnd].gameObject.transform.position = transform.position;
                bulletMagazine[rnd].gameObject.GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                bulletMagazine[rnd].gameObject.GetComponent<Bullet>().direction = Vector3.left;
                bulletMagazine[rnd].gameObject.GetComponent<Bullet>().parent = this.gameObject;

                check = 100;
            }
            else { check++; }
        }
    }
}