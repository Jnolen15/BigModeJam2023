using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerRocketScript : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _range = 20;
    [SerializeField] private float _damage = 100;
    [SerializeField] private float _explosionDuration = 1;
    [SerializeField] private float _acceleration = 0f;
    [SerializeField] private float _ySpeed = 0f;
    [SerializeField] private float _xSpeed = 0f;


    private float _yPosition;
    private float _distanceTraveled = 0;

    private bool _exploded = false;

    [SerializeField] private GameObject _rocketSprite;
    [SerializeField] private GameObject _explosion;

    // ====================== Setup ======================
    void Start()
    {
        _yPosition = transform.position.y;
    }

    // ====================== Function ======================

    void FixedUpdate()
    {
        if (!_exploded) _ySpeed += _acceleration;
        if (!_exploded) transform.Translate(new Vector3(_xSpeed * Time.timeScale, _ySpeed * Time.timeScale, 0));

        _distanceTraveled += Mathf.Abs(transform.position.y - _yPosition);
        _yPosition = transform.position.y;


        if (_distanceTraveled > _range) // destroying
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hostile" && !_exploded)
        {
            Explode();
        }
    }

    private void Explode()
    {
        _exploded = true;
        _explosion.SetActive(true);
        _rocketSprite.SetActive(false);
        SpriteRenderer _sprite = _explosion.GetComponentInChildren<SpriteRenderer>();
        _sprite.DOFade(0, _explosionDuration).SetEase(Ease.OutSine).OnComplete(() => Destroy(gameObject));
    }

    public void SetDrift(float drift)
    {
        _xSpeed = drift;
    }

    public float GetDamage()
    {
        return _damage;
    }
}
