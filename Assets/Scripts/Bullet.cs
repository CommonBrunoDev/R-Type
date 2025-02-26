using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 3.0f;
    public Vector3 direction = Vector3.zero;
    public GameObject parent;

    private void Update()
    {
        if (GameManager.Instance.gameState != GameState.Paused)
        {
            //Deactivates the bullet if it goes offscreen
            transform.Translate(direction * speed * Time.deltaTime);
            if ((Mathf.Clamp(transform.position.x, -9.5f, 9.5f) != transform.position.x)
            || (Mathf.Clamp(transform.position.y, -6f, 6f) != transform.position.y))
            { this.gameObject.SetActive(false); }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        if ((collision.CompareTag("Enemy") || collision.CompareTag("Player")) && collision.gameObject != parent)
        {
            collision.gameObject.GetComponent<IEntityDamageable>().TakeDamage();
            this.gameObject.SetActive(false);
        }
        else if (collision.CompareTag("Wall"))
        {this.gameObject.SetActive(false);}
    }
}
