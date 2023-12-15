using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShipLights : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private Material _lightMat;
    [SerializeField] private Light _light;
    [SerializeField] private Color _lightColor;
    [SerializeField] private Gradient _gradient;
    private bool _setDisco;

    // ====================== Setup ======================
    private void Start()
    {
        ScoreManager.OnReachSecret += DiscoLights;

        _light.color = _lightColor;
    }

    private void OnDestroy()
    {
        ScoreManager.OnReachSecret -= DiscoLights;

        _light.color = _lightColor;
        _lightMat.color = _lightColor;
        _lightMat.SetColor("_EmissionColor", _lightColor);
    }

    // ====================== Function ======================
    private void Update()
    {
        if (!_setDisco)
            return;

        _lightMat.SetColor("_EmissionColor", _lightMat.color);
        _light.color = _lightMat.color;
    }

    private void DiscoLights()
    {
        _setDisco = true;
        _lightMat.DOGradientColor(_gradient, 5f).SetLoops(-1, LoopType.Restart);
    }
}
