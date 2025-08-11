// File: Assets/Scripts/VehicleInteraction.cs

using UnityEngine;

public class VehicleInteraction : MonoBehaviour
{
    [SerializeField] private Transform playerCamPoint;   // Camera position when on foot
    [SerializeField] private Transform vehicleCamPoint;  // Camera position when in vehicle
    [SerializeField] private float camMoveSpeed = 5f;    // Lerp speed for camera
    public Transform player;
    public Transform vehicle;
    public GameObject playerModel;          // Mesh/visuals of the player
    public MonoBehaviour playerController;  // Your player movement script
    public MonoBehaviour vehicleController; // Your vehicle movement script

    private Camera mainCam;
    private bool inVehicle = false;
    private bool transitioning = false;

    void Start()
    {
        mainCam = Camera.main;
        playerModel.SetActive(true);
        playerController.enabled = true;
        vehicleController.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !transitioning)
        {
            inVehicle = !inVehicle; // Toggle state
            playerModel.SetActive(false);
            playerController.enabled = false;
            vehicleController.enabled = true;
            StopAllCoroutines();
            StartCoroutine(MoveCamera(inVehicle ? vehicleCamPoint : playerCamPoint));
        }
    }

    private System.Collections.IEnumerator MoveCamera(Transform targetPoint)
    {
        transitioning = true;

        // Smoothly move camera to target position
        while (Vector3.Distance(mainCam.transform.position, targetPoint.position) > 0.01f ||
               Quaternion.Angle(mainCam.transform.rotation, targetPoint.rotation) > 0.1f)
        {
            mainCam.transform.position = Vector3.Lerp(
                mainCam.transform.position,
                targetPoint.position,
                Time.deltaTime * camMoveSpeed
            );

            mainCam.transform.rotation = Quaternion.Slerp(
                mainCam.transform.rotation,
                targetPoint.rotation,
                Time.deltaTime * camMoveSpeed
            );

            yield return null;
        }

        // Snap exactly to target
        mainCam.transform.position = targetPoint.position;
        mainCam.transform.rotation = targetPoint.rotation;

        transitioning = false;
    }
}
