using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    // Parámetros del Animator del paquete Cainos
    private static readonly int DirectionHash = Animator.StringToHash("Direction");
    private static readonly int IsMovingHash = Animator.StringToHash("IsMoving");

    private Rigidbody2D rb;
    private Animator animator;
    private InputAction moveAction;
    private Vector2 moveInput;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Configuración top-down: sin gravedad y sin rotar al chocar
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;

        moveAction = new InputAction("Move", InputActionType.Value);
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/w")
            .With("Down", "<Keyboard>/s")
            .With("Left", "<Keyboard>/a")
            .With("Right", "<Keyboard>/d");
        moveAction.AddCompositeBinding("2DVector")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/rightArrow");
        moveAction.AddBinding("<Gamepad>/leftStick");
    }

    private void OnEnable() => moveAction.Enable();

    private void OnDisable() => moveAction.Disable();

    private void OnDestroy() => moveAction.Dispose();

    private void Update()
    {
        moveInput = Vector2.ClampMagnitude(moveAction.ReadValue<Vector2>(), 1f);
        UpdateAnimation();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput * speed;
    }

    private void UpdateAnimation()
    {
        if (animator == null) return;

        bool isMoving = moveInput.sqrMagnitude > 0.01f;
        animator.SetBool(IsMovingHash, isMoving);
        if (!isMoving) return;

        // 0 = abajo, 1 = arriba, 2 = derecha, 3 = izquierda
        int direction = Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y)
            ? (moveInput.x > 0 ? 2 : 3)
            : (moveInput.y > 0 ? 1 : 0);
        animator.SetInteger(DirectionHash, direction);
    }
}
