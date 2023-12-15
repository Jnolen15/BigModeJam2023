using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletScrip : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _range = 20;

    private float _speed = -0.001f;
    private float _yPosition;
    private float _distanceTraveled = 0;

    // ====================== Setup ======================
    void Start()
    {
        _yPosition = transform.position.y;
    }

    // ====================== Function ======================


    void FixedUpdate()
    {
        transform.Translate(new Vector3(0, _speed * Time.timeScale, 0));

        _distanceTraveled += Mathf.Abs(transform.position.y - _yPosition);
        _yPosition = transform.position.y;
        if (_distanceTraveled > _range) // destroying
            Destroy(gameObject);

    }

    public void SetSpeed(float speed)
    {
        _speed = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("enemyBullet collided with:" + collision.gameObject.name);
        if (collision.gameObject.tag == "Shield")
        {
            Destroy(gameObject);
        }
    }

}
