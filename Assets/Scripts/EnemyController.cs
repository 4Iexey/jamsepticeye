using UnityEngine;

public class EnemyController : MonoBehaviour, ICreature
{
    [Header("Stats")]
    [SerializeField] private int level = 1;
    [SerializeField] private float health = 10f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxSwingSpeed = 10f;
    [SerializeField] private float swingAcceleration = 10f;
    [SerializeField] private float weight = 10f;

    [Header("Behavior")]
    [SerializeField] private float wanderRadius = 10;
    [SerializeField] private float huntingRadius = 10;
    [SerializeField] private float fleeingRadius = 10;

    public int Level => level;
    public float Health => health;
    public float MoveSpeed => moveSpeed;
    public float MaxSwingSpeed => maxSwingSpeed;
    public float SwingAcceleration => swingAcceleration;
    public float Weight => weight;

    [SerializeField] private Vector2 target;
    [SerializeField] private Transform huntingTarget;
    private Rigidbody2D rb; 
    private float targetThreshold = 0.25f;
    private LayerMask obstacles;
    private float originalHealth;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        obstacles = LayerMask.GetMask("Default");
        originalHealth = health;
    }

    void Start()
    {
        target = GetWanderingTarget();
    }

    void FixedUpdate()
    {
        UpdateTarget();
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var weapon = collision.gameObject.GetComponent<IWeapon>();
        if (weapon != null)
        {
            float computedDamage = collision.relativeVelocity.magnitude * weapon.Damage;
            TakeDamage(computedDamage);
            huntingTarget = collision.gameObject.GetComponent<HingeJoint2D>()?.connectedBody.gameObject.transform;
        }
    }

    public void TakeDamage(float computedDamage)
    {
        health = Mathf.Max(0, health - computedDamage);
        if (health == 0)
        {
            gameObject.SetActive(false);
            Destroy(gameObject, 0.5f);
        }
    }

    private void UpdateTarget()
    {
        if (huntingTarget != null && Vector2.Distance(transform.position, huntingTarget.position) > Mathf.Min(huntingRadius, fleeingRadius))
        {
            huntingTarget = null;
        }

        // wandering state
        if ((huntingTarget == null || health < originalHealth / 4) && Vector2.Distance(transform.position, target) < targetThreshold)
        {
            // Debug.Log("wandering");
            target = GetWanderingTarget();
        }

        // fleeing state
        else if (huntingTarget != null && health < originalHealth / 4)
        {
            // Debug.Log("fleeing");
            target = (transform.position - huntingTarget.position).normalized * fleeingRadius * 2;
        }

        // hunting state
        else if (huntingTarget != null)
        {
            // Debug.Log("hunting");
            target = huntingTarget.position;
        }
    }

    private Vector2 GetWanderingTarget()
    {
        for (int i = 0; i < 100; i++)
        {
            Vector2 wanderPoint = (Vector2)transform.position + Random.insideUnitCircle * wanderRadius;

            var blocked = Physics2D.OverlapCircle(wanderPoint, 0.15f, obstacles);
            if (blocked) continue;

            return wanderPoint;
        }
        return transform.position;
    }
}
