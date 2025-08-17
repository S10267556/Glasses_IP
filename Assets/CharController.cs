using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharController : MonoBehaviour
{
    [Header("Movement")]
    public float speed = 5f;
    public float rotationSpeed = 720f;

    [Header("Jump & Gravity")]
    public float jumpHeight = 0.5f;
    public float gravity = -9.81f;

    [Header("References (auto-filled if left empty)")]
    public CharacterController controller;
    public Animator animator;

    // Debug-style fields to resemble the tutorial inspector
    [SerializeField] Vector3 moveDirection;
    [SerializeField] Vector3 jumpVelocity;
    [SerializeField] bool grounded;

    Transform cam;

    void Awake()
    {
        controller = controller ?? GetComponent<CharacterController>();
        animator = animator ?? GetComponent<Animator>();
        cam = Camera.main ? Camera.main.transform : null;
    }

    void Update()
    {
        grounded = controller.isGrounded;

        // keep the controller stuck to ground
        if (grounded && jumpVelocity.y < 0f) jumpVelocity.y = -2f;

        // --- input (WASD/Arrows) ---
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        // camera-relative movement on XZ plane
        Vector3 f = cam ? cam.forward : Vector3.forward;
        Vector3 r = cam ? cam.right : Vector3.right;
        f.y = r.y = 0f; f.Normalize(); r.Normalize();

        moveDirection = (f * v + r * h).normalized;

        // move and rotate toward movement
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            controller.Move(moveDirection * speed * Time.deltaTime);

            Quaternion look = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, look, rotationSpeed * Time.deltaTime);
        }

        // jump
        if (grounded && Input.GetButtonDown("Jump"))
        {
            // v = sqrt(2 * jumpHeight * -gravity)
            jumpVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // gravity
        jumpVelocity.y += gravity * Time.deltaTime;
        controller.Move(jumpVelocity * Time.deltaTime);

        // animator parameters (optional)
        if (animator)
        {
            float planarSpeed = new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude;
            animator.SetFloat("Speed", planarSpeed);   // use in your idle/walk/run blend
            animator.SetBool("Grounded", grounded);    // use for jump/land transitions
        }
    }
}
