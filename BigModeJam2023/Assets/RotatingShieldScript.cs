using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingShieldScript : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _rotationAmount = 1; // Rotation in Degrees
    [SerializeField] private float _damage = 5;

    // ====================== Setup ======================
    void Start()
    {
        
    }

    // ====================== Function ======================

    private void FixedUpdate()
    {
        transform.Rotate(0, 0, -_rotationAmount);
    }

    public float GetDamage()
    {
        return _damage;
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.tag == "Hostile")
        {
            Destroy(other);
        }
    }
    */

    private void OnDestroy()
    {
        Destroy(transform.parent.gameObject);
    }
}
