using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeBattery : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private TextMeshPro _durationText;
    [SerializeField] private Material _deadbatteryMat;

    // ====================== Function ======================
    public void UpdateCharge(float timeLeft)
    {
        if (timeLeft <= 0)
            timeLeft = 0;

        _durationText.text = timeLeft.ToString("F1") + "%";
    }

    public void Killbattery()
    {
        _meshRenderer.material = _deadbatteryMat;
    }
}
