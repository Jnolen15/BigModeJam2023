using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserScript : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _damage = 1;
    [SerializeField] private float _lifespan = 3;

    private GameObject _laserObj;

    private float _ySpeed = 0.001f;
    private float _xSpeed = 0f;
    private float _yPosition;
    private float _distanceTraveled = 0;


    // ====================== Setup ======================
    void Start()
    {
        _laserObj = transform.parent.gameObject;
    }

    // ====================== Function ======================

    void FixedUpdate()
    {
        

    }

    public void SetSpeed(float xSpeed, float ySpeed)
    {
        _xSpeed = xSpeed;
        _ySpeed = ySpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player shot: " + collision.name);
    }

    public float GetDamage()
    {
        return _damage;
    }
}
