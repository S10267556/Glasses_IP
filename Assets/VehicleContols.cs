// File: Assets/Scripts/VehicleControls.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BasicVehicleController : MonoBehaviour
{
    [Header("Movement Settings")]
    public float acceleration = 3000f;
    public float steeringSpeed = 90f;
    public float maxSpeed = 20f;
    public float naturalDecel = 2f;
    public float traction = 5f; // Higher = more grip

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = Vector3.down;
        rb.linearDamping = 0.5f;
        rb.angularDamping = 2f;
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxis("Vertical");
        float steerInput = Input.GetAxis("Horizontal");

        // Forward/backward force
        if (Mathf.Abs(moveInput) > 0.01f && rb.linearVelocity.magnitude < maxSpeed)
        {
            rb.AddForce(transform.forward * moveInput * acceleration * Time.fixedDeltaTime);
        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, naturalDecel * Time.fixedDeltaTime);
        }

        // Instant steering
        if (Mathf.Abs(steerInput) > 0.01f)
        {
            Quaternion turnOffset = Quaternion.Euler(0f, steerInput * steeringSpeed * Time.fixedDeltaTime, 0f);
            rb.MoveRotation(rb.rotation * turnOffset);
        }

        // Apply traction: reduce sideways velocity
        Vector3 localVel = transform.InverseTransformDirection(rb.linearVelocity);
        localVel.x = Mathf.Lerp(localVel.x, 0f, traction * Time.fixedDeltaTime);
        rb.linearVelocity = transform.TransformDirection(localVel);
    }
}
