using UnityEngine;
using UnityEngine.InputSystem;

public class MontgolfiereXR : MonoBehaviour
{
    [Header("Forces")]
    [SerializeField] private float riseForce = 2f;
    [SerializeField] private float fallForce = 2f;
    [SerializeField] private float maxUpForce = 20f;

    [Header("Input Actions")]
    public InputActionAsset inputSystem; // ton asset "InputSystem"

    private InputAction activBruleursAction;
    private InputAction activSoupapeAction;

    private Rigidbody rb;
    private float upForce = 0f;

    private bool isRising = false;
    private bool isFalling = false;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();

        // Récupération des actions depuis ton asset
        var actionMap = inputSystem.FindActionMap("Montgolfiere");
        activBruleursAction = actionMap.FindAction("ActivBruleurs");
        activSoupapeAction = actionMap.FindAction("ActivSoupape");

        // Souscrire aux callbacks
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
    }
}
