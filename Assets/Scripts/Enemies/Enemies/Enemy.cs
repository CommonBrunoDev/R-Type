using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IEntityDamageable
{
    [SerializeField] public EntityStatSO stats;
    [SerializeField] protected SpriteRenderer[] sprites;
    protected IEntityMovement movement;

    public bool dead = false;
    protected int hp;
    public int Health 
    {
        get { return hp; }
        set { hp = value; }
    }

    protected void Awake()
    {
        this.gameObject.SetActive(false);
        FindFirstObjectByType<EnemyManager>().enemies.Add(this);

        Health = (int)(stats.Health);
        switch(stats.Type)
        {
            case EntityType.Saw:
                movement = new ChaseMovement();
                break;
            case EntityType.Shooter:
                movement = new SentryMovement();
                break;
            case EntityType.Walker:
                movement = new WalkingMovement();
                break;
            case EntityType.Ceiling: case EntityType.Boss:
                movement = new NoMovement();
                break;
        }
    }
    protected void Update()
    {
        if (dead)
        {
            var c = sprites.Length;
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].color = new Color(sprites[i].color.r, sprites[i].color.g, sprites[i].color.b, sprites[i].color.a - 0.07f);
                if (sprites[i].color.a <= 0) { c--; }
            }
            this.gameObject.transform.localScale += new Vector3(0.04f, 0.04f, 0.04f);
            if (c <= 0) { this.gameObject.SetActive(false); }
        }
        else
        {
            if (GameManager.Instance.gameState != GameState.Paused)
            {
                movement.Move(transform, stats);
                for (int i = 0; i < sprites.Length; i++)
                {
                    if (sprites[i].color.g < 1)
                    { sprites[i].color = sprites[i].color + new Color(0, 0.04f, 0.04f, 0); }
                }
            }
        }
    }
    public void TakeDamage()
    {
        for (int i = 0; i < sprites.Length; i++)
        { sprites[i].color = new Color(1f, 0.4f, 0.4f, sprites[i].color.a); }

        Health--;
        if (Health <= 0)
        { Explode(); }
    }
    public void Explode()
    {
        this.gameObject.GetComponent<Collider2D>().enabled = false;
        dead = true;
        HUD.Instance.AddScore(100);
    }
}
