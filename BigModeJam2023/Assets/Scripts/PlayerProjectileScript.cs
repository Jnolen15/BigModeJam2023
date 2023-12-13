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
    [SerializeField] private GameObject _impactParticle;

    // ====================== Setup ======================
    void Start()
    {
        _yPosition = transform.position.y;
    }

    // ====================== Function ======================

    void FixedUpdate()
    {
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player shot: " + collision.name);
        Instantiate(_impactParticle, transform.position, transform.rotation);
    }

    public float GetDamage()
    {
        return _damage;
    }
}
