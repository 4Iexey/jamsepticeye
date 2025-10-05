using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, ICreature
{
    [Header("Stats")]
    [SerializeField] private int level = 1;
    [SerializeField] private float health = 10f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float maxSwingSpeed = 10f;
    [SerializeField] private float swingAcceleration = 10f;
    [SerializeField] private float weight = 10f;

    public int Level => level;
    public float Health => health;
    public float MoveSpeed => moveSpeed;
    public float MaxSwingSpeed => maxSwingSpeed;
    public float SwingAcceleration => swingAcceleration;
    public float Weight => weight;

    private InputSystem_Actions controls;
    private Vector2 moveInput;

    private Rigidbody2D rb;

    void Awake()
    {
        controls = new InputSystem_Actions();
        rb = GetComponent<Rigidbody2D>();

        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }

    void Start()
    {

    }

    void Update()
    {
        rb.linearVelocity = moveSpeed * moveInput;
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
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

    public void TakeDamage(float incomingDamage)
    {
        health = Mathf.Max(0, health - incomingDamage);
    }
}
