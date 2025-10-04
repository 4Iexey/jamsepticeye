using UnityEngine;

public class Weapon : MonoBehaviour, IWeapon
{
    [Header("Stats")]
    [SerializeField] private int _level = 1;
    [SerializeField] private float _damage = 1;
    [SerializeField] private float _weight = 10f;

    public int Level => _level;
    public float Damage => ComputeDamage();
    public float Weight => _weight;

    ICreature wieldingCreature;
    HingeJoint2D joint;

    void Awake()
    {
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
        if (wieldingCreature == null && creature.Level >= _level)
        {
            wieldingCreature = other.gameObject.GetComponent<ICreature>();
            joint.connectedBody = other.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    public float ComputeDamage()
    {
        if (wieldingCreature != null)
        {
            return _damage;
        }
        return 0;
    }
}
