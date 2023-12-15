using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingShieldScript : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _rotationAmount = 1; // Rotation in Degrees
    [SerializeField] private float _damage = 5;

    private GameObject _playerShip;

    // ====================== Setup ======================
    void Start()
    {
        _playerShip = GameObject.FindGameObjectWithTag("Player");
    }

    // ====================== Function ======================

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, -_rotationAmount);
        transform.position = _playerShip.transform.position;
    }

    public float GetDamage()
    {
        return _damage;
    }

    private void OnDestroy()
    {
        Destroy(gameObject);
    }
}
