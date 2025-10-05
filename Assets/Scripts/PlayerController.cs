using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Creature")]
    [SerializeField] private GameObject player;

    private InputSystem_Actions controls;
    private Vector2 moveInput;

    private Rigidbody2D rb;
    private CreatureController creatureController;

    void Awake()
    {
        controls = new InputSystem_Actions();
        rb = player.GetComponent<Rigidbody2D>();
        creatureController = player.GetComponent<CreatureController>();

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
        if (creatureController != null && creatureController.gameObject.activeSelf)
        {
            rb.linearVelocity = creatureController.MoveSpeed * moveInput;
        }
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        moveInput = Vector2.zero;
    }
}
