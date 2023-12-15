using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeHeldTracker : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private TextMeshPro _numHeld;

    // ====================== Setup ======================
    private void Start()
    {
        UpgradeTray.OnUpgradeNumChanged += UpdateNum;
    }

    private void OnDestroy()
    {
        UpgradeTray.OnUpgradeNumChanged -= UpdateNum;
    }

    // ====================== Function ======================
    private void UpdateNum(int num)
    {
        _numHeld.text = num.ToString();
    }
}
