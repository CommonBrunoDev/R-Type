using Unity.VisualScripting;
using UnityEngine;
public interface IEntityDamageable
{
    public int Health { get; set; }
    void TakeDamage();
    void Explode();
}
public interface IEntityMovement
{
    Vector2 MovementSpeed { get; }
    void Move(Transform transform, EntityStatSO stats);
}

public class PlayerMovement : IEntityMovement
{
    public Vector2 MovementSpeed { get; private set; } = Vector2.zero;
    public void Move(Transform transform, EntityStatSO stats)
    {
        Vector2 move = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        move.x = MovementSpeed.x + stats.AccelerationSpeed * move.x;
        move.y = MovementSpeed.y + stats.AccelerationSpeed * move.y;

        if (Mathf.Abs(move.x) >= stats.MaxSpeed) { move.x = move.x > 0 ? stats.MaxSpeed : -1 * stats.MaxSpeed; }
        if (Mathf.Abs(move.y) >= stats.MaxSpeed) { move.y = move.y > 0 ? stats.MaxSpeed : -1 * stats.MaxSpeed; }

        Vector2 stop = new Vector2(1, 1);
        if ((move.x > 0f && transform.position.x >= 8f) || (move.x < 0f && transform.position.x <= -8f)) { stop.x = 0f; }
        if ((move.y > 0f && transform.position.y >= 4.5f) || (move.y < 0f && transform.position.y <= -4.5f)) { stop.y = 0f; }

        if (move.x == MovementSpeed.x && move.y == MovementSpeed.y)
        { MovementSpeed = new Vector2(MovementSpeed.x * 0.75f, MovementSpeed.y * 0.75f); }
        else
        { MovementSpeed = new Vector2(move.x * stop.x, move.y * stop.y); }
        
        transform.Translate(new Vector3(MovementSpeed.x, MovementSpeed.y, 0) * Time.deltaTime);
    }
}
public class ChaseMovement : IEntityMovement
{
    public Vector2 MovementSpeed { get; private set; } = Vector2.zero;
    public void Move(Transform transform, EntityStatSO stats)
    {
        Vector2 move = Vector2.zero;
        move.x = transform.position.x >= Player.Instance.transform.position.x ? -1 : 1;
        move.y = transform.position.y >= Player.Instance.transform.position.y ? -1 : 1;

        move = move.normalized * stats.AccelerationSpeed + MovementSpeed;
        move = new Vector2(Mathf.Clamp(move.x, -1 * stats.MaxSpeed, stats.MaxSpeed), Mathf.Clamp(move.y, -1 * stats.MaxSpeed, stats.MaxSpeed));
        MovementSpeed = move;

        transform.Translate(new Vector3(MovementSpeed.x, MovementSpeed.y, 0) * Time.deltaTime);
    }
}

public class SentryMovement : IEntityMovement
{
    public Vector2 MovementSpeed { get; private set; } = Vector2.zero;
    public void Move(Transform transform, EntityStatSO stats)
    {
        Vector2 move = Vector2.zero;
        move.x = transform.position.x > 6 ? -1 : 1;
        move.y = transform.position.y >= Player.Instance.transform.position.y ? -1 : 1;

        move = move.normalized * stats.AccelerationSpeed + MovementSpeed;
        if (transform.position.x + move.x > 6) { move.x = 6 - transform.position.x; }
        else if (transform.position.x - move.x < 6) { move.x = 6 - transform.position.x; }
        move = new Vector2(Mathf.Clamp(move.x, -1 * stats.MaxSpeed, stats.MaxSpeed), Mathf.Clamp(move.y, -1 * stats.MaxSpeed, stats.MaxSpeed));

        transform.Translate(new Vector3(move.x, move.y, 0) * Time.deltaTime);
    }
}

public class NoMovement : IEntityMovement
{
    public Vector2 MovementSpeed { get; private set; } = Vector2.zero;
    public void Move(Transform transform, EntityStatSO stats){ Debug.Log("No movement test"); }
}

public class WalkingMovement : IEntityMovement
{
    public Vector2 MovementSpeed { get; private set; } = Vector2.zero;
    public void Move(Transform transform, EntityStatSO stats)
    {
        float xMove = -1;

        xMove = xMove * stats.AccelerationSpeed + MovementSpeed.x;
        xMove = Mathf.Clamp(xMove, -1 * stats.MaxSpeed, stats.MaxSpeed);

        transform.Translate(new Vector3(xMove, MovementSpeed.y, 0) * Time.deltaTime);
    }
}