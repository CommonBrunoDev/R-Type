using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[System.Serializable]
public struct FirePoint
{
    public Vector3 position;
    public Vector2 direction;
    public float angle;
}
public class EnemyCeiling : Enemy
{
    public Bullet[] bulletMagazine = new Bullet[8];
    public Bullet bulletRef;
    public FirePoint[] firePoints = new FirePoint[4];

    public float shootDelay = 0.5f;
    private float shootTimer = 0.5f;

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
        while (check < 40)
        {
            var rnd = Random.Range(0, bulletMagazine.Length);
            if (!bulletMagazine[rnd].gameObject.activeSelf)
            {
                FirePoint fp = firePoints[Random.Range(0, firePoints.Length)];
                bulletMagazine[rnd].gameObject.SetActive(true);
                bulletMagazine[rnd].gameObject.transform.position = transform.position + fp.position;
                bulletMagazine[rnd].gameObject.GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, fp.angle));
                bulletMagazine[rnd].gameObject.GetComponent<Bullet>().direction = fp.direction.normalized;
                bulletMagazine[rnd].gameObject.GetComponent<Bullet>().parent = this.gameObject;

                check = 100;
            }
            else { check++; }
        }
    }
}