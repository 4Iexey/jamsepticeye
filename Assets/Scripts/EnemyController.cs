using UnityEngine;

public enum EnemyState
{
    Wandering,
    Hunting,
    Fleeing,
}

public class EnemyController : MonoBehaviour, ICreature
{
    [Header("Stats")]
    [SerializeField] private int _level = 1;
    [SerializeField] private float _health = 10f;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _maxSwingSpeed = 10f;
    [SerializeField] private float _swingAcceleration = 10f;
    [SerializeField] private float _weight = 10f;

    public int Level => _level;
    public float Health => _health;
    public float MoveSpeed => _moveSpeed;
    public float MaxSwingSpeed => _maxSwingSpeed;
    public float SwingAcceleration => _swingAcceleration;
    public float Weight => _weight;

    private EnemyState state = EnemyState.Wandering;

    void Start()
    {

    }

    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var weapon = collision.gameObject.GetComponent<IWeapon>();
        if (weapon != null)
        {
            float computedDamage = collision.relativeVelocity.magnitude * weapon.Damage;
            TakeDamage(computedDamage);
        }
    }

    public void TakeDamage(float computedDamage)
    {
        _health = Mathf.Max(0, _health - computedDamage);
    }
}
