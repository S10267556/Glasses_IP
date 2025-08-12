using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("UI References")]
    [SerializeField] private GameObject deathPanel; // The whole death UI panel

    private void Awake()
    {
        if (Instance == null) Instance = this;
        if (deathPanel != null)
            deathPanel.SetActive(false); // Hide at start
    }

    public void ShowDeathUI()
    {
        if (deathPanel != null)
            deathPanel.SetActive(true);
    }
}
