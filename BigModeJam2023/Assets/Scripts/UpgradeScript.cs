using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScript : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _range = 10;
    [SerializeField] private float _speed =  -0.003f;
    [SerializeField] private float _drift = -0.001f;
    private float _yPosition;
    private float _distanceTraveled = 0;

    // ====================== Setup ======================
    void Start()
    {
        _yPosition = transform.position.y;
        _drift = Random.Range(-10, 10) / 10000f;
    }

    // ====================== Function ======================

    void FixedUpdate()
    {
        transform.Translate(new Vector3(_drift * Time.timeScale, _speed * Time.timeScale, 0));

        _distanceTraveled += transform.position.y - _yPosition;
        _yPosition = transform.position.y;
        if (_distanceTraveled > _range) // destroying
            Destroy(gameObject);

    }
}
