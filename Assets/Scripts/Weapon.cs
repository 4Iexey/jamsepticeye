using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [Header("Stats")]
    [SerializeField] private int level = 1;
    [SerializeField] private float damage = 1;
    [SerializeField] private float weight = 10f;

    [Header("Config")]
    [SerializeField] private float speedDamageTreshold = 1f;

    public int Level => level;
    public float Damage => ComputeDamage();
    public float Weight => weight;

    private ICreature wieldingCreature;
    private HingeJoint2D joint;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        joint = GetComponent<HingeJoint2D>();
        if (joint.connectedBody != null)
        {
            wieldingCreature = joint.connectedBody.gameObject.GetComponent<ICreature>();
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ICreature creature = other.gameObject.GetComponent<ICreature>();
        if (wieldingCreature == null && rb.linearVelocity.magnitude < speedDamageTreshold && rb.angularVelocity < speedDamageTreshold && creature != null && creature.Level >= level)
        {
            wieldingCreature = other.gameObject.GetComponent<ICreature>();
            joint.connectedBody = other.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    public float ComputeDamage()
    {
        if (wieldingCreature != null || rb.linearVelocity.magnitude > speedDamageTreshold || rb.angularVelocity > speedDamageTreshold)
        {
            return damage;
        }
        return 0;
    }
}
