using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, ICreature
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
        rb.linearVelocity = _moveSpeed * moveInput;
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
        _health = Mathf.Max(0, _health - incomingDamage);
    }
}
