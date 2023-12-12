using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileScript : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _range = 20;
    [SerializeField] private float _damage = 5;
    private float _ySpeed = 0.001f;
    private float _xSpeed = 0f;
    private float _yPosition;
    private float _distanceTraveled = 0;
    private Vector3 _direction;

    // ====================== Setup ======================
    void Start()
    {
        _yPosition = transform.position.y;
    }

    // ====================== Function ======================

    void FixedUpdate()
    {
        if(_direction != Vector3.zero) // Aim shooting
            transform.Translate(_direction * _ySpeed);
        else
            transform.Translate(new Vector3(_xSpeed * Time.timeScale, _ySpeed * Time.timeScale, 0));

        _distanceTraveled += Mathf.Abs(transform.position.y - _yPosition);
        _yPosition = transform.position.y;
        if (_distanceTraveled > _range) // destroying
            Destroy(gameObject);

    }

    public void SetSpeed(float xSpeed, float ySpeed)
    {
        _xSpeed = xSpeed;
        _ySpeed = ySpeed;
    }

    public void SetRotation(Vector3 rot)
    {
        _direction = rot;
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
