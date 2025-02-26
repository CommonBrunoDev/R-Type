using UnityEngine;

[CreateAssetMenu(fileName = "EntityStatSO", menuName = "Scriptable Objects/EnemyStatSO")]
public class EntityStatSO : ScriptableObject
{
    [SerializeField] EntityType type = EntityType.None;
    [SerializeField] float health = 1f;
    [SerializeField] float accelerationSpeed = 10f;
    [SerializeField] float maxSpeed = 10f;
    public EntityType Type { get { return type; } }
    public float Health { get { return health; } }
    public float AccelerationSpeed { get { return accelerationSpeed; } }
    public float MaxSpeed { get { return maxSpeed; } }
}
