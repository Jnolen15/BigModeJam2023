using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectileScript : MonoBehaviour
{
    // ====================== Refrences / Variables ======================

    [SerializeField] private float _speed = 0.001f;
    [SerializeField] private float _lifeSpan = 3;
    private GameplayManager _gameplayManager;
    private float _lifeTimeStamp = 0;

    // ====================== Setup ======================
    void Start()
    {
        _gameplayManager = GameObject.Find("GameplayManager").GetComponent<GameplayManager>();
        _lifeTimeStamp = Time.time + _lifeSpan;
    }

    // ====================== Function ======================

    void Update()
    {
        if (!_gameplayManager.GamePaused)  // moving
            transform.Translate(new Vector3(0, _speed * Time.timeScale, 0));
        if (Time.time > _lifeTimeStamp) // destroying
            Destroy(gameObject);

        if (_gameplayManager.GamePaused) // extending timestamp on pause
            _lifeTimeStamp += Time.deltaTime;
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
