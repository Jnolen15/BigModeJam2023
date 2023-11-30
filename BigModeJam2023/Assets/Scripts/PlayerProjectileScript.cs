using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileScript : MonoBehaviour
{
    // ====================== Refrences / Variables ======================
    [SerializeField] private float _range = 20;

    private float _speed = 0.001f;
    private float _lifeTimeStamp = 0;
    private float _yPosition;
    private float _distanceTraveled = 0;

    private GameplayManager _gameplayManager;

    // ====================== Setup ======================
    void Start()
    {
        _gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        _yPosition = transform.position.y;
    }

    // ====================== Function ======================

    void Update()
    {
        if (!_gameplayManager.GamePaused)  // moving
            transform.Translate(new Vector3(0, _speed * Time.timeScale, 0));

        _distanceTraveled += transform.position.y - _yPosition;
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
        Debug.Log("Player shot: " + collision.name);   
    }
}
