using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private static readonly int ParamXHash = Animator.StringToHash("X");
    private static readonly int ParamYHash = Animator.StringToHash("Y");

    private Rigidbody2D rb;
    private Animator animator;
    private InputAction moveAction;
    private Vector2 moveInput;
    private Vector2 lastDirection = Vector2.down;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

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
        bool isMoving = moveInput.sqrMagnitude > 0.01f;

if (isMoving)
{
    lastDirection = moveInput.normalized;
    animator.SetFloat("X", lastDirection.x);
    animator.SetFloat("Y", lastDirection.y);
}
else
{
    animator.SetFloat("X", lastDirection.x);
    animator.SetFloat("Y", lastDirection.y);
}

animator.SetBool("IsMoving", isMoving);
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

        if (isMoving)
        {
            lastDirection = moveInput.normalized;
            animator.SetFloat(ParamXHash, lastDirection.x);
            animator.SetFloat(ParamYHash, lastDirection.y);
        }
        else
        {
            animator.SetFloat(ParamXHash, lastDirection.x);
            animator.SetFloat(ParamYHash, lastDirection.y);
        }

        animator.SetBool("IsMoving", isMoving);
    }
}
