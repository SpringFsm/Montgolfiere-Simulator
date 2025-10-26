using UnityEngine;
using UnityEngine.InputSystem;

public class MontgolfiereXR : MonoBehaviour
{
    [Header("Forces")]
    [SerializeField] private float riseForce = 2f;
    [SerializeField] private float fallForce = 2f;
    [SerializeField] private float maxUpForce = 20f;

    [Header("Wind Settings")]
    [SerializeField] private AnimationCurve windX = AnimationCurve.Linear(0, 0, 50, 5);
    [SerializeField] private AnimationCurve windZ = AnimationCurve.Linear(0, 0, 50, -3);

    [Header("Input Actions")]
    public InputActionAsset inputSystem;

    private InputAction activBruleursAction;
    private InputAction activSoupapeAction;

    private Rigidbody rb;
    private float upForce = 0f;

    private bool isRising = false;
    private bool isFalling = false;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();

        var actionMap = inputSystem.FindActionMap("Montgolfiere");
        activBruleursAction = actionMap.FindAction("ActivBruleurs");
        activSoupapeAction = actionMap.FindAction("ActivSoupape");

        activBruleursAction.performed += ctx => isRising = true;
        activBruleursAction.canceled += ctx => isRising = false;

        activSoupapeAction.performed += ctx => isFalling = true;
        activSoupapeAction.canceled += ctx => isFalling = false;

        activBruleursAction.Enable();
        activSoupapeAction.Enable();
    }

    private void OnDisable()
    {
        activBruleursAction.Disable();
        activSoupapeAction.Disable();
    }

    private void FixedUpdate()
    {
        // Vertical forces
        if (isRising)
        {
            upForce += riseForce;
        }
        else if (isFalling)
        {
            upForce -= fallForce;
        }
        else
        {
            upForce = 0f;
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, -0.5f, rb.linearVelocity.z);
        }

        upForce = Mathf.Clamp(upForce, 0f, maxUpForce);
        rb.AddForce(Vector3.up * upForce, ForceMode.Force);

        // Wind force based on height
        float y = transform.position.y;
        float windForceX = windX.Evaluate(y);
        float windForceZ = windZ.Evaluate(y);
        Vector3 windForce = new Vector3(windForceX, 0f, windForceZ);

        rb.AddForce(windForce, ForceMode.Force);
    }
}
