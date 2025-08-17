// File: Assets/Scripts/PlayerWinController.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerWinController : MonoBehaviour
{
    private Rigidbody rb;
    private bool isWinning = false;
    private float targetSpeed;
    private Transform targetDirection;

    [SerializeField] private float turnSpeed = 2f; // how fast to rotate toward win direction

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (isWinning)
        {
            // If a win direction was given, rotate toward it
            if (targetDirection != null)
            {
                Vector3 targetForward = targetDirection.forward;
                Quaternion targetRot = Quaternion.LookRotation(targetForward, Vector3.up);
                rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRot, turnSpeed * Time.fixedDeltaTime));
            }

            // Always move forward relative to car’s current facing
            rb.linearVelocity = transform.forward * targetSpeed;
        }
    }

    // Called by WinTrigger
    public void TriggerWin(float driveSpeed, Transform winDir = null)
    {
        targetSpeed = driveSpeed;
        isWinning = true;
        targetDirection = winDir;

        // Disable player control script (replace with your controller type)
        MonoBehaviour playerController = GetComponent<MonoBehaviour>(); 
        if (playerController != null)
            playerController.enabled = false;
    }
}
