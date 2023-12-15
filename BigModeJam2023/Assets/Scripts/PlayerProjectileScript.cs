using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileScript : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _range = 20;
    [SerializeField] private float _damage = 5;
    [SerializeField] private float _penetrations = 0;

    private float _ySpeed = 0.001f;
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
        transform.Translate(new Vector3(0, _ySpeed * Time.timeScale, 0));

        _distanceTraveled += Mathf.Abs(transform.position.y - _yPosition);
        _yPosition = transform.position.y;
        if (_distanceTraveled > _range) // destroying
            Destroy(gameObject);
    }

    public void SetAngle(float angle)
    {
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    public void SetSpeed(float speed)
    {
        _ySpeed = speed;
    }
    public void SetRange(float range)
    {
        _range = range;
    }
    
    public void SetPiercing(float num)
    {
        _penetrations = num;
    }

    public void HitEnemy()
    {
        if (_penetrations <= 0) Destroy(gameObject);
        _penetrations -= 1;
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
