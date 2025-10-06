using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player Creature")]
    [SerializeField] private GameObject player;

    private InputSystem_Actions controls;
    private Vector2 moveInput;
    private float weaponInput;

    private Rigidbody2D playerRigidBody;
    private CreatureController creatureController;
    private Rigidbody2D weaponRigidBody;

    void Awake()
    {
        controls = new InputSystem_Actions();
        playerRigidBody = player.GetComponent<Rigidbody2D>();
        creatureController = player.GetComponent<CreatureController>();

        controls.Player.Move.performed += OnMovePerformed;
        controls.Player.Move.canceled += OnMoveCanceled;

        controls.Player.WeaponControl.performed += OnWeaponControlPerformed;
        controls.Player.WeaponControl.canceled += OnWeaponControlCanceled;
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
            playerRigidBody.linearVelocity = creatureController.MoveSpeed * moveInput;

            if (weaponRigidBody == null && creatureController.Joint != null) weaponRigidBody = creatureController.Joint.gameObject.GetComponent<Rigidbody2D>();
            if (creatureController.Joint != null && weaponInput != 0)
            {
                var motor = creatureController.Joint.motor;

                motor.motorSpeed = weaponInput * creatureController.MaxSwingSpeed;
                motor.maxMotorTorque = creatureController.SwingAcceleration * weaponRigidBody.mass;

                creatureController.Joint.motor = motor;
                creatureController.Joint.useMotor = true;
            }
            else if (creatureController.Joint != null)
            {
                creatureController.Joint.useMotor = false;
            }
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

    private void OnWeaponControlPerformed(InputAction.CallbackContext ctx)
    {
        weaponInput = ctx.ReadValue<float>();
    }
    
    private void OnWeaponControlCanceled(InputAction.CallbackContext ctx)
    {
        weaponInput = 0;
    }
}
