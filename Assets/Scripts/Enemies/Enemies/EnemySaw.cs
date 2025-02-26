using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemySaw : Enemy
{
    [SerializeField] private SpriteRenderer sawBody;
    [SerializeField] private float rotationSpeed;
    void Update()
    {
        base.Update();

        if (GameManager.Instance.gameState != GameState.Paused)
        {
            if (dead) rotationSpeed -= 1.5f;
            sawBody.transform.Rotate(new Vector3(0, 0, rotationSpeed));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.rigidbody.tag);
        if (collision.rigidbody.CompareTag("Player"))
        {collision.rigidbody.GetComponent<IEntityDamageable>().TakeDamage(); }
    }
}
