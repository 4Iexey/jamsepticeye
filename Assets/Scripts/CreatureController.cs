using UnityEngine;

public class CreatureController : MonoBehaviour, ICreature
{
    [Header("Stats")]
    [SerializeField] private int level = 1;
    [SerializeField] private float health = 10f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxSwingSpeed = 10f;
    [SerializeField] private float swingAcceleration = 10f;
    [SerializeField] private float weight = 10f;

    [Header("Weapon")]
    [SerializeField] private HingeJoint2D joint = null;

    public int Level => level;
    public float Health => health;
    public float MoveSpeed => moveSpeed;
    public float MaxSwingSpeed => maxSwingSpeed;
    public float SwingAcceleration => swingAcceleration;
    public float Weight => weight;
    public HingeJoint2D Joint
    {
        get => joint;
        set => joint = value;
    }

    public float originalHealth;
    private EnemyController enemyController;

    void Awake()
    {
        originalHealth = health;
        enemyController = GetComponent<EnemyController>();
    }

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
            if (enemyController != null) enemyController.huntingTarget = collision.gameObject.transform;
        }
    }

    public void TakeDamage(float incomingDamage)
    {
        health = Mathf.Max(0, health - incomingDamage);
        if (health <= 0)
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 0.5f);
        }
    }
}
