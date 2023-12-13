using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar3D : MonoBehaviour
{
    // ====================== Refrences / Variables ======================

    private PlayerShipController _shipController;
    private float _previousScale;
    private float _startingScale;

    // ====================== Setup ======================
    void Start()
    {
        _shipController = GameObject.Find("Player Ship").GetComponent<PlayerShipController>();
        _startingScale = transform.localScale.y;
        _previousScale = _startingScale;
    }

    // ====================== Function ======================

    void Update()
    {
        Vector3 scale = transform.localScale;
        scale.y = _shipController.GetHealthRatio() * _startingScale;

        transform.Translate(new Vector3(0, (scale.y - _previousScale)/2, 0));
        transform.localScale = scale;

        _previousScale = scale.y;
    }
}
