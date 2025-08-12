// File: Assets/Scripts/CrashTrigger.cs
using UnityEngine;

public class CarFrontTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && UIManager.Instance != null)
        {
            UIManager.Instance.ShowDeathUI();
        }
    }
}

