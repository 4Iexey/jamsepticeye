using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Behavior")]
    [SerializeField] private float wanderRadius = 10;
    [SerializeField] private float huntingRadius = 10;
    [SerializeField] private float fleeingRadius = 10;

    [SerializeField] private Vector2 target;
    [SerializeField] public Transform huntingTarget;
    private Rigidbody2D rb;
    private CreatureController creatureController;
    private float targetThreshold = 0.25f;
    private LayerMask obstacles;
    private float originalHealth;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        creatureController = GetComponent<CreatureController>();
        obstacles = LayerMask.GetMask("Default");
    }

    void Start()
    {
        target = GetWanderingTarget();
    }

    void FixedUpdate()
    {
        UpdateTarget();
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, creatureController.MoveSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
    }

    private void UpdateTarget()
    {
        if (huntingTarget != null && Vector2.Distance(transform.position, huntingTarget.position) > Mathf.Min(huntingRadius, fleeingRadius))
        {
            huntingTarget = null;
        }

        // wandering state
        if ((huntingTarget == null) && Vector2.Distance(transform.position, target) < targetThreshold)
        {
            // Debug.Log("wandering");
            target = GetWanderingTarget();
            EndUseWeapon();
        }

        // fleeing state
        else if (huntingTarget != null && creatureController.Health < creatureController.originalHealth / 4)
        {
            // Debug.Log("fleeing");
            target = (transform.position - huntingTarget.position).normalized * fleeingRadius * 2;
            TryToUseWeapon();
        }

        // hunting state
        else if (huntingTarget != null)
        {
            // Debug.Log("hunting");
            target = huntingTarget.position;
            TryToUseWeapon();
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

    private void TryToUseWeapon()
    {
        if (creatureController.Joint != null && !creatureController.Joint.useMotor)
        {
            var motor = creatureController.Joint.motor;

            motor.motorSpeed = creatureController.MaxSwingSpeed;
            motor.maxMotorTorque = creatureController.SwingAcceleration;

            creatureController.Joint.motor = motor;
            creatureController.Joint.useMotor = true;
        }
    }

    private void EndUseWeapon()
    {
        if (creatureController.Joint != null)
        {
            creatureController.Joint.useMotor = false;
        }
    }
}
