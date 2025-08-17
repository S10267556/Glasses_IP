// File: Assets/Scripts/PlayerCrashVFX.cs
using UnityEngine;

public class PlayerCrashVFX : MonoBehaviour
{
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


    public void ShowCrash()
    {
            if (crashVFX1 != null)
                crashVFX1.SetActive(true);
            if (crashVFX2 != null)
                crashVFX2.SetActive(true);
            if (crashVFX3 != null)
                crashVFX3.SetActive(true);
    }
}
