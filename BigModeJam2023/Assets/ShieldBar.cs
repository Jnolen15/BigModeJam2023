using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBar : MonoBehaviour
{
    // ====================== Refrences / Variables ======================

    private PlayerShipController _shipController;


    // ====================== Setup ======================
    void Start()
    {
        _shipController = GameObject.Find("Player Ship").GetComponent<PlayerShipController>();
    }

    // ====================== Function ======================

    void Update()
    {
        Vector3 scale = transform.localScale;
        scale.y = _shipController.GetShieldRatio();
        transform.localScale = scale;
    }
}
