using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 3f;

    private int paramXHash;
    private int paramYHash;
    private int paramIsMovingHash;

    private Rigidbody2D rb;
    private Animator animator;
    private InputAction moveAction;
    private Vector2 moveInput;
    private Vector2 lastDirection = Vector2.down;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        paramXHash = Animator.StringToHash("X");
        paramYHash = Animator.StringToHash("Y");
        paramIsMovingHash = Animator.StringToHash("isMoving");

        if (animator != null)
        {
            foreach (var p in animator.parameters)
            {
                if (p.name.Equals("X", System.StringComparison.OrdinalIgnoreCase))
                    paramXHash = p.nameHash;
                else if (p.name.Equals("Y", System.StringComparison.OrdinalIgnoreCase))
                    paramYHash = p.nameHash;
                else if (p.name.Equals("isMoving", System.StringComparison.OrdinalIgnoreCase))
                    paramIsMovingHash = p.nameHash;
            }
        }

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

        if (isMoving)
        {
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                lastDirection = new Vector2(Mathf.Sign(moveInput.x), 0f);
            }
            else
            {
                lastDirection = new Vector2(0f, Mathf.Sign(moveInput.y));
            }
        }

        animator.SetFloat(paramXHash, lastDirection.x);
        animator.SetFloat(paramYHash, lastDirection.y);
        animator.SetBool(paramIsMovingHash, isMoving);
    }
}
