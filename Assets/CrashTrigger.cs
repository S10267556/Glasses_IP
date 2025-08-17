// File: Assets/Scripts/CarFrontTrigger.cs
using UnityEngine;

public class CarFrontTrigger : MonoBehaviour
{
    [Header("Crash VFX")]
    [Tooltip("Assign a crash VFX GameObject (disabled by default) inside this car.")]
    [SerializeField] private GameObject crashVFX1;
    [SerializeField] private GameObject crashVFX2;
    [SerializeField] private GameObject crashVFX3;

    private void Awake()
    {
        if (crashVFX1 != null)
            crashVFX1.SetActive(false); 
        if (crashVFX2 != null)
            crashVFX2.SetActive(false); 
        if (crashVFX3 != null)
            crashVFX3.SetActive(false); 
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the trigger hits the Player or Player's vehicle
        if (other.CompareTag("Player") || other.CompareTag("PlayerVehicle"))
        {
            // Show this car's crash VFX
            if (crashVFX1 != null)
                crashVFX1.SetActive(true);
            if (crashVFX2 != null)
                crashVFX2.SetActive(true);
            if (crashVFX3 != null)
                crashVFX3.SetActive(true);

            // If the player vehicle has its own crash VFX, show it too
            PlayerCrashVFX playerVFX = other.GetComponentInParent<PlayerCrashVFX>();
            if (playerVFX != null)
                playerVFX.ShowCrash();
        }
    }
}
