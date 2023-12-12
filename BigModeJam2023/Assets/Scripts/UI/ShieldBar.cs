using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldBar : MonoBehaviour
{
    // ====================== Refrences / Variables ======================

    [SerializeField] private Material Active;
    [SerializeField] private Material Inactive;

    private PlayerShipController _shipController;
    private MeshRenderer _mesh;

    // ====================== Setup ======================
    void Start()
    {
        _shipController = GameObject.Find("Player Ship").GetComponent<PlayerShipController>();
        _mesh = GetComponent<MeshRenderer>();
    }

    // ====================== Function ======================

    void Update()
    {
        Vector3 scale = transform.localScale;
        scale.y = _shipController.GetShieldRatio();
        transform.localScale = scale;

        if (_shipController.ShieldActive())
        {
            if (_mesh.material != Active) _mesh.material = Active;
        } else
        {
            if (_mesh.material != Inactive) _mesh.material = Inactive;
        }
    }
}
