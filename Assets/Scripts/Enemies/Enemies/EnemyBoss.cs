using UnityEngine;
using Unity.VisualScripting;

public class EnemyBoss : Enemy
{
    public float actionTimeMin = 6;
    public float actionTimeMax = 8;
    private float actionTimer = 0;

    public Bullet[] bulletMagazine = new Bullet[3];
    public Bullet bulletRef;

    public Bullet[] diskMagazine = new Bullet[6];
    public Bullet diskRef;

    public float diskCounter = 0;
    public float diskTimer = 0f;
    public float diskDelay = 0.5f;

    public EnemySaw sawRef;
    public EnemySaw winSawRef;

    public float floatDir = 1;

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

        for (int i = 0; i < diskMagazine.Length; i++)
        {
            Bullet b = Instantiate(diskRef);
            b.gameObject.SetActive(false);
            diskMagazine[i] = b;
        }
    }

    void Update()
    {
        base.Update();

        if (GameManager.Instance.gameState != GameState.Paused)
        {
            if (actionTimer < 0)
            {
                BossAction();
                actionTimer = Random.Range(actionTimeMin, actionTimeMax);
            }
            else actionTimer -= Time.deltaTime;

            transform.position += new Vector3(0, floatDir * 0.01f);
            if ((transform.position.y > 1 && floatDir == 1)
            || (transform.position.y < -1 && floatDir == -1))
            { floatDir *= -1; }

            if (diskCounter > 0)
            {
                if (diskTimer < 0)
                {
                    ShootDisk();
                    diskCounter--;
                    diskTimer = diskDelay;
                }
                else { diskTimer -= Time.deltaTime; }
            }
        }
    }

    void BossAction()
    {
        var rndAction = Random.Range(0,3);
        switch (rndAction)
        {
            case 0:
                for (int i = 0;i < bulletMagazine.Length;i++)
                { ShootRandomBullet(); }
                break;


            case 1:
                for (int i = 0; i < 2; i++)
                {
                    var s = Instantiate(sawRef);
                    s.transform.position = new Vector3(transform.position.x + 2.5f, 3 + -6 * i);
                }

                break;


            case 2:
                var rndN = Random.Range(3,7);
                diskCounter = rndN;
                diskTimer = diskDelay;
                break;
        }
    }

    void ShootRandomBullet()
    {
        var check = 0;
        while (check < 99)
        {
            var rnd = Random.Range(0, bulletMagazine.Length);
            if (!bulletMagazine[rnd].gameObject.activeSelf)
            {
                bulletMagazine[rnd].gameObject.SetActive(true);
                bulletMagazine[rnd].gameObject.transform.position = new Vector3(transform.position.x, Random.Range(-2f,2f));
                bulletMagazine[rnd].gameObject.GetComponentInChildren<SpriteRenderer>().transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
                bulletMagazine[rnd].gameObject.GetComponent<Bullet>().direction = Vector3.left;
                bulletMagazine[rnd].gameObject.GetComponent<Bullet>().parent = this.gameObject;

                check = 100;
            }
            else { check++; }
        }
    }
    void ShootDisk()
    {
        var check = 0;
        while (check < 40)
        {
            var rnd = Random.Range(0, diskMagazine.Length);
            if (!diskMagazine[rnd].gameObject.activeSelf)
            {
                diskMagazine[rnd].gameObject.SetActive(true);
                diskMagazine[rnd].gameObject.transform.position = transform.position;
                diskMagazine[rnd].gameObject.GetComponent<Bullet>().direction = (Player.Instance.transform.position - diskMagazine[rnd].gameObject.transform.position).normalized;                   
                diskMagazine[rnd].gameObject.GetComponent<Bullet>().parent = this.gameObject;

                check = 40;
            }
            else { check++; }
        }
    }
}
