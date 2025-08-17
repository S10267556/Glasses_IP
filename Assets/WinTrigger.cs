// File: Assets/Scripts/WinTrigger.cs
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [SerializeField] private float driveOffSpeed = 10f;
    [SerializeField] private Transform winDirection;   // uses the rotation of the object not the direction 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerVehicle"))
        {
            PlayerWinController winController = other.GetComponentInParent<PlayerWinController>();
            if (winController != null)
            {
                winController.TriggerWin(driveOffSpeed, winDirection);
            }
        }
    }
}
